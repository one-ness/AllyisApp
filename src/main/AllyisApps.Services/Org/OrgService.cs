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

using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.TimeTracker;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Utilities;
using AllyisApps.Services.TimeTracker;

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

			return DBHelper.CreateOrganization(InfoObjectsUtility.GetDBEntityFromOrganizationInfo(organization), ownerId, (int)OrganizationRole.Owner, employeeId);
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

			return InfoObjectsUtility.InitializeOrganizationInfo(DBHelper.GetOrganization(orgId));
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

			if (this.Can(Actions.CoreAction.EditOrganization))
			{
				DBHelper.UpdateOrganization(InfoObjectsUtility.GetDBEntityFromOrganizationInfo(organization));

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

			string htmlbody = string.Format(
				"{0} has requested you join their organization on Allyis Apps, {1}!<br /> Click <a href=http://{2}.{3}/Account/Index?accessCode={4}>Here</a> to create an account and join!",
				requestingUserFullName,
				orgInfo.Name,
				GetSubdomainById(invitationInfo.OrganizationId),
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

			return DBHelper.CreateUserInvitation(InfoObjectsUtility.GetDBEntityFromInvitationInfo(invitationInfo));
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
				IEnumerable<SubscriptionDisplayInfo> subs = this.DBHelper.GetSubscriptionsDisplayByOrg(UserContext.ChosenOrganizationId).Select(s => InfoObjectsUtility.InitializeSubscriptionDisplayInfo(s));
				IEnumerable<InvitationSubRoleInfo> subRoles = DBHelper.GetInvitationSubRolesByInvitationId(invitationId).Select(i => InfoObjectsUtility.InitializeInvitationSubRoleInfo(i));
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
			return DBHelper.GetUserInvitationsByOrgId(UserContext.ChosenOrganizationId).Select(i => InfoObjectsUtility.InitializeInvitationInfo(i));
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

			return DBHelper.GetOrganizationMemberList(orgId).Select(o => InfoObjectsUtility.InitializeOrganizationUserInfo(o));
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
		/// Updates an organization User.
		/// </summary>
		/// <param name="userId">The updated user ID.</param>
		/// <param name="orgId">The updated org id.</param>
		/// <param name="orgRoleId">The updated org role id.</param>
		public void UpdateOrganizationUser(int userId, int orgId, int orgRoleId)
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
				OrgRoleId = orgRoleId
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
		public IEnumerable<InvitationSubRoleInfo> GetInvitationSubRoles()
		{
			return DBHelper.GetInvitationSubRolesByOrganizationId(UserContext.ChosenOrganizationId).Select(i => InfoObjectsUtility.InitializeInvitationSubRoleInfo(i));
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

			return DBHelper.GetProjectsByOrgId(orgId, isActive ? 1 : 0).Select(c => InfoObjectsUtility.InitializeCompleteProjectInfo(c));
		}

		/// <summary>
		/// Gets the user roles for an organization.
		/// </summary>
		/// <returns>List of UserRolesInfos.</returns>
		public IEnumerable<UserRolesInfo> GetUserRoles()
		{
			return DBHelper.GetRoles(UserContext.ChosenOrganizationId).Select(o => InfoObjectsUtility.InitializeUserRolesInfo(o));
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
            List<DataTable> tables = new List<DataTable>();
            foreach(DataTable table in importData.Tables)
            {
                tables.Add(table);
            }

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
                bool canCreateProjects = hasProjectName && hasProjectId && (hasCustomerName || hasCustomerId);
                List<DataTable> projectImportLinks = new List<DataTable>();
                List<DataTable> projectCustomerLinks = new List<DataTable>();
                if (!canCreateProjects && (hasProjectName || hasProjectId))
                {
                    // If one of these two connections exists on this table, that's ok; it won't get used. The variable will simply be set to this table redundantly.
                    projectImportLinks = tables.Where(t => t.Columns.Contains(ColumnHeaders.ProjectName) && t.Columns.Contains(ColumnHeaders.ProjectId)).ToList();
                    projectCustomerLinks = tables.Where(t => (t.Columns.Contains(ColumnHeaders.ProjectName) || t.Columns.Contains(ColumnHeaders.ProjectId)) &&
                                                            (t.Columns.Contains(ColumnHeaders.CustomerName) || t.Columns.Contains(ColumnHeaders.CustomerId))).ToList();

                    // Even if one of these isn't necessary (because both items are on this table), it won't be empty. Therefore, if either is emtpy, a connection is missing.
                    if (projectImportLinks.Count() > 0 && projectCustomerLinks.Count > 0)
                    {
                        canCreateProjects = true;
                    }
                }

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

                // Project-user importing: perfomed when identifying information for both project and user are present
                bool canImportProjectUser = (hasProjectName || hasProjectId) && (hasUserEmail || hasEmployeeId || hasUserName);

                // Time Entry importing: unlike customers, projects, and users, time entry data must have all time entry information on the same sheet
                // Requires indentifying data for user and project, as well as date, duration, and pay class
                bool hasTTDate = table.Columns.Contains(ColumnHeaders.Date);
                bool hasTTDuration = table.Columns.Contains(ColumnHeaders.Duration);
                bool hasTTPayClass = table.Columns.Contains(ColumnHeaders.PayClass);
                bool canImportTimeEntry = canImportProjectUser && hasTTDate && hasTTDuration && hasTTPayClass;
                #endregion

                // After all checks are complete, we go through row by row and import the information
                foreach (DataRow row in table.Rows)
                {
                    if (row.ItemArray.All(i => string.IsNullOrEmpty(i?.ToString()))) break; // Avoid iterating through empty rows

                    #region Customer Import

                    // If there is no identifying information for customers, all customer related importing is skipped.
                    if (hasCustomerName || hasCustomerId)
                    {
                        // Find the existing customer using name, or id if name isn't on this sheet.
                        CustomerInfo customer = customersProjects.Select(tup => tup.Item1).Where(c => hasCustomerName ? c.Name.Equals(row[ColumnHeaders.CustomerName].ToString()) : c.CustomerOrgId.Equals(row[ColumnHeaders.CustomerId].ToString())).FirstOrDefault();
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
                                    newCustomer.CustomerId = this.CreateCustomer(newCustomer).Value;
                                    if (newCustomer.CustomerId == -1)
                                    {
                                        result.CustomerFailures.Add(string.Format("Database error while creating customer {0}.", newCustomer.Name));
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

                    #endregion

                    #region Project Import

                    ProjectInfo project = null;

                    // If there is no identifying information for projects, all project related importing is skipped.
                    if (hasProjectName || hasProjectId)
                    {
                        // Find name and id for the project, if both exist
                        string name = null;
                        string orgId = null;
                        if (hasProjectName && hasProjectId)
                        {
                            name = row[ColumnHeaders.ProjectName].ToString();
                            orgId = row[ColumnHeaders.ProjectId].ToString();
                        }
                        else
                        {
                            foreach (DataTable link in projectImportLinks)
                            {
                                try
                                {
                                    name = hasProjectName ? row[ColumnHeaders.ProjectName].ToString() : link.Select(
                                        string.Format("[{0}] = '{1}'", ColumnHeaders.ProjectId, row[ColumnHeaders.ProjectId].ToString()))[0][ColumnHeaders.ProjectName].ToString();
                                    orgId = hasProjectId ? row[ColumnHeaders.ProjectId].ToString() : link.Select(
                                        string.Format("[{0}] = '{1}'", ColumnHeaders.ProjectName, row[ColumnHeaders.ProjectName].ToString()))[0][ColumnHeaders.ProjectId].ToString();
                                    break;
                                }
                                catch (IndexOutOfRangeException) { }
                            }
                        }

                        // Find the customer this project is under, from this sheet if possible, or from the link sheet if needed. 
                        CustomerInfo customer = customersProjects.Select(tup => tup.Item1).Where(c => hasCustomerName ? c.Name.Equals(row[ColumnHeaders.CustomerName].ToString()) : hasCustomerId ? c.CustomerOrgId.Equals(row[ColumnHeaders.CustomerId].ToString()) : c.Equals(null)).FirstOrDefault();
                        if (customer == null)
                        {
                            if (projectCustomerLinks.Count() > 0)
                            {
                                foreach (DataTable link in projectCustomerLinks)
                                {
                                    // If that information is not on this sheet, we'll use the project-customer link sheet
                                    bool linkHasCustomerName = link.Columns.Contains(ColumnHeaders.CustomerName);
                                    bool linkHasProjectName = link.Columns.Contains(ColumnHeaders.ProjectName);
                                    string customerIdentity = null;
                                    try
                                    {
                                        customerIdentity = link.Select(string.Format("[{0}] = '{1}'",
                                            linkHasProjectName ? ColumnHeaders.ProjectName : ColumnHeaders.ProjectId,
                                            linkHasProjectName ? name : orgId)
                                        )[0][linkHasCustomerName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId].ToString().Trim();
                                        customer = customersProjects.Select(tup => tup.Item1).Where(c => linkHasCustomerName ? c.Name.Equals(customerIdentity) : c.CustomerOrgId.Equals(customerIdentity)).FirstOrDefault();
                                        break; // Match found.
                                    } catch (IndexOutOfRangeException) { }
                                }
                            }
                        }

                        // Find the existing project.
                        if (customer != null)
                        {
                            // This project has a customer specified, so we find the existing project under that customer or create it if it doesn't exist.
                            project = customersProjects.Where(tup => tup.Item1.CustomerId == customer.CustomerId).FirstOrDefault().Item2.Where(
                                p => hasProjectName ? p.Name.Equals(row[ColumnHeaders.ProjectName].ToString()) : p.ProjectOrgId.Equals(row[ColumnHeaders.ProjectId].ToString())).FirstOrDefault();
                            if (project == null)
                            {
                                if (canCreateProjects)
                                {
                                    // No project was found, so a new one is created.
                                    if (name == null || orgId == null)
                                    {
                                        // Raise error: Could not find name/id for project id/name
                                        continue;
                                    }
                                    project = new ProjectInfo
                                    {
                                        CustomerId = customer.CustomerId,
                                        Name = name,
                                        Type = "Hourly",
                                        OrganizationId = this.UserContext.ChosenOrganizationId,
                                        ProjectOrgId = orgId,
                                        StartingDate = DateTime.Now,
                                        EndingDate = DateTime.Now.AddMonths(6)
                                    };
                                    project.ProjectId = this.CreateProject(project);
                                    if (project.ProjectId == -1)
                                    {
                                        project = null;
                                    }
                                    else
                                    {
                                        customersProjects.Where(tup => tup.Item1 == customer).FirstOrDefault().Item2.Add(project);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // No customer can be specified, so we try to find the project under any customer.
                            project = customersProjects.Select(tup => tup.Item2).Select(
                                plist => plist.Where(
                                    p => hasProjectName ? p.Name.Equals(row[ColumnHeaders.ProjectName].ToString()) : p.ProjectOrgId.Equals(row[ColumnHeaders.ProjectId].ToString())
                                ).FirstOrDefault()
                            ).FirstOrDefault();
                        }

                        // Importing non-required project data
                        if(project != null)
                        {
                            bool updated = false;
                            string startDate = null;
                            string endDate = null;

                            updated = this.readColumn(row, ColumnHeaders.ProjectType, val => project.Type = val) || updated;
                            updated = this.readColumn(row, ColumnHeaders.ProjectStartDate, val => startDate = val) || updated;
                            updated = this.readColumn(row, ColumnHeaders.CustomerCountry, val => endDate = val) || updated;
                            if (startDate != null) project.StartingDate = DateTime.Parse(startDate);
                            if (endDate != null) project.EndingDate = DateTime.Parse(endDate);

                            if (updated)
                            {
                                this.UpdateProject(project);
                            }
                        }
                    }

                    #endregion

                    #region User Import

                    UserInfo user = null;
                    if (hasUserEmail || hasEmployeeId || hasUserName)
                    {
                        // Find existing user by whatever information we have
                        Tuple<string, UserInfo> userTuple = null;
                        if(hasUserEmail)
                        {
                            userTuple = users.Where(tup => tup.Item2.Email.Equals(row[ColumnHeaders.UserEmail].ToString())).FirstOrDefault();
                        }
                        else
                        {
                            if(hasEmployeeId)
                            {
                                userTuple = users.Where(tup => tup.Item1.Equals(row[ColumnHeaders.EmployeeId].ToString())).FirstOrDefault();
                            }
                            else
                            {
                                userTuple = users.Where(tup => tup.Item2.FirstName.Equals(row[ColumnHeaders.UserFirstName].ToString()) && tup.Item2.LastName.Equals(row[ColumnHeaders.UserLastName].ToString())).FirstOrDefault();
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

                                /*  This function is a lot to take in, so here's an overview:
                                    There are 4 required fields, two of which must be together. In the worst case scenario we'll be using 2 different links to get them all (e.g. one sheet has
                                    email & id, another has id, last name, and first name). We start with the field(s) that we don't have and use any sheets discovered above that link from that field 
                                    to fields we do have. If one of them gives us a match, we store the found value and move on. This process is done in 2 passes (each pass only checks missing fields, so
                                    if they're all found, the pass does nothing and quickly finishes), allowing for the case of needing 2 links to get a value. If all four values haven't
                                    been found after that, we can be sure that they can't all be found.
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
                                                            if(fields[j] != null)
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

                                if(fields.All(s => !string.IsNullOrEmpty(s)))
                                {
                                    // All required info was found successfully
                                    if (Service.IsEmailAddressValid(fields[0]))
                                    {
                                        string[] names = fields[2].Split(new string[] { "__IMPORT__" }, StringSplitOptions.None);
                                        try
                                        {
                                            user = new UserInfo()
                                            {
                                                Email = fields[0],
                                                FirstName = names[0],
                                                LastName = names[1],
                                                PasswordHash = Lib.Crypto.ComputeSHA512Hash("password") // TODO: Figure out a better default password generation system
                                            };
                                            user.UserId = DBHelper.CreateUser(InfoObjectsUtility.GetDBEntityFromUserInfo(user));
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
                                                }
                                                catch (System.Data.SqlClient.SqlException)
                                                {
                                                    // Raise error: error creating org user (in test, it's always a duplicate employee id)
                                                }
                                                users.Add(new Tuple<string, UserInfo>(fields[1], user));
                                            }
                                            else
                                            {
                                                // Raise error: error creating new user
                                            }
                                        }
                                        catch (System.Data.SqlClient.SqlException)
                                        {
                                            // Raise error: error creating new user
                                        }
                                    }
                                    else
                                    {
                                        // Raise error: invalid email for user
                                    }
                                }

                            }
                        }

                        // Importing non-required user data
                        if (user != null)
                        {
                            bool updated = false;

                            updated = this.readColumn(row, ColumnHeaders.UserAddress, val => user.Address = val) || updated;
                            updated = this.readColumn(row, ColumnHeaders.UserCity, val => user.City = val) || updated;
                            updated = this.readColumn(row, ColumnHeaders.UserCountry, val => user.Country = val) || updated;
                            updated = this.readColumn(row, ColumnHeaders.UserDateOfBirth, val => user.State = val) || updated;
                            updated = this.readColumn(row, ColumnHeaders.UserName, val => user.UserName = val) || updated;
                            updated = this.readColumn(row, ColumnHeaders.UserPhoneExtension, val => user.PhoneExtension = val) || updated;
                            updated = this.readColumn(row, ColumnHeaders.UserPhoneNumber, val => user.PhoneNumber = val) || updated;
                            updated = this.readColumn(row, ColumnHeaders.UserPostalCode, val => user.PostalCode = val) || updated;
                            updated = this.readColumn(row, ColumnHeaders.UserState, val => user.State = val) || updated;
                            
                            if (updated)
                            {
                                this.SaveUserInfo(user);
                            }
                        }
                    }
                    #endregion

                    #region Project-user and Time Entry Import
                    if(canImportProjectUser)
                    {
                        // Double-check that previous adding/finding of project and user didn't fail
                        if(project != null && user != null)
                        {
                            // Find existing project user
                            if (!this.GetProjectsByUserAndOrganization(user.UserId).Where(p => p.ProjectId == project.ProjectId).Any())
                            {
                                // If no project user entry exists for this user and project, we create one.
                                this.CreateProjectUser(project.ProjectId, user.UserId);
                            }

                            // Time Entry Import
                            if(canImportTimeEntry)
                            {
                                // Check for subscription role
                                bool canImportThisEntry = false;
                                int timeTrackerProductId = Service.GetProductIdByName("TimeTracker");
                                if (!this.GetUsersWithSubscriptionToProductInOrganization(this.UserContext.ChosenOrganizationId, timeTrackerProductId).Where(u => u.UserId == user.UserId).Any())
                                {
                                    // No existing subscription for this user, so we create one.
                                    var ttSub = DBHelper.GetSubscriptionsDisplayByOrg(this.UserContext.ChosenOrganizationId).Where(s => s.ProductId == timeTrackerProductId).SingleOrDefault();
                                    if(ttSub != null && ttSub.SubscriptionsUsed < ttSub.NumberOfUsers)
                                    {
                                        this.DBHelper.UpdateSubscriptionUserProductRole((int)(ProductRole.TimeTrackerUser), ttSub.SubscriptionId, user.UserId);
                                        canImportThisEntry = true; // Successfully created.
                                    }
                                    else
                                    {
                                        // Raise error: Not enough slots left in Time Tracker subscription
                                    }
                                }
                                else
                                {
                                    // Found existing subscription user.
                                    canImportThisEntry = true;
                                }

                                // Import entry
                                if(canImportThisEntry)
                                {
                                    string date = null;
                                    string duration = null;
                                    string description = "";
                                    string payclass = "Regular";
                                    this.readColumn(row, ColumnHeaders.Date, val => date = val);
                                    this.readColumn(row, ColumnHeaders.Duration, val => duration = val);
                                    this.readColumn(row, ColumnHeaders.Description, val => description = val);
                                    this.readColumn(row, ColumnHeaders.PayClass, val => payclass = val);
                                    PayClassInfo payClass = DBHelper.GetPayClasses(UserContext.ChosenOrganizationId).Select(pc => InfoObjectsUtility.InitializePayClassInfo(pc)).Where(p => p.Name.ToUpper().Equals(payclass.ToUpper())).SingleOrDefault();
                                    DateTime theDate = DateTime.Parse(date);
                                    float theDuration = float.Parse(duration);
                                    if(date != null && duration != null & payClass != null)
                                    {
                                        // Find existing entry. If none, create new one
                                        List<TimeEntryDBEntity> entries = DBHelper.GetTimeEntriesByUserOverDateRange(new List<int> { user.UserId }, this.UserContext.ChosenOrganizationId, theDate, theDate).ToList();
                                        if (!entries.Where(e => e.Description.Equals(description) && e.Duration == theDuration && e.PayClassId == payClass.PayClassID && e.ProjectId == project.ProjectId).Any())
                                        {
                                            // All required information is present
                                            try
                                            {
                                                if (DBHelper.CreateTimeEntry(new DBModel.TimeTracker.TimeEntryDBEntity
                                                {
                                                    Date = DateTime.Parse(date),
                                                    Description = description,
                                                    Duration = float.Parse(duration),
                                                    FirstName = user.FirstName,
                                                    LastName = user.LastName,
                                                    PayClassId = payClass.PayClassID,
                                                    ProjectId = project.ProjectId,
                                                    UserId = user.UserId
                                                }) == -1)
                                                {
                                                    // Raise error: could not create time entry
                                                }
                                            }
                                            catch (FormatException)
                                            {
                                                // Raise error: date or duration has bad format
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
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
            } catch (IndexOutOfRangeException)
            {
                return null;
            }
        }
    }
}