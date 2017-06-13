//------------------------------------------------------------------------------
// <copyright file="CrmService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel;
using AllyisApps.DBModel.Crm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AllyisApps.Services
{
	/// <summary>
	/// Services for Customer Relationship Management related functions (customer, projects).
	/// </summary>
	public partial class AppService : BaseService
	{
		/// <summary>
		/// Gets a <see cref="Customer"/>.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>The customer entity.</returns>
		public Customer GetCustomer(int customerId)
		{
			return InitializeCustomer(DBHelper.GetCustomerInfo(customerId));
		}

		/// <summary>
		/// Gets the next logical customer id for the current organization, and a list of
		/// valid country names.
		/// </summary>
		/// <returns></returns>
		public Tuple<string, List<string>> GetNextCustIdAndCountries()
		{
			var spResults = DBHelper.GetNextCustIdAndCountries(UserContext.ChosenOrganizationId);
			return Tuple.Create(
				spResults.Item1 == null ? "0000000000000000" : new string(IncrementAlphanumericCharArray(spResults.Item1.ToCharArray())),
				spResults.Item2);
		}

		/// <summary>
		/// Gets a Customer for the given customer, and a list of valid country names.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns></returns>
		public Tuple<Customer, List<string>> GetCustomerAndCountries(int customerId)
		{
			var spResults = DBHelper.GetCustomerCountries(customerId);
			return Tuple.Create(
				InitializeCustomer(spResults.Item1),
				spResults.Item2);
		}

		/// <summary>
		/// Creates a customer.
		/// </summary>
		/// <param name="customer">Customer.</param>
		/// <returns>Customer id.</returns>
		public int? CreateCustomer(Customer customer)
		{
			if (this.Can(Actions.CoreAction.EditCustomer) && customer != null)
			{
				return DBHelper.CreateCustomerInfo(GetDBEntityFromCustomer(customer));
			}

			return null;
		}

        /// <summary>
        /// Updates a customer in the database.
        /// </summary>
        /// <param name="customer">Updated customer info.</param>
        /// <returns>Returns false if authorization fails.</returns>
        public int? UpdateCustomer(Customer customer)
        {
            if (this.Can(Actions.CoreAction.EditCustomer) && customer != null)
            {
                return DBHelper.UpdateCustomer(GetDBEntityFromCustomer(customer));
            }

            return null;
        }

        /*
        /// <summary>
        /// Updates a customer in the database.
        /// </summary>
        /// <param name="customer">Updated customer info.</param>
        /// <returns>Returns false if authorization fails.</returns>
        public bool UpdateCustomer(Customer customer)
        {
            if (this.Can(Actions.CoreAction.EditCustomer) && customer != null)
            {
                DBHelper.UpdateCustomer(GetDBEntityFromCustomer(customer));
                return true;
            }

            return false;
        }
        */

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <param name="customerId">Customer id.</param>
        /// <returns>Returns false if authorization fails.</returns>
        public string DeleteCustomer(int customerId)
        {
            if (this.Can(Actions.CoreAction.EditCustomer))
            {
                return DBHelper.DeleteCustomer(customerId);
            }
            return null;
        }

        /*
         /// <summary>
         /// Deletes a customer.
         /// </summary>
         /// <param name="customerId">Customer id.</param>
         /// <returns>Returns false if authorization fails.</returns>
         public bool DeleteCustomer(int customerId)
         {
             if (this.Can(Actions.CoreAction.EditCustomer))
             {
                 DBHelper.DeleteCustomer(customerId);
                 return true;
             }

             return false;
         }   
     */

        /// <summary>
        /// Reactivate a Customer
        /// </summary>
        /// <param name="customerId">Customer id.</param>
        /// <returns>Returns false if authorization fails.</returns>
        public string ReactivateCustomer(int customerId)
        {
            if (this.Can(Actions.CoreAction.EditCustomer))
            {
                return DBHelper.ReactivateCustomer(customerId);
            }
            return null;
        }

        ///// <summary>
        ///// Deletes a customer.
        ///// </summary>
        ///// <param name="customerId">Customer id.</param>
        ///// <returns>Returns false if authorization fails.</returns>
        //public bool ReactivateCustomer(int customerId)
        //{
        //    if (this.Can(Actions.CoreAction.EditCustomer))
        //    {
        //        DBHelper.ReactivateCustomer(customerId);
        //        return true;
        //    }

        //    return false;
        //}

        /// <summary>
        /// Gets a list of <see cref="Customer"/>'s for an organization.
        /// </summary>
        /// <param name="orgId">Organization Id.</param>
        /// <returns><see cref="IEnumerable{CustomerDBEntity}"/>.</returns>
        public IEnumerable<Customer> GetCustomerList(int orgId)
		{
			IEnumerable<CustomerDBEntity> dbeList = DBHelper.GetCustomerList(orgId);
			List<Customer> list = new List<Customer>();
			foreach (CustomerDBEntity dbe in dbeList)
			{
				if (dbe != null)
				{
					list.Add(InitializeCustomer(dbe));
				}
			}

			return list;
		}

		/// <summary>
		/// Returns a list of CompleteProjectInfos for the current organization with the IsProjectUser field filled
		/// out for the current user, and a list of CustomerInfos for the organization.
		/// </summary>
		/// <returns></returns>
		public Tuple<List<CompleteProjectInfo>, List<Customer>> GetProjectsAndCustomersForOrgAndUser()
		{
			var spResults = DBHelper.GetProjectsAndCustomersForOrgAndUser(UserContext.ChosenOrganizationId, UserContext.UserId);
			return Tuple.Create(
				spResults.Item1.Select(cpdb => InitializeCompleteProjectInfo(cpdb)).ToList(),
				spResults.Item2.Select(cdb => InitializeCustomer(cdb)).ToList());
		}

        /// <summary>
        /// Returns a list of CompleteProjectInfos for the current organization with the IsProjectUser field filled
        /// out for the current user, and a list of CustomerInfos for the organization.
        /// </summary>
        /// <returns></returns>
        public Tuple<List<CompleteProjectInfo>, List<Customer>> GetInactiveProjectsAndCustomersForOrgAndUser()
        {
            var spResults = DBHelper.GetInactiveProjectsAndCustomersForOrgAndUser(UserContext.ChosenOrganizationId, UserContext.UserId);
            return Tuple.Create(
                spResults.Item1.Select(cpdb => InitializeCompleteProjectInfo(cpdb)).ToList(),
                spResults.Item2.Select(cdb => InitializeCustomer(cdb)).ToList());
        }

        /// <summary>
        /// Returns a list of Projects for projects the given user is assigned to in the given organization
        /// (current organization by default), another list of Projects for all projects in the organization,
        /// the name of the user (as "Firstname Lastname"), and the user's email.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <param name="orgId">Organization Id.</param>
        /// <returns></returns>
        public Tuple<List<Project>, List<Project>, string, string> GetProjectsForOrgAndUser(int userId, int orgId = -1)
		{
			if (userId <= 0)
			{
				throw new ArgumentException("User Id cannot be zero or negative.", "userId");
			}

			if (orgId <= 0)
			{
				orgId = UserContext.ChosenOrganizationId;
			}

			var spResults = DBHelper.GetProjectsForOrgAndUser(userId, orgId);
			var userDBEntity = spResults.Item3;
			string name = string.Format("{0} {1}", userDBEntity.FirstName, userDBEntity.LastName);
			return Tuple.Create(
				spResults.Item1.Select(pdb => InitializeProject(pdb)).ToList(),
				spResults.Item2.Select(pdb => InitializeProject(pdb)).ToList(),
				name,
				userDBEntity.Email);
		}

		/// <summary>
		/// Gets a list of <see cref="Project"/>'s for a customer.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>List of ProjectInfo's.</returns>
		public IEnumerable<Project> GetProjectsByCustomer(int customerId)
		{
			if (customerId <= 0)
			{
				throw new ArgumentOutOfRangeException("customerId", "Customer Id cannot be 0 or negative.");
			}

			IEnumerable<ProjectDBEntity> dbeList = DBHelper.GetProjectsByCustomer(customerId);
			List<Project> list = new List<Project>();
			foreach (ProjectDBEntity dbe in dbeList)
			{
				if (dbe != null)
				{
					list.Add(InitializeProject(dbe));
				}
			}

			return list;
		}

		/// <summary>
		/// Creates a new project.
		/// </summary>
		/// <param name="newProject">Project with project information.</param>
		/// <returns>Project Id.</returns>
		public int CreateProject(Project newProject)
		{
			#region Validation

			if (newProject.CustomerId <= 0)
			{
				throw new ArgumentOutOfRangeException("customerId", "Customer Id cannot be 0 or negative.");
			}

			if (string.IsNullOrWhiteSpace(newProject.Name))
			{
				throw new ArgumentNullException("name", "Project name must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrEmpty(newProject.Type))
			{
				throw new ArgumentNullException("type", "Type must have a value.");
			}

			if (string.IsNullOrEmpty(newProject.ProjectOrgId))
			{
				throw new ArgumentNullException("projectOrgId", "Project must have an ID");
			}

			if (newProject.StartingDate.HasValue && newProject.EndingDate.HasValue && DateTime.Compare(newProject.StartingDate.Value, newProject.EndingDate.Value) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			return DBHelper.CreateProject(GetDBEntityFromProject(newProject));
		}

		/// <summary>
		/// Updates a project's properties.
		/// </summary>
		/// <param name="project">Project with updated properties.</param>
		public void UpdateProject(Project project)
		{
			#region Validation

			if (project.ProjectId <= 0)
			{
				throw new ArgumentOutOfRangeException("ProjectId", "Project Id cannot be 0 or negative.");
			}

			if (string.IsNullOrWhiteSpace(project.Name))
			{
				throw new ArgumentNullException("Name", "Project name must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrEmpty(project.Type))
			{
				throw new ArgumentNullException("Type", "Type must have a value.");
			}

			if (project.StartingDate.HasValue && project.EndingDate.HasValue && DateTime.Compare(project.StartingDate.Value, project.EndingDate.Value) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			if (this.Can(Actions.CoreAction.EditProject))
			{
				DBHelper.UpdateProject(GetDBEntityFromProject(project));
			}
		}

		/// <summary>
		/// Updates a project's properties and user list.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="name">Project name.</param>
		/// <param name="orgId">Project org id.</param>
		/// <param name="type">Project type.</param>
		/// <param name="start">Starting date. <see cref="DateTime"/></param>
		/// <param name="end">Ending date. <see cref="DateTime"/></param>
		/// <param name="userIDs">Updated on-project user list.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool UpdateProjectAndUsers(int projectId, string name, string orgId, string type, DateTime? start, DateTime? end, IEnumerable<int> userIDs)
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

			if (string.IsNullOrWhiteSpace(orgId))
			{
				throw new ArgumentNullException("orgId", "Project Org Id must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrEmpty(type))
			{
				throw new ArgumentNullException("type", "Type must have a value.");
			}

			if (start.HasValue && end.HasValue && DateTime.Compare(start.Value, end.Value) > 0)
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
				DBHelper.UpdateProjectAndUsers(projectId, name, orgId, type, start, end, userIDs);
				return true;
			}

			return false;
		}

        /// <summary>
        /// Reactivate a project
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>Returns false if authorization fails</returns>
        public bool ReactivateProject(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
            }

            if (this.Can(Actions.CoreAction.EditProject))
            {
                DBHelper.ReactivateProject(projectId);
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

			return DBHelper.GetProjectsByUserAndOrganization(userId, UserContext.ChosenOrganizationId, isActive ? 1 : 0).Select(c => InitializeCompleteProjectInfo(c));
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

			return InitializeCompleteProjectInfo(DBHelper.GetProjectById(projectId));
		}

		/// <summary>
		/// Gets a <see cref="CompleteProjectInfo"/>, with the IsProjectUser field filled out for the
		/// current user.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns>CompleteProjectInfo instance.</returns>
		public CompleteProjectInfo GetProjectAsUser(int projectId)
		{
			if (projectId < 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be negative.");
			}

			return InitializeCompleteProjectInfo(DBHelper.GetProjectByIdAndUser(projectId, UserContext.UserId));
		}

		/// <summary>
		/// Gets a CompleteProjectInfo for the given project, a list of UserInfos for the project's assigned
		/// users, and a list of SubscriptionUserInfos for all users in the current subscription.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns></returns>
		public Tuple<CompleteProjectInfo, List<User>, List<SubscriptionUserInfo>> GetProjectEditInfo(int projectId)
		{
			if (projectId < 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be negative.");
			}

			var spResults = DBHelper.GetProjectEditInfo(projectId, UserContext.ChosenSubscriptionId);
			return Tuple.Create(
				InitializeCompleteProjectInfo(spResults.Item1),
				spResults.Item2.Select(udb => InitializeUser(udb)).ToList(),
				spResults.Item3.Select(sudb => InitializeSubscriptionUserInfo(sudb)).ToList());
		}

		/// <summary>
		/// Gets the next logical project id for the given customer and a list of SubscriptionUserInfos for
		/// all useres in the current subscription.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns></returns>
		public Tuple<string, List<SubscriptionUserInfo>> GetNextProjectIdAndSubUsers(int customerId)
		{
			if (customerId < 0)
			{
				throw new ArgumentOutOfRangeException("customerId", "Customer Id cannot be negative.");
			}

			var spResults = DBHelper.GetNextProjectIdAndSubUsers(customerId, UserContext.ChosenSubscriptionId);
			return Tuple.Create(
				spResults.Item1 == null ? "0000000000000000" : new string(IncrementAlphanumericCharArray(spResults.Item1.ToCharArray())),
				spResults.Item2.Select(sudb => InitializeSubscriptionUserInfo(sudb)).ToList());
		}

		/// <summary>
		/// Gets all the projects in every customer in the entire organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>All the projects in the organization.</returns>
		public IEnumerable<Project> GetAllProjectsForOrganization(int orgId)
		{
			var result = new List<Project>();
			foreach (var customer in this.GetCustomerList(orgId))
			{
				result.AddRange(this.GetProjectsByCustomer(customer.CustomerId));
			}
			return result;
		}

		#region Info-DBEntity Conversions

		/// <summary>
		/// Initializes a <see cref="Customer"/> from a <see cref="CustomerDBEntity"/>.
		/// </summary>
		/// <param name="customer">The CustomerDBEntity to use.</param>
		/// <returns>A Customer object.</returns>
		public static Customer InitializeCustomer(CustomerDBEntity customer)
		{
			if (customer == null)
			{
				return null;
			}

            return new Customer()
            {
                Address = customer.Address,
                City = customer.City,
                ContactEmail = customer.ContactEmail,
                ContactPhoneNumber = customer.ContactPhoneNumber,
                Country = customer.Country,
                CreatedUTC = customer.CreatedUTC,
                CustomerId = customer.CustomerId,
                CustomerOrgId = customer.CustomerOrgId,
                EIN = customer.EIN,
                FaxNumber = customer.FaxNumber,
                Name = customer.Name,
                OrganizationId = customer.OrganizationId,
                PostalCode = customer.PostalCode,
                State = customer.State,
                Website = customer.Website,
                IsActive = customer.IsActive
			};
		}

		/// <summary>
		/// Initializes a <see cref="CustomerDBEntity"/> from a <see cref="Customer"/>.
		/// </summary>
		/// <param name="customer">The Customer to use.</param>
		/// <returns>A CustomerDBEntity object.</returns>
		public static CustomerDBEntity GetDBEntityFromCustomer(Customer customer)
		{
			if (customer == null)
			{
				return null;
			}

			return new CustomerDBEntity()
			{
				Address = customer.Address,
				City = customer.City,
				ContactEmail = customer.ContactEmail,
				ContactPhoneNumber = customer.ContactPhoneNumber,
				Country = customer.Country,
				CreatedUTC = customer.CreatedUTC,
				CustomerId = customer.CustomerId,
				CustomerOrgId = customer.CustomerOrgId,
				EIN = customer.EIN,
				FaxNumber = customer.FaxNumber,
				Name = customer.Name,
				OrganizationId = customer.OrganizationId,
				PostalCode = customer.PostalCode,
				State = customer.State,
				Website = customer.Website,
                IsActive = customer.IsActive
            };
		}

		/// <summary>
		/// Translates a <see cref="ProjectDBEntity"/> into a <see cref="Project"/>.
		/// </summary>
		/// <param name="project">ProjectDBEntity instance.</param>
		/// <returns>Project instance.</returns>
		public static Project InitializeProject(ProjectDBEntity project)
		{
			if (project == null)
			{
				return null;
			}

			return new Project
			{
				CustomerId = project.CustomerId,
				EndingDate = project.EndingDate,
				Name = project.Name,
				OrganizationId = project.OrganizationId,
				ProjectId = project.ProjectId,
				ProjectOrgId = project.ProjectOrgId,
				StartingDate = project.StartingDate,
				Type = project.Type
			};
		}

		/// <summary>
		/// Translates a <see cref="Project"/> into a <see cref="ProjectDBEntity"/>.
		/// </summary>
		/// <param name="project">Project instance.</param>
		/// <returns>ProjectDBEntity instance.</returns>
		public static ProjectDBEntity GetDBEntityFromProject(Project project)
		{
			if (project == null)
			{
				return null;
			}

			return new ProjectDBEntity
			{
				CustomerId = project.CustomerId,
				EndingDate = project.EndingDate,
				Name = project.Name,
				OrganizationId = project.OrganizationId,
				ProjectId = project.ProjectId,
				ProjectOrgId = project.ProjectOrgId,
				StartingDate = project.StartingDate,
				Type = project.Type
			};
		}

		/// <summary>
		/// Translates a <see cref="CompleteProjectDBEntity"/> into a <see cref="CompleteProjectInfo"/>.
		/// </summary>
		/// <param name="completeProject">CompleteProjectDBEntity instance.</param>
		/// <returns>CompleteProjectInfo instance.</returns>
		public static CompleteProjectInfo InitializeCompleteProjectInfo(CompleteProjectDBEntity completeProject)
		{
			if (completeProject == null)
			{
				return null;
			}

			return new CompleteProjectInfo
			{
				CreatedUTC = completeProject.CreatedUTC,
				CustomerId = completeProject.CustomerId,
				CustomerName = completeProject.CustomerName,
				CustomerOrgId = completeProject.CustomerOrgId,
				EndDate = completeProject.EndDate,
				IsActive = completeProject.IsActive,
				IsCustomerActive = completeProject.IsCustomerActive,
				IsUserActive = completeProject.IsUserActive,
				OrganizationId = completeProject.OrganizationId,
				OrganizationName = completeProject.OrganizationName,
				OrgRoleId = completeProject.OrgRoleId,
				PriceType = completeProject.PriceType,
				ProjectId = completeProject.ProjectId,
				ProjectName = completeProject.ProjectName,
				StartDate = completeProject.StartDate,
				ProjectOrgId = completeProject.ProjectOrgId,
				IsProjectUser = completeProject.IsProjectUser
			};
		}

		#endregion Info-DBEntity Conversions
	}
}
