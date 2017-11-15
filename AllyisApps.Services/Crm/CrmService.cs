//------------------------------------------------------------------------------
// <copyright file="CrmService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.DBModel.Crm;
using AllyisApps.DBModel.Lookup;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Lookup;

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
		/// <returns>.</returns>
		public async Task<string> GetNextCustId(int subscriptionId)
		{
			UserContext.SubscriptionAndRole subInfo = null;
			UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			var spResults = await DBHelper.GetNextCustId(subInfo.OrganizationId);
			return spResults.Item1 == null ? "0000000000000000" : new string(IncrementAlphanumericCharArray(spResults.Item1.ToCharArray()));
		}

		/// <summary>
		/// Gets a Customer for the given customer
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>.</returns>
		public async Task<Customer> GetCustomerInfo(int customerId)
		{
			var spResults = await DBHelper.GetCustomerProfile(customerId);
			Customer customer = InitializeCustomer(spResults.Item1);
			customer.Address = InitializeAddress(spResults.Item2);
			return customer;
		}

		/// <summary>
		/// Creates a customer.
		/// </summary>
		/// <param name="customer">Customer.</param>
		/// <param name="subscriptionId">.</param>
		/// <returns>Customer id.</returns>
		public async Task<int?> CreateCustomerAsync(Customer customer, int subscriptionId)
		{
			if (CheckStaffingManagerAction(StaffingManagerAction.EditCustomer, subscriptionId, false) || CheckTimeTrackerAction(TimeTrackerAction.EditCustomer, subscriptionId, false))
			{
				// TODO: make sure valid countries and states are added during import
				//customer.Address?.EnsureDBRef(this);
				return await DBHelper.CreateCustomerInfoAsync(GetDBEntitiesFromCustomerInfo(customer));
			}
			string message = string.Format("action {0} denied for subscription {1}", TimeTrackerAction.EditCustomer, subscriptionId);
			throw new AccessViolationException(message);
		}

		/// <summary>
		/// Updates a customer in the database.
		/// </summary>
		/// <param name="customer">Updated customer info.</param>
		/// <param name="subscriptionId">The customer's subscription Id.</param>
		/// <returns>Returns 1 if succeed, -1 if fail, and null if authorization fails.</returns>
		public async Task<int?> UpdateCustomerAsync(Customer customer, int subscriptionId)
		{
			CheckTimeTrackerAction(TimeTrackerAction.EditCustomer, subscriptionId);
			return await DBHelper.UpdateCustomerAsync(GetDBEntitiesFromCustomerInfo(customer));
		}

		/// <summary>
		/// Deletes a customer.
		/// </summary>
		/// <param name="customerId">Customer id.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public async Task<string> DeleteCustomer(int subscriptionId, int customerId)
		{
			CheckTimeTrackerAction(TimeTrackerAction.EditCustomer, subscriptionId);
			return await DBHelper.DeleteCustomer(customerId);
		}

		public async Task<bool> FullDeleteCustomer(int subscriptionId, int customerId)
		{
			CheckTimeTrackerAction(TimeTrackerAction.EditCustomer, subscriptionId);
			return await DBHelper.FullDeleteCustomer(customerId);
		}

		/// <summary>
		/// Reactivate a Customer.
		/// </summary>
		/// <param name="customerId">Customer id.</param>
		/// <param name="orgId">The Organization Id.</param>
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public string ReactivateCustomer(int customerId, int subscriptionId, int orgId)
		{
			CheckTimeTrackerAction(TimeTrackerAction.EditCustomer, subscriptionId);
			return DBHelper.ReactivateCustomer(customerId);
		}

		/// <summary>
		/// Gets a list of <see cref="Customer"/>'s for an organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns><see cref="IEnumerable{CustomerDBEntity}"/>.</returns>
		async public Task<IEnumerable<Customer>> GetCustomerList(int orgId)
		{
			IEnumerable<dynamic> dbeList = await DBHelper.GetCustomerList(orgId);
			List<Customer> list = new List<Customer>();
			foreach (dynamic dbe in dbeList)
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
		/// <param name="orgId">The organization Id.</param>
		/// <returns>.</returns>
		public async Task<Tuple<List<CompleteProject>, List<Customer>>> GetProjectsAndCustomersForOrgAndUser(int orgId)
		{
			var spResults = await DBHelper.GetProjectsAndCustomersForOrgAndUser(orgId, UserContext.UserId);
			return Tuple.Create(
				spResults.Item1.Select(cpdb => InitializeCompleteProjectInfo(cpdb)).ToList(),
				spResults.Item2.Select(cdb => (Customer)InitializeCustomer(cdb)).ToList());
		}

		/// <summary>
		/// Returns a list of CompleteProjectInfos for the current organization with the IsProjectUser field filled
		/// out for the current user, and a list of CustomerInfos for the organization.
		/// </summary>
		/// <param name="orgId">The Organization Id.</param>
		/// <returns>.</returns>
		public Tuple<List<CompleteProject>, List<Customer>> GetInactiveProjectsAndCustomersForOrgAndUser(int orgId)
		{
			var spResults = DBHelper.GetInactiveProjectsAndCustomersForOrgAndUser(orgId, UserContext.UserId);
			return Tuple.Create(
				spResults.Item1.Select(cpdb => InitializeCompleteProjectInfo(cpdb)).ToList(),
				spResults.Item2.Select(cdb => (Customer)InitializeCustomer(cdb)).ToList());
		}

		/// <summary>
		/// Returns a list of Projects for projects the given user is assigned to in the given organization
		/// (current organization by default), another list of Projects for all projects in the organization,
		/// the name of the user (as "Firstname Lastname"), and the user's email.
		/// </summary>
		public async Task<Tuple<List<Project.Project>, List<Project.Project>, string, string>> GetProjectsForOrgAndUser(int userId, int subscriptionId)
		{
			if (userId <= 0) throw new ArgumentException("userId");
			if (subscriptionId <= 0) throw new ArgumentException("subscriptionId");

			UserContext.SubscriptionAndRole subInfo = null;
			UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			if (subInfo != null)
			{
				var spResults = await DBHelper.GetProjectsForOrgAndUser(userId, subInfo.OrganizationId);
				var userDBEntity = spResults.Item3;
				string name = string.Format("{0} {1}", userDBEntity.FirstName, userDBEntity.LastName);
				return Tuple.Create(
					spResults.Item1.Select(pdb => InitializeProject(pdb)).ToList(),
					spResults.Item2.Select(pdb => InitializeProject(pdb)).ToList(),
					name,
					userDBEntity.Email);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Gets a list of <see cref="Project"/>'s for a customer.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <param name="onlyActive">Bool whether or not to only get active projects.</param>
		/// <returns>List of ProjectInfo's.</returns>
		public async Task<IEnumerable<Project.Project>> GetProjectsByCustomerAsync(int customerId, bool onlyActive = false)
		{
			if (customerId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(customerId), "Customer Id cannot be 0 or negative.");
			}

			var projects = await DBHelper.GetProjectsByCustomerAsync(customerId);

			if (onlyActive)
			{
				projects = projects.Where(p => p.IsActive);
			}

			var thing = projects.Where(p => p != null).Select(InitializeProject).ToList();
			return thing;
		}

		/// <summary>
		/// Gets a list of <see cref="Project"/>'s for a customer.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>List of ProjectInfo's.</returns>
		public async Task<IEnumerable<Project.Project>> GetInactiveProjectsByCustomer(int customerId)
		{
			if (customerId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(customerId), "Customer Id cannot be 0 or negative.");
			}

			var projects = await DBHelper.GetInactiveProjectsByCustomer(customerId);
			return projects.Where(p => p != null).Select(InitializeProject).ToList();
		}

		/// <summary>
		/// Creates a new project and update its user list if succeed.
		/// </summary>
		/// <param name="newProject">Project with project information.</param>
		/// <param name="userIds">List of users being assigned to the project.</param>
		/// <param name="subscriptionId">The subscription that the user is operating under.</param>
		/// <returns>Project Id if succeed, -1 if projectCode is taken.</returns>
		public async Task<int> CreateProjectAndUpdateItsUserList(Project.Project newProject, IEnumerable<int> userIds, int subscriptionId)
		{
			#region Validation

			CheckTimeTrackerAction(TimeTrackerAction.EditProject, subscriptionId);

			if (newProject.owningCustomer.CustomerId <= 0)
			{
				throw new ArgumentOutOfRangeException("customerId", "Customer Id cannot be 0 or negative.");
			}

			if (string.IsNullOrWhiteSpace(newProject.ProjectName))
			{
				throw new ArgumentNullException("name", "Project name must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrEmpty(newProject.ProjectCode))
			{
				throw new ArgumentNullException("projectCode", "Project must have an Id");
			}

			if (newProject.StartingDate.HasValue && newProject.EndingDate.HasValue && DateTime.Compare(newProject.StartingDate.Value, newProject.EndingDate.Value) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			return await DBHelper.CreateProjectAndUpdateItsUserList(GetDBEntityFromProject(newProject), userIds);
		}

		/// <summary>
		/// Creates a new project.
		/// </summary>
		/// <param name="newProject">Project with project information.</param>
		/// <returns>Project Id.</returns>
		public async Task<int> CreateProject(Project.Project newProject)
		{
			#region Validation

			if (String.IsNullOrWhiteSpace(newProject.owningCustomer.CustomerCode))
			{
				throw new ArgumentNullException("customerCode", "Customer Code must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrWhiteSpace(newProject.ProjectName))
			{
				throw new ArgumentNullException("name", "Project name must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrEmpty(newProject.ProjectCode))
			{
				throw new ArgumentNullException("ProjectCode", "Project must have an Id");
			}

			if (newProject.StartingDate.HasValue && newProject.EndingDate.HasValue && DateTime.Compare(newProject.StartingDate.Value, newProject.EndingDate.Value) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			return await DBHelper.CreateProject(GetDBEntityFromProject(newProject));
		}

		/// <summary>
		/// Updates a project's properties.
		/// </summary>
		/// <param name="subId">SubscriptionId.</param>
		/// <param name="project">Project with updated properties.</param>
		public async void UpdateProject(int subId, Project.Project project)
		{
			#region Validation

			if (project.ProjectId <= 0)
			{
				throw new ArgumentOutOfRangeException("ProjectId", "Project Id cannot be 0 or negative.");
			}

			if (string.IsNullOrWhiteSpace(project.ProjectName))
			{
				throw new ArgumentNullException("Name", "Project name must have a value and cannot be whitespace.");
			}

			if (project.StartingDate.HasValue && project.EndingDate.HasValue && DateTime.Compare(project.StartingDate.Value, project.EndingDate.Value) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			CheckTimeTrackerAction(TimeTrackerAction.EditProject, subId);
			DBHelper.UpdateProject(GetDBEntityFromProject(project));
			await Task.Yield();
		}

		/// <summary>
		/// Updates a project's properties and user list.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="name">Project name.</param>
		/// <param name="orgId">Project org id.</param>
		/// <param name="isHourly">Project type.  True == hourly, false == fixed. TODO: use this parameter to update the project's isHourly column.  Currently disabled attribute.</param>
		/// <param name="start">Starting date. <see cref="DateTime"/>.</param>
		/// <param name="end">Ending date. <see cref="DateTime"/>.</param>
		/// <param name="userIds">Updated on-project user list.</param>
		/// <param name="subscriptionId">.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public async Task<bool> UpdateProjectAndUsers(int projectId, string name, string orgId, DateTime? start, DateTime? end, IEnumerable<int> userIds, int subscriptionId, bool isHourly = true)
		{
			#region Validation

			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(projectId), "Project Id cannot be 0 or negative.");
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException(nameof(name), "Project name must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrWhiteSpace(orgId))
			{
				throw new ArgumentNullException(nameof(orgId), "Project Org Id must have a value and cannot be whitespace.");
			}

			if (start.HasValue && end.HasValue && DateTime.Compare(start.Value, end.Value) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			if (userIds == null)
			{
				userIds = new List<int>();
			}

			#endregion Validation

			CheckTimeTrackerAction(TimeTrackerAction.EditProject, subscriptionId);
			DBHelper.UpdateProjectAndUsers(projectId, name, orgId, isHourly, start, end, userIds);
			await Task.Yield();
			return true;
		}

		/// <summary>
		/// Reactivate a project.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="organizationId">Organization Id.</param>
		/// <param name="subscriptionId"> subscription Id.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool ReactivateProject(int projectId, int organizationId, int subscriptionId)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(projectId), "Project Id cannot be 0 or negative.");
			}

			CheckTimeTrackerAction(TimeTrackerAction.EditProject, subscriptionId);
			DBHelper.ReactivateProject(projectId);
			return true;
		}

		/// <summary>
		/// Deletes a project
		/// </summary>
		/// <param name="projectId">The project id.</param>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <returns>The number of rows deleted -- includes projectUsers deleted.</returns>
		public async Task<int> FullDeleteProject(int projectId, int subscriptionId)
		{
			CheckTimeTrackerAction(TimeTrackerAction.EditProject, subscriptionId);

			var timeEntries = await DBHelper.GetTimeEntriesByProjectId(projectId);

			if (timeEntries.Any())
			{
				return -1;
			}

			return await DBHelper.FullDeleteProject(projectId);
		}

		/// <summary>
		/// Deletes a project.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="subscriptionId">.</param>
		/// <returns>Returns null if authorization fails, project name is succeed, empty string if not found.</returns>
		public async Task<string> DeleteProject(int projectId, int subscriptionId)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(projectId), "Project Id cannot be 0 or negative.");
			}

			CheckTimeTrackerAction(TimeTrackerAction.EditProject, subscriptionId);

			await CheckUpdateProjectStartEndDate(projectId, null, DateTime.Now);

			return await DBHelper.DeleteProject(projectId);
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
				throw new ArgumentOutOfRangeException(nameof(projectId), "Project Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(userId), "User Id cannot be 0 or negative.");
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
		public async Task<int> UpdateProjectUser(int projectId, int userId, bool isActive)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(projectId), "Project Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(userId), "User Id cannot be 0 or negative.");
			}

			return await DBHelper.UpdateProjectUser(projectId, userId, isActive ? 1 : 0);
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
				throw new ArgumentOutOfRangeException(nameof(projectId), "Project Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(userId), "User Id cannot be 0 or negative.");
			}

			return DBHelper.DeleteProjectUser(projectId, userId) == 1;
		}

		/// <summary>
		/// Gets all the projects a user can use in the chosen organization.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="orgId">The organization's Id.</param>
		/// <param name="onlyActive">True (default) to only return active projects, false to include all projects, active or not.</param>
		/// <returns>A list of all the projects a user can access in an organization.</returns>
		public async Task<IEnumerable<CompleteProject>> GetProjectsByUserAndOrganization(int userId, int orgId = -1, bool onlyActive = true)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(userId), "User Id cannot be 0 or negative.");
			}

			var results = await DBHelper.GetProjectsByUserAndOrganization(userId, orgId, onlyActive ? 1 : 0);
			return results.Select(c => InitializeCompleteProjectInfo(c));
		}

		/// <summary>
		/// Gets a <see cref="CompleteProject"/>.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns>CompleteProject instance.</returns>
		public CompleteProject GetProject(int projectId)
		{
			if (projectId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(projectId), "Project Id cannot be negative.");
			}

			return InitializeCompleteProjectInfo(DBHelper.GetProjectById(projectId));
		}

		/// <summary>
		/// Gets a <see cref="CompleteProject"/>, with the IsProjectUser field filled out for the
		/// current user.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns>CompleteProject instance.</returns>
		public async Task<CompleteProject> GetProjectAsUser(int projectId)
		{
			if (projectId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(projectId), "Project Id cannot be negative.");
			}

			return InitializeCompleteProjectInfo(await DBHelper.GetProjectByIdAndUser(projectId, UserContext.UserId));
		}

		/// <summary>
		/// Gets a CompleteProject for the given project, a list of UserInfos for the project's assigned
		/// users, and a list of SubscriptionUserInfos for all users in the current subscription.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="subscriptionId">.</param>
		/// <returns>.</returns>
		public Tuple<CompleteProject, List<User>, List<SubscriptionUser>> GetProjectEditInfo(int projectId, int subscriptionId)
		{
			if (projectId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(projectId), "Project Id cannot be negative.");
			}

			var spResults = DBHelper.GetProjectEditInfo(projectId, subscriptionId);
			return Tuple.Create(
				InitializeCompleteProjectInfo(spResults.Item1),
				spResults.Item2.Select(udb => InitializeUser(udb, false)).ToList(),
				spResults.Item3.Select(sudb => InitializeSubscriptionUser(sudb)).ToList());
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="orgId"></param>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		async public Task<string> GetNextProjectId(int orgId, int subscriptionId)
		{
			var results = "";
			try
			{
				results = await DBHelper.GetNextProjectId(orgId, subscriptionId);
			}
			catch (Exception e)
			{
				results = e.ToString();
				results = "";
			}
			return string.IsNullOrEmpty(results) ? "0000000000000000" : new string(IncrementAlphanumericCharArray(results.ToCharArray()));
		}

		/// <summary>
		/// Gets the next logical project id for the given customer and a list of SubscriptionUserInfos for
		/// all useres in the current subscription.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <param name="subscriptionId">.</param>
		/// <returns>.</returns>
		public async Task<Tuple<string, List<SubscriptionUser>>> GetNextProjectIdAndSubUsers(int customerId, int subscriptionId)
		{
			if (customerId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(customerId), "Customer Id cannot be negative.");
			}

			var spResults = await DBHelper.GetNextProjectIdAndSubUsers(customerId, subscriptionId);
			return Tuple.Create(
				spResults.Item1 == null ? "0000000000000000" : new string(IncrementAlphanumericCharArray(spResults.Item1.ToCharArray())),
				spResults.Item2.Select(sudb => InitializeSubscriptionUser(sudb)).ToList());
		}

		/// <summary>
		/// Gets all the projects in every customer in the entire organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="onlyActive">Bool whether or not to only get active projects.</param>
		/// <returns>All the projects in the organization.</returns>
		public async Task<IEnumerable<Project.Project>> GetAllProjectsForOrganizationAsync(int orgId, bool onlyActive)
		{
			var result = new List<Project.Project>();
			var customers = await GetCustomerList(orgId);
			foreach (var customer in customers)
			{
				result.AddRange(await GetProjectsByCustomerAsync(customer.CustomerId, onlyActive));
			}
			return result;
		}

		#region Info-DBEntity Conversions

		/// <summary>
		/// Initializes a <see cref="Address"/> from a <see cref="AddressDBEntity"/>.
		/// </summary>
		/// <param name="address">.</param>
		/// <returns>.</returns>
		public static Address InitializeAddress(AddressDBEntity address)
		{
			if (address == null)
			{
				return null;
			}

			return new Address
			{
				AddressId = address.AddressId,
				Address1 = address.Address1,
				Address2 = address.Address2,
				City = address.City,
				StateName = address.State,
				PostalCode = address.PostalCode,
				CountryCode = address.CountryCode,
				StateId = address.StateId,
				CountryName = address.Country
			};
		}

		/// <summary>
		/// Initialize address from dynamic infomation
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public static Address InitializeAddress(dynamic address)
		{
			if (address == null)
			{
				return null;
			}

			return new Address
			{
				AddressId = address.AddressId,
				Address1 = address.Address1 ?? address.Address,
				Address2 = address.Address2,
				City = address.City,
				StateName = address.State ?? address.StateName,
				PostalCode = address.PostalCode,
				CountryCode = address.CountryCode,
				StateId = address.StateId,
				CountryName = address.Country ?? address.CountryName
			};
		}

		public static AddressDBEntity GetDBEntityFromAddress(Address address)
		{
			return new AddressDBEntity
			{
				AddressId = address?.AddressId,
				Address1 = address?.Address1,
				Address2 = address?.Address2,
				City = address?.City,
				Country = address?.CountryName,
				CountryCode = address?.CountryCode,
				CountryId = null,
				PostalCode = address?.PostalCode,
				State = address?.StateName,
				StateId = address?.StateId
			};
		}

		public Customer IntializeCustomer(CustomerDBEntity customer, bool loadAddress = true)
		{
			if (customer == null)
			{
				return null;
			}
			return new Customer
			{
				Address = loadAddress ? getAddress(customer.AddressId) : null,
				ContactEmail = customer.ContactEmail,
				ContactPhoneNumber = customer.ContactPhoneNumber,
				CreatedUtc = customer.CreatedUtc,
				CustomerId = customer.CustomerId,
				CustomerCode = customer.CustomerCode,
				EIN = customer.EIN,
				FaxNumber = customer.FaxNumber,
				CustomerName = customer.CustomerName,
				OrganizationId = customer.OrganizationId,
				Website = customer.Website,
				IsActive = customer.IsActive
			};
		}

		/// <summary>
		/// Initializes a <see cref="Customer"/> from a query"/>.
		/// </summary>
		/// <returns>A Customer object.</returns>
		public Customer InitializeCustomer(dynamic customer)
		{
			if (customer == null)
			{
				return null;
			}

			Address address = null;
			if (customer.AddressId != null)
			{
				address = new Address
				{
					Address1 = customer.Address,
					Address2 = null,
					AddressId = customer.AddressId,
					StateId = customer.StateId,
					City = customer.City,
					StateName = customer.StateName,
					CountryCode = customer.CountryCode,
					PostalCode = customer.PostalCode,
					CountryName = customer.CountryName
				};
			}
			return new Customer
			{
				Address = address,
				ContactEmail = customer.ContactEmail,
				ContactPhoneNumber = customer.ContactPhoneNumber,
				CreatedUtc = customer.CreatedUtc,
				CustomerId = customer.CustomerId,
				CustomerCode = customer.CustomerCode,
				EIN = customer.EIN,
				FaxNumber = customer.FaxNumber,
				CustomerName = customer.CustomerName,
				OrganizationId = customer.OrganizationId,
				Website = customer.Website,
				IsActive = customer.IsActive
			};
		}

		///// <summary>
		///// Initializes a <see cref="CustomerDBEntity"/> from a <see cref="Customer"/>.
		///// </summary>
		///// <param name="customer">The Customer to use.</param>
		///// <returns>A CustomerDBEntity object.</returns>
		//public CustomerDBEntity GetDBEntityFromCustomer(Customer customer)
		//{
		//    if (customer == null)
		//    {
		//        return null;
		//    }

		//    //Ensure that customer buisneess object does not attempt to set CountryName without ID
		//    //Should not happen but can from import service.
		//    customer.Address?.EnsureDBRef(this);
		//    return new CustomerDBEntity()
		//    {
		//        ContactEmail = customer.ContactEmail,
		//        ContactPhoneNumber = customer.ContactPhoneNumber,
		//        CreatedUtc = customer.CreatedUtc,
		//        CustomerId = customer.CustomerId,
		//        CustomerCode = customer.CustomerCode,
		//        EIN = customer.EIN,
		//        FaxNumber = customer.FaxNumber,
		//        CustomerName = customer.CustomerName,
		//        OrganizationId = customer.OrganizationId,
		//        Website = customer.Website,
		//        IsActive = customer.IsActive,
		//        AddressId = customer.Address?.AddressId,
		//        Address = customer.Address?.Address1,
		//        City = customer.Address?.City,
		//        CountryCode = customer.Address?.CountryCode,
		//        CountryName = customer.Address?.CountryName,
		//        StateId = customer.Address?.StateId,
		//        PostalCode = customer.Address?.PostalCode,
		//        StateName = customer.Address?.StateName
		//    };
		//}

		public Tuple<CustomerDBEntity, AddressDBEntity> GetDBEntitiesFromCustomerInfo(Customer customer)
		{
			return new Tuple<CustomerDBEntity, AddressDBEntity>(
				new CustomerDBEntity
				{
					AddressId = customer.Address?.AddressId,
					ContactEmail = customer.ContactEmail,
					ContactPhoneNumber = customer.ContactPhoneNumber,
					CreatedUtc = customer.CreatedUtc,
					CustomerId = customer.CustomerId,
					CustomerCode = customer.CustomerCode,
					EIN = customer.EIN,
					FaxNumber = customer.FaxNumber,
					CustomerName = customer.CustomerName,
					OrganizationId = customer.OrganizationId,
					Website = customer.Website,
					IsActive = customer.IsActive ?? true
				},
				new AddressDBEntity
				{
					AddressId = customer.Address?.AddressId,
					Address1 = customer.Address?.Address1,
					City = customer.Address?.City,
					Country = customer.Address?.CountryName,
					CountryCode = customer.Address?.CountryCode,
					PostalCode = customer.Address?.PostalCode,
					State = customer.Address?.StateName,
					StateId = customer.Address?.StateId,
				});
		}

		/// <summary>
		/// Translates a <see cref="ProjectDBEntity"/> into a <see cref="Project"/>.
		/// </summary>
		/// <param name="project">ProjectDBEntity instance.</param>
		/// <returns>Project instance.</returns>
		public static Project.Project InitializeProject(ProjectDBEntity project)
		{
			if (project == null)
			{
				return null;
			}

			return new Project.Project
			{
				owningCustomer = new Customer
				{
					CustomerId = project.CustomerId,
					CustomerName = project.CustomerName,
					CustomerCode = project.CustomerCode
				},
				EndingDate = project.EndingDate,
				ProjectName = project.ProjectName,
				OrganizationId = project.OrganizationId,
				ProjectId = project.ProjectId,
				ProjectCode = project.ProjectCode,
				StartingDate = project.StartingDate,
				IsHourly = project.IsHourly
			};
		}

		/// <summary>
		/// Translates a <see cref="Project"/> into a <see cref="ProjectDBEntity"/>.
		/// </summary>
		/// <param name="project">Project instance.</param>
		/// <returns>ProjectDBEntity instance.</returns>
		public static ProjectDBEntity GetDBEntityFromProject(Project.Project project)
		{
			if (project == null)
			{
				return null;
			}

			return new ProjectDBEntity
			{
				CustomerId = project.owningCustomer.CustomerId,
				EndingDate = project.EndingDate,
				ProjectName = project.ProjectName,
				OrganizationId = project.OrganizationId,
				ProjectId = project.ProjectId,
				ProjectCode = project.ProjectCode,
				StartingDate = project.StartingDate,
				IsHourly = project.IsHourly,
				IsActive = project.IsActive
			};
		}

		/// <summary>
		/// Translates a <see cref="ProjectDBEntity"/> into a <see cref="CompleteProject"/>.
		/// </summary>
		/// <param name="completeProject">CompleteProjectDBEntity instance.</param>
		/// <returns>CompleteProject instance.</returns>
		public static CompleteProject InitializeCompleteProjectInfo(ProjectDBEntity completeProject)
		{
			if (completeProject == null)
			{
				return null;
			}

			return new CompleteProject
			{
				CreatedUtc = completeProject.CreatedUtc,
				owningCustomer = new Customer
				{
					CustomerId = completeProject.CustomerId,
					CustomerName = completeProject.CustomerName,
					CustomerCode = completeProject.CustomerCode,
				},
				EndDate = completeProject.EndDate,
				IsActive = completeProject.IsActive,
				IsCustomerActive = completeProject.IsCustomerActive,
				IsUserActive = completeProject.IsUserActive,
				OrganizationId = completeProject.OrganizationId,
				OrganizationName = completeProject.OrganizationName,
				PriceType = completeProject.PriceType,
				ProjectId = completeProject.ProjectId,
				ProjectName = completeProject.ProjectName,
				StartDate = completeProject.StartDate,
				ProjectCode = completeProject.ProjectCode,
				IsProjectUser = completeProject.IsProjectUser
			};
		}

		#endregion Info-DBEntity Conversions
	}
}