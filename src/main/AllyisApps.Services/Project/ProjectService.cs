//------------------------------------------------------------------------------
// <copyright file="ProjectService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;

using AllyisApps.DBModel.Crm;
using AllyisApps.Services.Account;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Utilities;
using System.Threading.Tasks;

namespace AllyisApps.Services.Project
{
	/// <summary>
	/// The Poject Service.
	/// </summary>
	public class ProjectService : BaseService
	{
		/// <summary>
		/// Authorization in use for select methods.
		/// </summary>
		private AuthorizationService authorizationService;

        /// <summary>
        /// Crm Service in use for select methods
        /// </summary>
        private CrmService CrmService;

        /// <summary>
        /// Account Service in use for select methods
        /// </summary>
        private AccountService AccountService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectService"/> class.
		/// </summary>
		/// <param name="connectionString">The connectionString.</param>
		public ProjectService(string connectionString) : base(connectionString)
		{
		}

		///// <summary>
		///// Initializes a new instance of the <see cref="ProjectService"/> class.
		///// </summary>
		///// <param name="connectionString">The connection string.</param>
		///// <param name="userContext">The user context.</param>
		//public ProjectService(string connectionString, UserContext userContext) : base(connectionString, userContext)
		//{
		//	this.authorizationService = new AuthorizationService(connectionString, userContext);
  //          //this.CrmService = new CrmService(connectionString);
  //          //this.CrmService.SetUserContext(userContext);
		//}

        /// <summary>
        /// Provides links to account and crm service objects
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="accountService"></param>
        /// <param name="crmService"></param>
        public void SetServices(AuthorizationService authorizationService, AccountService accountService, CrmService crmService)
        {
            this.authorizationService = authorizationService;
            this.AccountService = accountService;
            this.CrmService = crmService;
        }

		/// <summary>
		/// Gets a list of <see cref="ProjectInfo"/>'s for a customer.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>List of ProjectInfo's.</returns>
		public IEnumerable<ProjectInfo> GetProjectsByCustomer(int customerId)
		{
			if (customerId <= 0)
			{
				throw new ArgumentOutOfRangeException("customerId", "Customer Id cannot be 0 or negative.");
			}

			IEnumerable<ProjectDBEntity> dbeList = DBHelper.GetProjectsByCustomer(customerId);
			List<ProjectInfo> list = new List<ProjectInfo>();
			foreach (ProjectDBEntity dbe in dbeList)
			{
				if (dbe != null)
				{
					list.Add(new ProjectInfo
					{
						ProjectId = dbe.ProjectId,
						CustomerId = dbe.CustomerId,
						OrganizationId = dbe.OrganizationId,
						Name = dbe.Name
					});
				}
			}

			return list;
		}

		/// <summary>
		/// Creates a new project.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="customerId">Customer Id.</param>
		/// <param name="name">Project name.</param>
		/// <param name="type">Project type.</param>
		/// <param name="start">Starting. <see cref="DateTime"/></param>
		/// <param name="end">Ending.<see cref="DateTime"/></param>
		/// <returns>Project Id.</returns>
		public int CreateProject(int orgId, int customerId, string name, string type, DateTime start, DateTime end)
		{
			#region Validation
			if (orgId <= 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be 0 or negative.");
			}

			if (customerId <= 0)
			{
				throw new ArgumentOutOfRangeException("customerId", "Customer Id cannot be 0 or negative.");
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name", "Project name must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrEmpty(type))
			{
				throw new ArgumentNullException("type", "Type must have a value.");
			}

			if (start == null)
			{
				throw new ArgumentNullException("start", "Project must have a start time");
			}

			if (end == null)
			{
				throw new ArgumentNullException("end", "Project must have an end time");
			}

			if (DateTime.Compare(start, end) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}
			#endregion Validation

			return DBHelper.CreateProject(orgId, customerId, name, type, start, end);
		}

		/// <summary>
		/// Creates a new project.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <param name="name">Project name.</param>
		/// <param name="type">Project type.</param>
		/// <param name="start">Starting. <see cref="DateTime"/></param>
		/// <param name="end">Ending. <see cref="DateTime"/></param>
		/// <returns>Project Id.</returns>
		public int CreateProjectFromCustomerIdOnly(int customerId, string name, string type, DateTime start, DateTime end)
		{
			#region Validation
			if (customerId <= 0)
			{
				throw new ArgumentOutOfRangeException("customerId", "Customer Id cannot be 0 or negative.");
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name", "Project name must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrEmpty(type))
			{
				throw new ArgumentNullException("type", "Type must have a value.");
			}

			if (start == null)
			{
				throw new ArgumentNullException("start", "Project must have a start time");
			}

			if (end == null)
			{
				throw new ArgumentNullException("end", "Project must have an end time");
			}

			if (DateTime.Compare(start, end) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}
			#endregion Validation

			return DBHelper.CreateProjectFromCustomerIdOnly(customerId, name, type, start, end);
		}

        public void ImportProjects(DataTable projectData)
        {
            // Get existing customers
            IEnumerable<CustomerInfo> customerList = CrmService.GetCustomerList(this.UserContext.ChosenOrganizationId);

            foreach (DataRow row in projectData.Rows)
            {
                if (row.ItemArray.All(i => string.IsNullOrEmpty(i?.ToString()))) break; // Avoid iterating through empty rows

                /* Projects are dependant on Customers; so, to import a project, we must ensure that the associated customer
                    also gets imported first. */
                string customerName = row[ColumnHeaders.CustomerName].ToString();
                string projectName = row[ColumnHeaders.ProjectName].ToString();
                int? id = 0;

                /* TODO: Once we know more about what the imported file will look like (specifically, column names for data),
                we can add more CustomerInfo values from the imported file. To do so, go to ServiceConstants.cs and
                add a constant variable under the ColumnHeaders class for the excel file's column header.
                */
                if (customerList.Count() == 0 || 
                    (id = customerList.Where(C=>C.Name == customerName).Select(C => C.CustomerId).DefaultIfEmpty(0).FirstOrDefault()) == 0) // Only create customers that do not already exist in the org; get the id if they do
                {
                    CustomerInfo newCustomer = new CustomerInfo() { Name = customerName, OrganizationId = this.UserContext.ChosenOrganizationId };
                    id = CrmService.CreateCustomer(newCustomer);
                    customerList = customerList.Concat(new[] { newCustomer });
                }
                if (!this.GetProjectsByCustomer(id.Value).Any(P => P.Name == projectName)) // Only create projects that do not already exist under the customer
                    this.CreateProject(
                        this.UserContext.ChosenOrganizationId,
                        id.Value,                             
                        projectName,
                        row[ColumnHeaders.ProjectType].ToString(),
                        Convert.ToDateTime(row[ColumnHeaders.ProjectStartDate]),
                        Convert.ToDateTime(row[ColumnHeaders.ProjectEndDate])
                        );
            }
        }
        

        /// <summary>
        /// Updates a project's properties and user list.
        /// </summary>
        /// <param name="projectId">Project Id.</param>
        /// <param name="name">Project name.</param>
        /// <param name="type">Project type.</param>
        /// <param name="start">Starting date. <see cref="DateTime"/></param>
        /// <param name="end">Ending date. <see cref="DateTime"/></param>
        /// <param name="userIDs">Updated on-project user list.</param>
        /// <returns>Returns false if authorization fails.</returns>
        public bool UpdateProjectAndUsers(int projectId, string name, string type, DateTime start, DateTime end, IEnumerable<int> userIDs)
		{
			#region Validation
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name", "Project name must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrEmpty(type))
			{
				throw new ArgumentNullException("type", "Type must have a value.");
			}

			if (start == null)
			{
				throw new ArgumentNullException("start", "Project must have a start time");
			}

			if (end == null)
			{
				throw new ArgumentNullException("end", "Project must have an end time");
			}

			if (DateTime.Compare(start, end) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			if (userIDs == null)
			{
				userIDs = new List<int>();
			}
			#endregion Validation

			if (this.authorizationService.Can(Services.Account.Actions.CoreAction.EditProject))
			{
				DBHelper.UpdateProjectAndUsers(projectId, name, type, start, end, userIDs);
				return true;
			}

			return false;
		}

        /// <summary>
        /// Import project user information from a data table. If a Project Name in the data table does not exist already, it will be created
        /// as long as there is also a corresponding Customer Name column with information in it. Otherwise, it will be ignored and the user
        /// information for that project will also be ignored. If a row has a value for Customer Name, the project(s) listed in that row will
        /// be only for that customer.
        /// </summary>
        public async Task ImportProjectUsers(DataTable projectUserData)
        {
            List<Tuple<CustomerInfo, List<ProjectInfo>>> projects = new List<Tuple<CustomerInfo, List<ProjectInfo>>>(); // Structure for customer and project relationships
            List<Tuple<int, List<int>>> projectUsers = new List<Tuple<int, List<int>>>(); // (<projectId, List<userId's>>) Structure for final userId lists to add to projects

            foreach(CustomerInfo customer in CrmService.GetCustomerList(this.UserContext.ChosenOrganizationId))
            {
                projects.Add(new Tuple<CustomerInfo, List<ProjectInfo>>(
                    customer,
                    this.GetProjectsByCustomer(customer.CustomerId).ToList()
                ));
            }

            foreach(DataRow row in projectUserData.Rows)
            {
                if (row.ItemArray.All(i => string.IsNullOrEmpty(i?.ToString()))) break; // Avoid iterating through empty rows

                // Multiples are allowed in either column, separated by commas. Adding users can be one-to-one, one-to-many by user or project, or
                // many-to-many (each user in list is added to each project in list). These two columns must be supplied and filled out in each row.
                string[] projectNameData;
                string[] userEmailData;
                try
                {
                    projectNameData = row[ColumnHeaders.ProjectName].ToString().Split(',');
                } catch (ArgumentException)
                {
                    throw new ArgumentException("The supplied spreadsheet has no column for " + ColumnHeaders.ProjectName);
                }
                try
                {
                    userEmailData = row[ColumnHeaders.UserEmail].ToString().Split(',');
                }
                catch (ArgumentException)
                {
                    throw new ArgumentException("The supplied spreadsheet has no column for " + ColumnHeaders.UserEmail);
                }
                // Rows missing data for user email or project name will simply be skippped.
                if (string.IsNullOrEmpty(projectNameData[0]))
                {
                    break; // TODO: Add this a returned error list
                }
                if (string.IsNullOrEmpty(userEmailData[0]))
                {
                    break; // TODO: Add this a returned error list
                }

                // Optional customer name specification.
                string customerName = "";
                bool customerIsSpecified = false;
                try
                {
                    customerName = row[ColumnHeaders.CustomerName].ToString();
                    customerIsSpecified = true;
                }
                catch (ArgumentException) { }
                
                foreach(string projectName in projectNameData)
                {
                    int projectId = 0;
                    if (customerIsSpecified) // Adding project users for a specific customer. If the customer doesn't exist, it will be added. If the project doesn't exist, it will be added.
                    {
                        int? customerId = 0;
                        if (projects.Count() == 0 ||
                            (customerId = projects.Where(t => t.Item1.Name == customerName).Select(t => t.Item1.CustomerId).DefaultIfEmpty(0).FirstOrDefault()) == 0) // Only create customers that do not already exist in the org; get the id if they do
                        {
                            CustomerInfo newCustomer = new CustomerInfo() { Name = customerName, OrganizationId = this.UserContext.ChosenOrganizationId };
                            customerId = CrmService.CreateCustomer(newCustomer);
                            projects.Add(new Tuple<CustomerInfo, List<ProjectInfo>>(newCustomer, new List<ProjectInfo>()));
                        }

                        List<ProjectInfo> projectList = projects.Where(t => t.Item1.CustomerId == customerId).Select(t => t.Item2).Single(); // Projects under this customer
                        if (projectList.Count() == 0 ||
                            (projectId = projectList.Where(p => p.Name == projectName).Select(p => p.ProjectId).DefaultIfEmpty(0).FirstOrDefault()) == 0) // Only create projects that do not already exist in the customer; get the id if they do
                        {
                            // If adding a new project, default values will be used for type, start date, and end date unless those column headers are also present and filled out.
                            string projectType = "Hourly";
                            DateTime projectStartDate = DateTime.Now.Date;
                            DateTime projectEndDate = projectStartDate.AddMonths(6);
                            try
                            {
                                string projectTypeData = row[ColumnHeaders.ProjectType].ToString();
                                projectType = string.IsNullOrEmpty(projectTypeData) ? projectType : projectTypeData;
                            }
                            catch (ArgumentException) { }
                            try
                            {
                                projectStartDate = Convert.ToDateTime(row[ColumnHeaders.ProjectStartDate]);
                            }
                            catch (ArgumentException) { }
                            catch (FormatException) { }
                            try
                            {
                                projectEndDate = Convert.ToDateTime(row[ColumnHeaders.ProjectEndDate]);
                            }
                            catch (ArgumentException) { }
                            catch (FormatException) { }

                            projectId = this.CreateProject(
                                this.UserContext.ChosenOrganizationId,
                                customerId.Value,
                                projectName,
                                projectType,
                                projectStartDate,
                                projectEndDate
                            );

                            projectList.Add(new ProjectInfo
                            {
                                ProjectId = projectId,
                                OrganizationId = this.UserContext.ChosenOrganizationId,
                                CustomerId = customerId.Value,
                                Name = projectName
                            });
                        }
                    }
                    else // Customer is not specified. Project name must exist under some customer in the organization. If not, the project is skipped.
                    {
                        projectId = projects.Select(                    // In the tuples
                            t => t.Item2.Where(                         // in the project lists
                                p => p.Name.Equals(projectName)         // take the projects where the name matches
                            ).Select(                                   // and return their id's
                                p => p.ProjectId                        // then take just the first one (or 0 if none)
                            ).DefaultIfEmpty(0).FirstOrDefault()).Where( // resulting in a single list of ints, one for each tuple
                                i => i > 0                              // Then, take all those that aren't 0
                            ).DefaultIfEmpty(0).FirstOrDefault();       // and return the first, or just 0 if all were 0
                    }

                    if (projectId == 0) // No matching project found, skip to next project.
                    {
                        break;
                    }

                    foreach(string userEmail in userEmailData)
                    {
                        UserInfo userInfo = await AccountService.GetUserByEmail(userEmail);
                        int userId = userInfo == null ? 0 : userInfo.UserId;
                        if(userInfo == null)
                        {
                            string userFirstName;
                            string userLastName;
                            try
                            {
                                userFirstName = row[ColumnHeaders.UserFirstName].ToString();
                                userLastName = row[ColumnHeaders.UserLastName].ToString();
                            }
                            catch (ArgumentException) // If first and last name data are not supplied for a non-existing user, it cannot be added
                            {
                                break;
                            }

                            userId = AccountService.SetupNewUser(
                                userEmail,
                                userFirstName,
                                userLastName,
                                null,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                "Password", // TODO: Create system of setting up default password and emailing it to new user
                                UserContext.ChosenLanguageID);
                        }
                    }
                }
            }
        }

		/// <summary>
		/// Deletes a project.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool DeleteProject(int projectId)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			if (this.authorizationService.Can(Services.Account.Actions.CoreAction.EditProject))
			{
				DBHelper.DeleteProject(projectId);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates a project user.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="userId">User Id.</param>
		public void CreateProjectUser(int projectId, int userId)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			DBHelper.CreateProjectUser(projectId, userId);
		}

		/// <summary>
		/// Updates a project user.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="userId">User Id.</param>
		/// <param name="isActive">Active status to update to.</param>
		/// <returns>Number of rows updated.</returns>
		public int UpdateProjectUser(int projectId, int userId, bool isActive)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			return DBHelper.UpdateProjectUser(projectId, userId, isActive ? 1 : 0);
		}

		/// <summary>
		/// Deletes a project user.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="userId">User Id.</param>
		/// <returns>Bool indication of success.</returns>
		public bool DeleteProjectUser(int projectId, int userId)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			return DBHelper.DeleteProjectUser(projectId, userId) == 1;
		}

		/// <summary>
		/// Gets a list of <see cref="UserInfo"/>'s for a project.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns>List of UserInfo's.</returns>
		public IEnumerable<UserInfo> GetUsersByProjectId(int projectId)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			return DBHelper.GetUsersByProjectId(projectId).Select(u => InfoObjectsUtility.InitializeUserInfo(u));
		}

		/// <summary>
		/// Gets the organizaiton Id from a project Id.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns>Organization Id.</returns>
		public int GetOrganizationFromProject(int projectId)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			return DBHelper.GetOrganizationFromProject(projectId);
		}

		/// <summary>
		/// Gets all the projects a user can use in the chosen organization.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="isActive">True (default) to only return active projects, false to include all projects, active or not.</param>
		/// <returns>A list of all the projects a user can access in an organization.</returns>
		public IEnumerable<CompleteProjectInfo> GetProjectsByUserAndOrganization(int userId, bool isActive = true)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			return DBHelper.GetProjectsByUserAndOrganization(userId, UserContext.ChosenOrganizationId, isActive ? 1 : 0).Select(c => InfoObjectsUtility.InitializeCompleteProjectInfo(c));
		}

		/// <summary>
		/// Gets a <see cref="CompleteProjectInfo"/>.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns>CompleteProjectInfo instance.</returns>
		public CompleteProjectInfo GetProject(int projectId)
		{
			if (projectId < 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be negative.");
			}

			return InfoObjectsUtility.InitializeCompleteProjectInfo(DBHelper.GetProjectById(projectId));
		}

		/// <summary>
		/// Gets a list of <see cref="CompleteProjectInfo"/>s for all projects a user can use.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <returns>List of CompleteProjectInfo objects.</returns>
		public IEnumerable<CompleteProjectInfo> GetProjectsByUserId(int userId)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			return DBHelper.GetProjectsByUserId(userId).Select(p => InfoObjectsUtility.InitializeCompleteProjectInfo(p));
		}
	}
}