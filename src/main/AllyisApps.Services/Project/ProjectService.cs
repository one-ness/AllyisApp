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
using AllyisApps.Services.Utilities;
using AllyisApps.Services.Crm;

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
			this.authorizationService = new AuthorizationService(connectionString, userContext);
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

        /// <summary>
        /// Imports customers into the database from an excel file
        /// Excel file connection code based on http://stackoverflow.com/questions/14261655/best-fastest-way-to-read-an-excel-sheet-into-a-datatable
        /// </summary>
        /// <param name="filepath">The path to the excel file</param>
        public void ImportProjects(string filepath)
        {
            #region OleDbSetup
            string sSheetName = null;
            string sConnection = null;
            DataTable dtTablesList = default(DataTable);
            OleDbCommand oleExcelCommand = default(OleDbCommand);
            OleDbDataReader oleExcelReader = default(OleDbDataReader);
            OleDbConnection oleExcelConnection = default(OleDbConnection);

            sConnection = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\"", filepath);

            oleExcelConnection = new OleDbConnection(sConnection);
            oleExcelConnection.Open();

            dtTablesList = oleExcelConnection.GetSchema("Tables");

            if (dtTablesList.Rows.Count > 0)
            {
                sSheetName = dtTablesList.Rows[0]["TABLE_NAME"].ToString();
            }

            dtTablesList.Clear();
            dtTablesList.Dispose();
            #endregion OledbSetup

            if (!string.IsNullOrEmpty(sSheetName))
            {
                oleExcelCommand = oleExcelConnection.CreateCommand();
                oleExcelCommand.CommandText = "Select * From [" + sSheetName + "]";
                oleExcelCommand.CommandType = CommandType.Text;
                oleExcelReader = oleExcelCommand.ExecuteReader();
                //nOutputRow = 0;

                //Get existing customers just once
                IEnumerable<CustomerInfo> customerList = CrmService.GetCustomerList(this.UserContext.ChosenOrganizationId);
                while (oleExcelReader.Read())
                {
                    /* Projects are dependant on Customers; so, to import a project, we must ensure that the associated customer
                    also gets imported first. */
                    string customerName = oleExcelReader.GetString(oleExcelReader.GetOrdinal(ColumnHeaders.CustomerName));
                    string projectName = oleExcelReader.GetString(oleExcelReader.GetOrdinal(ColumnHeaders.ProjectName));
                    int? id;

                    /* TODO: Once we know more about what the imported file will look like (specifically, column names for data),
                    we can add more CustomerInfo values from the imported file. To do so, go to ServiceConstants.cs and
                    add a constant variable under the ColumnHeaders class for the excel file's column header, then use it to grab the column
                    ordinal to grab the value. See grabbing the customer name and project name, above.
                    */
                    if (!(id = customerList.Where(C => C.Name == customerName).SingleOrDefault().CustomerId).HasValue) // Only create customers that do not already exist in the org; get the id if they do
                        id = CrmService.CreateCustomer(new CustomerInfo()
                        {
                            Name = customerName,
                            OrganizationId = this.UserContext.ChosenOrganizationId
                        });
                    if (!this.GetProjectsByCustomer(id.Value).Any(P => P.Name == projectName)) // Only create projects that do not already exist under the customer
                        this.CreateProject(
                            this.UserContext.ChosenOrganizationId,
                            id.Value,
                            projectName,
                            oleExcelReader.GetString(oleExcelReader.GetOrdinal(ColumnHeaders.ProjectType)),
                            oleExcelReader.GetDateTime(oleExcelReader.GetOrdinal(ColumnHeaders.ProjectStartDate)),
                            oleExcelReader.GetDateTime(oleExcelReader.GetOrdinal(ColumnHeaders.ProjectEndDate))
                            );
                }
                oleExcelReader.Close();
            }
            oleExcelConnection.Close();
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