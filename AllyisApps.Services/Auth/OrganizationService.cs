//------------------------------------------------------------------------------
// <copyright file="OrgService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Hrm;
using AllyisApps.Lib;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Lookup;
using AllyisApps.Services.Hrm;
using System.Data.SqlClient;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all Organization related services.
	/// </summary>
	public partial class AppService : BaseService
	{
		/// <summary>
		/// Creates an organization.
		/// TODO: wrap the calls in a transaction
		/// </summary>
		public async Task<int> SetupNewOrganization(string employeeId, string organizationName, string phoneNumber, string faxNumber, string siteUrl, string subDomainName, string address1, string city, int? stateId, string postalCode, string countryCode)
		{
			if (string.IsNullOrWhiteSpace(employeeId)) throw new ArgumentNullException(nameof(employeeId));
			if (string.IsNullOrWhiteSpace(organizationName)) throw new ArgumentNullException(nameof(organizationName));

			// create address
			int? addressId = await this.DBHelper.CreateAddressAsync(address1, null, city, stateId, postalCode, countryCode);

			// create organization
			int orgId = 0;
			try
			{
				orgId = await this.DBHelper.CreateOrganizationAsync(organizationName, (int)OrganizationStatusEnum.Active, siteUrl, phoneNumber, faxNumber, subDomainName, addressId == 0 ? null : addressId);

				// create default payclasses for the organization
				var list = new List<PayClassDBEntity>();
				list.Add(new PayClassDBEntity() { BuiltinPayClassId = (int)BuiltinPayClassEnum.Custom, OrganizationId = orgId, PayClassName = "Bereavement Leave" });
				list.Add(new PayClassDBEntity() { BuiltinPayClassId = (int)BuiltinPayClassEnum.Custom, OrganizationId = orgId, PayClassName = "Jury Duty" });
				list.Add(new PayClassDBEntity() { BuiltinPayClassId = (int)BuiltinPayClassEnum.Custom, OrganizationId = orgId, PayClassName = "Other Leave" });
				list.Add(new PayClassDBEntity() { BuiltinPayClassId = (int)BuiltinPayClassEnum.Holiday, OrganizationId = orgId, PayClassName = "Holiday" });
				list.Add(new PayClassDBEntity() { BuiltinPayClassId = (int)BuiltinPayClassEnum.Overtime, OrganizationId = orgId, PayClassName = "Overtime" });
				list.Add(new PayClassDBEntity() { BuiltinPayClassId = (int)BuiltinPayClassEnum.PaidTimeOff, OrganizationId = orgId, PayClassName = "Paid Time Off" });
				list.Add(new PayClassDBEntity() { BuiltinPayClassId = (int)BuiltinPayClassEnum.Regular, OrganizationId = orgId, PayClassName = "Regular" });
				list.Add(new PayClassDBEntity() { BuiltinPayClassId = (int)BuiltinPayClassEnum.UnpaidTimeOff, OrganizationId = orgId, PayClassName = "Unpaid Time Off" });
				await this.DBHelper.CreatePayClassesAsync(orgId, list);

				// create default employee type for the organization
				var etid = await this.DBHelper.CreateEmployeeType(orgId, "Full Time Employee");

				// add default payclasses to default employee type
				await this.DBHelper.AddOrgPayClassesToEmployeeType(orgId, etid);

				// add default roles and permissions for allyis apps
				var roleIds = await this.CreateDefaultAllyisAppsRolesAndPermissions(orgId);

				// create organization user with that employee type id, employee id and role id
				// note: this is the first user in the org, hence the employeeId won't be a duplicate
				await this.AddUserToOrganization(orgId, this.UserContext.UserId, employeeId, etid, roleIds.Item1);
			}
			catch (SqlException ex)
			{
				if (ex.Message.ToLower().Contains("unique"))
				{
					// unique constraint, sudomain already taken
					// delete the address that was created
					if (addressId > 0)
					{
						await this.DBHelper.DeleteAddressAsync(addressId.Value);
					}
				}
				else
				{
					throw;
				}
			}

			return orgId;
		}

		/// <summary>
		/// add user to organization
		/// TODO: wrap the calls in a transaction
		/// </summary>
		public async Task AddUserToOrganization(int orgId, int userId, string employeeId, int employeeTypeId, int roleId, decimal approvalLimit = 0, Dictionary<int, int> subscriptionRoles = null)
		{
			// add the user to organization
			await this.DBHelper.CreateOrganizationUser(orgId, userId, roleId, employeeId, employeeTypeId, approvalLimit);

			// get all active subscriptions of the organization
			var subs = await this.DBHelper.GetSubscriptionsAsync(orgId, (int)SubscriptionStatusEnum.Active);

			// add to given subscription with given roles
			var subroles = new Dictionary<int, int>();
			if (subscriptionRoles != null)
			{
				foreach (var item in subscriptionRoles)
				{
					// check if this subscription is active for this org
					SubscriptionDBEntity sub = null;
					if (subs.TryGetValue(item.Key, out sub))
					{
						// yes
						// TODO: for now, we assume the role provided is a valid role for that product.
						// we may have to validate in the future
						subroles.Add(item.Key, item.Value);
					}
				}
			}
			else
			{
				// no subcription and roles supplied, hence add the user to all the subscriptions with not in product role
				foreach (var item in subs)
				{
					subroles.Add(item.Key, ProductRole.NotInProduct);
				}
			}

			// update the db
			foreach (var item in subroles)
			{
				await this.DBHelper.CreateSubscriptionUserAsync(item.Key, this.UserContext.UserId, item.Value);
			}
		}

		/// <summary>
		/// Gets an <see cref="Organization"/>.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>The Organization.</returns>
		public async Task<Organization> GetOrganizationAsync(int orgId)
		{
			if (orgId <= 0) throw new ArgumentOutOfRangeException(nameof(orgId));

			await CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Read, AppEntity.Organization, orgId);
			return await InitializeOrganization(await DBHelper.GetOrganizationAsync(orgId));
		}

		public async Task<List<Invitation>> GetInvitationsAsync(int orgId)
		{
			if (orgId <= 0) throw new ArgumentOutOfRangeException(nameof(orgId), "Organization Id must be greater than 0.");

			await CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Read, AppEntity.OrganizationUser, orgId);

			var collection = await DBHelper.GetInvitationsAsync(orgId, (int)InvitationStatusEnum.Any);

			return collection.Select(item => new Invitation
			{
				DecisionDateUtc = item.DecisionDateUtc,
				Email = item.Email,
				EmployeeId = item.EmployeeId,
				FirstName = item.FirstName,
				InvitationCreatedUtc = item.InvitationCreatedUtc,
				InvitationId = item.InvitationId,
				InvitationStatus = (InvitationStatusEnum)item.InvitationStatus,
				LastName = item.LastName,
				OrganizationId = orgId,
				ProductRolesJson = item.ProductRolesJson,
				OrganizationRoleId = item.OrganizationRoleId
			}).ToList();
		}

		/// <summary>
		/// get a list of subscriptions for the given organization
		/// </summary>
		public async Task<List<Subscription>> GetSubscriptionsAsync(int orgId, bool readingList = false)
		{
			if (orgId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(orgId), $"{nameof(orgId)} must be greater than 0.");
			}

			if (!readingList)
			{
				await CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Read, AppEntity.Subscription, orgId);
			}

			var result = new List<Subscription>();
			dynamic entities = await DBHelper.GetSubscriptionsAsync(orgId, 1);
			foreach (var curr_model in entities)
			{
                SubscriptionDBEntity item = curr_model.Value;
				var data = new Subscription();
				data.ProductAreaUrl = item.ProductAreaUrl;
				data.IsActive = item.IsActive;
				data.NumberOfUsers = item.UserCount;
				data.OrganizationId = item.OrganizationId;
				data.ProductDescription = item.ProductDescription;
				data.ProductId = (ProductIdEnum)item.ProductId;
				data.ProductName = item.ProductName;
				data.PromoExpirationDateUtc = item.PromoExpirationDateUtc;
				data.ProductIconUrl = item.ProductIconUrl;
				data.CreatedUtc = item.SubscriptionCreatedUtc;
				data.SubscriptionId = item.SubscriptionId;
				data.SubscriptionName = item.SubscriptionName;
				result.Add(data);
			}

			return result;
		}

		/// <summary>
		/// Gets the Organization for the current chosen organization, along with OrganizationUserInfos for each user in the
		/// organization, SubscriptionDisplayInfos for any subscriptions in the organization, InvitationInfos for any invitiations
		/// pending in the organization, the organization's billing stripe handle, and a list of all products.
		/// </summary>
		/// <returns>.</returns>
		public async Task<Organization> GetOrganizationManagementInfo(int orgId)
		{
			var spResults = await DBHelper.GetOrganizationManagementInfo(orgId);
			Organization org = await InitializeOrganization(spResults.Item1);
			org.Users = spResults.Item2.Select(InitializeOrganizationUser).ToList();
			org.Subscriptions = spResults.Item3.Select(InitializeSubscription).ToList();
			org.Invitations = spResults.Item4.Select(InitializeInvitationInfo).ToList();
			org.StripeToken = spResults.Item5;
			return org;
		}

		/// <summary>
		/// Updates an organization chosen by the current user.
		/// </summary>
		public async Task<bool> UpdateOrganization(int organizationId, string organizationName, string siteUrl, string address1, string city, int? stateId, string countryCode, string postalCode, string phoneNumber, string faxNumber, string subDomain)
		{
			if (organizationId <= 0) throw new ArgumentOutOfRangeException(nameof(organizationId));
			if (string.IsNullOrWhiteSpace(organizationName)) throw new ArgumentNullException(nameof(organizationName));

			bool result = false;
			await CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Edit, AppEntity.Organization, organizationId);

			// to update address, firt get the address of the organization
			int? addressId = 0;
			var org = await this.GetOrganizationAsync(organizationId);
			if (org.Address != null)
			{
				// address exists, update
				addressId = org.Address.AddressId;
				await this.DBHelper.UpdateAddressAsync(addressId.Value, address1, null, city, stateId, postalCode, countryCode);
			}
			else
			{
				// address doesn't exist. any non-null input from user?
				if (!string.IsNullOrWhiteSpace(address1) || !string.IsNullOrWhiteSpace(city) || !string.IsNullOrWhiteSpace(postalCode) || !string.IsNullOrWhiteSpace(countryCode) || stateId.HasValue)
				{
					// yes, create address
					addressId = await this.DBHelper.CreateAddressAsync(address1, null, city, stateId, postalCode, countryCode);
				}
			}

			// update the org next
			try
			{
				await DBHelper.UpdateOrganization(organizationId, organizationName, siteUrl, phoneNumber, faxNumber, subDomain, addressId);
				result = true;
			}
			catch (SqlException ex)
			{
				if (ex.Message.ToLower().Contains("unique"))
				{
					// unique constraint, sudomain already taken
					// delete the address that was created
					if (addressId > 0)
					{
						await this.DBHelper.DeleteAddressAsync(addressId.Value);
						result = false;
					}
				}
				else
				{
					throw;
				}
			}

			return result;
		}

		/// <summary>
		/// Deletes the user's current chosen organization.
		/// </summary>
		/// <returns>Returns false if permissions fail.</returns>
		public async Task DeleteOrganization(int orgId)
		{
			if (orgId <= 0) throw new ArgumentOutOfRangeException(nameof(orgId));
			await CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Delete, AppEntity.Organization, orgId);
			await this.DBHelper.UpdateOrganizationStatusAsync(orgId, (int)OrganizationStatusEnum.Inactive);
		}

		/// <summary>
		/// Creates an invitation for a new user in the database, and also sends an email to the new user with their access code.
		/// </summary>
		public async Task<int> InviteUser(string url, string email, string firstName, string lastName, int organizationId, string organizationName, OrganizationRoleEnum organizationRoleId, string employeedId, string prodJson, int? employeetypeId)
		{
			if (organizationId <= 0) throw new ArgumentOutOfRangeException(nameof(organizationId));
			await CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Create, AppEntity.OrganizationUser, organizationId);
			if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
			if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException(nameof(firstName));
			if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException(nameof(lastName));
			if (string.IsNullOrWhiteSpace(employeedId)) throw new ArgumentNullException(nameof(employeedId));

			int empTypeId = 0;
			if (employeetypeId == null)
			{
				var employeeType = (await GetEmployeeTypeByOrganization(organizationId)).First();
				empTypeId = employeeType.EmployeeTypeId;
			}
			else
			{
				empTypeId = employeetypeId.Value;
			}
			// Creation of invitation
			var result = await DBHelper.CreateInvitation(email, firstName, lastName, organizationId, (int)organizationRoleId, empTypeId, employeedId, prodJson);

			switch (result)
			{
				case -1:
					throw new DuplicateNameException("User is already a member of the organization.");
				case -2:
					throw new InvalidOperationException("Employee Id is already taken.");
			}

			SendInviteEmail(url, email);

			// Return invitation id
			return result;
		}

		/// <summary>
		/// Send email for invite.
		/// </summary>
		/// <param name="url"></param>
		/// <param name="email"></param>
		public void SendInviteEmail(string url, string email)
		{
			// Send invitation email
			string orgName = string.Empty;
			string htmlbody = string.Format(
				"{0} {1} has requested you join their organization {2} on Allyis Apps!<br /> Click <a href={3}>Here</a> to create an account and join!",
				UserContext.FirstName,
				UserContext.LastName,
				orgName,
				url);

			string msgbody = new System.Web.HtmlString(htmlbody).ToString();
			var task = this.Mailer.SendEmailAsync(
				ServiceSettings.SupportEmail,
				email,
				"Join Allyis Apps!",
				msgbody);
			// TODO: how to indicate there was an error sending the email? how to send the invite email again in that case?
			var mailSuccess = task.Result;
		}

		public async void NotifyInviteAcceptAsync(int invitationId)
		{
			if (invitationId <= 0) throw new ArgumentException(nameof(invitationId));
			var invitation = await DBHelper.GetInvitation(invitationId);
			string orgName = string.Empty;
			string msgbody = string.Format("{0} {1} has joined your organization {2} on Allyis Apps!", invitation.FirstName, invitation.LastName, orgName);
			foreach (string email in DBHelper.GetOrganizationOwnerEmails(invitation.OrganizationId))
			{
				await this.Mailer.SendEmailAsync(
						ServiceSettings.SupportEmail,
						email,
						"New member join notification.",
						msgbody);
			}
		}

		public async void NotifyInviteRejectAsync(int invitationId)
		{
			if (invitationId <= 0) throw new ArgumentException(nameof(invitationId));
			var invitation = await DBHelper.GetInvitation(invitationId);
			string orgName = string.Empty;
			string msgbody = string.Format("{0} {1} has rejected joining your organization {2} on Allyis Apps!", invitation.FirstName, invitation.LastName, orgName);
			foreach (string email in DBHelper.GetOrganizationOwnerEmails(invitation.OrganizationId))
			{
				await this.Mailer.SendEmailAsync(
						ServiceSettings.SupportEmail,
						email,
						"Invite reject notification.",
						msgbody);
			}
		}

		/// <summary>
		/// Removes an invitation.
		/// </summary>
		/// <param name="invitationId">Invitation Id.</param>
		/// <returns>Returns false if permissions fail.</returns>
		public async Task<bool> RemoveInvitation(int invitationId)
		{
			if (invitationId <= 0) throw new ArgumentException(nameof(invitationId));

			var invitation = await GetInvitation(invitationId);
			await CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Delete, AppEntity.OrganizationUser, invitation.OrganizationId);
			return DBHelper.DeleteInvitation(invitationId);
		}

		/// <summary>
		/// Removes an invitation.
		/// </summary>
		/// <param name="InvitationIds">Invitation Id.</param>
		/// <param name="orgId">organizaitons Id. </param>>
		/// <returns>Returns false if permissions fail.</returns>
		public async Task<bool> RemoveInvitations(int[] InvitationIds, int orgId)
		{
			if (InvitationIds[0] <= 0) throw new ArgumentException("invitationId");

			bool worked = true;
			await CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Delete, AppEntity.OrganizationUser, orgId);

			foreach (var invitationId in InvitationIds)
			{
				var invite = await GetInvitation(invitationId);
				if (!DBHelper.DeleteInvitation(invitationId)) worked = false;
			}

			return worked;
		}

		/// <summary>
		/// Gets the member list for an organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>The member list.</returns>
		public IEnumerable<OrganizationUser> GetOrganizationMemberList(int orgId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(orgId), "Organization Id cannot be negative.");
			}

			return DBHelper.GetOrganizationMemberList(orgId).Select(InitializeOrganizationUser);
		}

		/// <summary>
		/// get the list of users in the given organization
		/// </summary>
		public async Task<List<OrganizationUser>> GetOrganizationUsersAsync(int orgId, bool loadAddress = true)
		{
			if (orgId <= 0) throw new ArgumentOutOfRangeException(nameof(orgId));

			await CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Read, AppEntity.OrganizationUser, orgId);
			var ouDicy = await this.DBHelper.GetOrganizationUsersAsync(orgId);
			var uDicy = await this.DBHelper.GetUsersbyIdsAsync(ouDicy.Keys.ToList());
			var result = new List<OrganizationUser>();
			foreach (var item in uDicy.Values)
			{
				result.Add(new OrganizationUser(await this.InitializeUserAsync(item, loadAddress)));
			}

			foreach (var item in result)
			{
				item.EmployeeId = ouDicy[item.UserId].EmployeeId;
				item.EmployeeTypeId = ouDicy[item.UserId].EmployeeTypeId;
				item.ExpenseApprovalLimit = ouDicy[item.UserId].MaxAmount;
				item.OrganizationRoleId = ouDicy[item.UserId].OrganizationRoleId;
				item.OrganizationUserCreatedUtc = ouDicy[item.UserId].OrganizationUserCreatedUtc;
			}

			return result;
		}

		public async Task<int> GetOrganizationInvitationCountAsync(int orgId, InvitationStatusEnum statusMask)
		{
			if (orgId <= 0) throw new ArgumentOutOfRangeException(nameof(orgId));
			await CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Read, AppEntity.OrganizationUser, orgId);

			return await this.DBHelper.GetOrganizationInvitationCountAsync(orgId, (int)statusMask);
		}

		/// <summary>
		/// Gets all the projects in an organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="onlyActive">True (default) to only return active projects, false to include all projects, active or not.</param>
		/// <returns>A list of project info objects based on Organization.</returns>
		async public Task<IEnumerable<CompleteProject>> GetProjectsByOrganization(int orgId, bool onlyActive = true)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(orgId), "Organization Id cannot be negative.");
			}

			return (await DBHelper.GetProjectsByOrgId(orgId, onlyActive ? 1 : 0)).Select(InitializeCompleteProjectInfo);
		}

		/// <summary>
		/// Assigns a new organization role to the given users for the current organization.
		/// </summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="newOrganizationRole">Organization role to assign, or -1 to remove from organization.</param>
		/// <param name="organizationId">The organization Id.</param>
		/// <returns>The number of affected users.</returns>
		public int UpdateOrganizationUsersRole(List<int> userIds, int newOrganizationRole, int organizationId)
		{
			#region Validation

			if (!Enum.IsDefined(typeof(OrganizationRoleEnum), newOrganizationRole))
			{
				throw new ArgumentOutOfRangeException(nameof(newOrganizationRole), "Organization role must match a value of the OrganizationRole enum.");
			}
			foreach (int id in userIds)
			{
				if (id == UserContext.UserId) throw new ArgumentException("Can't change self role in the organization.", nameof(userIds));
			}
			if (userIds == null || userIds.Count == 0)
			{
				throw new ArgumentException("userIds", "No user ids provided.");
			}

			#endregion Validation

			return DBHelper.UpdateOrganizationUsersRole(userIds, organizationId, newOrganizationRole);
		}

		/// <summary>
		/// Deletes all users from the org that are in the given userIds list.
		/// </summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="organizationId">The organization Id.</param>
		/// <returns>The number of affected users.</returns>
		public async Task<int> DeleteOrganizationUsers(List<int> userIds, int organizationId)
		{
			#region Validation

			if (userIds == null || userIds.Count == 0)
			{
				throw new ArgumentNullException(nameof(userIds), "No user ids provided.");
			}
			if (userIds.Any(id => id == UserContext.UserId))
			{
				throw new ArgumentException("Cannot delete self from the organization.", nameof(userIds));
			}

			#endregion Validation

			return await DBHelper.DeleteOrganizationUsers(userIds, organizationId);
		}

        /// <summary>
        /// Gets the product role for a user.
        /// </summary>
        /// <param name="subscriptionId">The subscription that the user belongs to.</param>
        /// <param name="productRoleId">User Id.</param>
        /// <returns>The product role id for the user in the subscription. Returns null if user is not in the subscription or the subscription is inactive.</returns>
        public Task<string> GetProductRoleName(int subscriptionId, int productRoleId)
        {
            #region Validation

            if (productRoleId <= 0) throw new ArgumentOutOfRangeException(nameof(productRoleId), "ProductRole Id must be greater than 0.");
            if (subscriptionId <= 0) throw new ArgumentOutOfRangeException(nameof(subscriptionId), "Subscription Id must be greater than 0.");

            #endregion Validation

            return DBHelper.GetProductRoleName(subscriptionId, productRoleId);
        }

        /// <summary>
        /// Gets the product role for a user.
        /// </summary>
        /// <param name="subscriptionId">The subscription that the user belongs to.</param>
        /// <param name="userId">User Id.</param>
        /// <returns>The product role id for the user in the subscription. Returns null if user is not in the subscription or the subscription is inactive.</returns>
        public Task<int?> GetSubscriptionRoleForUser(int subscriptionId, int userId)
		{
			#region Validation

			if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId), "User Id must be greater than 0.");
			if (subscriptionId <= 0) throw new ArgumentOutOfRangeException(nameof(subscriptionId), "Subscription Id must be greater than 0.");

			#endregion Validation

			return DBHelper.GetSubscriptionRoleForUser(subscriptionId, userId);
		}

		#region Info-DBEntity Conversions

		/// <summary>
		/// Translates an OrganizationUserDBEntity into an OrganizationUser business object.
		/// </summary>
		/// <param name="organizationUser">OrganizationUserDBEntity instance.</param>
		/// <returns>OrganizationUser instance.</returns>
		public static OrganizationUser InitializeOrganizationUser(OrganizationUserDBEntity organizationUser)
		{
			if (organizationUser == null)
			{
				return null;
			}

			return new OrganizationUser
			{
				OrganizationUserCreatedUtc = organizationUser.OrganizationUserCreatedUtc,
				EmployeeId = organizationUser.EmployeeId,
				OrganizationId = organizationUser.OrganizationId,
				OrganizationRoleId = organizationUser.OrganizationRoleId,
				EmployeeTypeId = organizationUser.EmployeeTypeId,
				UserId = organizationUser.UserId,
				ExpenseApprovalLimit = organizationUser.MaxAmount
			};
		}

		public OrganizationUser InitializeOrganizationUser(dynamic entity)
		{
			if (entity == null) return null;

			var result = new OrganizationUser();
			result.AccessFailedCount = entity.AccessFailedCount;
			result.Address = InitializeAddress(entity);
			result.DateOfBirth = entity.DateOfBirth;
			result.Email = entity.Email;
			result.EmailConfirmationCode = entity.EmailConfirmationCode;
			result.EmployeeId = entity.EmployeeId;
			//result.Invitations =
			result.FirstName = entity.FirstName;
			result.IsEmailConfirmed = entity.IsEmailConfirmed;
			result.IsLockoutEnabled = entity.IsLockoutEnabled;
			result.IsPhoneNumberConfirmed = entity.IsPhoneNumberConfirmed;
			result.IsTwoFactorEnabled = entity.IsTwoFactorEnabled;
			result.LastName = entity.LastName;
			result.LastUsedSubscriptionId = entity.LastUsedSubscriptionId;
			result.LockoutEndDateUtc = entity.LockoutEndDateUtc;
			result.ExpenseApprovalLimit = entity.MaxAmount;
			result.OrganizationId = entity.OrganizationId;
			result.OrganizationRoleId = entity.OrganizationRoleId;
			//result.Organizations =
			result.OrganizationUserCreatedUtc = entity.OrganizationUserCreatedUtc;
			result.PasswordHash = entity.PasswordHash;
			result.PasswordResetCode = entity.PasswordResetCode;
			result.PhoneExtension = entity.PhoneExtension;
			result.PhoneNumber = entity.PhoneNumber;
			//result.Subscriptions =
			result.UserCreatedUtc = entity.UserCreatedUtc;
			result.UserId = entity.UserId;

			result.EmployeeTypeId = entity.EmployeeTypeId;
			return result;
		}

		/// <summary>
		/// Translates an OrganizationDBEntity into an Organization business object.
		/// </summary>
		/// <param name="organizationInfo">OrganizationDBEntity instance.</param>
		/// <returns>Organization instance.</returns>
		public Organization InitializeOrganization(dynamic organizationInfo)
		{
			if (organizationInfo == null)
			{
				return null;
			}
			Address address = null;

			if (organizationInfo.AddressId != null)
			{
				address = InitializeAddress(organizationInfo);
			}

			return new Organization
			{
				CreatedUtc = organizationInfo.CreatedUtc,
				FaxNumber = organizationInfo.FaxNumber,
				OrganizationName = organizationInfo.OrganizationName,
				OrganizationId = organizationInfo.OrganizationId,
				PhoneNumber = organizationInfo.PhoneNumber,
				SiteUrl = organizationInfo.SiteUrl,
				Subdomain = organizationInfo.Subdomain,
				Address = address,
				UserCount = organizationInfo.UserCount
			};
		}

		/// <summary>
		/// Translates an InvitationDBEntity into an Invitation business object.
		/// </summary>
		/// <param name="invitation">InvitationDBEntity instance.</param>
		/// <returns>Invitation instance.</returns>
		public static Invitation InitializeInvitationInfo(InvitationDBEntity invitation)
		{
			if (invitation == null)
			{
				return null;
			}

			return new Invitation
			{
				Email = invitation.Email,
				FirstName = invitation.FirstName,
				InvitationId = invitation.InvitationId,
				LastName = invitation.LastName,
				OrganizationId = invitation.OrganizationId,
				EmployeeId = invitation.EmployeeId,
				DecisionDateUtc = invitation.DecisionDateUtc,
				InvitationStatus = (InvitationStatusEnum)invitation.InvitationStatus,
				ProductRolesJson = invitation.ProductRolesJson
			};
		}

		/// <summary>
		/// Translates a ProductDBEntity into a Product business object.
		/// </summary>
		/// <param name="product">ProductDBEntity instance.</param>
		/// <returns>Product instance.</returns>
		public static Product InitializeProduct(ProductDBEntity product)
		{
			if (product == null)
			{
				return null;
			}

			return new Product
			{
				ProductDescription = product.Description,
				ProductId = (ProductIdEnum)product.ProductId,
				ProductName = product.ProductName,
				AreaUrl = product.AreaUrl
			};
		}

		/// <summary>
		/// Translates an Invitation business object into an InvitationDBEntity.
		/// </summary>
		/// <param name="invitation">Invitation instance.</param>
		/// <returns>InvitationDBEntity instance.</returns>
		public static InvitationDBEntity GetDBEntityFromInvitationInfo(Invitation invitation)
		{
			if (invitation == null)
			{
				return null;
			}

			return new InvitationDBEntity
			{
				Email = invitation.Email,
				FirstName = invitation.FirstName,
				InvitationId = invitation.InvitationId,
				LastName = invitation.LastName,
				OrganizationId = invitation.OrganizationId,
				EmployeeId = invitation.EmployeeId,
				DecisionDateUtc = invitation.DecisionDateUtc,
				InvitationStatus = (int)invitation.InvitationStatus,
				ProductRolesJson = invitation.ProductRolesJson
			};
		}

		#endregion Info-DBEntity Conversions
	}
}