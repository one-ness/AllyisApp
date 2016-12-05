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

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectService"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="userContext">The user context.</param>
        public ProjectService(string connectionString, UserContext userContext) : base(connectionString, userContext)
        {
            //this.authorizationService = new AuthorizationService(connectionString, userContext);
            //this.CrmService = new CrmService(connectionString);
            //this.CrmService.SetUserContext(userContext);
        }

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
        public async Task ImportUsersProjectsCustomers(DataTable importData)
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

            foreach(DataRow row in importData.Rows)
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
                        customerId = CrmService.CreateCustomer(newCustomer);
                        projects.Add(new Tuple<CustomerInfo, List<ProjectInfo>>(newCustomer, new List<ProjectInfo>()));
                    } else
                    {
                        customerProjectList = projects.Where(t => t.Item1.CustomerId == customerId).Select(t => t.Item2).Single(); // Projects under this customer
                    }

                    //Updating customer info

                    CustomerInfo updateCustomer = CrmService.GetCustomer(customerId.Value);
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
                    
                    if(updated)
                    {
                        CrmService.UpdateCustomer(updateCustomer);
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

                if(projectNameData != "")
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
            } catch (ArgumentException) { }
            return false;
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