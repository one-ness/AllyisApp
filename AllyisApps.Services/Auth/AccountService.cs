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
						OrganizationRole = (OrganizationRoleEnum)item.OrganizationRoleId,
						OrganizationName = item.OrganizationName
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

		/// <summary>
		/// get the current logged in user
		/// </summary>
		public async Task<UserOld> GetCurrentUserOldAsync()
		{
			return await GetUserOldAsync(UserContext.UserId);
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
		/// get user
		/// - address, organizations, subscriptions and invitations
		/// </summary>
		public async Task<UserOld> GetUserOldAsync(int userId, int organizationId = 0, OrgAction actionContext = OrgAction.ReadUser)
		{
			if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));

			dynamic sets = await DBHelper.GetUserOld(userId);
			UserOld user = null; // this.InitializeUser(sets.User);
			dynamic subs = sets.Subscriptions;
			foreach (dynamic item in subs)
			{
				var sub = new UserSubscription
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
					var orgIds = (
						from item in UserContext.OrganizationsAndRoles
						select item.Value.OrganizationId into orgId
						let org =
							user.Organizations.FirstOrDefault(x => x.OrganizationId == orgId)
						where org != null
						select orgId
						).ToList();

					// is there any?
					bool permFound = false;
					if (orgIds.Count <= 0)
					{
						// no
						throw new AccessViolationException(string.Format("User {0} not found in any of the organizations.", user.UserId));
					}

					// yes, does the logged in user have readuser permission in at least one of them?
					foreach (int item in orgIds)
					{
						permFound = CheckOrgAction(actionContext, item, false);
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
					var org = user.Organizations.FirstOrDefault(x => x.OrganizationId == organizationId);
					if (org == null)
					{
						// no
						throw new AccessViolationException(string.Format("User {0} not found in the organization {1}.", user.UserId, organizationId));
					}

					// yes, check the logged in user's permission
					CheckOrgAction(actionContext, organizationId);
				}
			}

			return user;
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

			CheckOrgAction(OrgAction.EditUser, orgId);

			var result = UpdateEmployeeIdAndOrgRoleResult.Success;
			CheckOrgAction(OrgAction.EditUser, orgId);
			if (userId == UserContext.UserId)
			{
				// employee is trying to update oneself
				var org = UserContext.OrganizationsAndRoles.FirstOrDefault(x => x.Value.OrganizationId == orgId);
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
					ProductRoleName = item.ProductRoleName
				})
				.ToList();
		}
	}
}
