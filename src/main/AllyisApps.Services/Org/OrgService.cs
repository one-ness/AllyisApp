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
using AllyisApps.Services.Billing;
using AllyisApps.Services.Utilities;

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
		/// <returns>Organizaiton Id.</returns>
		public int CreateOrganization(OrganizationInfo organization, int ownerId)
		{
			if (organization == null)
			{
				throw new ArgumentNullException("organization", "Organization must not be null.");
			}

			if (ownerId <= 0)
			{
				throw new ArgumentOutOfRangeException("ownerId", "Organization owner's user id cannot be 0 or negative.");
			}

			return DBHelper.CreateOrganization(InfoObjectsUtility.GetDBEntityFromOrganizationInfo(organization), ownerId, (int)OrganizationRole.Owner);
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
		public void AddToOrganization(int userId, int orgId, int projectId, int orgRole)
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
			#endregion Validation

			DBHelper.CreateOrganizationUser(new OrganizationUserDBEntity() // ...add them to that organization as a member
			{
				UserId = userId,
				OrganizationId = orgId,
				OrgRoleId = orgRole
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
        /// Import data from a workbook. Imports customers, projects, users, project/user relationships, and/or time entry data.
        /// </summary>
        /// <param name="importData">Workbook with data to import.</param>
        public void Import(DataSet importData)
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

            // Then, we loop through and see what can be imported from each table in turn. Order doesn't matter, since missing information
            // will be sought from other tables as needed.
            foreach (DataTable table in tables)
            {
                // Customer importing: requires both customer name and customer id. Other information is optional, and can be filled in later.
                // First, we check if both required fields are present in this table, or if only one is, whether both are on another table.
                bool hasCustomerName = table.Columns.Contains(ColumnHeaders.CustomerName);
                bool hasCustomerId = table.Columns.Contains(ColumnHeaders.CustomerId);
                bool canCreateCustomers = hasCustomerName && hasCustomerId;
                DataTable customerImportLink = null;
                if (hasCustomerName ^ hasCustomerId)
                {
                    customerImportLink = tables.Where(t => t.Columns.Contains(ColumnHeaders.CustomerName) && t.Columns.Contains(ColumnHeaders.CustomerId)).FirstOrDefault();
                    if (customerImportLink != null)
                    {
                        canCreateCustomers = true;
                    }
                }

                // Finally, after all checks are complete, we go through row by row and import the information
                foreach (DataRow row in table.Rows)
                {
                    if (row.ItemArray.All(i => string.IsNullOrEmpty(i?.ToString()))) break; // Avoid iterating through empty rows

                    // If there is no identifying information for customers, all customer related importing is skipped.
                    if (canCreateCustomers)
                    {
                        // Customer: find the existing customer using name, or id of name isn't on this sheet.
                        CustomerInfo customer = customersProjects.Select(tup => tup.Item1).Where(c => hasCustomerName ? c.Name.Equals(row[ColumnHeaders.CustomerName].ToString()) : c.CustomerOrgId.Equals(row[ColumnHeaders.CustomerId].ToString())).FirstOrDefault();
                        if (customer == null)
                        {
                            var idbool = hasCustomerId;
                            var thissheet = row[ColumnHeaders.CustomerName].ToString();
                            var idheader = ColumnHeaders.CustomerName;
                            var selectstring = string.Format("'{0}' = '{1}'", idheader, row[ColumnHeaders.CustomerName].ToString());
                            var selectresult = customerImportLink.Select(selectstring);
                            var selectresultfirst = selectresult[0];
                            var selectresultlookup = selectresultfirst[ColumnHeaders.CustomerId].ToString();

                            // No customer was found, so a new one is created.
                            CustomerInfo newCustomer = new CustomerInfo
                            {
                                // For each required field, if it is not present on this sheet, then the linked sheet is used to look it up based on the other value.
                                Name = hasCustomerName ? row[ColumnHeaders.CustomerName].ToString() : customerImportLink.Select(
                                    string.Format("{0} = {1}", ColumnHeaders.CustomerId, row[ColumnHeaders.CustomerId].ToString()))[0][ColumnHeaders.CustomerName].ToString(),
                                CustomerOrgId = hasCustomerId ? row[ColumnHeaders.CustomerId].ToString() : customerImportLink.Select(
                                    string.Format("{0} = {1}", ColumnHeaders.CustomerName, row[ColumnHeaders.CustomerName].ToString()))[0][ColumnHeaders.CustomerId].ToString()
                            };
                            //newCustomer.CustomerId = this.CreateCustomer(newCustomer).Value;
                            customersProjects.Add(new Tuple<CustomerInfo, List<ProjectInfo>>(
                                newCustomer,
                                new List<ProjectInfo>()
                            ));
                            customer = newCustomer;
                        }

                        // Importing non-required customer data
                        bool updated = false;

                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerStreetAddress, val => customer.Address = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerCity, val => customer.City = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerCountry, val => customer.Country = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerState, val => customer.State = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerPostalCode, val => customer.PostalCode = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerEmail, val => customer.ContactEmail = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerPhoneNumber, val => customer.ContactPhoneNumber = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerFaxNumber, val => customer.FaxNumber = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerEIN, val => customer.EIN = val);

                        if (updated)
                        {
                            //this.UpdateCustomer(customer);
                        }
                    }
                }
            }
            
            #region old code
            /*
            #region Table Links

            // First, we sweep through the worksheets present and look at what types of data (column headers) are on each, noting
            // which sheets provide crucial links between required properties.                                             

            // Customer and project basic properties
            List<DataTable> customerNameAndId = new List<DataTable>();
            List<DataTable> projectNameAndId = new List<DataTable>();

            // Project-customer link: can be either name or id for either
            List<DataTable> projectAndCustomer = new List<DataTable>();
            List<bool> projectUsesName = new List<bool>(); // These values correspond to the same index in the projectAndCustomer table lists.
            List<bool> customerUsesName = new List<bool>(); // True if the name column is present (or both), false if the id column is present.

            // User properties: four are necessary, so there are six possible links that are checked in turn as needed.
            List<DataTable> userEmailAndId = new List<DataTable>();
            List<DataTable> userEmailAndFirstName = new List<DataTable>();
            List<DataTable> userEmailAndLastName = new List<DataTable>();
            List<DataTable> userIdAndFirstName = new List<DataTable>();
            List<DataTable> userIdAndLastName = new List<DataTable>();
            List<DataTable> userFirstNameAndLastName = new List<DataTable>();

            // TODO: Fill out time entry requirement

            foreach (DataTable table in importData.Tables)
            {
                List<string> columnHeaders = new List<string>();
                foreach(DataColumn column in table.Columns)
                {
                    columnHeaders.Add(column.ColumnName);
                }

                if (columnHeaders.Contains(ColumnHeaders.CustomerName) && columnHeaders.Contains(ColumnHeaders.CustomerId))
                {
                    customerNameAndId.Add(table);
                }

                if (columnHeaders.Contains(ColumnHeaders.ProjectName) && columnHeaders.Contains(ColumnHeaders.ProjectId))
                {
                    projectNameAndId.Add(table);
                }

                if ((columnHeaders.Contains(ColumnHeaders.CustomerName) || columnHeaders.Contains(ColumnHeaders.CustomerId)) &&
                    (columnHeaders.Contains(ColumnHeaders.ProjectName) || columnHeaders.Contains(ColumnHeaders.ProjectId)))
                {
                    projectAndCustomer.Add(table);
                    projectUsesName.Add(columnHeaders.Contains(ColumnHeaders.ProjectName));
                    customerUsesName.Add(columnHeaders.Contains(ColumnHeaders.CustomerName));
                }

                if (columnHeaders.Contains(ColumnHeaders.UserEmail) && columnHeaders.Contains(ColumnHeaders.EmployeeId))
                {
                    userEmailAndId.Add(table);
                }

                if (columnHeaders.Contains(ColumnHeaders.UserEmail) && columnHeaders.Contains(ColumnHeaders.UserFirstName))
                {
                    userEmailAndFirstName.Add(table);
                }

                if (columnHeaders.Contains(ColumnHeaders.UserEmail) && columnHeaders.Contains(ColumnHeaders.UserLastName))
                {
                    userEmailAndLastName.Add(table);
                }

                if (columnHeaders.Contains(ColumnHeaders.EmployeeId) && columnHeaders.Contains(ColumnHeaders.UserFirstName))
                {
                    userIdAndFirstName.Add(table);
                }

                if (columnHeaders.Contains(ColumnHeaders.EmployeeId) && columnHeaders.Contains(ColumnHeaders.UserLastName))
                {
                    userIdAndLastName.Add(table);
                }

                if (columnHeaders.Contains(ColumnHeaders.UserFirstName) && columnHeaders.Contains(ColumnHeaders.UserLastName))
                {
                    userFirstNameAndLastName.Add(table);
                }
            }

            #endregion

            // Retrieval of existing customer and project data
            List<Tuple<CustomerInfo, List<ProjectInfo>>> projects = new List<Tuple<CustomerInfo, List<ProjectInfo>>>();
            foreach (CustomerInfo customer in this.GetCustomerList(this.UserContext.ChosenOrganizationId))
            {
                projects.Add(new Tuple<CustomerInfo, List<ProjectInfo>>(
                    customer,
                    this.GetProjectsByCustomer(customer.CustomerId).ToList()
                ));
            }

            // (<projectId, List<userId's>>) Structure for final userId lists to add to projects
            List<Tuple<int, List<int>>> projectUsers = new List<Tuple<int, List<int>>>();

            // Looping through rows in tables
            foreach(DataTable table in importData.Tables)
            {
                foreach(DataRow row in table.Rows)
                {
                    if (row.ItemArray.All(i => string.IsNullOrEmpty(i?.ToString()))) break; // Avoid iterating through empty rows

                    // Customer name                                                                                                                    // NEXT: Use name or customer id column
                    string customerName = "";
                    int? customerId = 0;
                    bool customerIsSpecified = this.readColumn(row, ColumnHeaders.CustomerName, val => customerName = val);
                    List<ProjectInfo> customerProjectList = new List<ProjectInfo>();

                    // Creating customer & importing info
                    if (customerIsSpecified)
                    {
                        // Only create customers that do not already exist in the org; get the id if they do
                        if (projects.Count() == 0 ||
                            (customerId = projects.Where(t => t.Item1.Name == customerName).Select(t => t.Item1.CustomerId).DefaultIfEmpty(0).FirstOrDefault()) == 0)
                        {
                            // Customer creation
                            CustomerInfo newCustomer = new CustomerInfo() { Name = customerName, OrganizationId = this.UserContext.ChosenOrganizationId };
                            customerId = this.CreateCustomer(newCustomer);
                            projects.Add(new Tuple<CustomerInfo, List<ProjectInfo>>(newCustomer, new List<ProjectInfo>()));
                        }
                        else
                        {
                            customerProjectList = projects.Where(t => t.Item1.CustomerId == customerId).Select(t => t.Item2).Single(); // Projects under this customer
                        }

                        //Updating customer info

                        CustomerInfo updateCustomer = this.GetCustomer(customerId.Value);
                        bool updated = false;

                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerStreetAddress, val => updateCustomer.Address = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerCity, val => updateCustomer.City = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerCountry, val => updateCustomer.Country = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerState, val => updateCustomer.State = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerPostalCode, val => updateCustomer.PostalCode = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerEmail, val => updateCustomer.ContactEmail = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerPhoneNumber, val => updateCustomer.ContactPhoneNumber = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerFaxNumber, val => updateCustomer.FaxNumber = val);
                        updated = updated || this.readColumn(row, ColumnHeaders.CustomerEIN, val => updateCustomer.EIN = val);

                        if (updated)
                        {
                            this.UpdateCustomer(updateCustomer);
                        }
                    }
                }
            }

            foreach (DataRow row in importData.Tables[0].Rows)
            {
                if (row.ItemArray.All(i => string.IsNullOrEmpty(i?.ToString()))) break; // Avoid iterating through empty rows

                // Customer name
                string customerName = "";
                int? customerId = 0;
                bool customerIsSpecified = this.readColumn(row, ColumnHeaders.CustomerName, val => customerName = val);
                List<ProjectInfo> customerProjectList = new List<ProjectInfo>();

                // Creating customer & importing info
                if (customerIsSpecified)
                {
                    // Only create customers that do not already exist in the org; get the id if they do
                    if (projects.Count() == 0 ||
                        (customerId = projects.Where(t => t.Item1.Name == customerName).Select(t => t.Item1.CustomerId).DefaultIfEmpty(0).FirstOrDefault()) == 0)
                    {
                        // Customer creation
                        CustomerInfo newCustomer = new CustomerInfo() { Name = customerName, OrganizationId = this.UserContext.ChosenOrganizationId };
                        customerId = this.CreateCustomer(newCustomer);
                        projects.Add(new Tuple<CustomerInfo, List<ProjectInfo>>(newCustomer, new List<ProjectInfo>()));
                    }
                    else
                    {
                        customerProjectList = projects.Where(t => t.Item1.CustomerId == customerId).Select(t => t.Item2).Single(); // Projects under this customer
                    }

                    //Updating customer info

                    CustomerInfo updateCustomer = this.GetCustomer(customerId.Value);
                    bool updated = false;

                    updated = updated || this.readColumn(row, ColumnHeaders.CustomerStreetAddress, val => updateCustomer.Address = val);
                    updated = updated || this.readColumn(row, ColumnHeaders.CustomerCity, val => updateCustomer.City = val);
                    updated = updated || this.readColumn(row, ColumnHeaders.CustomerCountry, val => updateCustomer.Country = val);
                    updated = updated || this.readColumn(row, ColumnHeaders.CustomerState, val => updateCustomer.State = val);
                    updated = updated || this.readColumn(row, ColumnHeaders.CustomerPostalCode, val => updateCustomer.PostalCode = val);
                    updated = updated || this.readColumn(row, ColumnHeaders.CustomerEmail, val => updateCustomer.ContactEmail = val);
                    updated = updated || this.readColumn(row, ColumnHeaders.CustomerPhoneNumber, val => updateCustomer.ContactPhoneNumber = val);
                    updated = updated || this.readColumn(row, ColumnHeaders.CustomerFaxNumber, val => updateCustomer.FaxNumber = val);
                    updated = updated || this.readColumn(row, ColumnHeaders.CustomerEIN, val => updateCustomer.EIN = val);

                    if (updated)
                    {
                        this.UpdateCustomer(updateCustomer);
                    }
                }

                // Project name(s)
                string projectNameData = "";
                this.readColumn(row, ColumnHeaders.ProjectName, val => projectNameData = val);
                string[] projectNames = projectNameData.Split(',');

                // User email(s)
                string userEmailData = "";
                this.readColumn(row, ColumnHeaders.UserEmail, val => userEmailData = val);
                string[] userEmails = userEmailData.Split(',');

                if (projectNameData != "")
                {
                    foreach (string projectName in projectNames)
                    {
                        int projectId = 0;
                        // Only create projects that do not already exist in the customer; get the id if they do
                        if (customerProjectList.Count() == 0 ||
                            (projectId = customerProjectList.Where(p => p.Name == projectName).Select(p => p.ProjectId).DefaultIfEmpty(0).FirstOrDefault()) == 0)
                        {
                            // Projects being created, even if there are many, will share project information data from other columns. If none is provided, defaults are used.
                            string projectType = "Hourly";
                            string projectStartDate = DateTime.Now.Date.ToString();
                            string projectEndDate = DateTime.Now.Date.AddMonths(6).ToString();
                            this.readColumn(row, ColumnHeaders.ProjectType, val => projectType = val);
                            this.readColumn(row, ColumnHeaders.ProjectStartDate, val => projectStartDate = val);
                            this.readColumn(row, ColumnHeaders.ProjectEndDate, val => projectEndDate = val);

                            projectId = this.CreateProject(
                                UserContext.ChosenOrganizationId,
                                customerId.Value,
                                projectName,
                                projectType,
                                DateTime.Parse(projectStartDate),
                                DateTime.Parse(projectEndDate)
                            );
                        }
                    }
                }



            }
            */
            #endregion
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
    }
}