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
using AllyisApps.Services.Utilities;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
	/// <summary>
	/// The Poject Service.
	/// </summary>
	public partial class Service : BaseService
	{

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
						Name = dbe.Name,
                        ProjectOrgId = dbe.ProjectOrgId
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
        /// <param name="projectOrgId">The project ID as used by the organization</param>
        /// <param name="start">Starting. <see cref="DateTime"/></param>
        /// <param name="end">Ending.<see cref="DateTime"/></param>
        /// <returns>Project Id.</returns>
        public int CreateProject(int orgId, int customerId, string name, string type, string projectOrgId, DateTime start, DateTime end)
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

            if (string.IsNullOrEmpty(projectOrgId))
            {
                throw new ArgumentNullException("projectOrgId", "Project must have an ID");
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

			return DBHelper.CreateProject(orgId, customerId, name, type, projectOrgId, start, end);
		}

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="customerId">Customer Id.</param>
        /// <param name="name">Project name.</param>
        /// <param name="type">Project type.</param>
        /// <param name="projectOrgId">The ID of the project used by the organization</param>
        /// <param name="start">Starting. <see cref="DateTime"/></param>
        /// <param name="end">Ending. <see cref="DateTime"/></param>
        /// <returns>Project Id.</returns>
        public int CreateProjectFromCustomerIdOnly(int customerId, string name, string type, string projectOrgId, DateTime start, DateTime end)
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
            
            if (string.IsNullOrEmpty(projectOrgId))
            {
                throw new ArgumentNullException("projectOrgId", "Project must have an ID");
            }
			if (DateTime.Compare(start, end) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}
			#endregion Validation

			return DBHelper.CreateProjectFromCustomerIdOnly(customerId, name, type, projectOrgId, start, end);
		}

        public void ImportProjects(DataTable projectData)
        {
            // Get existing customers
            IEnumerable<CustomerInfo> customerList = this.GetCustomerList(this.UserContext.ChosenOrganizationId);

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
                    id = this.CreateCustomer(newCustomer);
                    customerList = customerList.Concat(new[] { newCustomer });
                }
                if (!this.GetProjectsByCustomer(id.Value).Any(P => P.Name == projectName)) // Only create projects that do not already exist under the customer
                    this.CreateProject(
                        this.UserContext.ChosenOrganizationId,
                        id.Value,                             
                        projectName,
                        row[ColumnHeaders.ProjectType].ToString(),
                        "",
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

			if (this.Can(Actions.CoreAction.EditProject))
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

			if (this.Can(Actions.CoreAction.EditProject))
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

        /// <summary>
        /// Returns a recommended available Project Org ID
        /// </summary>
        /// <returns></returns>
        public string GetRecommendedProjectId()
        {
            var projects = GetAllProjectsForOrganization(this.UserContext.ChosenOrganizationId);
            if (projects.Count() > 0) return new string (this.IncrementAlphanumericCharArray(projects.OrderBy(p => p.ProjectOrgId).LastOrDefault().ProjectOrgId.ToCharArray()));
            else return "0000000000000000"; // 16 max chars, arbitrary default value
        }

        public IEnumerable<ProjectInfo> GetAllProjectsForOrganization(int orgId)
        {
            var result = new List<ProjectInfo>();
            foreach (var customer in this.CrmService.GetCustomerList(orgId))
            {
                result.AddRange(this.GetProjectsByCustomer(customer.CustomerId));
            }
            return result;
        }
    }
}