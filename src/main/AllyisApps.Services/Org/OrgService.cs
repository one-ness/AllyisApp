//------------------------------------------------------------------------------
// <copyright file="OrgService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.TimeTracker;
using AllyisApps.Services.Billing;
using AllyisApps.Services.TimeTracker;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all Organization related services.
	/// </summary>
	public partial class Service : BaseService
	{
		/// <summary>
		/// Gets the subdomain name from the organization Id.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>Subdomain name.</returns>
		public static string GetSubdomainById(int orgId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			return DBHelper.Instance.GetSubdomainById(orgId);
		}

		/// <summary>
		/// Gets the organization Id from the subdomain name.
		/// </summary>
		/// <param name="subdomain">Subdomain name.</param>
		/// <returns>Organization id.</returns>
		public static int GetIdBySubdomain(string subdomain)
		{
			//// This is actually checked automatically on any change of the subdomain field, even selecting (or clearing) it.
			//// A null value needs to be allowed or an exception is thrown before you even type anything.
			////if (subdomain == null)
			////{
			////	throw new ArgumentNullException("subdomain", "Subdomain name must not be null.");
			////}

			return DBHelper.Instance.GetIdBySubdomain(subdomain);
		}

		/// <summary>
		/// Creates an organization.
		/// </summary>
		/// <param name="organization">Organization info.</param>
		/// <param name="ownerId">Organization owner user Id.</param>
		/// <param name="employeeId">Organization owner employee Id.</param>
		/// <returns>Organizaiton Id.</returns>
		public int CreateOrganization(OrganizationInfo organization, int ownerId, string employeeId)
		{
			if (organization == null)
			{
				throw new ArgumentNullException("organization", "Organization must not be null.");
			}

			if (ownerId <= 0)
			{
				throw new ArgumentOutOfRangeException("ownerId", "Organization owner's user id cannot be 0 or negative.");
			}

			return DBHelper.CreateOrganization(GetDBEntityFromOrganizationInfo(organization), ownerId, (int)OrganizationRole.Owner, employeeId);
		}

		/// <summary>
		/// Gets an <see cref="OrganizationInfo"/>.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>The OrganizationInfo.</returns>
		public OrganizationInfo GetOrganization(int orgId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			return InitializeOrganizationInfo(DBHelper.GetOrganization(orgId));
		}

		/// <summary>
		/// Gets the OrganizationInfo for the current chosen organization, along with OrganizationUserInfos for each user in the
		/// organization, SubscriptionDisplayInfos for any subscriptions in the organization, InvitationInfos for any invitiations
		/// pending in the organization, and the organization's billing stripe handle.
		/// </summary>
		/// <returns></returns>
		public Tuple<OrganizationInfo, List<OrganizationUserInfo>, List<SubscriptionDisplayInfo>, List<InvitationInfo>, string, List<ProductInfo>> GetOrganizationManagementInfo()
		{
			var spResults = DBHelper.GetOrganizationManagementInfo(UserContext.ChosenOrganizationId);
			return Tuple.Create(
				InitializeOrganizationInfo(spResults.Item1),
				spResults.Item2.Select(oudb => InitializeOrganizationUserInfo(oudb)).ToList(),
				spResults.Item3.Select(sddb => InitializeSubscriptionDisplayInfo(sddb)).ToList(),
				spResults.Item4.Select(idb => InitializeInvitationInfo(idb)).ToList(),
				spResults.Item5,
				spResults.Item6.Select(pdb => InitializeProductInfo(pdb)).ToList());
		}

		/// <summary>
		/// Gets the OrganizationInfo for the current chosen organization, along with the list of valid countries and the
		/// employee id for the current user in the current chosen organization.
		/// </summary>
		/// <returns></returns>
		public Tuple<OrganizationInfo, List<string>, string> GetOrgWithCountriesAndEmployeeId()
		{
			var spResults = DBHelper.GetOrgWithCountriesAndEmployeeId(UserContext.ChosenOrganizationId, UserContext.UserId);
			return Tuple.Create(
				InitializeOrganizationInfo(spResults.Item1),
				spResults.Item2,
				spResults.Item3);
		}

		/// <summary>
		/// Updates an organization chosen by the current user.
		/// </summary>
		/// <param name="organization">Updated organization info.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool UpdateOrganization(OrganizationInfo organization)
		{
			if (organization == null)
			{
				throw new ArgumentNullException("organization", "Organization must not be null.");
			}
			int existingSubdomainId = Service.GetIdBySubdomain(organization.Subdomain);
			if (existingSubdomainId != 0 && existingSubdomainId != organization.OrganizationId)
			{
				throw new ArgumentException("organization", "Subdomain is already taken.");
			}

			if (this.Can(Actions.CoreAction.EditOrganization))
			{
				DBHelper.UpdateOrganization(GetDBEntityFromOrganizationInfo(organization));

				return true;
			}

			return false;
		}

		/// <summary>
		/// Updates the active organization.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="orgId">Organization Id.</param>
		public void UpdateActiveOrganization(int userId, int orgId)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			// Note: This method cannot use UserContext.UserId because this method is called before the service obejct's UserContext is set.
			DBHelper.UpdateActiveOrganization(userId, orgId);
		}

		/// <summary>
		/// Gets the role of the given user in the given organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="userId">User Id.</param>
		/// <returns>An OrgRoleInfo instance.</returns>
		public OrgRoleInfo GetOrgRole(int orgId, int userId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			OrgRoleDBEntity role = DBHelper.GetPermissionLevel(orgId, userId);
			if (role == null)
			{
				return null;
			}

			return new OrgRoleInfo
			{
				OrgRoleId = role.OrgRoleId,
				OrgRoleName = role.Name
			};
		}

		/// <summary>
		/// Adds a user to an organization and project.
		/// </summary>
		/// <param name="userId">Id of user to add.</param>
		/// <param name="orgId">Id of organization to add user to.</param>
		/// <param name="projectId">Id of project to add user to.</param>
		/// <param name="orgRole">The role to add the user as.</param>
		/// <param name="employeeId">An Id for the employee to be used by the organization</param>
		public void AddToOrganization(int userId, int orgId, int projectId, int orgRole, string employeeId)
		{
			#region Validation

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			if (!Enum.IsDefined(typeof(OrganizationRoleIdEnum), orgRole))
			{
				throw new ArgumentOutOfRangeException("orgRole", "Organization role value must correspond to a defined role in OrganizationRoleIdEnum.");
			}
			if (employeeId == null)
			{
				throw new ArgumentOutOfRangeException("employeeId", "The EmployeeId must not be null");
			}

			#endregion Validation

			DBHelper.CreateOrganizationUser(new OrganizationUserDBEntity() // ...add them to that organization as a member
			{
				UserId = userId,
				OrganizationId = orgId,
				OrgRoleId = orgRole,
				EmployeeId = employeeId
			});

			DBHelper.CreateProjectUser(projectId, userId);
		}

		/// <summary>
		/// Deletes the user's current chosen organization.
		/// </summary>
		/// <returns>Returns false if permissions fail.</returns>
		public bool DeleteOrganization()
		{
			if (this.Can(Actions.CoreAction.EditOrganization))
			{
				DBHelper.DeleteOrganization(UserContext.ChosenOrganizationId);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates an invitation for a new user in the database, and also sends an email to the new user with their access code.
		/// </summary>
		/// <param name="requestingUserFullName">Full name of requesting user (not invitee).</param>
		/// <param name="webRoot">The url webroot, taken from Global Settings.</param>
		/// <param name="invitationInfo">An <see cref="InvitationInfo"/> with invitee information filled out.</param>
		/// <returns>The invitation Id.</returns>
		public async Task<int> InviteNewUser(string requestingUserFullName, string webRoot, InvitationInfo invitationInfo)
		{
			#region Validation

			if (string.IsNullOrEmpty(requestingUserFullName))
			{
				throw new ArgumentNullException("requestingUserFullName", "Requesting user full name must have a value.");
			}

			if (string.IsNullOrEmpty(webRoot))
			{
				throw new ArgumentNullException("webRoot", "Webroot must have a value.");
			}

			if (invitationInfo == null)
			{
				throw new ArgumentNullException("invitationInfo", "Invitation info object must not be null.");
			}

			#endregion Validation

			//EmailService mail = new EmailService();
			OrganizationInfo orgInfo = this.GetOrganization(invitationInfo.OrganizationId);

			//string htmlbody = string.Format(
			//	"{0} has requested you join their organization on Allyis Apps, {1}!<br /> Click <a href=http://{2}.{3}/Account/Index?accessCode={4}>Here</a> to create an account and join!",
			//	requestingUserFullName,
			//	orgInfo.Name,
			//	GetSubdomainById(invitationInfo.OrganizationId),
			//	webRoot,
			//	invitationInfo.AccessCode);

			string htmlbody = string.Format(
				"{0} has requested you join their organization on Allyis Apps, {1}!<br /> Click <a href=http://{2}/Account/Index?accessCode={3}>Here</a> to create an account and join!",
				requestingUserFullName,
				orgInfo.Name,
				webRoot,
				invitationInfo.AccessCode);

			string msgbody = new System.Web.HtmlString(htmlbody).ToString();
			await Lib.Mailer.SendEmailAsync(
				"noreply@allyis.com",
				invitationInfo.Email,
				"Join Allyis Apps!",
				msgbody);
			//await mail.CreateMessage(
			//msgbody,
			//invitationInfo.Email,
			//"Join Allyis Apps!");

			return DBHelper.CreateUserInvitation(GetDBEntityFromInvitationInfo(invitationInfo));
		}

		/// <summary>
		/// Removes an invitation.
		/// </summary>
		/// <param name="invitationId">Invitation Id.</param>
		/// <returns>Returns false if permissions fail.</returns>
		public bool RemoveInvitation(int invitationId)
		{
			if (invitationId <= 0)
			{
				throw new ArgumentOutOfRangeException("invitationId", "Invitation Id cannot be 0 or negative.");
			}

			if (this.Can(Actions.CoreAction.EditOrganization))
			{
				IEnumerable<InvitationInfo> invites = this.GetUserInvitations();
				InvitationInfo thisInvite = invites.Where(x => x.InvitationId == invitationId).SingleOrDefault();
				IEnumerable<SubscriptionDisplayInfo> subs = this.DBHelper.GetSubscriptionsDisplayByOrg(UserContext.ChosenOrganizationId).Select(s => InitializeSubscriptionDisplayInfo(s));
				IEnumerable<InvitationSubRoleInfo> subRoles = DBHelper.GetInvitationSubRolesByInvitationId(invitationId).Select(i => InitializeInvitationSubRoleInfo(i));
				foreach (InvitationSubRoleInfo subRole in subRoles)
				{
					SubscriptionDisplayInfo currentSub = subs.Where(x => x.SubscriptionId == subRole.SubscriptionId).SingleOrDefault();
					if (currentSub != null && currentSub.SubscriptionsUsed < currentSub.NumberOfUsers)
					{
						DBHelper.Instance.DeleteInvitationSubRole(subRole.InvitationId, subRole.SubscriptionId);
					}
				}

				DBHelper.RemoveUserInvitation(invitationId);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Getst a list of the user invitations for the current organization.
		/// </summary>
		/// <returns>List of InvitationInfos of organization's user invitations.</returns>
		public IEnumerable<InvitationInfo> GetUserInvitations()
		{
			return DBHelper.GetUserInvitationsByOrgId(UserContext.ChosenOrganizationId).Select(i => InitializeInvitationInfo(i));
		}

		/// <summary>
		/// Creates a subscription role for an invitation.
		/// </summary>
		/// <param name="invitationId">Invitation id.</param>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <param name="selectedRole">Selected role.</param>
		public void CreateInvitationSubRole(int invitationId, int subscriptionId, int selectedRole)
		{
			#region Validation

			if (invitationId <= 0)
			{
				throw new ArgumentOutOfRangeException("invitationId", "Invitation Id cannot be 0 or negative.");
			}

			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription Id cannot be 0 or negative.");
			}

			if (selectedRole <= 0)
			{ // TODO: Figure out if there is any further validation that can be done for this number.
				throw new ArgumentOutOfRangeException("selectedRole", "Selected role cannot be negative.");
			}

			#endregion Validation

			DBHelper.CreateInvitationSubRole(invitationId, subscriptionId, selectedRole);
		}

		/// <summary>
		/// Updates a user's subscription product role.
		/// </summary>
		/// <param name="selectedRole">The Role.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <param name="userId">User Id.</param>
		public void UpdateSubscriptionUserProductRole(int selectedRole, int subscriptionId, int userId)
		{
			#region Validation

			if (selectedRole <= 0)
			{ // TODO: Figure out if there is any further validation that can be done for this number.
				throw new ArgumentOutOfRangeException("selectedRole", "Selected role cannot be negative.");
			}

			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			#endregion Validation

			DBHelper.UpdateSubscriptionUserProductRole(selectedRole, subscriptionId, userId);
		}

		/// <summary>
		/// Gets the member list for an organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>The member list.</returns>
		public IEnumerable<OrganizationUserInfo> GetOrganizationMemberList(int orgId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			return DBHelper.GetOrganizationMemberList(orgId).Select(o => InitializeOrganizationUserInfo(o));
		}

		// TODO: Look more closely at the use of this method in UploadCsvFileAction to see if some other existing service method can be used instead, and this one retired.

		/// <summary>
		/// Gets the first name of a user in the current organization by email.
		/// </summary>
		/// <param name="email">Email address.</param>
		/// <returns>User's first name.</returns>
		public string GetOrgUserFirstName(string email)
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

			#endregion Validation

			return DBHelper.GetOrgUserFirstName(UserContext.ChosenOrganizationId, email);
		}

		/// <summary>
		/// Gets a list of subscription details for the current organization.
		/// </summary>
		/// <returns><see cref="IEnumerable{SubscriptionInfo}"/></returns>
		public IEnumerable<SubscriptionInfo> GetSubscriptionDetails()
		{
			IEnumerable<SubscriptionDBEntity> subDBEList = DBHelper.GetSubscriptionDetails(UserContext.ChosenOrganizationId);
			List<SubscriptionInfo> list = new List<SubscriptionInfo>();
			foreach (SubscriptionDBEntity subDBE in subDBEList)
			{
				if (subDBE != null)
				{
					list.Add(new SubscriptionInfo
					{
						OrganizationName = subDBE.OrganizationName,
						OrganizationId = subDBE.OrganizationId,
						SubscriptionId = subDBE.SubscriptionId,
						SkuId = subDBE.SkuId,
						NumberOfUsers = subDBE.NumberOfUsers,
						Licenses = subDBE.Licenses,
						CreatedUTC = subDBE.CreatedUTC,
						IsActive = subDBE.IsActive,
						Name = subDBE.Name
					});
				}
			}

			return list;
		}

		/// <summary>
		/// Gets the EmployeeId for the given user and org.
		/// </summary>
		/// <param name="userId">The userId.</param>
		/// <param name="orgId">The orgId.</param>
		/// <returns>The employeeId.</returns>
		public string GetEmployeeId(int userId, int orgId)
		{
			#region Validation

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			#endregion Validation

			return DBHelper.GetEmployeeId(userId, orgId);
		}

		/// <summary>
		/// Updates an organization User.
		/// </summary>
		/// <param name="userId">The updated user ID.</param>
		/// <param name="orgId">The updated org id.</param>
		/// <param name="orgRoleId">The updated org role id.</param>
		/// <param name="EmployeeId">The updated employee id. If left null, it will be the current employee id.</param>
		public void UpdateOrganizationUser(int userId, int orgId, int orgRoleId, string EmployeeId = null)
		{
			#region Validation

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			if (!Enum.IsDefined(typeof(OrganizationRoleIdEnum), orgRoleId))
			{
				throw new ArgumentOutOfRangeException("orgRoleId", "Organization role value must correspond to a defined role in OrganizationRoleIdEnum.");
			}

			#endregion Validation

			DBHelper.UpdateOrganizationUser(new OrganizationUserDBEntity
			{
				UserId = userId,
				OrganizationId = orgId,
				OrgRoleId = orgRoleId,
				EmployeeId = EmployeeId == null ? this.GetEmployeeId(userId, orgId) : EmployeeId
			});
		}

		/// <summary>
		/// Removes an organization user.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="userId">User Id.</param>
		public void RemoveOrganizationUser(int orgId, int userId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			DBHelper.RemoveOrganizationUser(orgId, userId);
		}

		/// <summary>
		/// Gets a list of <see cref="InvitationSubRoleInfo"/>s for the current organization.
		/// </summary>
		/// <returns>List of InvitationSubRoleInfos.</returns>
		public IEnumerable<InvitationSubRoleInfo> GetInvitationSubRoles(int organizationId = -1)
		{
			if (organizationId == -1) organizationId = UserContext.ChosenOrganizationId;

			return DBHelper.GetInvitationSubRolesByOrganizationId(organizationId).Select(i => InitializeInvitationSubRoleInfo(i));
		}

		/// <summary>
		/// Gets a list of <see cref="SubscriptionUserInfo"/>'s for the current organization and subscription.
		/// </summary>
		/// <returns>A list of SubscriptionUserInfos for the given organization and subscription.</returns>
		public IEnumerable<SubscriptionUserInfo> GetUsers()
		{
			IEnumerable<SubscriptionUserDBEntity> sui = DBHelper.GetUsersByOrganization(UserContext.ChosenOrganizationId, UserContext.ChosenSubscriptionId);
			List<SubscriptionUserInfo> list = new List<SubscriptionUserInfo>();
			foreach (SubscriptionUserDBEntity dbe in sui)
			{
				if (dbe != null)
				{
					list.Add(new SubscriptionUserInfo
					{
						FirstName = dbe.FirstName,
						LastName = dbe.LastName,
						ProductRoleId = dbe.ProductRoleId,
						ProductRoleName = dbe.ProductRoleName,
						UserId = dbe.UserId,
						CreatedUTC = dbe.CreatedUTC,
						SubscriptionId = dbe.SubscriptionId,
						SkuId = dbe.SkuId
					});
				}
			}

			return list;
		}

		// Note: This is in OrgService and not ProjectService because it is used at least once outside of a product area (i.e. there's no
		// instance of ProjectService there).

		/// <summary>
		/// Gets all the projects in an organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="isActive">True (default) to only return active projects, false to include all projects, active or not.</param>
		/// <returns>A list of project info objects based on Organization.</returns>
		public IEnumerable<CompleteProjectInfo> GetProjectsByOrganization(int orgId, bool isActive = true)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			return DBHelper.GetProjectsByOrgId(orgId, isActive ? 1 : 0).Select(c => InitializeCompleteProjectInfo(c));
		}

		/// <summary>
		/// Gets the user roles for an organization.
		/// </summary>
		/// <returns>List of UserRolesInfos.</returns>
		public IEnumerable<UserRolesInfo> GetUserRoles()
		{
			return DBHelper.GetRoles(UserContext.ChosenOrganizationId).Select(o => InitializeUserRolesInfo(o));
		}

		/// <summary>
		/// Gets a recommended EmployeeId that does not yet exist in the org
		/// </summary>
		/// <returns></returns>
		public string GetRecommendedEmployeeId()
		{
			// return this.IncrementAlphanumericCharArray(this.GetOrganizationMemberList(orgId).LastOrDefault().EmployeeId.ToCharArray()).ToString();
			return new string(this.IncrementAlphanumericCharArray(this.GetOrganizationMemberList(this.UserContext.ChosenOrganizationId).Select(user => user.EmployeeId).ToList().Union( // Get a list of all employee ids in the org combined with
				this.GetUserInvitations().Select(invitation => invitation.EmployeeId).ToList()).OrderBy(id => id).LastOrDefault().ToCharArray()));                // the invitations of the org, then look at the latest one and increment it
																																								  // TODO: Make a db procedure and all subsequent methods to simply grab all of the ids instead of using this list union
		}

		/// <summary>
		/// Import data from a workbook. Imports customers, projects, users, project/user relationships, and/or time entry data.
		/// </summary>
		/// <param name="importData">Workbook with data to import.</param>
		public ImportActionResult Import(DataSet importData)
		{
			// For some reason, linq won't work directly with DataSets, so we start by just moving the tables over to a linq-able List
			// The tables are ranked and sorted in order to get customers to import first, before projects, avoiding some very complicated look-up logic.
			List<DataTable> tables = new List<DataTable>();
			List<Tuple<DataTable, int>> sortableTables = new List<Tuple<DataTable, int>>();
			foreach (DataTable table in importData.Tables)
			{
				int rank = (table.Columns.Contains(ColumnHeaders.CustomerName) || table.Columns.Contains(ColumnHeaders.CustomerId) ? 3 : 0);
				rank = table.Columns.Contains(ColumnHeaders.ProjectName) || table.Columns.Contains(ColumnHeaders.ProjectId) ? rank == 3 ? 2 : 1 : rank;
				sortableTables.Add(new Tuple<DataTable, int>(table, rank));
			}
			tables = sortableTables.OrderBy(tup => tup.Item2 * -1).Select(tup => tup.Item1).ToList();

			// Retrieval of existing customer and project data
			List<Tuple<CustomerInfo, List<ProjectInfo>>> customersProjects = new List<Tuple<CustomerInfo, List<ProjectInfo>>>();
			foreach (CustomerInfo customer in this.GetCustomerList(this.UserContext.ChosenOrganizationId))
			{
				customersProjects.Add(new Tuple<CustomerInfo, List<ProjectInfo>>(
					customer,
					this.GetProjectsByCustomer(customer.CustomerId).ToList()
				));
			}

			// Retrieval of existing user data
			List<Tuple<string, UserInfo>> users = this.GetOrganizationMemberList(this.UserContext.ChosenOrganizationId).Select(o => new Tuple<string, UserInfo>(o.EmployeeId, this.GetUserInfo(o.UserId))).ToList();

			// Retrieval of existing user product subscription data
			int ttProductId = Service.GetProductIdByName("TimeTracker");
			SubscriptionDisplayDBEntity ttSub = DBHelper.GetSubscriptionsDisplayByOrg(this.UserContext.ChosenOrganizationId).Where(s => s.ProductId == ttProductId).SingleOrDefault();
			List<UserInfo> userSubs = this.GetUsersWithSubscriptionToProductInOrganization(this.UserContext.ChosenOrganizationId, ttProductId).ToList();

			// Retrieval of existing pay class data
			List<PayClassInfo> payClasses = DBHelper.GetPayClasses(UserContext.ChosenOrganizationId).Select(pc => InitializePayClassInfo(pc)).ToList();

			// Result object
			ImportActionResult result = new ImportActionResult();

			// Loop through and see what can be imported from each table in turn. Order doesn't matter, since missing information
			// will be sought from other tables as needed.
			foreach (DataTable table in tables)
			{
				#region Column Header Checks

				// Customer importing: requires both customer name and customer id. Other information is optional, and can be filled in later.
				bool hasCustomerName = table.Columns.Contains(ColumnHeaders.CustomerName);
				bool hasCustomerId = table.Columns.Contains(ColumnHeaders.CustomerId);
				bool canCreateCustomers = hasCustomerName && hasCustomerId;
				List<DataTable> customerImportLinks = new List<DataTable>();
				if (hasCustomerName ^ hasCustomerId)
				{
					// If only one thing is on this sheet, we see if both exist together on another sheet
					customerImportLinks = tables.Where(t => t.Columns.Contains(ColumnHeaders.CustomerName) && t.Columns.Contains(ColumnHeaders.CustomerId)).ToList();
					if (customerImportLinks.Count() > 0)
					{
						canCreateCustomers = true;
					}
				}

				// Non-required customer columns. This is checked ahead of time to eliminate useless column lookups on each row and save a lot of time.
				bool hasCustomerStreetAddress = table.Columns.Contains(ColumnHeaders.CustomerStreetAddress);
				bool hasCustomerCity = table.Columns.Contains(ColumnHeaders.CustomerCity);
				bool hasCustomerCountry = table.Columns.Contains(ColumnHeaders.CustomerCountry);
				bool hasCustomerState = table.Columns.Contains(ColumnHeaders.CustomerState);
				bool hasCustomerPostalCode = table.Columns.Contains(ColumnHeaders.CustomerPostalCode);
				bool hasCustomerEmail = table.Columns.Contains(ColumnHeaders.CustomerEmail);
				bool hasCustomerPhoneNumber = table.Columns.Contains(ColumnHeaders.CustomerPhoneNumber);
				bool hasCustomerFaxNumber = table.Columns.Contains(ColumnHeaders.CustomerFaxNumber);
				bool hasCustomerEIN = table.Columns.Contains(ColumnHeaders.CustomerEIN);
				bool hasNonRequiredCustomerInfo = hasCustomerStreetAddress || hasCustomerCity || hasCustomerCountry || hasCustomerState ||
						 hasCustomerPostalCode || hasCustomerEmail || hasCustomerPhoneNumber || hasCustomerFaxNumber || hasCustomerEIN;

				// Project importing: requires both project name and project id, as well as one identifying field for a customer (name or id)
				bool hasProjectName = table.Columns.Contains(ColumnHeaders.ProjectName);
				bool hasProjectId = table.Columns.Contains(ColumnHeaders.ProjectId);
				List<DataTable>[,] projectLinks = new List<DataTable>[3, 3];
				projectLinks[0, 1] = projectLinks[1, 0] = tables.Where(t => t.Columns.Contains(ColumnHeaders.ProjectName) && t.Columns.Contains(ColumnHeaders.ProjectId)).ToList();
				projectLinks[0, 2] = projectLinks[2, 0] = tables.Where(t => t.Columns.Contains(ColumnHeaders.ProjectName) && (t.Columns.Contains(ColumnHeaders.CustomerName) || t.Columns.Contains(ColumnHeaders.CustomerId))).ToList();
				projectLinks[1, 2] = projectLinks[2, 1] = tables.Where(t => t.Columns.Contains(ColumnHeaders.ProjectId) && (t.Columns.Contains(ColumnHeaders.CustomerName) || t.Columns.Contains(ColumnHeaders.CustomerId))).ToList();
				bool canImportProjects = (hasProjectName || projectLinks[0, 1].Count > 0 || projectLinks[0, 2].Count > 0) &&
					(hasProjectId || projectLinks[1, 0].Count > 0 || projectLinks[1, 2].Count > 0) &&
					(hasCustomerName || hasCustomerId || projectLinks[2, 0].Count > 0 || projectLinks[2, 1].Count > 0);

				// Non-required project columns
				bool hasProjectType = table.Columns.Contains(ColumnHeaders.ProjectType);
				bool hasProjectStartDate = table.Columns.Contains(ColumnHeaders.ProjectStartDate);
				bool hasProjectEndDate = table.Columns.Contains(ColumnHeaders.ProjectEndDate);
				bool hasNonRequiredProjectInfo = hasProjectType || hasProjectStartDate || hasProjectEndDate;

				// User importing: requires email, id, first and last name
				bool hasUserEmail = table.Columns.Contains(ColumnHeaders.UserEmail);
				bool hasEmployeeId = table.Columns.Contains(ColumnHeaders.EmployeeId);
				bool hasUserName = table.Columns.Contains(ColumnHeaders.UserFirstName) && table.Columns.Contains(ColumnHeaders.UserLastName);
				List<DataTable>[,] userLinks = new List<DataTable>[3, 3];
				userLinks[0, 1] = userLinks[1, 0] = tables.Where(t => t.Columns.Contains(ColumnHeaders.UserEmail) && t.Columns.Contains(ColumnHeaders.EmployeeId)).ToList();
				userLinks[0, 2] = userLinks[2, 0] = tables.Where(t => t.Columns.Contains(ColumnHeaders.UserEmail) && t.Columns.Contains(ColumnHeaders.UserFirstName) && t.Columns.Contains(ColumnHeaders.UserLastName)).ToList();
				userLinks[1, 2] = userLinks[2, 1] = tables.Where(t => t.Columns.Contains(ColumnHeaders.EmployeeId) && t.Columns.Contains(ColumnHeaders.UserFirstName) && t.Columns.Contains(ColumnHeaders.UserLastName)).ToList();
				bool canImportUsers =
					(hasUserEmail ? true : userLinks[0, 1].Count > 0 || userLinks[0, 2].Count > 0) &&
					(hasEmployeeId ? true : userLinks[1, 0].Count > 0 || userLinks[1, 2].Count > 0) &&
					(hasUserName ? true : userLinks[2, 0].Count > 0 || userLinks[2, 1].Count > 0);

				// Non-required user columns
				bool hasUserAddress = table.Columns.Contains(ColumnHeaders.UserAddress);
				bool hasUserCity = table.Columns.Contains(ColumnHeaders.UserCity);
				bool hasUserCountry = table.Columns.Contains(ColumnHeaders.UserCountry);
				bool hasUserDateOfBirth = table.Columns.Contains(ColumnHeaders.UserDateOfBirth);
				bool hasUserUsername = table.Columns.Contains(ColumnHeaders.UserName);
				bool hasUserPhoneExtension = table.Columns.Contains(ColumnHeaders.UserPhoneExtension);
				bool hasUserPhoneNumber = table.Columns.Contains(ColumnHeaders.UserPhoneNumber);
				bool hasUserPostalCode = table.Columns.Contains(ColumnHeaders.UserPostalCode);
				bool hasUserState = table.Columns.Contains(ColumnHeaders.UserState);
				bool hasNonRequiredUserInfo = hasUserAddress || hasUserCity || hasUserCountry || hasUserDateOfBirth || hasUserUsername ||
					hasUserPhoneExtension || hasUserPhoneNumber || hasUserPostalCode || hasUserState;

				// Project-user importing: perfomed when identifying information for both project and user are present
				bool canImportProjectUser = (hasProjectName || hasProjectId) && (hasUserEmail || hasEmployeeId || hasUserName);

				// Time Entry importing: unlike customers, projects, and users, time entry data must have all time entry information on the same sheet
				// Requires indentifying data for user and project, as well as date, duration, and pay class
				bool hasTTDate = table.Columns.Contains(ColumnHeaders.Date);
				bool hasTTDuration = table.Columns.Contains(ColumnHeaders.Duration);
				bool hasTTPayClass = table.Columns.Contains(ColumnHeaders.PayClass);
				bool canImportTimeEntry = canImportProjectUser && hasTTDate && hasTTDuration && hasTTPayClass;
				if (canImportTimeEntry && ttSub == null)
				{
					// No Time Tracker subscription
					result.TimeEntryFailures.Add("Cannot import time entries: no subscription to Time Tracker.");
					canImportTimeEntry = false;
				}

				// Non-required time entry column
				bool hasTTDescription = table.Columns.Contains(ColumnHeaders.Description);

				#endregion Column Header Checks

				// After all checks are complete, we go through row by row and import the information
				foreach (DataRow row in table.Rows)
				{
					if (row.ItemArray.All(i => string.IsNullOrEmpty(i?.ToString()))) break; // Avoid iterating through empty rows

					#region Customer Import

					CustomerInfo customer = null;

					// If there is no identifying information for customers, all customer related importing is skipped.
					if (hasCustomerName || hasCustomerId)
					{
						// Find the existing customer using name, or id if name isn't on this sheet.
						customer = customersProjects.Select(tup => tup.Item1).Where(c => hasCustomerName ? c.Name.Equals(row[ColumnHeaders.CustomerName].ToString()) : c.CustomerOrgId.Equals(row[ColumnHeaders.CustomerId].ToString())).FirstOrDefault();
						if (customer == null)
						{
							if (canCreateCustomers)
							{
								// No customer was found, so a new one is created.
								CustomerInfo newCustomer = null;
								if (customerImportLinks.Count == 0)
								{
									// If customerImportLinks is empty, it's because all the information is on this sheet.
									string name = null;
									string orgId = null;
									this.readColumn(row, ColumnHeaders.CustomerName, n => name = n);
									this.readColumn(row, ColumnHeaders.CustomerId, n => orgId = n);
									if (name == null && orgId == null)
									{
										result.CustomerFailures.Add(string.Format("Error importing customer on sheet {0}, row {1}: both {2} and {3} cannot be read.", table.TableName, table.Rows.IndexOf(row) + 2, ColumnHeaders.CustomerName, ColumnHeaders.CustomerId));
										continue;
									}

									if (name == null || orgId == null)
									{
										result.CustomerFailures.Add(string.Format("Could not create customer {0}: no matching {1}.", name == null ? orgId : name, name == null ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId));
										continue;
									}

									newCustomer = new CustomerInfo
									{
										Name = name,
										CustomerOrgId = orgId,
										OrganizationId = this.UserContext.ChosenOrganizationId
									};
								}
								else
								{
									// If customerImportLinks has been set, we have to grab some information from another sheet.
									string knownValue = null;
									string readValue = null;
									this.readColumn(row, hasCustomerName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId, n => knownValue = n);
									if (knownValue == null)
									{
										result.CustomerFailures.Add(string.Format("Error importing customer on sheet {0}, row {1}: {2} cannot be read.", table.TableName, table.Rows.IndexOf(row) + 2, hasCustomerName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId));
										continue;
									}

									foreach (DataTable link in customerImportLinks)
									{
										try
										{
											readValue = link.Select(string.Format("[{0}] = '{1}'", hasCustomerName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId, knownValue))[0][hasCustomerName ? ColumnHeaders.CustomerId : ColumnHeaders.CustomerName].ToString();
											if (readValue != null) break;
										}
										catch (IndexOutOfRangeException) { }
									}

									if (readValue == null)
									{
										result.CustomerFailures.Add(string.Format("Could not create customer {0}: no matching {1}.", knownValue, hasCustomerName ? ColumnHeaders.CustomerId : ColumnHeaders.CustomerName));
										continue;
									}

									newCustomer = new CustomerInfo
									{
										Name = hasCustomerName ? knownValue : readValue,
										CustomerOrgId = hasCustomerName ? readValue : knownValue,
										OrganizationId = this.UserContext.ChosenOrganizationId
									};
								}

								if (newCustomer != null)
								{
									int? newCustomerId = this.CreateCustomer(newCustomer);
									if (newCustomerId == null)
									{
										result.CustomerFailures.Add(string.Format("Could not create customer {0}: permission failure.", newCustomer.Name));
										continue;
									}

									newCustomer.CustomerId = newCustomerId.Value;
									if (newCustomer.CustomerId == -1)
									{
										result.CustomerFailures.Add(string.Format("Database error while creating customer {0}.", newCustomer.Name));
										continue;
									}

									customersProjects.Add(new Tuple<CustomerInfo, List<ProjectInfo>>(
										newCustomer,
										new List<ProjectInfo>()
									));
									customer = newCustomer;
									result.CustomersImported += 1;
								}
							}
							else
							{
								// Not enough information to create customer
								result.CustomerFailures.Add(string.Format("Could not create customer {0}: no matching {1}.", row[hasCustomerName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId].ToString(), hasCustomerName ? ColumnHeaders.CustomerId : ColumnHeaders.CustomerName));
							}
						}

						// Importing non-required customer data
						if (customer != null && hasNonRequiredCustomerInfo)
						{
							bool updated = false;

							if (hasCustomerStreetAddress) updated = this.readColumn(row, ColumnHeaders.CustomerStreetAddress, val => customer.Address = val) || updated;
							if (hasCustomerCity) updated = this.readColumn(row, ColumnHeaders.CustomerCity, val => customer.City = val) || updated;
							if (hasCustomerCountry) updated = this.readColumn(row, ColumnHeaders.CustomerCountry, val => customer.Country = val) || updated;
							if (hasCustomerState) updated = this.readColumn(row, ColumnHeaders.CustomerState, val => customer.State = val) || updated;
							if (hasCustomerPostalCode) updated = this.readColumn(row, ColumnHeaders.CustomerPostalCode, val => customer.PostalCode = val) || updated;
							if (hasCustomerEmail) updated = this.readColumn(row, ColumnHeaders.CustomerEmail, val => customer.ContactEmail = val) || updated;
							if (hasCustomerPhoneNumber) updated = this.readColumn(row, ColumnHeaders.CustomerPhoneNumber, val => customer.ContactPhoneNumber = val) || updated;
							if (hasCustomerFaxNumber) updated = this.readColumn(row, ColumnHeaders.CustomerFaxNumber, val => customer.FaxNumber = val) || updated;
							if (hasCustomerEIN) updated = this.readColumn(row, ColumnHeaders.CustomerEIN, val => customer.EIN = val) || updated;

							if (updated)
							{
								this.UpdateCustomer(customer);
							}
						}
					}

					#endregion Customer Import

					#region Project Import

					ProjectInfo project = null;

					// If there is no identifying information for projects, all project related importing is skipped.
					if (hasProjectName || hasProjectId)
					{
						bool thisRowHasProjectName = hasProjectName;
						bool thisRowHasProjectId = hasProjectId;

						// Start with getting the project information that is known from this sheet
						string knownValue = null;
						string readValue = null;
						this.readColumn(row, hasProjectName ? ColumnHeaders.ProjectName : ColumnHeaders.ProjectId, p => knownValue = p);
						if (hasProjectName && hasProjectId)
						{
							// If both columns exist, knownValue is Name and readValue will be Id
							if (!this.readColumn(row, ColumnHeaders.ProjectId, p => readValue = p))
							{
								if (knownValue == null)
								{
									// Failed to read both values
									result.ProjectFailures.Add(string.Format("Error importing project on sheet {0}, row {1}: both {2} and {3} cannot be read.", table.TableName, table.Rows.IndexOf(row) + 2, ColumnHeaders.ProjectName, ColumnHeaders.ProjectId)); //'all', line 5
									continue;
								}

								// Failed to read Id, but read Name successfully.
								thisRowHasProjectId = false;
							}

							if (knownValue == null)
							{
								// Failed to read Name. If reading the Id also failed, the continue above would have been hit, so it must have succeeded.
								// This means that we should change knownValue to Id.
								thisRowHasProjectName = false;
								knownValue = readValue;
								readValue = null;
							}
						}

						if (customer != null)
						{
							// We now have the customer and at least one piece of identifying project information. That's enough to tell if the project already exists.
							project = customersProjects.Where(tup => tup.Item1.CustomerId == customer.CustomerId).FirstOrDefault().Item2.Where(
								p => thisRowHasProjectName ? p.Name.Equals(knownValue) : p.ProjectOrgId.Equals(knownValue)).FirstOrDefault();
							if (project == null)
							{
								// Project does not exist, so we should create it
								if (!canImportProjects)
								{
									result.ProjectFailures.Add(string.Format("Could not create project {0}: no corresponding {1}.", knownValue, thisRowHasProjectName ? ColumnHeaders.ProjectId : ColumnHeaders.ProjectName));
									continue;
								}

								if (thisRowHasProjectName ^ thisRowHasProjectId)
								{
									// We still need the other bit of project info
									foreach (DataTable link in projectLinks[0, 1])
									{
										try
										{
											readValue = link.Select(string.Format("[{0}] = '{1}'", thisRowHasProjectName ? ColumnHeaders.ProjectName : ColumnHeaders.ProjectId, knownValue))[0][thisRowHasProjectName ? ColumnHeaders.ProjectId : ColumnHeaders.ProjectName].ToString();
											if (!string.IsNullOrEmpty(readValue))
											{
												break; // Match found.
											}
										}
										catch (IndexOutOfRangeException) { }
									}

									if (string.IsNullOrEmpty(readValue))
									{
										result.ProjectFailures.Add(string.Format("Could not create project {0}: no corresponding {1}.", knownValue, thisRowHasProjectName ? ColumnHeaders.ProjectId : ColumnHeaders.ProjectName));
										continue;
									}
								}

								// All required information is known: time to create the project
								project = new ProjectInfo
								{
									CustomerId = customer.CustomerId,
									Name = thisRowHasProjectName ? knownValue : readValue,
									Type = "Hourly",
									OrganizationId = this.UserContext.ChosenOrganizationId,
									ProjectOrgId = thisRowHasProjectName ? readValue : knownValue,
									StartingDate = DateTime.Now,
									EndingDate = DateTime.Now.AddMonths(6)
								};
								project.ProjectId = this.CreateProject(project);
								if (project.ProjectId == -1)
								{
									result.ProjectFailures.Add(string.Format("Database error while creating project {0}", project.Name));
									project = null;
								}
								else
								{
									customersProjects.Where(tup => tup.Item1 == customer).FirstOrDefault().Item2.Add(project);
									result.ProjectsImported += 1;
								}
							}
						}
						else
						{
							//if(!canImportProjects)
							//{
							//    result.ProjectFailures.Add(string.Format("Could not create project {0}: no corresponding {1}.", knownValue, thisRowHasProjectName ? ColumnHeaders.ProjectId : ColumnHeaders.ProjectName));
							//    continue;
							//}

							// No customer yet specified. Now, we have to use all the links to try and get customer and the complete project info
							string[] fields =
								{
									thisRowHasProjectName ? knownValue : null,
									thisRowHasProjectId ? thisRowHasProjectName ? readValue : knownValue : null,
									null
								};
							bool customerFieldIsName = true;

							/*
                                There are 3 required fields, and we may need to traverse at most 2 links to get them all, with no knowledge of which links will succeed or fail in providing
                                the needed information. To solve this, we do 2 passes (i), each time checking for the missing information (j) using the links we've found to the information
                                we already have (k). On each pass, any known information is skipped, so time won't be wasted if the first pass succeeds. This way, any combination of paths
                                to acquire the missing information from the known information is covered.
                            */
							for (int i = 0; i < 2; i++)
							{
								// i = pass, out of 2
								for (int j = 0; j < 3; j++)
								{
									// j = field we are currently trying to find
									for (int k = 0; k < 3; k++)
									{
										// k = field we are trying to find j from, using a link
										if (fields[j] == null)
										{
											if (j == k) continue;
											if (fields[k] != null)
											{
												foreach (DataTable link in projectLinks[j, k])
												{
													try
													{
														bool thisLinkCustomerFieldIsName = k == 2 || j == 2 ? link.Columns.Contains(ColumnHeaders.CustomerName) : false;
														fields[j] = link.Select(string.Format("[{0}] = '{1}'",
															k == 0 ? ColumnHeaders.ProjectName : k == 1 ? ColumnHeaders.ProjectId : thisLinkCustomerFieldIsName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId,
															fields[k].Replace("'", "''")
														))[0][j == 0 ? ColumnHeaders.ProjectName : j == 1 ? ColumnHeaders.ProjectId : thisLinkCustomerFieldIsName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId].ToString();
														if (fields[j] != null)
														{
															customerFieldIsName = j == 2 ? thisLinkCustomerFieldIsName : customerFieldIsName;
															break;
														}
													}
													catch (IndexOutOfRangeException) { }
												}
											}
										}
									}
								}
							}

							// After that, if we don't have all the information, it's safe to say it can't be found
							if (!string.IsNullOrEmpty(fields[2]))
							{
								customer = customersProjects.Select(tup => tup.Item1).Where(c => customerFieldIsName ? c.Name.Equals(fields[2]) : c.CustomerOrgId.Equals(fields[2])).FirstOrDefault();

								if (customer == null)
								{
									result.ProjectFailures.Add(string.Format("Could not create project {0}: No customer to create it under.", knownValue));
									continue;
								}

								project = customersProjects.Where(tup => tup.Item1.CustomerId == customer.CustomerId).FirstOrDefault().Item2.Where(p => p.Name.Equals(fields[0])).FirstOrDefault();
								if (project == null)
								{
									// Project does not exist, so we should create it
									if (string.IsNullOrEmpty(fields[0]) || string.IsNullOrEmpty(fields[1]))
									{
										result.ProjectFailures.Add(string.Format("Could not create project {0}: no corresponding {1}.", knownValue, thisRowHasProjectName ? ColumnHeaders.ProjectId : ColumnHeaders.ProjectName));
										continue;
									}

									// All required information is known: time to create the project
									project = new ProjectInfo
									{
										CustomerId = customer.CustomerId,
										Name = fields[0],
										Type = "Hourly",
										OrganizationId = this.UserContext.ChosenOrganizationId,
										ProjectOrgId = fields[1],
										StartingDate = DateTime.Now,
										EndingDate = DateTime.Now.AddMonths(6)
									};
									project.ProjectId = this.CreateProject(project);
									if (project.ProjectId == -1)
									{
										result.ProjectFailures.Add(string.Format("Database error while creating project {0}", project.Name));
										project = null;
									}
									else
									{
										customersProjects.Where(tup => tup.Item1 == customer).FirstOrDefault().Item2.Add(project);
										result.ProjectsImported += 1;
									}
								}
							}
							else
							{
								// No customer could be found for this project, so we try to find a matching project under any existing customer
								project = customersProjects.Select(
									tup => tup.Item2).Select(
										plst => plst.Where(
											p => thisRowHasProjectName ? p.Name.Equals(knownValue) && (!string.IsNullOrEmpty(fields[1]) ? p.ProjectOrgId.Equals(fields[1]) : true) :
												p.ProjectOrgId.Equals(knownValue) && (!string.IsNullOrEmpty(fields[0]) ? p.Name.Equals(fields[0]) : true)
										).FirstOrDefault()
									).Where(p => p != null).FirstOrDefault();

								if (project == null)
								{
									result.ProjectFailures.Add(string.Format("Could not find any project {0}, and no customer was specified to create project under.", string.IsNullOrEmpty(fields[0]) ? fields[1] : fields[0]));
									continue;
								}
							}
						}

						// Importing non-required project data
						if (project != null && hasNonRequiredProjectInfo)
						{
							bool updated = false;
							string startDate = null;
							string endDate = null;

							if (hasProjectType) updated = this.readColumn(row, ColumnHeaders.ProjectType, val => project.Type = val) || updated;
							if (hasProjectStartDate) updated = this.readColumn(row, ColumnHeaders.ProjectStartDate, val => startDate = val) || updated;
							if (hasProjectEndDate) updated = this.readColumn(row, ColumnHeaders.ProjectEndDate, val => endDate = val) || updated;
							if (startDate != null) project.StartingDate = DateTime.Parse(startDate);
							if (endDate != null) project.EndingDate = DateTime.Parse(endDate);

							if (updated)
							{
								this.UpdateProject(project);
							}
						}
					}

					#endregion Project Import

					#region User Import

					UserInfo user = null;
					if (hasUserEmail || hasEmployeeId || hasUserName)
					{
						Tuple<string, UserInfo> userTuple = null;

						// Find existing user by whatever information we have
						string readValue = null;
						if (hasUserEmail)
						{
							if (this.readColumn(row, ColumnHeaders.UserEmail, e => readValue = e))
							{
								userTuple = users.Where(tup => tup.Item2.Email.Equals(readValue)).FirstOrDefault();
							}
						}
						if (userTuple == null)
						{
							if (hasEmployeeId)
							{
								if (this.readColumn(row, ColumnHeaders.EmployeeId, e => readValue = e))
								{
									userTuple = users.Where(tup => tup.Item1.Equals(readValue)).FirstOrDefault();
								}
							}
							if (userTuple == null)
							{
								string readLastName = null;
								if (this.readColumn(row, ColumnHeaders.UserFirstName, e => readValue = e) && this.readColumn(row, ColumnHeaders.UserLastName, e => readLastName = e))
								{
									userTuple = users.Where(tup => tup.Item2.FirstName.Equals(readValue) && tup.Item2.LastName.Equals(readLastName)).FirstOrDefault();
								}
							}
						}
						user = userTuple == null ? null : userTuple.Item2;

						if (user == null)
						{
							if (canImportUsers)
							{
								// No user found, create one if possible
								// Find all required fields, if they exist
								string[] fields =
								{
									hasUserEmail ? row[ColumnHeaders.UserEmail].ToString() : null,
									hasEmployeeId ? row[ColumnHeaders.EmployeeId].ToString() : null,
                                    // Since first and last name must be together and treated as one piece of information, they are joined in this datastructure. Hopefully, we'll never
                                    // have a user who's name includes the text __IMPORT__
                                    hasUserName ? row[ColumnHeaders.UserFirstName].ToString() + "__IMPORT__" + row[ColumnHeaders.UserLastName].ToString() : null
								};

								/*
                                    There are 3 required fields, and we may need to traverse at most 2 links to get them all, with no knowledge of which links will succeed or fail in providing
                                    the needed information. To solve this, we do 2 passes (i), each time checking for the missing information (j) using the links we've found to the information
                                    we already have (k). On each pass, any known information is skipped, so time won't be wasted if the first pass succeeds. This way, any combination of paths
                                    to acquire the missing information from the known information is covered.
                                */
								for (int i = 0; i < 2; i++)
								{
									// i = pass, out of 2
									for (int j = 0; j < 3; j++)
									{
										// j = field we are currently trying to find
										for (int k = 0; k < 3; k++)
										{
											// k = field we are trying to find j from, using a link
											if (fields[j] == null)
											{
												if (j == k) continue;
												if (fields[k] != null)
												{
													foreach (DataTable link in userLinks[j, k])
													{
														try
														{
															fields[j] = this.readUserDataColumn(k, j, link, fields[k]); // A private method that can handle reading one column or the case of both name columns, with no difference in usage here.
															if (fields[j] != null)
															{
																break;
															}
														}
														catch (IndexOutOfRangeException) { }
													}
												}
											}
										}
									}
								}

								if (fields.Any(s => string.IsNullOrEmpty(s)))
								{
									// Couldn't get all the information
									bool[] fieldStatuses = fields.Select(f => string.IsNullOrEmpty(f)).ToArray();
									result.UserFailures.Add(string.Format("Could not create user {0}: missing {1}{2}.", (fieldStatuses[0] ? fieldStatuses[1] ?
										string.Join(" ", fields[2].Split(new string[] { "__IMPORT__" }, StringSplitOptions.None)) : fields[1] : fields[0]),
										fieldStatuses[0] ? ColumnHeaders.UserEmail : fieldStatuses[1] ? ColumnHeaders.EmployeeId : string.Format("{0}/{1}", ColumnHeaders.UserFirstName, ColumnHeaders.UserLastName),
										fieldStatuses.Where(s => s).Count() == 2 ? string.Format(" and {0}", !fieldStatuses[2] ? ColumnHeaders.EmployeeId : string.Format("{0}/{1}", ColumnHeaders.UserFirstName, ColumnHeaders.UserLastName)) : ""));
									continue;
								}

								// All required info was found successfully
								string[] names = fields[2].Split(new string[] { "__IMPORT__" }, StringSplitOptions.None);

								if (!Service.IsEmailAddressValid(fields[0]))
								{
									result.UserFailures.Add(string.Format("Could not create user {0} {1}: invalid email format ({2}).", names[0], names[1], fields[0]));
									continue;
								}

								try
								{
									user = this.GetUserByEmail(fields[0]); // User may already exist, but not be a member of this organization
									if (user == null)
									{
										user = new UserInfo()
										{
											Email = fields[0],
											FirstName = names[0],
											LastName = names[1],
											PasswordHash = Lib.Crypto.ComputeSHA512Hash("password") // TODO: Figure out a better default password generation system
										};
										user.UserId = DBHelper.CreateUser(GetDBEntityFromUserInfo(user));
										result.UsersImported += 1;
									}
									if (user.UserId != -1)
									{
										try
										{
											DBHelper.CreateOrganizationUser(new OrganizationUserDBEntity()
											{
												EmployeeId = fields[1],
												OrganizationId = this.UserContext.ChosenOrganizationId,
												OrgRoleId = (int)(OrganizationRole.Member),
												UserId = user.UserId
											});
											result.UsersAddedToOrganization += 1;
										}
										catch (System.Data.SqlClient.SqlException)
										{
											result.OrgUserFailures.Add(string.Format("Database error assigning user {0} {1} to organization. Could be a duplicate employee id ({2}).", names[0], names[1], fields[1]));
											continue;
										}
										users.Add(new Tuple<string, UserInfo>(fields[1], user));
									}
									else
									{
										result.UserFailures.Add(string.Format("Database error creating user {0} {1}.", names[0], names[1]));
										continue;
									}
								}
								catch (System.Data.SqlClient.SqlException)
								{
									result.UserFailures.Add(string.Format("Database error creating user {0} {1}.", names[0], names[1]));
									continue;
								}
							}
						}

						// Importing non-required user data
						if (user != null && hasNonRequiredUserInfo)
						{
							bool updated = false;

							if (hasUserAddress) updated = this.readColumn(row, ColumnHeaders.UserAddress, val => user.Address = val) || updated;
							if (hasUserCity) updated = this.readColumn(row, ColumnHeaders.UserCity, val => user.City = val) || updated;
							if (hasUserCountry) updated = this.readColumn(row, ColumnHeaders.UserCountry, val => user.Country = val) || updated;
							string dateOfBirth = null;
							if (hasUserDateOfBirth) updated = this.readColumn(row, ColumnHeaders.UserDateOfBirth, val => dateOfBirth = val) || updated;
							if (!string.IsNullOrEmpty(dateOfBirth))
							{
								user.DateOfBirth = DateTime.Parse(dateOfBirth);
							}
							if (hasUserUsername) updated = this.readColumn(row, ColumnHeaders.UserName, val => user.UserName = val) || updated;
							if (hasUserPhoneExtension) updated = this.readColumn(row, ColumnHeaders.UserPhoneExtension, val => user.PhoneExtension = val) || updated;
							if (hasUserPhoneNumber) updated = this.readColumn(row, ColumnHeaders.UserPhoneNumber, val => user.PhoneNumber = val) || updated;
							if (hasUserPostalCode) updated = this.readColumn(row, ColumnHeaders.UserPostalCode, val => user.PostalCode = val) || updated;
							if (hasUserState) updated = this.readColumn(row, ColumnHeaders.UserState, val => user.State = val) || updated;

							if (updated)
							{
								this.SaveUserInfo(user);
							}
						}
					}

					#endregion User Import

					#region Project-user and Time Entry Import

					if (canImportProjectUser)
					{
						// Double-check that previous adding/finding of project and user didn't fail
						if (project != null && user != null)
						{
							// Find existing project user
							if (!this.GetProjectsByUserAndOrganization(user.UserId).Where(p => p.ProjectId == project.ProjectId).Any())
							{
								// If no project user entry exists for this user and project, we create one.
								this.CreateProjectUser(project.ProjectId, user.UserId);
							}

							// Time Entry Import
							if (canImportTimeEntry)
							{
								// Check for subscription role
								bool canImportThisEntry = false;
								if (!userSubs.Where(u => u.UserId == user.UserId).Any())
								{
									// No existing subscription for this user, so we create one.
									if (ttSub.SubscriptionsUsed < ttSub.NumberOfUsers)
									{
										this.DBHelper.UpdateSubscriptionUserProductRole((int)(ProductRole.TimeTrackerUser), ttSub.SubscriptionId, user.UserId);
										userSubs.Add(user);
										result.UsersAddedToSubscription += 1;
										canImportThisEntry = true; // Successfully created.
									}
									else
									{
										result.UserSubscriptionFailures.Add(string.Format("Cannot add user {0} {1} to Time Tracker subscription: number of users for subscription is at maximum ({2}).", user.FirstName, user.LastName, ttSub.SubscriptionsUsed));
										continue;
									}
								}
								else
								{
									// Found existing subscription user.
									canImportThisEntry = true;
								}

								// Import entry
								if (canImportThisEntry)
								{
									string date = null;
									string duration = null;
									string description = "";
									string payclass = "Regular";

									this.readColumn(row, ColumnHeaders.Date, val => date = val);
									this.readColumn(row, ColumnHeaders.Duration, val => duration = val);
									if (hasTTDescription) this.readColumn(row, ColumnHeaders.Description, val => description = val);
									this.readColumn(row, ColumnHeaders.PayClass, val => payclass = val);

									PayClassInfo payClass = payClasses.Where(p => p.Name.ToUpper().Equals(payclass.ToUpper())).SingleOrDefault();
									DateTime theDate;
									float theDuration;

									if (payClass == null)
									{
										result.TimeEntryFailures.Add(string.Format("Error importing time entry on sheet {0}, row {1}: unknown {2} ({3}).", table.TableName, table.Rows.IndexOf(row) + 2, ColumnHeaders.PayClass, payclass));
										continue;
									}

									try
									{
										theDate = DateTime.Parse(date);
										if (theDate.Year < 1753) throw new FormatException();
									}
									catch (Exception)
									{
										result.TimeEntryFailures.Add(string.Format("Error importing time entry on sheet {0}, row {1}: bad date format ({2}).", table.TableName, table.Rows.IndexOf(row) + 2, date));
										continue;
									}

									try
									{
										theDuration = float.Parse(duration);
									}
									catch (Exception)
									{
										result.TimeEntryFailures.Add(string.Format("Error importing time entry on sheet {0}, row {1}: bad duration format ({2}).", table.TableName, table.Rows.IndexOf(row) + 2, duration));
										continue;
									}

									// Find existing entry. If none, create new one     TODO: See if there's a good way to populate this by sheet rather than by row, or once at the top
									List<TimeEntryDBEntity> entries = DBHelper.GetTimeEntriesByUserOverDateRange(new List<int> { user.UserId }, this.UserContext.ChosenOrganizationId, theDate, theDate).ToList();
									if (!entries.Where(e => e.Description.Equals(description) && e.Duration == theDuration && e.PayClassId == payClass.PayClassID && e.ProjectId == project.ProjectId).Any())
									{
										if (entries.Select(e => e.Duration).Sum() + theDuration > 24)
										{
											result.TimeEntryFailures.Add(string.Format("Error importing time entry on sheet {0}, row {1}: cannot have more than 24 hours of work in one day.", table.TableName, table.Rows.IndexOf(row) + 2));
											continue;
										}

										// All required information is present and valid
										if (DBHelper.CreateTimeEntry(new DBModel.TimeTracker.TimeEntryDBEntity
										{
											Date = theDate,
											Description = description,
											Duration = theDuration,
											FirstName = user.FirstName,
											LastName = user.LastName,
											PayClassId = payClass.PayClassID,
											ProjectId = project.ProjectId,
											UserId = user.UserId
										}) == -1)
										{
											result.TimeEntryFailures.Add(string.Format("Database error importing time entry on sheet {0}, row {1}.", table.TableName, table.Rows.IndexOf(row) + 2));
										}
										else
										{
											result.TimeEntriesImported += 1;
										}
									}
								}
							}
						}
					}

					#endregion Project-user and Time Entry Import
				}
			}

			return result;
		}

		/// <summary>
		/// Helper method for reading column data from a spreadsheet. It will try to read data in the given column name from the given
		/// DataRow. If it exists and there is data there (it's not blank), it will then execute the lambda function using the discovered
		/// data, and return true. If either the column does not exist or the row has nothing in that column, the lambda will never be
		/// executed and the function will return false.
		/// </summary>
		/// <param name="row">DataRow.</param>
		/// <param name="columnName">Column name to read.</param>
		/// <param name="useValue">Function to execute using data from column, if present.</param>
		/// <returns>True is data found and function executed, false otherwise.</returns>
		private bool readColumn(DataRow row, string columnName, Func<string, string> useValue)
		{
			try
			{
				string value = row[columnName].ToString();
				if (!string.IsNullOrEmpty(value))
				{
					useValue(value);
					return true;
				}
			}
			catch (ArgumentException) { }
			return false;
		}

		/// <summary>
		/// Reads user required fields from matches in a linking data table.
		/// </summary>
		/// <param name="fieldIdFrom">Field index for value linking from (0 = email, 1 = employee id, 2 = name).</param>
		/// <param name="fieldIdTo">Field index for value linking to.</param>
		/// <param name="link">DataTable linking fields.</param>
		/// <param name="fromValue">Value of field linking from.</param>
		/// <returns>Matching value of field linking to, or null.</returns>
		private string readUserDataColumn(int fieldIdFrom, int fieldIdTo, DataTable link, string fromValue)
		{
			try
			{
				fromValue = fromValue.Replace("'", "''"); //Escape any 's in the names
				string selectText = null;
				if (fieldIdFrom == 2)
				{
					string[] names = fromValue.Split(new string[] { "__IMPORT__" }, StringSplitOptions.None);
					selectText = string.Format("[{0}] = '{1}' AND [{2}] = '{3}'", ColumnHeaders.UserFirstName, names[0], ColumnHeaders.UserLastName, names[1]);
				}
				else
				{
					selectText = string.Format("[{0}] = '{1}'", fieldIdFrom == 0 ? ColumnHeaders.UserEmail : ColumnHeaders.EmployeeId, fromValue);
				}
				DataRow row = link.Select(selectText)[0];
				if (fieldIdTo == 2)
				{
					return row[ColumnHeaders.UserFirstName].ToString() + "__IMPORT__" + row[ColumnHeaders.UserLastName].ToString();
				}
				else
				{
					return row[fieldIdTo == 0 ? ColumnHeaders.UserEmail : ColumnHeaders.EmployeeId].ToString();
				}
			}
			catch (IndexOutOfRangeException)
			{
				return null;
			}
		}

		/// <summary>
		/// Translates an OrganizationUserDBEntity into an OrganizationUserInfo business object.
		/// </summary>
		/// <param name="organizationUser">OrganizationUserDBEntity instance.</param>
		/// <returns>OrganizationUserInfo instance.</returns>
		public static OrganizationUserInfo InitializeOrganizationUserInfo(OrganizationUserDBEntity organizationUser)
		{
			if (organizationUser == null)
			{
				return null;
			}

			return new OrganizationUserInfo
			{
				CreatedUTC = organizationUser.CreatedUTC,
				EmployeeId = organizationUser.EmployeeId,
				OrganizationId = organizationUser.OrganizationId,
				OrgRoleId = organizationUser.OrgRoleId,
				UserId = organizationUser.UserId,
				Email = organizationUser.Email,
				FirstName = organizationUser.FirstName,
				LastName = organizationUser.LastName
			};
		}

		/// <summary>
		/// Translates an OrganizationDBEntity into an OrganizationInfo business object.
		/// </summary>
		/// <param name="organization">OrganizationDBEntity instance.</param>
		/// <returns>OrganizationInfo instance.</returns>
		public static OrganizationInfo InitializeOrganizationInfo(OrganizationDBEntity organization)
		{
			if (organization == null)
			{
				return null;
			}

			return new OrganizationInfo
			{
				Address = organization.Address,
				City = organization.City,
				Country = organization.Country,
				DateCreated = organization.CreatedUTC,
				FaxNumber = organization.FaxNumber,
				Name = organization.Name,
				OrganizationId = organization.OrganizationId,
				PhoneNumber = organization.PhoneNumber,
				SiteUrl = organization.SiteUrl,
				State = organization.State,
				Subdomain = organization.Subdomain,
				PostalCode = organization.PostalCode
			};
		}

		/// <summary>
		/// Translates an OrganizationInfo business object into an OrganizationDBEntity.
		/// </summary>
		/// <param name="organization">OrganizationInfo instance.</param>
		/// <returns>OrganizationDBEntity instance.</returns>
		public static OrganizationDBEntity GetDBEntityFromOrganizationInfo(OrganizationInfo organization)
		{
			if (organization == null)
			{
				return null;
			}

			return new OrganizationDBEntity
			{
				Address = organization.Address,
				City = organization.City,
				Country = organization.Country,
				CreatedUTC = organization.DateCreated,
				FaxNumber = organization.FaxNumber,
				Name = organization.Name,
				OrganizationId = organization.OrganizationId,
				PhoneNumber = organization.PhoneNumber,
				SiteUrl = organization.SiteUrl,
				State = organization.State,
				Subdomain = organization.Subdomain,
				PostalCode = organization.PostalCode
			};
		}

		/// <summary>
		/// Translates an InvitationSubRoleDBEntity into an InvitationSubRoleInfo business object.
		/// </summary>
		/// <param name="invitationSubRole">InvitationSubRoleDBEntity instance.</param>
		/// <returns>InvitationSubRoleInfo instance.</returns>
		public static InvitationSubRoleInfo InitializeInvitationSubRoleInfo(InvitationSubRoleDBEntity invitationSubRole)
		{
			if (invitationSubRole == null)
			{
				return null;
			}

			return new InvitationSubRoleInfo
			{
				InvitationId = invitationSubRole.InvitationId,
				ProductRoleId = invitationSubRole.ProductRoleId,
				SubscriptionId = invitationSubRole.SubscriptionId
			};
		}

		/// <summary>
		/// Translates an InvitationDBEntity into an InvitationInfo business object.
		/// </summary>
		/// <param name="invitation">InvitationDBEntity instance.</param>
		/// <returns>InvitationInfo instance.</returns>
		public static InvitationInfo InitializeInvitationInfo(InvitationDBEntity invitation)
		{
			if (invitation == null)
			{
				return null;
			}

			return new InvitationInfo
			{
				AccessCode = invitation.AccessCode,
				DateOfBirth = invitation.DateOfBirth,
				Email = invitation.Email,
				CompressedEmail = Service.GetCompressedEmail(invitation.Email),
				FirstName = invitation.FirstName,
				InvitationId = invitation.InvitationId,
				LastName = invitation.LastName,
				OrganizationId = invitation.OrganizationId,
				OrganizationName = invitation.OrganizationName,
				OrgRole = invitation.OrgRole,
				OrgRoleName = invitation.OrgRoleName,
				ProjectId = invitation.ProjectId,
				EmployeeId = invitation.EmployeeId
			};
		}

		/// <summary>
		/// Translates a ProductDBEntity into a ProductInfo business object.
		/// </summary>
		/// <param name="product">ProductDBEntity instance.</param>
		/// <returns>ProductInfo instance.</returns>
		public static ProductInfo InitializeProductInfo(ProductDBEntity product)
		{
			if (product == null)
			{
				return null;
			}

			return new ProductInfo
			{
				ProductDescription = product.Description,
				ProductId = product.ProductId,
				ProductName = product.Name
			};
		}

		/// <summary>
		/// Translates an InvitationInfo business object into an InvitationDBEntity.
		/// </summary>
		/// <param name="invitation">InvitationInfo instance.</param>
		/// <returns>InvitationDBEntity instance.</returns>
		public static InvitationDBEntity GetDBEntityFromInvitationInfo(InvitationInfo invitation)
		{
			if (invitation == null)
			{
				return null;
			}

			return new InvitationDBEntity
			{
				AccessCode = invitation.AccessCode,
				DateOfBirth = invitation.DateOfBirth,
				Email = invitation.Email,
				FirstName = invitation.FirstName,
				InvitationId = invitation.InvitationId,
				LastName = invitation.LastName,
				OrganizationId = invitation.OrganizationId,
				OrgRole = invitation.OrgRole,
				ProjectId = invitation.ProjectId,
				EmployeeId = invitation.EmployeeId
			};
		}

		/// <summary>
		/// Initialized PayClassInfo object based on a given PayClassDBEntity.
		/// </summary>
		/// <param name="payClass">The PayClassDBEntity object to use.</param>
		/// <returns>The initialied PayClassInfo object.</returns>
		public static PayClassInfo InitializePayClassInfo(PayClassDBEntity payClass)
		{
			return new PayClassInfo
			{
				OrganizationId = payClass.OrganizationId,
				CreatedUTC = payClass.CreatedUTC,
				Name = payClass.Name,
				ModifiedUTC = payClass.ModifiedUTC,
				PayClassID = payClass.PayClassID
			};
		}
	}
}
