//------------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Lookup;
using AllyisApps.Lib;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Lookup;
using Newtonsoft.Json.Linq;
using AllyisApps.Services.Cache;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all account related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
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

		public bool DeleteExpenseItem(int itemId)
		{
			return this.DBHelper.DeleteExpenseItem(itemId);
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
		/// Gets information for an invite.
		/// </summary>
		/// <param name="invitationId">Id of invite.</param>
		/// <returns>Inviation info </returns>
		public Invitation GetInvitationByID(int invitationId)
		{
			return InitializeInvitationInfo(DBHelper.GetInvitation(invitationId));
		}

		/// <summary>
		/// Accepts an invitation, adding the user to the invitation's organization, subscriptions, and projects, then deletes the invitations.
		/// </summary>
		public bool AcceptUserInvitation(int invitationId)
		{
			if (invitationId <= 0) throw new ArgumentOutOfRangeException("invitationId");

			Invitation inviteInfo = InitializeInvitationInfo(this.DBHelper.GetInvitation(invitationId));
			var result = (this.DBHelper.AcceptInvitation(invitationId, this.UserContext.UserId) == 1);
			if (result)
			{
				if (inviteInfo.ProductRolesJson != null)
				{
					JObject roleString = JObject.Parse(inviteInfo.ProductRolesJson);

					var ttRole = roleString[((int)SkuIdEnum.TimeTrackerBasic).ToString()].Value<int>();
					var etRole = roleString[((int)SkuIdEnum.ExpenseTrackerBasic).ToString()].Value<int>();
					var smRole = roleString[((int)SkuIdEnum.StaffingManagerBasic).ToString()].Value<int>();


					if (ttRole > 0)
					{
						var result2 = (this.DBHelper.UpdateSubscriptionUserRoles(new List<int>() { UserContext.UserId }, inviteInfo.OrganizationId, ttRole, (int)ProductIdEnum.TimeTracker));
					}

					if (etRole > 0)
					{
						var prodId = GetProductSubscriptionInfo(inviteInfo.OrganizationId, SkuIdEnum.ExpenseTrackerBasic).Product.ProductId;
						var result3 = (this.DBHelper.UpdateSubscriptionUserRoles(new List<int>() { UserContext.UserId }, inviteInfo.OrganizationId, etRole, (int)ProductIdEnum.ExpenseTracker));
					}

					if (smRole > 0)
					{
						var prodId = GetProductSubscriptionInfo(inviteInfo.OrganizationId, SkuIdEnum.StaffingManagerBasic).Product.ProductId;
						var result4 = (this.DBHelper.UpdateSubscriptionUserRoles(new List<int>() { UserContext.UserId }, inviteInfo.OrganizationId, smRole, (int)ProductIdEnum.StaffingManager));
					}

				}
				NotifyInviteAcceptAsync(invitationId);
			}
			return result;
		}

		/// <summary>
		/// Rejects an invitation, deleting it from a user's pending invites.
		/// </summary>
		/// <param name="invitationId">The id of the invitation to reject.</param>
		/// <returns>The resulting message.</returns>
		public bool RejectInvitation(int invitationId)
		{
			bool rejected = (DBHelper.RejectInvitation(invitationId) == 1);
			if (rejected)
			{
				NotifyInviteRejectAsync(invitationId);
			}

			return rejected;
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
		public User ValidateLogin(string email, string password)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException("email");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password");

			User result = null;
			result = InitializeUser(this.DBHelper.GetUserByEmail(email), false);

			if (result != null)
			{
				// email exists, hash the given password and compare with hash in db
				PassWordValidationResult passwordValidation = Crypto.ValidateAndUpdate(password, result.PasswordHash);
				if (passwordValidation.successfulMatch)
				{
					// Store updated password hash if needed
					if (passwordValidation.updatedHash != null)
					{
						DBHelper.UpdateUserPassword(result.UserId, passwordValidation.updatedHash);
					}
				}
				else
				{
					return null;
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
					result.OrganizationsAndRoles.Add(item.OrganizationId, new UserContext.OrganizationAndRole()
					{
						OrganizationId = item.OrganizationId,
						OrganizationRole = (OrganizationRole)item.OrganizationRoleId,
						MaxAmount = item.MaxAmount ?? 0
					});
				}

				// get subscriptions and roles
				foreach (var item in expando.SubscriptionsAndRoles)
				{
					result.SubscriptionsAndRoles.Add(item.SubscriptionId,
						new UserContext.SubscriptionAndRole()
						{
							AreaUrl = item.AreaUrl,
							ProductId = (ProductIdEnum)item.ProductId,
							ProductRoleId = item.ProductRoleId,
							SkuId = (SkuIdEnum)item.SkuId,
							SubscriptionId = item.SubscriptionId,
							OrganizationId = item.OrganizationId
						});
				}

				// set result to self
				this.SetUserContext(result);
			}

			return result;
		}

		/// <summary>
		/// get the current logged in user
		/// </summary>
		public User GetCurrentUser()
		{
			return this.GetUser(this.UserContext.UserId);
		}

		/// <summary>
		/// Get User Address, Organizaiontins, and Inviations for account page
		/// </summary>
		public User GetUser(int userId)
		{
			if (userId <= 0) throw new ArgumentOutOfRangeException("userId");

			dynamic infos = this.DBHelper.GetUser(userId);
			User userInfo = this.InitializeUser(infos.User);
			IEnumerable<dynamic> Organizations = infos.Organizations;
			IEnumerable<dynamic> Subscriptions = infos.Subscriptions;

			userInfo.Subscriptions = Subscriptions.Select(sub =>
				new UserSubscription()
				{
					AreaUrl = sub.AreaUrl,
					OrganizationId = sub.OrganizationId,
					ProductId = (ProductIdEnum)sub.ProductId,
					ProductName = sub.ProductName,
					SkuId = (SkuIdEnum)sub.SkuId,
					SubscriptionId = sub.SubscriptionId,
					SubscriptionName = sub.SubscriptionName,
					ProductRole = new ProductRole()
					{
						ProductRoleId = sub.ProductRoleId,
						ProductId = (ProductIdEnum)sub.ProductId
					},
					UserId = userId,
					IconUrl = sub.IconUrl
				}
			).ToList();

			userInfo.Organizations = Organizations.Select(
				org =>
					new UserOrganization()
					{
						Organization = InitializeOrganization(org),
						OrganizationRole = (OrganizationRole)org.OrganizationRoleId,
						UserId = userId,
					}
			).ToList();

			IEnumerable<dynamic> Invitations = infos.Invitations;
			userInfo.Invitations = Invitations.Select(
				inv =>
					new Invitation()
					{
						Email = inv.Email,
						EmployeeId = inv.EmployeeId,
						FirstName = inv.FirstName,
						LastName = inv.LastName,
						InvitationId = inv.InvitationId,
						OrganizationId = inv.OrganizationId,
					}
				).ToList();
			return userInfo;
		}

		/// <summary>
		/// update the current user profile
		/// </summary>
		public void UpdateCurrentUserProfile(int? dateOfBirth, string firstName, string lastName, string phoneNumber, int? addressId, string address, string city, int? stateId, string postalCode, string countryCode)
		{
			this.DBHelper.UpdateUserProfile(this.UserContext.UserId, firstName, lastName, Utility.GetNullableDateTimeFromDays(dateOfBirth), phoneNumber, addressId, address, null, city, stateId, postalCode, countryCode);
		}

		/// <summary>
		/// update the current user profile
		/// </summary>
		public void UpdateUserProfile(int userId, int? dateOfBirth, string firstName, string lastName, string phoneNumber, int? addressId, string address, string city, int? stateId, string postalCode, string countryCode)
		{
			this.DBHelper.UpdateUserProfile(userId, firstName, lastName, Utility.GetNullableDateTimeFromDays(dateOfBirth), phoneNumber, addressId, address, null, city, stateId, postalCode, countryCode);
		}

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
		/// Updates an organization member's info.
		/// </summary>
		public bool UpdateMember(int userId, int orgId, string employeeId, int roleId, string firstName, string lastName, bool isInvited)
		{
			if (userId <= 0) throw new ArgumentOutOfRangeException("userId");
			if (orgId <= 0) throw new ArgumentOutOfRangeException("orgId");
			if (string.IsNullOrWhiteSpace(employeeId)) throw new ArgumentNullException("employeeId");
			if (roleId <= 0) throw new ArgumentOutOfRangeException("roleId");

			return DBHelper.UpdateMember(userId, orgId, employeeId, roleId, firstName, lastName, isInvited) == 1 ? true : false;
		}

		/// <summary>
		/// Sets the language preference for the current user.
		/// </summary>
		public void SetLanguage(string cultureName)
		{
			if (string.IsNullOrWhiteSpace(cultureName)) throw new ArgumentNullException("cultureName");

			this.DBHelper.UpdateUserLanguagePreference(UserContext.UserId, cultureName);
		}

		/// <summary>
		/// Gets the browser-compatible universal culture language string (e.g. "en-US") based on language Id.
		/// </summary>
		public Language GetLanguage(string cultureName)
		{
			if (string.IsNullOrWhiteSpace(cultureName))
			{
				cultureName = System.Globalization.CultureInfo.CurrentCulture.Name;
			}

			LanguageDBEntity language = this.DBHelper.GetLanguage(cultureName);
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
				PassWordValidationResult validation = Crypto.ValidateAndUpdate(oldPassword, passwordHash);
				if (validation.successfulMatch)
				{
					// old password is correct.
					result = true;
					this.DBHelper.UpdateUserPassword(UserContext.UserId, Crypto.GetPasswordHash(newPassword));
				}
			}

			return result;
		}

		/// <summary>
		/// Confirms the users email.
		/// </summary>
		public bool ConfirmUserEmail(Guid code)
		{
			if (code == null) throw new ArgumentNullException("code");
			return DBHelper.UpdateEmailConfirmed(code) == 1 ? true : false;
		}

		public IEnumerable<Organization> GetOrganizationsByUserId(int userID)
		{
			return DBHelper.GetOrganizationsByUserId(userID).Select(o => (Organization)InitializeOrganization(o));
		}

		private User InitializeUser(dynamic user)
		{
			User newUser = new User()
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
				Address = InitializeAddress(user)
			};
			return newUser;
		}

		/// <summary>
		/// Translates a UserDBEntity into a User business object.
		/// </summary>
		/// <param name="user">UserDBEntity instance.</param>
		/// <param name="loadAddress"></param>
		/// <returns>User instance.</returns>
		private User InitializeUser(UserDBEntity user, bool loadAddress = true)
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
				Address = address
			};
		}

		/// <summary>
		/// Translates a <see cref="UserRolesDBEntity"/> into a <see cref="UserRole"/>.
		/// </summary>
		/// <param name="userRoles">UserRolesDBEntity instance.</param>
		/// <returns>UserRole instance.</returns>
		private UserRole InitializeUserRole(UserRolesDBEntity userRoles)
		{
			if (userRoles == null)
			{
				return null;
			}

			return new UserRole
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
		/// Translates a <see cref="SubscriptionUserDBEntity"/> into a <see cref="SubscriptionUser"/>"/>.
		/// </summary>
		/// <param name="subUser">SubscriptionUserDBEntity instance.</param>
		/// <returns>SubscriptionUser instance.</returns>
		public SubscriptionUser InitializeSubscriptionUser(SubscriptionUserDBEntity subUser)
		{
			if (subUser == null)
			{
				return null;
			}

			return new SubscriptionUser
			{
				FirstName = subUser.FirstName,
				LastName = subUser.LastName,
				CreatedUtc = subUser.CreatedUtc,
				SubscriptionId = subUser.SubscriptionId,
				UserId = subUser.UserId,
				ProductRoleId = subUser.ProductRoleId,
				Email = subUser.Email
			};
		}

		public void UpdateUserOrgMaxAmount(OrganizationUser userInfo)
		{
			OrganizationUserDBEntity entity = new OrganizationUserDBEntity()
			{
				UserId = userInfo.UserId,
				OrganizationId = userInfo.OrganizationId
			};
			DBHelper.UpdateUserMaxAmount(entity);
		}

		public decimal GetOrganizationUserMaxAmount(int userId, int orgId)
		{
			return DBHelper.GetUserOrgMaxAmount(userId, orgId);
		}

		/// <summary>
		/// constructs the next unique employee id to be added to an invitation
		/// </summary>
		public async Task<string> GetNextEmployeeId(int organizationId)
		{
			if (organizationId <= 0) throw new ArgumentOutOfRangeException("organizationId");

			string maxId = await this.DBHelper.GetMaxEmployeeId(organizationId);
			char[] idchars = maxId.ToCharArray();

			// define legal characters
			var characters = new List<char>();
			for (char c = '0'; c <= '9'; c++) characters.Add(c); // Add numeric characters first
			for (char c = 'A'; c <= 'Z'; c++) characters.Add(c); // Add upper-case next
			for (char c = 'a'; c <= 'z'; c++) characters.Add(c); // Add lower-case last

			// increment the string
			for (int i = maxId.Length - 1; i >= 0; --i)
			{
				if (idchars[i] == characters[characters.Count - 1])
				{
					// If last value, round it to the first one and continue the loop to the next index
					idchars[i] = characters[0];
				}
				else
				{
					// The value can simply be incremented, so break out of the loop
					idchars[i] = characters[characters.IndexOf(idchars[i]) + 1];
					break;
				}
			}

			return new string(idchars);
		}

		/// <summary>
		/// get the list of roles for the given product, for the given organization
		/// </summary>
		public async Task<List<ProductRole>> GetProductRolesAsync(int orgId, ProductIdEnum pid)
		{
			// NOTE: orgid is ignored for now
			if ((int)pid < 0) throw new ArgumentOutOfRangeException("pid");

			var collection = await this.DBHelper.GetProductRolesAsync(orgId, (int)pid);
			var result = new List<ProductRole>();
			foreach (var item in collection)
			{
				var role = new ProductRole();
				role.OrganizationId = orgId;
				role.ProductId = (ProductIdEnum)item.ProductId;
				role.ProductRoleId = item.ProductRoleId;
				role.ProductRoleName = item.ProductRoleName;
				result.Add(role);
			}

			return result;
		}

	}
}
