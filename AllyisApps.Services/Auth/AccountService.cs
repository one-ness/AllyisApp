//------------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Lookup;
using AllyisApps.Lib;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Lookup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
    /// <summary>
    /// Business logic for all account related operations.
    /// </summary>
    public partial class AppService : BaseService
    {
        #region public static

        /// <summary>
        /// Returns a compressed version of the given email address if it is too long.
        /// </summary>
        /// <param name="fullEmail">Full email address.</param>
        /// <returns>Compressed email address, or the full address if it is short enough (or null).</returns>
        public static string GetCompressedEmail(string fullEmail)
        {
            if (!string.IsNullOrEmpty(fullEmail) && fullEmail.Length > 50)
            {
                string cemail = string.Format("{0}...{1}", fullEmail.Substring(0, 20), fullEmail.Substring(fullEmail.Length - 15));
                return cemail;
            }
            else
            {
                return fullEmail;
            }
        }

        #endregion public static

        #region public

        /// <summary>
        /// Returns a collection of valid states/provinces for the given country.
        /// </summary>
        /// <param name="countryName">Country name.</param>
        /// <returns><see cref="IEnumerable"/> of valid states/provinces.</returns>
        public IEnumerable ValidStates(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName)) throw new ArgumentException("countryName");
            return DBHelper.ValidStates(countryName);
        }

        /// <summary>
        /// Gets the list of valid countries.
        /// </summary>
        /// <returns>A collection of valid countries.</returns>
        public Dictionary<string, string> GetCountries()
        {
            return DBHelper.GetCountries();
        }

        /// <summary>
        /// get the list of states for the given country
        /// </summary>
        public Dictionary<int, string> GetStates(string countryCode)
        {
            return this.DBHelper.GetStates(countryCode);
        }

        /// <summary>
        /// Gets the list of valid languages selections.
        /// </summary>
        /// <returns>A list of languages.</returns>
        public IEnumerable<Language> ValidLanguages()
        {
            return DBHelper.ValidLanguages().Select(s => new Language
            {
                LanguageName = s.LanguageName,
                CultureName = s.CultureName
            });
        }

        /// <summary>
        /// Retrieves a list of all pending invitations for a given user by the user's email.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <returns>The invitation associated with the user.</returns>
        public List<InvitationInfo> GetInvitationsByUser(string userEmail)
        {
            #region Validation

            if (!Utility.IsValidEmail(userEmail))
            {
                throw new FormatException("Email address must be in a valid format.");
            }

            #endregion Validation

            var invitationsDB = DBHelper.GetUserInvitationsByUserData(new UserDBEntity()
            {
                Email = userEmail
            });

            return invitationsDB.Select(idb => InitializeInvitationInfo(idb)).ToList();
        }

        /// <summary>
        /// Accepts an invitation, adding the user to the invitation's organization, subscriptions, and projects, then deletes the invitations.
        /// </summary>
        /// <param name="invitationId">The invitationId.</param>
        /// <returns>The resulting action message if succeed, null if fail.</returns>
        public string AcceptUserInvitation(int invitationId)
        {
            #region Validation

            if (invitationId <= 0) throw new ArgumentOutOfRangeException("invitationId", "The invitation id cannot be zero or negative.");

            #endregion Validation

            NotifyInviteAcceptAsync(invitationId);

            var results = DBHelper.AcceptInvitation(invitationId, UserContext.UserId);

            if (results == null) return null;

            return string.Format("You have successfully joined {0} in the role of {1}.", results.Item1, results.Item2);
        }

        /// <summary>
        /// Rejects an invitation, deleting it from a user's pending invites.
        /// </summary>
        /// <param name="invitationId">The id of the invitation to reject.</param>
        /// <returns>The resulting message.</returns>
        public string RejectUserInvitation(int invitationId)
        {
            try
            {
                DBHelper.RejectUserInvitation(invitationId);
                NotifyInviteRejectAsync(invitationId);
                return "The invitation has been rejected.";
            }
            catch (SqlException)
            {
                return null;
            }
           
        }

        /// <summary>
        /// Setup a new user.
        /// </summary>
        public async Task<int> SetupNewUser(
            string email,
            string password,
            string firstName,
            string lastName,
            Guid emailConfirmationCode,
            DateTime? dateOfBirth,
            string phoneNumber,
            string address1,
            string address2,
            string city,
            int? stateId,
            string postalCode,
            string countryCode,
            string confirmEmailSubject,
            string confirmEmailMessage)
        {
            if (!Utility.IsValidEmail(email)) throw new ArgumentException("email");
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException("firstName:");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException("lastName");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password");
            if (emailConfirmationCode == null) throw new ArgumentException("emailConfirmationCode");
            var result = 0;
            try
            {
                result = await this.DBHelper.CreateUserAsync(
                    email, Crypto.GetPasswordHash(password), firstName, lastName, emailConfirmationCode, dateOfBirth, phoneNumber, Language.DefaultLanguageCultureName,
                    address1, address2, city, stateId, postalCode, countryCode);

                // user created, send confirmation email
                await Mailer.SendEmailAsync(this.ServiceSettings.SupportEmail, email, confirmEmailSubject, confirmEmailMessage);
            }
            catch (SqlException ex)
            {
                if (ex.Message.ToLower().Contains("unique"))
                {
                    // unique constraint violation of email
                }
                else
                {
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// Validates the given email and password with the database.
        /// </summary>
        /// <param name="email">The login email.</param>
        /// <param name="password">The login password.</param>
        public UserContext ValidateLogin(string email, string password)
        {
            if (!Utility.IsValidEmail(email)) throw new ArgumentException("email");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password");

            UserContext result = null;
            var user = this.DBHelper.GetUserByEmail(email);
            if (user != null)
            {
                // email exists, hash the given password and compare with hash in db
                Tuple<bool, string> passwordValidation = Crypto.ValidateAndUpdate(password, user.PasswordHash);
                if (passwordValidation.Item1)
                {
                    result = new UserContext(user.UserId, email, user.FirstName, user.LastName);

                    // Store updated password hash if needed
                    if (passwordValidation.Item2 != null)
                    {
                        DBHelper.UpdateUserPassword(user.UserId, passwordValidation.Item2);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Uses the database to return a fully populated UserContext from the userId.
        /// </summary>
        /// <param name="userId">The user Id to look up.</param>
        /// <returns>The User context after population.</returns>
        public UserContext PopulateUserContext(int userId)
        {
            if (userId <= 0) throw new ArgumentException("userId");

            UserContext result = null;

            // get context from db
            dynamic expando = this.DBHelper.GetUserContext(userId);

            // get user information
            if (expando != null && expando.User != null)
            {
                // user found.
                result = new UserContext();
                result.UserId = expando.User.UserId;
                result.FirstName = expando.User.FirstName;
                result.LastName = expando.User.LastName;
                result.Email = expando.User.Email;

                // get organization and roles
                foreach (var item in expando.OrganizationsAndRoles)
                {
                    result.OrganizationsAndRoles.Add(item.OrganizationId, new UserContext.OrganizationAndRole() {
                        OrganizationId = item.OrganizationId,
                        OrganizationRole = (OrganizationRole)item.OrganizationRoleId });
                }

                // get subscriptions and roles
                foreach (var item in expando.SubscriptionsAndRoles)
                {

                    UserSubscription sub = new UserSubscription();
                    sub.AreaUrl = item.AreaUrl;
                    sub.OrganizationId = item.OrganizationId;
                    sub.ProductId = (ProductIdEnum)item.ProductId;
                    sub.ProductName = item.ProductName;
                    sub.ProductRoleId = item.ProductRoleId;
                    //sub.ProductRoleName = item.ProductRoleName; Logic Fails here as this is a one-to-many for this information I don't think we use it.
                    sub.SkuId = item.SkuId;
                    sub.SubscriptionId = item.SubscriptionId;
                    sub.SubscriptionName = item.SubscriptionName;
                    result.UserSubscriptions.Add(sub.SubscriptionId, sub);
                }

                // set result to self
                this.SetUserContext(result);
            }

            return result;
        }

        /// <summary>
        /// Updates the active subsciption for the current user.
        /// </summary>
        /// <param name="subscriptionId">Subscription Id.</param>
        public void UpdateActiveSubscription(int? subscriptionId)
        {
            if (subscriptionId.HasValue && subscriptionId.Value <= 0) throw new ArgumentException("subscriptionId");
            DBHelper.UpdateActiveSubscription(UserContext.UserId, subscriptionId);
        }

        /// <summary>
        /// Gets the user info for a specific user.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>A User instance with the current user's info.</returns>
        public User GetUserInfo(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
            }
            var results = DBHelper.GetUserInfo(userId);
            User user = InitializeUser(results.Item1, false);
            if (results.Item2 != null)
            {
                user.Address = InitializeAddress(results.Item2);
            }
            return user;
        }

        /// <summary>
        /// get the user profile
        /// </summary>
        public User GetCurrentUserInfo()
        {
            if (this.UserContext?.UserId == null)
            {
                return null;
            }
            var result = GetUserInfo(this.UserContext.UserId);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public UserAccount GetUser(int? userID){
            if (userID.HasValue)
            {
                var infos = DBHelper.GetUser(userID.Value);
                UserAccount account = new UserAccount(infos.Item1, infos.Item2, infos.Item3, infos.Item4);
                return account;
            }
            else
            {
                return null;
            }
        }

		/// <summary>
		/// update the current user profile
		/// </summary>
		public void UpdateCurrentUserProfile(int? dateOfBirth, string firstName, string lastName, string phoneNumber, int? addressId, string address, string city, int? stateId, string postalCode, string countryCode)
		{
			this.DBHelper.UpdateUserProfile(this.UserContext.UserId, firstName, lastName, this.GetDateTimeFromDays(dateOfBirth), phoneNumber, addressId, address, null, city, stateId, postalCode, countryCode);
		}

        

		///// <summary>
		///// Gets the User for the current user, along with Organizations for each organization the
		///// user is a member of, and InvitationInfos for any invitations for the user.
		///// </summary>
		///// <returns>.</returns>
		//public Tuple<User, List<Organization>, List<InvitationInfo>, Address> GetUserOrgsAndInvitationInfo()
		//{
		//	var spResults = DBHelper.GetUserOrgsAndInvitations(UserContext.UserId);
		//	return Tuple.Create<User, List<Organization>, List<InvitationInfo>, Address>(
		//		InitializeUser(spResults.Item1),
		//		spResults.Item2.Select(odb => (Organization)InitializeOrganization(odb)).ToList(),
		//		spResults.Item3.Select(idb => InitializeInvitationInfo(idb)).ToList(),
		//		InitializeAddress(spResults.Item4));
		//}

		/// <summary>
		/// Gets the user info from an email address.
		/// </summary>
		/// <param name="email">Email address.</param>
		/// <returns>A UserInfo instance with the user's info.</returns>
		public User GetUserByEmail(string email)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException("email");

			return InitializeUser(DBHelper.GetUserByEmail(email));
		}

		/// <summary>
		/// Saves the user info in the database.
		/// </summary>
		/// <param name="model">UserInfo containing updated info.</param>
		public void SaveUserInfo(User model)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model", "UserInfo object must not be null.");
			}

			//// TODO: Add UserInfo->UserDBEntity conversion at bottom
			DBHelper.UpdateUserProfile(model.UserId,
				model.FirstName, model.LastName, model.DateOfBirth, model.PhoneNumber,
				model.Address?.AddressId, model.Address?.Address1, model.Address?.Address2, model.Address?.City, model.Address?.StateId, model.Address?.PostalCode, model.Address?.CountryCode
			);
		}

		/// <summary>
		/// Updates an organization member's info.
		/// </summary>
		/// <param name="modelData">The data from the form that the controller passed in.</param>
		public bool UpdateMember(Dictionary<string, dynamic> modelData)
		{
			if (modelData["userId"] <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			if (modelData["orgId"] < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			if (string.IsNullOrEmpty(modelData["employeeId"]))
			{
				throw new ArgumentNullException("employeeId", "Employee Id must have a value");
			}

			if (modelData["employeeRoleId"] < 0)
			{
				throw new ArgumentOutOfRangeException("employeeRoleId", "Employee Role Id cannot be negative.");
			}

			return DBHelper.UpdateMember(modelData) == 1 ? false : true;
		}

		/// <summary>
		/// Sets the language preference for the current user.
		/// </summary>
		/// <param name="CultureName">The language Id.</param>
		public void SetLanguage(string CultureName)
		{
			if (CultureName == null)
			{
				throw new ArgumentOutOfRangeException("CultureName", "Culture Name cannot be empty.");
			}

			DBHelper.UpdateUserLanguagePreference(UserContext.UserId, CultureName);
		}

		/// <summary>
		/// Gets the browser-compatible universal culture language string (e.g. "en-US") based on language Id.
		/// </summary>
		/// <param name="CultureName">The language Id. May use 0 to indicate no language setting.</param>
		/// <returns>Culture string.</returns>
		public Language GetLanguage(string CultureName)
		{
			if (CultureName == null)
			{
				throw new ArgumentOutOfRangeException("CultureName", "Culture Name cannot be empty.");
			}

			// No language setting, use browser setting. May return null if browser culture is unsupported in this app's database.
			if (CultureName == null)
			{
				return this.ValidLanguages().Where(c => System.Globalization.CultureInfo.CurrentCulture.Name.Equals(c.CultureName)).SingleOrDefault();
			}

			LanguageDBEntity language = DBHelper.GetLanguage(CultureName);
			return new Language
			{
				LanguageName = language.LanguageName,
				CultureName = language.CultureName
			};
		}

		/// <summary>
		/// Sends an email with password reset link to the given email address.
		/// </summary>
		/// <param name="email">The user email address.</param>
		/// <param name="code">The password reset code that is .</param>
		/// <param name="callbackUrl">The Url to include as the "click here" link, with stand-ins for userid and code (as "{userid}" and "{code}".</param>
		/// <returns>A value indicating whether the given email address matched with an existing user.</returns>
		public async Task<bool> SendPasswordResetMessage(string email, string code, string callbackUrl)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException("email");
			if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException("code");
			if (string.IsNullOrWhiteSpace(callbackUrl)) throw new ArgumentNullException("callbackUrl");

			bool result = false;
			int rowsUpdated = this.DBHelper.UpdateUserPasswordResetCode(email, code);
			if (rowsUpdated > 0)
			{
				// Send reset email
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("Please reset your password by clicking <a href=\"{0}\">this reset link</a>.", callbackUrl);
				string msgbody = new System.Web.HtmlString(sb.ToString()).ToString();
				result = await Mailer.SendEmailAsync(this.ServiceSettings.SupportEmail, email, "Reset password", msgbody);
			}

			return result;
		}

		/// <summary>
		/// Reset password. Returns the number of rows updated in the db.
		/// </summary>
		public async Task<int> ResetPassword(Guid code, string password)
		{
			if (code == null) throw new ArgumentNullException("code");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password");

			int result = 0;
			await Task.Run(() =>
			{
				// update password for user and reset password code to null
				result = this.DBHelper.UpdateUserPasswordUsingCode(Crypto.GetPasswordHash(password), code);
			});

			return result;
		}

		/// <summary>
		/// Changes a user's password.
		/// </summary>
		/// <param name="oldPassword">Old password, for verification.</param>
		/// <param name="newPassword">New password to change it to.</param>
		/// <returns>True for a successful change, false if anything fails.</returns>
		public bool ChangePassword(string oldPassword, string newPassword)
		{
			if (string.IsNullOrWhiteSpace(oldPassword)) throw new ArgumentNullException("oldPassword");
			if (string.IsNullOrWhiteSpace(newPassword)) throw new ArgumentNullException("newPassword");

			bool result = false;
			string passwordHash = this.DBHelper.GetPasswordHashById(UserContext.UserId);
			if (!string.IsNullOrWhiteSpace(passwordHash))
			{
				Tuple<bool, string> validation = Crypto.ValidateAndUpdate(oldPassword, passwordHash);
				if (validation.Item1)
				{
					// old password is correct.
					result = true;
					this.DBHelper.UpdateUserPassword(UserContext.UserId, Crypto.GetPasswordHash(newPassword));
				}
			}

			return result;
		}

		/// <summary>
		/// Gets a password hash.
		/// </summary>
		/// <param name = "password" >The password entered by user.</param>
		/// <returns>Password hash.</returns>
		public string GetPasswordHash(string password)
		{
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password");
			return Crypto.GetPasswordHash(password);
		}

		/// <summary>
		/// Confirms the users email.
		/// </summary>
		public bool ConfirmUserEmail(Guid code)
		{
			if (code == null) throw new ArgumentNullException("code");
			return DBHelper.UpdateEmailConfirmed(code) > 0 ? true : false;
		}

		/// <summary>
		/// Gets a list of <see cref="Organization"/>s for all organizations the user is a part of.
		/// </summary>
		/// <returns>Collection of Organizations.</returns>
		public IEnumerable<Organization> GetOrganizationsByUserId()
		{
			return GetOrganizationsByUserId(UserContext.UserId);
		}

		public IEnumerable<Organization> GetOrganizationsByUserId(int userID)
		{
			return DBHelper.GetOrganizationsByUserId(userID).Select(o => (Organization)InitializeOrganization(o));
		}

		#endregion public

		#region Info-DBEntity Conversions

		/// <summary>
		/// Translates a UserDBEntity into a User business object.
		/// </summary>
		/// <param name="user">UserDBEntity instance.</param>
		/// <param name="loadAddress"></param>
		/// <returns>User instance.</returns>
		public User InitializeUser(UserDBEntity user, bool loadAddress = true)
		{
			if (user == null)
			{
				return null;
			}
			Address address = null;
			if (user.AddressId != null && loadAddress)
			{
				address = getAddress(user.AddressId);
			}
			return new User
			{
				AccessFailedCount = user.AccessFailedCount,
				DateOfBirth = user.DateOfBirth,
				Email = user.Email,
				IsEmailConfirmed = user.IsEmailConfirmed,
				FirstName = user.FirstName,
				LastName = user.LastName,
				IsLockoutEnabled = user.IsLockoutEnabled,
				LockoutEndDateUtc = user.LockoutEndDateUtc,
				PasswordHash = user.PasswordHash,
				PasswordResetCode = user.PasswordResetCode,
				PhoneExtension = user.PhoneExtension,
				PhoneNumber = user.PhoneNumber,
				IsPhoneNumberConfirmed = user.IsPhoneNumberConfirmed,
				IsTwoFactorEnabled = user.IsTwoFactorEnabled,
				UserId = user.UserId,
				Address = address,
			};
		}

        /// <summary>
        /// Initialzie user from dynamic object 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static User InitializeUser(dynamic userInfo)
        {
            Address address = InitializeAddress((dynamic)userInfo);

            return new User
            {
                AccessFailedCount = userInfo.AccessFailedCount,
                DateOfBirth = userInfo.DateOfBirth,
                Email = userInfo.Email,
                IsEmailConfirmed = userInfo.IsEmailConfirmed,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                IsLockoutEnabled = userInfo.IsLockoutEnabled,
                LockoutEndDateUtc = userInfo.LockoutEndDateUtc,
                PasswordHash = userInfo.PasswordHash,
                PasswordResetCode = userInfo.PasswordResetCode,
                PhoneExtension = userInfo.PhoneExtension,
                PhoneNumber = userInfo.PhoneNumber,
                IsPhoneNumberConfirmed = userInfo.IsPhoneNumberConfirmed,
                IsTwoFactorEnabled = userInfo.IsTwoFactorEnabled,
                UserId = userInfo.UserId,
                Address = address,
            };
        }
        /// <summary>
        /// Translates a User into a UserDBEntity.
        /// </summary>
        /// <param name="user">User instance.</param>
        /// <returns>UserDBEntity instance.</returns>
        public UserDBEntity GetDBEntityFromUser(User user)
		{
			return new UserDBEntity()
			{
				AddressId = user.Address?.AddressId,
				AccessFailedCount = user.AccessFailedCount,
				DateOfBirth = user.DateOfBirth,
				Email = user.Email,
				IsEmailConfirmed = user.IsEmailConfirmed,
				FirstName = user.FirstName,
				LastName = user.LastName,
				IsLockoutEnabled = user.IsLockoutEnabled,
				LockoutEndDateUtc = user.LockoutEndDateUtc,
				PasswordHash = user.PasswordHash,
				PasswordResetCode = user.PasswordResetCode,
				PhoneExtension = user.PhoneExtension,
				PhoneNumber = user.PhoneNumber,
				IsPhoneNumberConfirmed = user.IsPhoneNumberConfirmed,
				IsTwoFactorEnabled = user.IsTwoFactorEnabled,
				UserId = user.UserId,
				PreferredLanguageId = "en-US"          // TODO: Put this into UserInfo and do proper lookup
			};
		}

		/// <summary>
		/// Translates a <see cref="UserRolesDBEntity"/> into a <see cref="UserRolesInfo"/>.
		/// </summary>
		/// <param name="userRoles">UserRolesDBEntity instance.</param>
		/// <returns>UserRolesInfo instance.</returns>
		public UserRolesInfo InitializeUserRolesInfo(UserRolesDBEntity userRoles)
		{
			if (userRoles == null)
			{
				return null;
			}

			return new UserRolesInfo
			{
				Email = userRoles.Email,
				FirstName = userRoles.FirstName,
				LastName = userRoles.LastName,
				Name = userRoles.Name,
				OrganizationRoleId = userRoles.OrganizationRoleId,
				ProductRoleId = userRoles.ProductRoleId == null ? -1 : userRoles.ProductRoleId.Value,
				SubscriptionId = userRoles.SubscriptionId == null ? -1 : userRoles.SubscriptionId.Value,
				UserId = userRoles.UserId
			};
		}

		/// <summary>
		/// Translates a <see cref="SubscriptionUserDBEntity"/> into a <see cref="SubscriptionUserInfo"/>"/>.
		/// </summary>
		/// <param name="subUser">SubscriptionUserDBEntity instance.</param>
		/// <returns>SubscriptionUserInfo instance.</returns>
		public SubscriptionUserInfo InitializeSubscriptionUserInfo(SubscriptionUserDBEntity subUser)
		{
			if (subUser == null)
			{
				return null;
			}

			return new SubscriptionUserInfo
			{
				FirstName = subUser.FirstName,
				LastName = subUser.LastName,
				CreatedUtc = subUser.CreatedUtc,
				SubscriptionId = subUser.SubscriptionId,
				UserId = subUser.UserId
			};
		}

		#endregion Info-DBEntity Conversions
	}
}
