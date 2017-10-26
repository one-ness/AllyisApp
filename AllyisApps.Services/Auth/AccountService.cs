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
using AllyisApps.Services.Cache;
using AllyisApps.Services.Lookup;
using Newtonsoft.Json.Linq;

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
		public Dictionary<string, Country> GetCountries()
		{
			return CacheContainer.CountriesCache;
		}

		/// <summary>
		/// get the list of states for the given country
		/// </summary>
		public List<State> GetStates(string countryCode)
		{
			return CacheContainer.StatesCache[countryCode];
		}

		public bool DeleteExpenseItem(int itemId)
		{
			return DBHelper.DeleteExpenseItem(itemId);
		}

		/// <summary>
		/// Gets the list of valid languages selections.
		/// </summary>
		/// <returns>A list of languages.</returns>
		public async Task<IEnumerable<Language>> ValidLanguages()
		{
			var results = await DBHelper.ValidLanguages();
			return results.Select(s => new Language
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
		public async Task<Invitation> GetInvitationById(int invitationId)
		{
			var results = InitializeInvitationInfo(await DBHelper.GetInvitation(invitationId));
			return results;
		}

		/// <summary>
		/// Accepts an invitation, adding the user to the invitation's organization, subscriptions, and projects, then deletes the invitations.
		/// </summary>
		public async Task<bool> AcceptUserInvitation(int invitationId)
		{
			if (invitationId <= 0) throw new ArgumentOutOfRangeException("invitationId");

			Invitation inviteInfo = InitializeInvitationInfo(await DBHelper.GetInvitation(invitationId));
			var result = DBHelper.AcceptInvitation(invitationId, UserContext.UserId) == 1;
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
						var result2 = await DBHelper.UpdateSubscriptionUserRoles(new List<int> { UserContext.UserId }, inviteInfo.OrganizationId, ttRole, (int)ProductIdEnum.TimeTracker);
					}

					if (etRole > 0)
					{
						var prodId = GetProductSubscriptionInfo(inviteInfo.OrganizationId, SkuIdEnum.ExpenseTrackerBasic).Product.ProductId;
						var result3 = await DBHelper.UpdateSubscriptionUserRoles(new List<int> { UserContext.UserId }, inviteInfo.OrganizationId, etRole, (int)ProductIdEnum.ExpenseTracker);
					}

					if (smRole > 0)
					{
						var prodId = GetProductSubscriptionInfo(inviteInfo.OrganizationId, SkuIdEnum.StaffingManagerBasic).Product.ProductId;
						var result4 = await DBHelper.UpdateSubscriptionUserRoles(new List<int> { UserContext.UserId }, inviteInfo.OrganizationId, smRole, (int)ProductIdEnum.StaffingManager);
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
		public async Task<bool> RejectInvitation(int invitationId)
		{
			bool rejected = await DBHelper.RejectInvitation(invitationId) == 1;
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
				result = await DBHelper.CreateUserAsync(
					email, Crypto.GetPasswordHash(password), firstName, lastName, emailConfirmationCode, dateOfBirth, phoneNumber, Language.DefaultLanguageCultureName,
					address1, address2, city, stateId, postalCode, countryCode);

				// user created, send confirmation email
				await Mailer.SendEmailAsync(ServiceSettings.SupportEmail, email, confirmEmailSubject, confirmEmailMessage);
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
		public async Task<User> ValidateLogin(string email, string password)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException("email");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password");

			User result = null;
			result = InitializeUser(await DBHelper.GetUserByEmail(email), false);

			if (result != null)
			{
				// email exists, hash the given password and compare with hash in db
				PassWordValidationResult passwordValidation = Crypto.ValidateAndUpdate(password, result.PasswordHash);
				if (passwordValidation.successfulMatch)
				{
					// Store updated password hash if needed
					if (passwordValidation.updatedHash != null)
					{
						await DBHelper.UpdateUserPassword(result.UserId, passwordValidation.updatedHash);
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
			dynamic expando = DBHelper.GetUserContext(userId);

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
					result.OrganizationsAndRoles.Add(item.OrganizationId, new UserContext.OrganizationAndRole
					{
						OrganizationId = item.OrganizationId,
						OrganizationRole = (OrganizationRoleEnum)item.OrganizationRoleId,
					});
				}

				// get subscriptions and roles
				foreach (var item in expando.SubscriptionsAndRoles)
				{
					result.SubscriptionsAndRoles.Add(item.SubscriptionId,
						new UserContext.SubscriptionAndRole
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
				SetUserContext(result);
			}

			return result;
		}

		/// <summary>
		/// get the current logged in user
		/// </summary>
		public async Task<User> GetCurrentUserAsync()
		{
			return await GetUserAsync(UserContext.UserId);
		}

		/// <summary>
		/// get user
		/// - address, organizations, subscriptions and invitations
		/// </summary>
		public async Task<User> GetUserAsync(int userId, int organizationId = 0)
		{
			if (userId <= 0) throw new ArgumentOutOfRangeException("userId");

			dynamic sets = await DBHelper.GetUser(userId);
			User user = this.InitializeUser(sets.User);
			dynamic subs = sets.Subscriptions;
			foreach (var item in subs)
			{
				UserSubscription sub = new UserSubscription
				{
					IsActive = item.IsActive,
					NumberOfUsers = item.NumberOfUsers,
					OrganizationId = item.OrganizationId,
					ProductAreaUrl = item.ProductAreaUrl,
					ProductDescription = item.ProductDescription,
					ProductId = (ProductIdEnum)item.ProductId,
					ProductName = item.ProductName,
					ProductRoleId = item.ProductRoleId,
					PromoExpirationDateUtc = item.PromotionExpirationDateUtc,
					SkuDescription = item.SkuDescription,
					SkuIconUrl = item.IconUrl,
					SkuId = (SkuIdEnum)item.SkuId,
					SkuName = item.SkuName,
					CreatedUtc = item.SubscriptionCreatedUtc,
					SubscriptionId = item.SubscriptionId,
					SubscriptionName = item.SubscriptionName,
					JoinedDateUtc = item.SubscriptionUserCreatedUtc,
					UserId = item.UserId
				};
				user.Subscriptions.Add(sub);
			}

			dynamic orgs = sets.Organizations;
			foreach (var item in orgs)
			{
				UserOrganization org = new UserOrganization();
				org.Address = new Address();
				org.Address.Address1 = item.Address1;
				org.Address.AddressId = item.AddressId;
				org.Address.City = item.City;
				org.Address.CountryCode = item.CountryCode;
				org.Address.CountryName = item.CountryName;
				org.Address.PostalCode = item.PostalCode;
				org.Address.StateId = item.StateId;
				org.Address.StateName = item.StateName;
				org.CreatedUtc = item.OrganizationCreatedUtc;
				org.EmployeeId = item.EmployeeId;
				org.FaxNumber = item.FaxNumber;
				org.IsActive = item.IsActive;
				org.JoinedDateUtc = item.OrganizationUserCreatedUtc;
				org.MaxApprovalAmount = item.ApprovalAmount;
				org.OrganizationId = item.OrganizationId;
				org.OrganizationName = item.OrganizationName;
				org.OrganizationRole = (OrganizationRoleEnum)item.OrganizationRoleId;
				org.PhoneNumber = item.PhoneNumber;
				org.SiteUrl = item.SiteUrl;
				org.UserId = item.UserId;
				user.Organizations.Add(org);
			}

			dynamic invites = sets.Invitations;
			foreach (var item in invites)
			{
				Invitation invite = new Invitation();
				invite.CompressedEmail = item.Email;
				invite.Email = item.Email;
				invite.EmployeeId = item.EmployeeId;
				invite.FirstName = item.FirstName;
				invite.InvitationCreatedUtc = item.InvitationCreatedUtc;
				invite.InvitationId = item.InvitationId;
				invite.InvitationStatus = (InvitationStatusEnum)item.InvitationStatus;
				invite.LastName = item.LastName;
				invite.OrganizationId = item.OrganizationId;
				invite.OrganizationName = item.OrganizationName;
				invite.ProductRolesJson = item.ProductRolesJson;
				user.Invitations.Add(invite);
			}

			if (UserContext.UserId != user.UserId)
			{
				// logged in user is trying to read a different user's information
				// was an org id provided?
				if (organizationId <= 0)
				{
					// no, get the list of organizations that both of them are member of
					List<int> orgIds = new List<int>();
					foreach (var item in UserContext.OrganizationsAndRoles)
					{
						var orgId = item.Value.OrganizationId;
						var org = user.Organizations.Where(x => x.OrganizationId == orgId).FirstOrDefault();
						if (org != null)
						{
							orgIds.Add(orgId);
						}
					}

					// is there any?
					bool permFound = false;
					if (orgIds.Count <= 0)
					{
						// no
						throw new AccessViolationException(string.Format("User {0} not found in any of the organizations.", user.UserId));
					}
					else
					{
						// yes, does the logged in user have readuser permission in at least one of them?
						foreach (var item in orgIds)
						{
							permFound = CheckOrgAction(OrgAction.ReadUser, item, false);
						}
					}

					if (!permFound)
					{
						// no
						throw new AccessViolationException(string.Format("User {0} does not have permission to read user {1}.", UserContext.UserId, user.UserId));
					}
				}
				else
				{
					// does the user belong to that organization?
					var org = user.Organizations.Where(x => x.OrganizationId == organizationId).FirstOrDefault();
					if (org == null)
					{
						// no
						throw new AccessViolationException(string.Format("User {0} not found in the organization {1}.", user.UserId, organizationId));
					}
					else
					{
						// yes, check the logged in user's permission
						CheckOrgAction(OrgAction.ReadUser, organizationId);
					}
				}
			}

			return user;
		}

		/// <summary>
		/// update the current user profile
		/// </summary>
		public async Task UpdateCurrentUserProfile(int? dateOfBirth, string firstName, string lastName, string phoneNumber, int? addressId, string address, string city, int? stateId, string postalCode, string countryCode)
		{
			await DBHelper.UpdateUserProfile(UserContext.UserId, firstName, lastName, Utility.GetDateTimeFromDays((int)dateOfBirth), phoneNumber, addressId, address, null, city, stateId, postalCode, countryCode);
		}

		/// <summary>
		/// update the current user profile
		/// </summary>
		public async Task UpdateUserProfile(int userId, int? dateOfBirth, string firstName, string lastName, string phoneNumber, int? addressId, string address, string city, int? stateId, string postalCode, string countryCode)
		{
			await DBHelper.UpdateUserProfile(userId, firstName, lastName, Utility.GetDateTimeFromDays((int)dateOfBirth), phoneNumber, addressId, address, null, city, stateId, postalCode, countryCode);
		}

		/// <summary>
		/// update the employee id and role of the user in the given organization
		/// </summary>
		public async Task<UpdateEmployeeIdAndOrgRoleResult> UpdateEmployeeIdAndOrgRole(int orgId, int userId, string employeeId, OrganizationRoleEnum orgRoleId)
		{
			if (orgId <= 0) throw new ArgumentOutOfRangeException("orgId");
			if (userId <= 0) throw new ArgumentOutOfRangeException("userId");
			if (string.IsNullOrWhiteSpace(employeeId)) throw new ArgumentNullException("employeeId");

			CheckOrgAction(OrgAction.EditUser, orgId);

			var result = UpdateEmployeeIdAndOrgRoleResult.Success;
			CheckOrgAction(OrgAction.EditUser, orgId);
			if (userId == UserContext.UserId)
			{
				// employee is trying to update oneself
				var org = UserContext.OrganizationsAndRoles.Where(x => x.Value.OrganizationId == orgId).FirstOrDefault();
				if (org.Value.OrganizationRole != orgRoleId)
				{
					// employee is trying to change the oneself's role, not allowed
					result = UpdateEmployeeIdAndOrgRoleResult.CannotSelfUpdateOrgRole;
				}
			}

			if (result == UpdateEmployeeIdAndOrgRoleResult.Success)
			{
				try
				{
					if (await DBHelper.UpdateEmployeeIdAndOrgRole(orgId, userId, employeeId, (int)orgRoleId) != 1)
					{
						// TODO: how to revert this catastrophic change?
						throw new InvalidOperationException(string.Format("Error: either organization {0} or user {1} is corrupted.", orgId, userId));
					}
				}
				catch (Exception ex)
				{
					if (ex.Message.ToLower().Contains("unique"))
					{
						result = UpdateEmployeeIdAndOrgRoleResult.EmployeeIdNotUnique;
					}
					else
					{
						throw;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Gets the user info from an email address.
		/// </summary>
		/// <param name="email">Email address.</param>
		/// <returns>A UserInfo instance with the user's info.</returns>
		public async Task<User> GetUserByEmail(string email)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException("email");

			return InitializeUser(await DBHelper.GetUserByEmail(email));
		}

		/// <summary>
		/// Updates an organization member's info.
		/// </summary>
		public async Task<bool> UpdateMember(int userId, int orgId, string employeeId, int roleId, string firstName, string lastName, bool isInvited)
		{
			if (userId <= 0) throw new ArgumentOutOfRangeException("userId");
			if (orgId <= 0) throw new ArgumentOutOfRangeException("orgId");
			if (string.IsNullOrWhiteSpace(employeeId)) throw new ArgumentNullException("employeeId");
			if (roleId <= 0) throw new ArgumentOutOfRangeException("roleId");

			return await DBHelper.UpdateMember(userId, orgId, employeeId, roleId, firstName, lastName, isInvited) == 1 ? true : false;
		}

		/// <summary>
		/// Sets the language preference for the current user.
		/// </summary>
		public void SetLanguage(string cultureName)
		{
			if (string.IsNullOrWhiteSpace(cultureName)) throw new ArgumentNullException("cultureName");

			DBHelper.UpdateUserLanguagePreference(UserContext.UserId, cultureName);
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

			LanguageDBEntity language = DBHelper.GetLanguage(cultureName);
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
			int rowsUpdated = DBHelper.UpdateUserPasswordResetCode(email, code);
			if (rowsUpdated > 0)
			{
				// Send reset email
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("Please reset your password by clicking <a href=\"{0}\">this reset link</a>.", callbackUrl);
				string msgbody = new System.Web.HtmlString(sb.ToString()).ToString();
				result = await Mailer.SendEmailAsync(ServiceSettings.SupportEmail, email, "Reset password", msgbody);
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
				result = DBHelper.UpdateUserPasswordUsingCode(Crypto.GetPasswordHash(password), code);
			});

			return result;
		}

		/// <summary>
		/// Changes a user's password.
		/// </summary>
		/// <param name="oldPassword">Old password, for verification.</param>
		/// <param name="newPassword">New password to change it to.</param>
		/// <returns>True for a successful change, false if anything fails.</returns>
		public async Task<bool> ChangePassword(string oldPassword, string newPassword)
		{
			if (string.IsNullOrWhiteSpace(oldPassword)) throw new ArgumentNullException("oldPassword");
			if (string.IsNullOrWhiteSpace(newPassword)) throw new ArgumentNullException("newPassword");

			bool result = false;
			string passwordHash = DBHelper.GetPasswordHashById(UserContext.UserId);
			if (!string.IsNullOrWhiteSpace(passwordHash))
			{
				PassWordValidationResult validation = Crypto.ValidateAndUpdate(oldPassword, passwordHash);
				if (validation.successfulMatch)
				{
					// old password is correct.
					result = true;
					await DBHelper.UpdateUserPassword(UserContext.UserId, Crypto.GetPasswordHash(newPassword));
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
			User newUser = new User
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

		public async Task UpdateUserOrgMaxAmount(OrganizationUser userInfo)
		{
			OrganizationUserDBEntity entity = new OrganizationUserDBEntity
			{
				UserId = userInfo.UserId,
				OrganizationId = userInfo.OrganizationId
			};
			await DBHelper.UpdateUserMaxAmount(entity);
		}

		public async Task<decimal> GetOrganizationUserMaxAmount(int userId, int orgId)
		{
			return await DBHelper.GetUserOrgMaxAmount(userId, orgId);
		}

		/// <summary>
		/// constructs the next unique employee id to be added to an invitation
		/// </summary>
		public async Task<string> GetNextEmployeeId(int organizationId)
		{
			if (organizationId <= 0) throw new ArgumentOutOfRangeException("organizationId");

			string maxId = await DBHelper.GetMaxEmployeeId(organizationId);
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

			var collection = await DBHelper.GetProductRolesAsync(orgId, (int)pid);
			var result = new List<ProductRole>();
			foreach (var item in collection)
			{
				result.Add(new ProductRole
				{
					OrganizationId = orgId,
					ProductId = (ProductIdEnum)item.ProductId,
					ProductRoleId = item.ProductRoleId,
					ProductRoleName = item.ProductRoleName
				});
			}

			return result;
		}
	}
}
