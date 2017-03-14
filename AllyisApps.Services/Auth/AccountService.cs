//------------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Shared;
using AllyisApps.Lib;
using AllyisApps.Services.Billing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all account related operations.
	/// </summary>
	public partial class Service : BaseService
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

		/// <summary>
		/// Verifies an email address format.
		/// </summary>
		/// <param name="email">The email address.</param>
		/// <returns>True if it is a valid format, false if not.</returns>
		public static bool IsEmailAddressValid(string email)
		{
			Regex emailVerification = new Regex("^[a-zA-Z0-9!#$%^&*\\-'/=+?_{|}~`.]+@[a-zA-Z0-9!#$%^&*\\-'/=+?_{|}~`.]+\\.[a-zA-Z]+$");
			return emailVerification.IsMatch(email);
		}

		/// <summary>
		/// Verfies a url format for a web address (http or https).
		/// </summary>
		/// <param name="url">The url.</param>
		/// <returns>True if it is a valid web url, false if not.</returns>
		public static bool IsUrlValid(string url)
		{
			Uri result;
			return Uri.TryCreate(url, UriKind.Absolute, out result) && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
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
			if (string.IsNullOrEmpty(countryName))
			{
				throw new ArgumentNullException("countryName", "Country name must have a value.");
			}

			return DBHelper.ValidStates(countryName);
		}

		/// <summary>
		/// Gets the list of valid countries.
		/// </summary>
		/// <returns>A collection of valid countries.</returns>
		public IEnumerable<string> ValidCountries()
		{
			return DBHelper.ValidCountries();
		}

		/// <summary>
		/// Gets the list of valid languages selections.
		/// </summary>
		/// <returns>A list of languages.</returns>
		public IEnumerable<Language> ValidLanguages()
		{
			return DBHelper.ValidLanguages().Select(s => new Language
			{
				LanguageID = s.LanguageID,
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

			if (!Service.IsEmailAddressValid(userEmail))
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
		/// <param name="invite">The invite.</param>
		/// <returns>The resulting action message.</returns>
		public string AcceptUserInvitation(InvitationInfo invite)
		{
			#region Validation

			if (invite == null)
			{
				throw new ArgumentOutOfRangeException("invite", "The invitation is null.");
			}

			#endregion Validation

			var user = this.GetUserByEmail(invite.Email);
			if (invite.ProjectId.HasValue)
			{
				this.DBHelper.CreateProjectUser(invite.ProjectId.Value, user.UserId);
			}

			this.DBHelper.CreateOrganizationUser(new OrganizationUserDBEntity() // ...add them to that organization as their organization role.
			{
				UserId = user.UserId,
				OrganizationId = invite.OrganizationId,
				OrgRoleId = invite.OrgRole,
				EmployeeId = invite.EmployeeId
			});

			IEnumerable<InvitationSubRole> roles = this.DBHelper.GetInvitationSubRolesByInvitationId(invite.InvitationId).Select(i => InitializeInvitationSubRole(i));
			IEnumerable<SubscriptionDisplayInfo> subs = this.DBHelper.GetSubscriptionsDisplayByOrg(invite.OrganizationId).Select(s => InitializeSubscriptionDisplayInfo(s));

			foreach (InvitationSubRole role in roles)
			{
				SubscriptionDisplayInfo currentSub = subs.Where(x => x.SubscriptionId == role.SubscriptionId).SingleOrDefault();
				if (currentSub != null && currentSub.SubscriptionsUsed < currentSub.NumberOfUsers)
				{
					this.DBHelper.UpdateSubscriptionUserProductRole(role.ProductRoleId, role.SubscriptionId, user.UserId);
				}
			}

			this.DBHelper.RemoveUserInvitation(invite.InvitationId);
			return string.Format(
				"You have successfully joined {0} as a{1}{2}.",
				DBHelper.GetOrganization(invite.OrganizationId).Name,
				invite.OrgRole == (int)OrganizationRole.Member ? " " : "n ",
				OrganizationRole.Member.ToString());
		}

		/// <summary>
		/// Rejects an invitation, deleting it from a user's pending invites.
		/// </summary>
		/// <param name="invite">The invitation to reject.</param>
		/// <returns>The resulting message.</returns>
		public string RejectUserInvitation(InvitationInfo invite)
		{
			this.DBHelper.RemoveUserInvitation(invite.InvitationId);
			return string.Format(
				"The invitation to join {0} has been rejected.",
				DBHelper.GetOrganization(invite.OrganizationId).Name);
		}		

		/// <summary>
		/// Setup a new user.
		/// </summary>
		/// <param name="email">The new user's email.</param>
		/// <param name="firstName">The new user's first name.</param>
		/// <param name="lastName">The new user's last name.</param>
		/// <param name="dateOfBirth">The new user's date of birth.</param>
		/// <param name="address">The new user's street address.</param>
		/// <param name="city">The new user's city portion of their address.</param>
		/// <param name="state">The new user's state portion of their address.</param>
		/// <param name="country">The new user's country portion of their address.</param>
		/// <param name="postalCode">The new user's postal code.</param>
		/// <param name="phone">The new user's phone number.</param>
		/// <param name="password">The new user's password.</param>
		/// <param name="languagePreference">The new user's language preference.</param>
		/// <param name="emailConfirmed">A boolean representing whether the new user's email has been confirmed.  Defaults to false.</param>
		/// <param name="accessFailedCount">A count of the number of times the user has failed to access their account. Defaults to 0.</param>
		/// <param name="twoFactorEnabled">A boolean representing whether the new user has enabled 2FA. Defaults to false.</param>
		/// <param name="lockOutEnabled">A boolean representing whether the new user is locked out from failed access attempts.  Defaults to false.</param>
		/// <param name="lockOutEndDateUtc">The time whent the user's lockout will end.  Defaults to null.</param>
		/// <returns>Returns 0 if user already exists, 0 if there is failure.</returns>
		public int SetupNewUser(
			string email,
			string firstName,
			string lastName,
			DateTime? dateOfBirth,
			string address,
			string city,
			string state,
			string country,
			string postalCode,
			string phone,
			string password,
			int languagePreference,
			bool emailConfirmed = false,
			int accessFailedCount = 0,
			bool twoFactorEnabled = false,
			bool lockOutEnabled = false,
			DateTime? lockOutEndDateUtc = null)
		{
			#region Validation

			if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
			{
				throw new ArgumentException("User name must have a value.");
			}

			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException("email", "Email address must have a value.");
			}
			else if (!Service.IsEmailAddressValid(email))
			{
				throw new FormatException("Email address must be in a valid format.");
			}

			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentNullException("password", "Password must have a value.");
			}

			#endregion Validation

			int result = 0;
			try
			{
				UserDBEntity entity = new UserDBEntity()
				{
					Email = email,
					FirstName = firstName,
					LastName = lastName,
					DateOfBirth = dateOfBirth,
					Address = address,
					City = city,
					State = state,
					Country = country,
					PostalCode = postalCode,
					PhoneNumber = phone,
					PasswordHash = Crypto.GetPasswordHash(password), //Crypto.ComputeSHA512Hash(password),
					EmailConfirmed = emailConfirmed,
					AccessFailedCount = accessFailedCount,
					TwoFactorEnabled = twoFactorEnabled,
					LockoutEnabled = lockOutEnabled,
					LockoutEndDateUtc = lockOutEndDateUtc,
					LanguagePreference = languagePreference
				};

				result = this.DBHelper.CreateUser(entity);
			}
			catch (SqlException ex)
			{
				if (ex.Message.ToLower().Contains("unique"))
				{
					// unique constraint violation of email
					result = 0;
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
		/// <returns>The  login task.</returns>
		public UserContext ValidateLogin(string email, string password)
		{
			#region Validation

			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException("email", "Email address must have a value.");
			}
			else if (!Service.IsEmailAddressValid(email))
			{
				throw new FormatException("Email address must be in a valid format.");
			}

			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentNullException("password", "Password must have a value.");
			}

			#endregion Validation

			UserContext result = null;
			var user = this.DBHelper.GetUserByEmail(email);
			Tuple<bool, string> passwordValidation = Crypto.ValidateAndUpdate(password, user.PasswordHash);
			if (user != null && passwordValidation.Item1/*string.Compare(Crypto.ComputeSHA512Hash(password), user.PasswordHash, true) == 0*/)
			{
				result = new UserContext(user.UserId, user.UserName, email);

				// Store updated password hash if needed
				if (passwordValidation.Item2 != null)
				{
					DBHelper.UpdateUserPassword(user.UserId, passwordValidation.Item2);
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
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User ID cannot be 0 or negative.");
			}

			UserContext result = null;

			List<UserContextDBEntity> contextInfo = this.DBHelper.GetUserContextInfo(userId);
			if (contextInfo.Count > 0) // Count will be 0 if the user is not part of any organizations.
			{
				UserContextDBEntity firstRow = contextInfo[0];
				result = new UserContext(userId, firstRow.UserName, firstRow.Email, firstRow.ActiveOrganizationId == null ? 0 : firstRow.ActiveOrganizationId.Value, firstRow.LastSubscriptionId == null ? 0 : firstRow.LastSubscriptionId.Value);
				result.ChosenLanguageID = firstRow.LanguagePreference.Value;
				result.UserOrganizationInfoList = new List<UserOrganizationInfo>();
				foreach (UserContextDBEntity row in contextInfo)
				{
					// Get the org for this row
					if (row.OrganizationId != null)
					{
						UserOrganizationInfo orgInfo = result.UserOrganizationInfoList.Where(o => o.OrganizationId == row.OrganizationId.Value).FirstOrDefault();

						// If the org for this row isn't added yet, add it
						if (orgInfo == null)
						{
							orgInfo = new UserOrganizationInfo
							{
								OrganizationId = row.OrganizationId.Value,
								OrganizationName = row.OrganizationName,
								OrganizationRole = (OrganizationRole)row.OrgRoleId.Value,
								UserSubscriptionInfoList = new List<UserSubscriptionInfo>()
							};
							result.UserOrganizationInfoList.Add(orgInfo);
						}

						// If there is subscription info on this line, at it to the org
						if (row.SubscriptionId != null)
						{
							orgInfo.UserSubscriptionInfoList.Add(new UserSubscriptionInfo
							{
								SubscriptionId = row.SubscriptionId.Value,
								ProductId = (ProductIdEnum)row.ProductId.Value,
								ProductName = row.ProductName,
								RoleName  = row.RoleName,
								ProductRole = (ProductRole)row.ProductRoleId.Value,
								SkuId = row.SkuId.Value
							});
						}
					}
				}

				if (!result.UserOrganizationInfoList.Exists(x => x.OrganizationId == result.ChosenOrganizationId))
				{
					result.ChosenOrganizationId = 0;
				}

				if (result.UserOrganizationInfoList.Count == 1)
				{
					// if there is only one org, make that their chosen
					result.ChosenOrganizationId = result.UserOrganizationInfoList.First().OrganizationId;
				}
			}

			this.SetUserContext(result);
			return result;
		}

		/// <summary>
		/// Gets the user info for the current user.
		/// </summary>
		/// <returns>A User instance with the current user's info.</returns>
		public User GetUser()
		{
			return GetUser(UserContext.UserId);
		}

		/// <summary>
		/// Gets the User for the current user, along with Organizations for each organization the
		/// user is a member of, and InvitationInfos for any invitations for the user.
		/// </summary>
		/// <returns></returns>
		public Tuple<User, List<Organization>, List<InvitationInfo>> GetUserOrgsAndInvitationInfo()
		{
			var spResults = DBHelper.GetUserOrgsAndInvitations(UserContext.UserId);
			return Tuple.Create<User, List<Organization>, List<InvitationInfo>>(
				InitializeUser(spResults.Item1),
				spResults.Item2.Select(odb => InitializeOrganization(odb)).ToList(),
				spResults.Item3.Select(idb => InitializeInvitationInfo(idb)).ToList());
		}

		/// <summary>
		/// Gets the user info for a specific user.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <returns>A User instance with the current user's info.</returns>
		public User GetUser(int userId)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User ID cannot be 0 or negative.");
			}

			return InitializeUser(DBHelper.GetUserInfo(userId));
		}

		/// <summary>
		/// Gets the user info from an email address.
		/// </summary>
		/// <param name="email">Email address.</param>
		/// <returns>A UserInfo instance with the user's info.</returns>
		public User GetUserByEmail(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException("email", "Email address must have a value.");
			}
			else if (!Service.IsEmailAddressValid(email))
			{
				throw new FormatException("Email address must be in a valid format.");
			}

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

			// TODO: Add UserInfo->UserDBEntity conversion at bottom
			DBHelper.UpdateUser(new UserDBEntity
			{
				AccessFailedCount = model.AccessFailedCount,
				ActiveOrganizationId = model.ActiveOrganizationId,
				Address = model.Address,
				City = model.City,
				Country = model.Country,
				DateOfBirth = model.DateOfBirth,
				Email = model.Email,
				EmailConfirmed = model.EmailConfirmed,
				FirstName = model.FirstName,
				LastName = model.LastName,
				LastSubscriptionId = model.LastSubscriptionId,
				LockoutEnabled = model.LockoutEnabled,
				LockoutEndDateUtc = model.LockoutEndDateUtc,
				PasswordHash = model.PasswordHash,
				PasswordResetCode = model.PasswordResetCode,
				PhoneExtension = model.PhoneExtension,
				PhoneNumber = model.PhoneNumber,
				PhoneNumberConfirmed = model.PhoneNumberConfirmed,
				State = model.State,
				TwoFactorEnabled = model.TwoFactorEnabled,
				UserId = model.UserId,
				UserName = model.UserName,
				PostalCode = model.PostalCode
			});
		}

		/// <summary>
		/// Sets the language preference for the current user.
		/// </summary>
		/// <param name="languageID">The language ID.</param>
		public void SetLanguage(int languageID)
		{
			if (languageID < 0)
			{
				throw new ArgumentOutOfRangeException("languageID", "Language ID cannot be negative.");
			}

			DBHelper.UpdateUserLanguagePreference(UserContext.UserId, languageID);
		}

		/// <summary>
		/// Gets the browser-compatible universal culture language string (e.g. "en-US") based on language ID.
		/// </summary>
		/// <param name="languageID">The language ID. May use 0 to indicate no language setting.</param>
		/// <returns>Culture string.</returns>
		public Language GetLanguage(int languageID)
		{
			if (languageID < 0)
			{
				throw new ArgumentOutOfRangeException("languageID", "Language ID cannot be negative.");
			}

			// No language setting, use browser setting. May return null if browser culture is unsupported in this app's database.
			if (languageID == 0)
			{
				return this.ValidLanguages().Where(c => System.Globalization.CultureInfo.CurrentCulture.Name.Equals(c.CultureName)).SingleOrDefault();
			}

			LanguageDBEntity language = DBHelper.GetLanguage(languageID);
			return new Language
			{
				LanguageID = language.LanguageID,
				LanguageName = language.LanguageName,
				CultureName = language.CultureName
			};
		}

		/// <summary>
		/// Sends an email with password reset link to the given email address.
		/// </summary>
		/// <param name="email">The user email address.</param>
		/// <param name="callbackUrl">The Url to include as the "click here" link, with stand-ins for userid and code (as "{userid}" and "{code}".</param>
		/// <returns>A value indicating whether the given email address matched with an existing user.</returns>
		public async Task<bool> SendPasswordResetMessage(string email, string callbackUrl)
		{
			#region Validation
			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException("email", "Email address must have a value.");
			}
			else if (!Service.IsEmailAddressValid(email))
			{
				throw new FormatException("Email address must be in a valid format.");
			}
			#endregion

			var user = this.DBHelper.GetUserByEmail(email);
			if (user == null)
			{
				return false;
			}

			Guid code = Guid.NewGuid();
			this.DBHelper.UpdateUserPasswordResetCode(user.UserId, code.ToString());

			// Send reset email
			string filledInCallbackUrl = callbackUrl.Replace("{userid}", user.UserId.ToString()).Replace("{code}", code.ToString());
			string msgbody = new System.Web.HtmlString(string.Format("Please reset your password by clicking <a href=\"{0}\">here</a>", filledInCallbackUrl)).ToString();
			await Lib.Mailer.SendEmailAsync("noreply@allyisapps.com", email, "Reset password", msgbody);

			return true;
		}

		/// <summary>
		/// Reset password.
		/// </summary>
		/// <param name="userId">The user ID.</param>
		/// <param name="code">The code.</param>
		/// <param name="password">The password.</param>
		/// <returns>The reset password task.</returns>
		public async Task<bool> ResetPassword(int userId, string code, string password)
		{
			#region Validation

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User ID cannot be 0 or negative.");
			}

			if (string.IsNullOrEmpty(code))
			{
				throw new ArgumentNullException("code", "Code must have a value.");
			}
			else
			{
				Guid guidOutput;
				if (!Guid.TryParse(code, out guidOutput))
				{
					throw new ArgumentException("Code must be a valid Guid.");
				}
			}

			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentNullException("password", "Password must have a value.");
			}

			#endregion Validation

			return await Task<bool>.Run(() =>
			{
				// hash the password
				string passwordHash = GetPasswordHash(password);

				// update password for user and reset password code to null
				return this.DBHelper.UpdateUserPasswordUsingCode(userId, passwordHash, code) == userId;
			});
		}

		/// <summary>
		/// Changes a user's password.
		/// </summary>
		/// <param name="oldPassword">Old password, for verification.</param>
		/// <param name="newPassword">New password to change it to.</param>
		/// <returns>True for a successful change, false if anything fails.</returns>
		public bool ChangePassword(string oldPassword, string newPassword)
		{
			if (string.IsNullOrEmpty(oldPassword))
			{
				throw new ArgumentNullException("oldPassword", "Old password must have a value.");
			}
			else if (string.IsNullOrEmpty(newPassword))
			{
				throw new ArgumentNullException("newPassword", "New password must have a value.");
			}

			var userInfo = DBHelper.Instance.GetUserInfo(Convert.ToInt32(UserContext.UserId));
			UserDBEntity user = DBHelper.Instance.GetUserByEmail(userInfo.Email);
			if (user != null && Crypto.ValidatePassword(oldPassword, user.PasswordHash)/*string.Compare(this.GetPasswordHash(oldPassword), user.PasswordHash, true) == 0*/)
			{
				DBHelper.UpdateUserPassword(UserContext.UserId, this.GetPasswordHash(newPassword));

				user = DBHelper.GetUserByEmail(userInfo.Email);

				if (user != null)
				{
					return Crypto.ValidatePassword(newPassword, user.PasswordHash)/*string.Compare(this.GetPasswordHash(newPassword), user.PasswordHash) == 0*/;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets a password hash.
		/// </summary>
		/// <param name = "password" >The password entered by user.</param>
		/// <returns>Password hash.</returns>
		public string GetPasswordHash(string password)
		{
			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentNullException("password", "Password must have a value.");
			}

			return Crypto.GetPasswordHash(password)/*Crypto.ComputeSHA512Hash(password)*/;
		}

		/// <summary>
		/// Send confirmation email after registration.
		/// </summary>
		/// <param name="userId">The user id.</param>
		/// <returns>The async confirmation email code task.</returns>
		public async Task<string> GetConfirmEmailCode(int userId)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User ID cannot be 0 or negative.");
			}

			string result = Guid.NewGuid().ToString();
			await Task.Run(() =>
			{
				return;
			});

			return result;
		}

		/// <summary>
		/// Send confirmation email.
		/// </summary>
		/// <param name="from">Who the message is from.</param>
		/// <param name="to">Who the message is to.</param>
		/// <param name="confirmEmailUrl">The URL for email confirmation.</param>
		/// <returns>The async confirmation email task.</returns>
		public async Task SendConfirmationEmail(string from, string to, string confirmEmailUrl)
		{
			#region Validation

			if (string.IsNullOrEmpty(from))
			{
				throw new ArgumentNullException("from", "From email address must have a value.");
			}
			else if (!Service.IsEmailAddressValid(from))
			{
				throw new FormatException("From email address must be in a valid format.");
			}

			if (string.IsNullOrEmpty(to))
			{
				throw new ArgumentNullException("to", "To email address must have a value.");
			}
			else if (!Service.IsEmailAddressValid(to))
			{
				throw new FormatException("To email address must be in a valid format.");
			}

			if (string.IsNullOrEmpty(confirmEmailUrl))
			{
				throw new ArgumentNullException("confirmEmailUrl", "Confirm email url must have a value.");
			}
			else if (!Service.IsUrlValid(confirmEmailUrl))
			{
				throw new FormatException("Confirm email url must be in a valid format.");
			}

			#endregion Validation

			string bodyHtml = string.Format("Please confirm your email by clicking <a href=\"{0}\">here</a>", confirmEmailUrl);
			await Mailer.SendEmailAsync(from, to, "Confirm Email", bodyHtml);
		}

		/// <summary>
		/// Confirms the users email.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="code">The confirmation code.</param>
		/// <returns>True on success, false if the email is already confirmed or some other failure occurs.</returns>
		public async Task<bool> ConfirmEmailAsync(int userId, string code)
		{
			#region Validation

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("user Id", "User ID cannot be 0 or negative.");
			}

			if (string.IsNullOrEmpty(code))
			{
				throw new ArgumentNullException("code", "Code must have a value.");
			}
			else
			{
				Guid guidOutput;
				if (!Guid.TryParse(code, out guidOutput))
				{
					throw new ArgumentException("Code must be a valid Guid.");
				}
			}

			#endregion Validation

			if (DBHelper.GetUserInfo(userId).EmailConfirmed)
			{
				return false;
			}

			return await Task<bool>.Run(() =>
			{
				// TODO: Adjust the GetUserInfo procedure to grab the confirmation code from the table, or create one that does
				// return DBHelper.GetUserInfo(userId).EmailConfirmationCode == code;
				return true;
			});
		}

		/// <summary>
		/// Gets a list of <see cref="Organization"/>s for all organizations the user is a part of.
		/// </summary>
		/// <returns>Collection of Organizations.</returns>
		public IEnumerable<Organization> GetOrganizationsByUserId()
		{
			return DBHelper.GetOrganizationsByUserId(UserContext.UserId).Select(o => InitializeOrganization(o));
		}

		#endregion public


		#region Info-DBEntity Conversions
		/// <summary>
		/// Translates a UserDBEntity into a User business object.
		/// </summary>
		/// <param name="user">UserDBEntity instance.</param>
		/// <returns>User instance.</returns>
		public static User InitializeUser(UserDBEntity user)
		{
			if (user == null)
			{
				return null;
			}

			return new User
			{
				AccessFailedCount = user.AccessFailedCount,
				ActiveOrganizationId = user.ActiveOrganizationId,
				Address = user.Address,
				City = user.City,
				Country = user.Country,
				DateOfBirth = user.DateOfBirth,
				Email = user.Email,
				EmailConfirmed = user.EmailConfirmed,
				FirstName = user.FirstName,
				LastName = user.LastName,
				LastSubscriptionId = user.LastSubscriptionId,
				LockoutEnabled = user.LockoutEnabled,
				LockoutEndDateUtc = user.LockoutEndDateUtc,
				PasswordHash = user.PasswordHash,
				PasswordResetCode = user.PasswordResetCode,
				PhoneExtension = user.PhoneExtension,
				PhoneNumber = user.PhoneNumber,
				PhoneNumberConfirmed = user.PhoneNumberConfirmed,
				State = user.State,
				TwoFactorEnabled = user.TwoFactorEnabled,
				UserId = user.UserId,
				UserName = user.UserName,
				PostalCode = user.PostalCode
			};
		}

		/// <summary>
		/// Translates a User into a UserDBEntity.
		/// </summary>
		/// <param name="user">User instance.</param>
		/// <returns>UserDBEntity instance.</returns>
		public static UserDBEntity GetDBEntityFromUser(User user)
		{
			if (user == null)
			{
				return null;
			}

			return new UserDBEntity
			{
				AccessFailedCount = user.AccessFailedCount,
				ActiveOrganizationId = user.ActiveOrganizationId,
				Address = user.Address,
				City = user.City,
				Country = user.Country,
				DateOfBirth = user.DateOfBirth,
				Email = user.Email,
				EmailConfirmed = user.EmailConfirmed,
				FirstName = user.FirstName,
				LastName = user.LastName,
				LastSubscriptionId = user.LastSubscriptionId,
				LockoutEnabled = user.LockoutEnabled,
				LockoutEndDateUtc = user.LockoutEndDateUtc,
				PasswordHash = user.PasswordHash,
				PasswordResetCode = user.PasswordResetCode,
				PhoneExtension = user.PhoneExtension,
				PhoneNumber = user.PhoneNumber,
				PhoneNumberConfirmed = user.PhoneNumberConfirmed,
				State = user.State,
				TwoFactorEnabled = user.TwoFactorEnabled,
				UserId = user.UserId,
				UserName = user.UserName,
				PostalCode = user.PostalCode,
				LanguagePreference = 1          // TODO: Put this into UserInfo and do proper lookup
			};
		}

		/// <summary>
		/// Translates a <see cref="UserRolesDBEntity"/> into a <see cref="UserRolesInfo"/>.
		/// </summary>
		/// <param name="userRoles">UserRolesDBEntity instance.</param>
		/// <returns>UserRolesInfo instance.</returns>
		public static UserRolesInfo InitializeUserRolesInfo(UserRolesDBEntity userRoles)
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
				OrgRoleId = userRoles.OrgRoleId,
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
		public static SubscriptionUserInfo InitializeSubscriptionUserInfo(SubscriptionUserDBEntity subUser)
		{
			if (subUser == null)
			{
				return null;
			}

			return new SubscriptionUserInfo
			{
				CreatedUTC = subUser.CreatedUTC,
				FirstName = subUser.FirstName,
				LastName = subUser.LastName,
				ProductRoleId = subUser.ProductRoleId,
				ProductRoleName = subUser.ProductRoleName,
				SkuId = subUser.SkuId,
				SubscriptionId = subUser.SubscriptionId,
				UserId = subUser.UserId
			};
		}
		#endregion
	}
}
