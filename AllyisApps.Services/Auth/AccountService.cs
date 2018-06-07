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
		public async Task<Invitation> GetInvitation(int invitationId)
		{
			InvitationDBEntity invitation = await DBHelper.GetInvitation(invitationId);
			return InitializeInvitationInfo(invitation);
		}

		/// <summary>
		/// Accepts an invitation, adding the user to the invitation's organization, subscriptions, and projects, then deletes the invitations.
		/// </summary>
		public async Task<bool> AcceptUserInvitation(int invitationId)
		{
			if (invitationId <= 0) throw new ArgumentOutOfRangeException(nameof(invitationId));

			bool result = DBHelper.AcceptInvitation(invitationId, UserContext.UserId) == 1;
			Invitation invite = InitializeInvitationInfo(await DBHelper.GetInvitation(invitationId));

			if (result && !string.IsNullOrWhiteSpace(invite.ProductRolesJson))
			{
				var roles = new List<InvitationPermissionsJson>();
				try
				{
					roles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InvitationPermissionsJson>>(invite.ProductRolesJson);
				}
				catch
				{
					// if roleString does not convert then assume that it is using old specification resets to empty json list.
				}

				// add the new user roles.  If not specified, user is added as unassigned
				var subscriptions = await GetSubscriptionsAsync(invite.OrganizationId, true);
				foreach (var subscription in subscriptions)
				{
					var role = roles.FirstOrDefault(r => r.SubscriptionId == subscription.SubscriptionId);
					if (role != null)
					{
						await DBHelper.MergeSubscriptionUserProductRole(role.ProductRoleId, role.SubscriptionId, UserContext.UserId);
					}
					else
					{
						int unassignedProductRoleId = GetUnassignedProductRoleId(subscription.ProductId);
						await DBHelper.MergeSubscriptionUserProductRole(unassignedProductRoleId, subscription.SubscriptionId, UserContext.UserId);
					}

					if (subscription.ProductId == ProductIdEnum.TimeTracker)
					{
						var projId = await GetDefaultProject(invite.OrganizationId);
						CreateProjectUser(projId, UserContext.UserId);
					}
				}
			}

			NotifyInviteAcceptAsync(invitationId);

			return result;
		}

		private int extractRole(JObject roleString, ProductIdEnum productId, SkuIdEnum skuId)
		{
			JToken test;
			bool exists = roleString.TryGetValue((((int)productId)).ToString(), out test);
			if (exists)
			{
				return test.Value<int>();
			}
			else
			{
				exists = roleString.TryGetValue((((int)skuId)).ToString(), out test);
				if (exists)
				{
					return test.Value<int>();
				}
				else
				{
					throw new Exception("Failed to extract role from JSON: " + roleString.ToString());
				}
			}
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
		/// TODO: wrap these calls in a transaction
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
			string confirmEmailMessage,
			LoginProviderEnum loginProvider = LoginProviderEnum.AllyisApps)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException(nameof(email));
			if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException(nameof(firstName));
			if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException(nameof(lastName));
			if (emailConfirmationCode == null) throw new ArgumentException(nameof(emailConfirmationCode));
			var result = 0;
			try
			{
				// hash the password, create the address and then the user
				var hash = string.IsNullOrWhiteSpace(password) ? null : Crypto.GetPasswordHash(password);
				int? aid = await this.DBHelper.CreateAddressAsync(address1, address2, city, stateId, postalCode, countryCode);
				result = await this.DBHelper.CreateUserAsync(email, hash, firstName, lastName, emailConfirmationCode, dateOfBirth, phoneNumber, Language.DefaultLanguageCultureName, aid == 0 ? null : aid, (int)loginProvider);

				// user created, send confirmation email
				await this.Mailer.SendEmailAsync(ServiceSettings.SupportEmail, email, confirmEmailSubject, confirmEmailMessage);
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
		/// change the login provider of the logged in user (allyis apps user)
		/// this will also set password hash to null
		/// </summary>
		public async Task ChangeLoginProviderAsync(LoginProviderEnum provider)
		{
			if (this.UserContext.LoginProvider == LoginProviderEnum.AllyisApps && provider != LoginProviderEnum.AllyisApps)
			{
				await this.DBHelper.UpdateUserLoginProviderAsync(this.UserContext.UserId, (int)provider);
			}
		}
		/// <summary>
		/// Validates the given email and password with the database.
		/// </summary>
		/// <param name="email">The login email.</param>
		/// <param name="password">The login password.</param>
		public async Task<User> ValidateLogin(string email, string password)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException(nameof(email));
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(nameof(password));

			User result = null;
			var user = await this.DBHelper.GetUserByEmailAsync(email);
			if (user != null && user.LoginProviderId == (int)LoginProviderEnum.AllyisApps && (!string.IsNullOrWhiteSpace(user.PasswordHash)))
			{
				// email exists, hash the given password and compare with hash in db
				PassWordValidationResult passwordValidation = Crypto.ValidateAndUpdate(password, user.PasswordHash);
				if (passwordValidation.SuccessfulMatch)
				{
					// store updated password hash if needed
					if (passwordValidation.UpdatedHash != null)
					{
						await DBHelper.UpdateUserPasswordAsync(user.UserId, passwordValidation.UpdatedHash);
					}

					// get user obj
					result = await this.InitializeUserAsync(user, false);
				}
			}

			return result;
		}

		/// <summary>
		/// gets the user context for the given user (should be the logged in user). the method
		/// returns only "active" organizations and subscriptions, which is the key to the whole permissions model.
		/// </summary>
		public UserContext PopulateUserContext(int userId)
		{
			if (userId <= 0) throw new ArgumentException("userId");

			UserContext result = null;

			// get context from db
			dynamic expando = this.DBHelper.GetUserContext(userId, (int)OrganizationStatusEnum.Active, (int)SubscriptionStatusEnum.Active);

			// get user information
			if (expando != null && expando.User != null)
			{
				// user found.
				result = new UserContext();
				result.UserId = expando.User.UserId;
				result.FirstName = expando.User.FirstName;
				result.LastName = expando.User.LastName;
				result.Email = expando.User.Email;
				result.LoginProvider = (LoginProviderEnum)expando.User.LoginProviderId;

				// get organization and roles
				foreach (var item in expando.OrganizationsAndRoles)
				{
					result.OrganizationsAndRoles.Add(item.OrganizationId, new UserContext.OrganizationAndRole
					{
						OrganizationId = item.OrganizationId,
						OrganizationRoleId = item.OrganizationRoleId
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
							SubscriptionId = item.SubscriptionId,
							OrganizationId = item.OrganizationId,
							SubscriptionName = item.SubscriptionName
						});
				}

				// set result to self
				SetUserContext(result);
			}

			return result;
		}

		public async Task<User> GetCurrentUserAsync(bool loadAddress = true)
		{
			return await GetUserAsync(this.UserContext.UserId, loadAddress);
		}

		public async Task<User> GetUserAsync(int userId, bool loadAddress = true)
		{
			if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));

			var user = await this.DBHelper.GetUserAsync(userId);
			return await this.InitializeUserAsync(user, loadAddress);
		}

		/// <summary>
		/// get the list of pending invitations for the current user, indexed by org id
		/// </summary>
		public async Task<Dictionary<int, Invitation>> GetCurrentUserPendingInvitationsAsync()
		{
			Dictionary<int, Invitation> result = new Dictionary<int, Invitation>();
			// get list of pending invitations for the current user email
			var entities = await this.DBHelper.GetInvitationsByEmailAsync(this.UserContext.Email, (int)InvitationStatusEnum.Pending);
			foreach (var item in entities)
			{
				// get the org			
				var org = await this.GetOrganizationAsync(item.OrganizationId);
				var obj = new Invitation();
				obj.DecisionDateUtc = item.DecisionDateUtc;
				obj.Email = item.Email;
				obj.EmployeeId = item.EmployeeId;
				obj.EmployeeTypeId = item.EmployeeTypeId;
				obj.FirstName = item.FirstName;
				obj.InvitationCreatedUtc = item.InvitationCreatedUtc;
				obj.InvitationId = item.InvitationId;
				obj.InvitationStatus = (InvitationStatusEnum)item.InvitationStatus;
				obj.LastName = item.LastName;
				obj.OrganizationId = item.OrganizationId;
				obj.OrganizationName = org.OrganizationName;
				obj.OrganizationRole = (OrganizationRoleEnum)item.OrganizationRoleId;
				obj.ProductRolesJson = item.ProductRolesJson;
				result.Add(obj.OrganizationId, obj);
			}

			return result;
		}

		public async Task<Dictionary<int, Organization>> GetActiveOrganizationsByIdsAsync(List<int> ids)
		{
			Dictionary<int, Organization> result = new Dictionary<int, Organization>();
			if (ids != null && ids.Count > 0)
			{
				var entities = await this.DBHelper.GetOrganizationsByIdsAsync(ids, (int)OrganizationStatusEnum.Active);
				foreach (var item in entities)
				{
					result.Add(item.OrganizationId, await this.InitializeOrganization(item));
				}
			}

			return result;
		}

		/// <summary>
		/// get organization from its db entity
		/// </summary>
		public async Task<Organization> InitializeOrganization(OrganizationDBEntity entity, bool loadAddress = true)
		{
			return new Organization
			{
				CreatedUtc = entity.OrganizationCreatedUtc,
				FaxNumber = entity.FaxNumber,
				OrganizationId = entity.OrganizationId,
				OrganizationName = entity.OrganizationName,
				OrganizationStatus = (OrganizationStatusEnum)entity.OrganizationStatus,
				PhoneNumber = entity.PhoneNumber,
				SiteUrl = entity.SiteUrl,
				Subdomain = entity.Subdomain,
				Address = (entity.AddressId.HasValue && loadAddress) ? await this.GetAddressAsync(entity.AddressId.Value) : null,
				UserCount = entity.UserCount
			};
		}

		/// <summary>
		/// update the current user profile
		/// </summary>
		public async Task UpdateCurrentUserProfile(DateTime dateOfBirth, string firstName, string lastName, string phoneNumber, int? addressId, string address, string city, int? stateId, string postalCode, string countryCode)
		{
			await this.UpdateUserProfile(this.UserContext.UserId, dateOfBirth, firstName, lastName, phoneNumber, addressId, address, city, stateId, postalCode, countryCode);
		}

		/// <summary>
		/// update the current user profile
		/// TODO: wrap in transaction
		/// </summary>
		public async Task UpdateUserProfile(int userId, DateTime dateOfBirth, string firstName, string lastName, string phoneNumber, int? addressId, string address, string city, int? stateId, string postalCode, string countryCode)
		{
			// update address first
			if (addressId.HasValue)
			{
				// update the address
				await this.DBHelper.UpdateAddressAsync(addressId.Value, address, null, city, stateId, postalCode, countryCode);
			}
			else
			{
				// create address
				addressId = await this.DBHelper.CreateAddressAsync(address, null, city, stateId, postalCode, countryCode);
			}

			// update the user next
			await DBHelper.UpdateUser(userId, firstName, lastName, dateOfBirth, phoneNumber, addressId == 0 ? null : addressId);
		}

		/// <summary>
		/// update the employee id and role of the user in the given organization
		/// </summary>
		public async Task<UpdateEmployeeIdAndOrgRoleResult> UpdateEmployeeIdAndOrgRole(int orgId, int userId, string employeeId, OrganizationRoleEnum orgRoleId)
		{
			if (orgId <= 0) throw new ArgumentOutOfRangeException(nameof(orgId));
			if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
			if (string.IsNullOrWhiteSpace(employeeId)) throw new ArgumentNullException(nameof(employeeId));

			//CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.EditUser, orgId);

			var result = UpdateEmployeeIdAndOrgRoleResult.Success;
			//CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.EditUser, orgId);
			if (userId == UserContext.UserId)
			{
				// employee is trying to update oneself
				var org = UserContext.OrganizationsAndRoles.FirstOrDefault(x => x.Value.OrganizationId == orgId);
				//if (org.Value.OrganizationRoleId != orgRoleId)
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
		public async Task<UserOld> GetUserOldByEmailAsync(string email)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException(nameof(email));

			await this.InitializeUserAsync(await DBHelper.GetUserByEmailAsync(email));

			return null;
		}

		/// <summary>
		/// Gets the user info from an email address.
		/// </summary>
		public async Task<User> GetUserByEmailAsync(string email, bool loadAddress = true)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException(nameof(email));

			return await this.InitializeUserAsync(await DBHelper.GetUserByEmailAsync(email), loadAddress);
		}

		/// <summary>
		/// Updates an organization member's info.
		/// </summary>
		public async Task<bool> UpdateMember(int userId, int orgId, string employeeId, int roleId, string firstName, string lastName, bool isInvited)
		{
			if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
			if (orgId <= 0) throw new ArgumentOutOfRangeException(nameof(orgId));
			if (string.IsNullOrWhiteSpace(employeeId)) throw new ArgumentNullException(nameof(employeeId));
			if (roleId <= 0) throw new ArgumentOutOfRangeException(nameof(roleId));

			return await DBHelper.UpdateMember(userId, orgId, employeeId, roleId, firstName, lastName, isInvited) == 1;
		}

		/// <summary>
		/// Sets the language preference for the current user.
		/// </summary>
		public void SetLanguage(string cultureName)
		{
			if (string.IsNullOrWhiteSpace(cultureName)) throw new ArgumentNullException(nameof(cultureName));

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
		public async Task<bool> SendPasswordResetMessageAsync(string email, string code, string callbackUrl)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException(nameof(email));
			if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));
			if (string.IsNullOrWhiteSpace(callbackUrl)) throw new ArgumentNullException(nameof(callbackUrl));

			bool result = false;
			int rowsUpdated = await this.DBHelper.UpdateUserPasswordResetCodeAsync(email, code);
			if (rowsUpdated == 1)
			{
				// Send reset email
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("Please reset your password by clicking <a href=\"{0}\">this reset link</a>.", callbackUrl);
				string msgbody = new System.Web.HtmlString(sb.ToString()).ToString();
				result = await this.Mailer.SendEmailAsync(ServiceSettings.SupportEmail, email, "Reset password", msgbody);
			}
			else if (rowsUpdated == 0)
			{
				// email didn't exist
				// TODO: log
			}

			return result;
		}

		/// <summary>
		/// Reset password. Returns true if password was updated.
		/// </summary>
		public async Task<bool> ResetPasswordAsync(Guid code, string password)
		{
			if (code == null) throw new ArgumentNullException(nameof(code));
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

			return ((await this.DBHelper.UpdateUserPasswordUsingCodeAsync(Crypto.GetPasswordHash(password), code)) == 1);
		}

		/// <summary>
		/// Changes a user's password.
		/// </summary>
		/// <param name="oldPassword">Old password, for verification.</param>
		/// <param name="newPassword">New password to change it to.</param>
		/// <returns>True for a successful change, false if anything fails.</returns>
		public async Task<bool> ChangePasswordAsync(string oldPassword, string newPassword)
		{
			if (string.IsNullOrWhiteSpace(oldPassword)) throw new ArgumentNullException(nameof(oldPassword));
			if (string.IsNullOrWhiteSpace(newPassword)) throw new ArgumentNullException(nameof(newPassword));

			bool result = false;
			string passwordHash = await DBHelper.GetUserPasswordAsync(UserContext.UserId);
			if (!string.IsNullOrWhiteSpace(passwordHash))
			{
				if (Crypto.ValidateAndUpdate(oldPassword, passwordHash).SuccessfulMatch)
				{
					result = true;
					// old password is correct.
					await DBHelper.UpdateUserPasswordAsync(UserContext.UserId, Crypto.GetPasswordHash(newPassword));
				}
			}

			return result;
		}

		/// <summary>
		/// Confirms the users email.
		/// </summary>
		public bool ConfirmUserEmail(Guid code)
		{
			if (code == null) throw new ArgumentNullException(nameof(code));
			return DBHelper.UpdateEmailConfirmed(code) == 1;
		}

		/// <summary>
		/// initialize user from userdbentity
		/// </summary>
		private async Task<User> InitializeUserAsync(UserDBEntity user, bool loadAddress = true)
		{
			if (user == null)
				return null;

			Address address = null;
			var addressLoaded = false;
			if (user.AddressId.HasValue && loadAddress)
			{
				address = await this.GetAddressAsync(user.AddressId.Value);
				addressLoaded = true;
			}

			return new User
			{
				AccessFailedCount = user.AccessFailedCount,
				Address = address,
				DateOfBirth = user.DateOfBirth,
				Email = user.Email,
				EmailConfirmationCode = user.EmailConfirmationCode,
				FirstName = user.FirstName,
				LastName = user.LastName,
				IsAddressLoaded = addressLoaded,
				IsEmailConfirmed = user.IsEmailConfirmed,
				IsLockoutEnabled = user.IsLockoutEnabled,
				LastUsedSubscriptionId = user.LastUsedSubscriptionId,
				LockoutEndDateUtc = user.LockoutEndDateUtc,
				LoginProvider = (LoginProviderEnum)user.LoginProviderId,
				PasswordHash = user.PasswordHash,
				PasswordResetCode = user.PasswordResetCode,
				PhoneExtension = user.PhoneExtension,
				PhoneNumber = user.PhoneNumber,
				IsPhoneNumberConfirmed = user.IsPhoneNumberConfirmed,
				IsTwoFactorEnabled = user.IsTwoFactorEnabled,
				UserCreatedUtc = user.UserCreatedUtc,
				UserId = user.UserId,
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

		public static int GetUnassignedProductRoleId(ProductIdEnum productId)
		{
			switch (productId)
			{
				case ProductIdEnum.ExpenseTracker:
					return (int)ExpenseTrackerRole.NotInProduct;

				case ProductIdEnum.StaffingManager:
					return (int)StaffingManagerRole.NotInProduct;

				case ProductIdEnum.TimeTracker:
					return (int)TimeTrackerRole.NotInProduct;

				default:
					throw new InvalidOperationException("You selected an invalid product to subscribe to.");
			}
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
			if (organizationId <= 0) throw new ArgumentOutOfRangeException(nameof(organizationId));

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
		public async Task<List<ProductRole>> GetProductRoles(int orgId, ProductIdEnum pid)
		{
			// NOTE: orgid is ignored for now
			if (pid < 0) throw new ArgumentOutOfRangeException(nameof(pid));

			var collection = await DBHelper.GetProductRolesAsync(orgId, (int)pid);

			return collection.Select(item =>
				new ProductRole
				{
					OrganizationId = orgId,
					ProductId = (ProductIdEnum)item.ProductId,
					ProductRoleId = item.ProductRoleId,
					ProductRoleName = item.ProductRoleShortName
				})
				.ToList();
		}

		/// <summary>
		/// create the default product roles and permissions for the given product and org or sub id
		/// </summary>
		public async Task CreateDefaultProductRolesAndPermissions(ProductIdEnum productId, int orgOrSubId)
		{
			if (orgOrSubId <= 0) throw new ArgumentOutOfRangeException(nameof(orgOrSubId));

			switch (productId)
			{
				case ProductIdEnum.AllyisApps:
					await this.CreateDefaultAllyisAppsRolesAndPermissions(orgOrSubId);
					break;

				case ProductIdEnum.ExpenseTracker:
					break;

				case ProductIdEnum.StaffingManager:
					break;

				case ProductIdEnum.TimeTracker:
					break;

				default:
					break;
			}
		}

		/// <summary>
		/// creates the built-in roles for allyis apps. returns the admin and user role ids
		/// </summary>
		private async Task<Tuple<int, int>> CreateDefaultAllyisAppsRolesAndPermissions(int orgOrSubId)
		{
			var adminRoleId = await this.CreateAllyisAppsAdminRoleAndPermissions(orgOrSubId);
			var userRoleId = await this.CreateAllyisAppsUserRoleAndPermissions(orgOrSubId);
			return new Tuple<int, int>(adminRoleId, userRoleId);
		}

		private async Task<int> CreateAllyisAppsAdminRoleAndPermissions(int orgOrSubId)
		{
			// create admin role
			var result = await this.DBHelper.CreateProductRoleAsync((int)ProductIdEnum.AllyisApps, "Admin", "Organization Administrator", orgOrSubId, (int)BuiltinRoleEnum.Admin);

			// create permissions for this role
			var list = new List<PermissionDBEntity>();
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Delete, AppEntityId = (int)AppEntity.Organization, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Read, AppEntityId = (int)AppEntity.Organization, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Update, AppEntityId = (int)AppEntity.Organization, ProductRoleId = result });

			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Create, AppEntityId = (int)AppEntity.OrganizationUser, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Delete, AppEntityId = (int)AppEntity.OrganizationUser, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Read, AppEntityId = (int)AppEntity.OrganizationUser, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Update, AppEntityId = (int)AppEntity.OrganizationUser, ProductRoleId = result });

			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Create, AppEntityId = (int)AppEntity.Subscription, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Delete, AppEntityId = (int)AppEntity.Subscription, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Read, AppEntityId = (int)AppEntity.Subscription, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Update, AppEntityId = (int)AppEntity.Subscription, ProductRoleId = result });

			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Create, AppEntityId = (int)AppEntity.SubscriptionUser, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Delete, AppEntityId = (int)AppEntity.SubscriptionUser, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Read, AppEntityId = (int)AppEntity.SubscriptionUser, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Update, AppEntityId = (int)AppEntity.SubscriptionUser, ProductRoleId = result });

			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Create, AppEntityId = (int)AppEntity.Billing, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Delete, AppEntityId = (int)AppEntity.Billing, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Read, AppEntityId = (int)AppEntity.Billing, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Update, AppEntityId = (int)AppEntity.Billing, ProductRoleId = result });

			await this.DBHelper.CreatePermissionsAsync(list);

			return result;
		}

		private async Task<int> CreateAllyisAppsUserRoleAndPermissions(int orgOrSubId)
		{
			// create user role
			var result = await this.DBHelper.CreateProductRoleAsync((int)ProductIdEnum.AllyisApps, "User", "Organization User", orgOrSubId, (int)BuiltinRoleEnum.User);

			// create permissions for this role
			var list = new List<PermissionDBEntity>();
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Read, AppEntityId = (int)AppEntity.Organization, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Read, AppEntityId = (int)AppEntity.OrganizationUser, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Read, AppEntityId = (int)AppEntity.Subscription, ProductRoleId = result });
			list.Add(new PermissionDBEntity() { UserActionId = (int)UserAction.Read, AppEntityId = (int)AppEntity.SubscriptionUser, ProductRoleId = result });

			await this.DBHelper.CreatePermissionsAsync(list);

			return result;
		}
	}
}
