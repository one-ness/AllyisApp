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
			if (customerId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(customerId), "Customer id must be greater than 0.");
			}

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
		public async Task<int> CreateCustomerAsync(Customer customer, int subscriptionId)
		{
			// Permission validation
			if (!CheckStaffingManagerAction(StaffingManagerAction.EditCustomer, subscriptionId, false) && !CheckTimeTrackerAction(TimeTrackerAction.EditCustomer, subscriptionId, false))
				throw new AccessViolationException($"action {TimeTrackerAction.EditCustomer} denied for subscription {subscriptionId}");

			// TODO: make sure valid countries and states are added during import
			//customer.Address?.EnsureDBRef(this);
			return await DBHelper.CreateCustomerInfoAsync(GetDBEntitiesFromCustomerInfo(customer));
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

			// Deactivating customer validation
			if (!customer.IsActive)
			{
				var projects = await GetProjectsByCustomerAsync(customer.CustomerId);
				if (projects.Any(project => project.IsCurrentlyActive))
				{
					return -2;
				}
			}

			return await DBHelper.UpdateCustomerAsync(GetDBEntitiesFromCustomerInfo(customer));
		}

		/// <summary>
		/// Updates the customer [IsActive] column, after checking for user permissions.
		/// </summary>
		/// <param name="customerId">Customer id.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <param name="isActive">Bool to set the [IsActive] column of the given customer.</param>
		/// <returns>Name of the customer, null if none found.</returns>
		public async Task<string> UpdateCustomerIsActive(int subscriptionId, int customerId, bool isActive)
		{
			// Permission validation for both staffing and time tracker
			if (!CheckStaffingManagerAction(StaffingManagerAction.EditCustomer, subscriptionId, false) && !CheckTimeTrackerAction(TimeTrackerAction.EditCustomer, subscriptionId, false))
				throw new AccessViolationException($"action {TimeTrackerAction.EditCustomer} denied for subscription {subscriptionId}");

			return await DBHelper.UpdateCustomerIsActive(customerId, isActive);
		}

		public async Task<bool> DeleteCustomer(int subscriptionId, int customerId)
		{
			// Permission validation for both staffing and time tracker
			if (!CheckStaffingManagerAction(StaffingManagerAction.EditCustomer, subscriptionId, false) && !CheckTimeTrackerAction(TimeTrackerAction.EditCustomer, subscriptionId, false))
				throw new AccessViolationException($"action {TimeTrackerAction.EditCustomer} denied for subscription {subscriptionId}");

			if (customerId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(customerId), "Customer id must be greater than 0.");
			}

			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(subscriptionId), "Subscription id must be greater than 0.");
			}

			return await DBHelper.DeleteCustomer(customerId);
		}

		/// <summary>
		/// Gets a list of <see cref="Customer"/>'s for an organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns><see cref="IEnumerable{CustomerDBEntity}"/>.</returns>
		public async Task<IEnumerable<Customer>> GetCustomersByOrganizationId(int orgId)
		{
			var customers = await DBHelper.GetCustomersByOrganizationId(orgId);

			return customers
				.Where(dbe => dbe != null)
				.Select(dbe => (Customer)InitializeCustomer(dbe))
				.ToList();
		}

		/// <summary>
		/// Returns a list of Projects for projects the given user is assigned to in the given organization
		/// (current organization by default), another list of Projects for all projects in the organization,
		/// the name of the user (as "Firstname Lastname"), and the user's email.
		/// </summary>
		public async Task<Tuple<List<Project.Project>, List<Project.Project>, string, string>> GetProjectsForOrgAndUser(int userId, int subscriptionId)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(userId), "User id must be greater than 0.");
			}

			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(subscriptionId), "Subscription id must be greater than 0.");
			}

			UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out UserContext.SubscriptionAndRole subInfo);

			if (subInfo == null) return null;

			var spResults = await DBHelper.GetProjectsForOrgAndUser(userId, subInfo.OrganizationId);
			var userDBEntity = spResults.Item3;
			string name = $"{userDBEntity.FirstName} {userDBEntity.LastName}";
			return Tuple.Create(
				spResults.Item1.Select(InitializeProject).ToList(),
				spResults.Item2.Select(InitializeProject).ToList(),
				name,
				userDBEntity.Email);
		}

		/// <summary>
		/// Gets a list of <see cref="Project"/>'s for a customer.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>List of ProjectInfo's.</returns>
		public async Task<IEnumerable<Project.Project>> GetProjectsByCustomerAsync(int customerId)
		{
			if (customerId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(customerId), "Customer Id cannot be 0 or negative.");
			}

			var projects = await DBHelper.GetProjectsByCustomerAsync(customerId);

			return projects.Where(p => p != null).Select(InitializeProject);
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

			if (newProject.OwningCustomer.CustomerId <= 0)
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

			if (newProject.StartDate.HasValue && newProject.EndDate.HasValue && DateTime.Compare(newProject.StartDate.Value, newProject.EndDate.Value) > 0)
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

			if (String.IsNullOrWhiteSpace(newProject.OwningCustomer.CustomerCode))
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

			if (newProject.StartDate.HasValue && newProject.EndDate.HasValue && DateTime.Compare(newProject.StartDate.Value, newProject.EndDate.Value) > 0)
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

			if (project.StartDate.HasValue && project.EndDate.HasValue && DateTime.Compare(project.StartDate.Value, project.EndDate.Value) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			CheckTimeTrackerAction(TimeTrackerAction.EditProject, subId);
			DBHelper.UpdateProject(GetDBEntityFromProject(project));
			await Task.Yield();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="orgId"></param>
		/// <returns></returns>
		public async Task<int> GetDefaultProject(int orgId)
		{
			#region Validation

			if (orgId <= 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Org Id cannot be 0 or negative.");
			}

			#endregion Validation

			return await DBHelper.GetDefaultProject(orgId);
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
		/// Deletes a project
		/// </summary>
		/// <param name="projectId">The project id.</param>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <returns>The number of rows deleted -- includes projectUsers deleted.</returns>
		public async Task<int> DeleteProject(int projectId, int subscriptionId)
		{
			CheckTimeTrackerAction(TimeTrackerAction.EditProject, subscriptionId);

			var timeEntries = await DBHelper.GetTimeEntriesByProjectId(projectId);
			var project = DBHelper.GetProjectById(projectId);

			if (timeEntries.Any())
			{
				return -1;
			}
			if (project.IsDefault)
			{
				return -2;
			}

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
		/// <returns>A list of all the projects a user can access in an organization.</returns>
		public async Task<IEnumerable<CompleteProject>> GetProjectsByUserAndOrganization(int userId, int orgId)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(userId), "User Id cannot be 0 or negative.");
			}

			var results = await DBHelper.GetProjectsByUserAndOrganization(userId, orgId, false);
			return results.Select(InitializeCompleteProjectInfo);
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
				spResults.Item2.Select(udb => InitializeUser(udb)).ToList(),
				spResults.Item3.Select(InitializeSubscriptionUser).ToList());
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="orgId"></param>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public async Task<string> GetNextProjectId(int orgId, int subscriptionId)
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
		/// <returns>All the projects in the organization.</returns>
		public async Task<IEnumerable<Project.Project>> GetAllProjectsForOrganizationAsync(int orgId)
		{
			var result = new List<Project.Project>();
			var customers = await GetCustomersByOrganizationId(orgId);
			foreach (var customer in customers)
			{
				result.AddRange(await GetProjectsByCustomerAsync(customer.CustomerId));
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
		public static Customer InitializeCustomer(dynamic customer)
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

			var newCustomer = new Customer
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
				IsActive = customer.IsActive,
				ActiveProjects = customer.ActiveProjectCount,
				InactiveProjects = customer.ProjectCount - customer.ActiveProjectCount
			};

			return newCustomer;
		}

		public static Tuple<CustomerDBEntity, AddressDBEntity> GetDBEntitiesFromCustomerInfo(Customer customer)
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
					IsActive = customer.IsActive
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
					StateId = customer.Address?.StateId
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
				OwningCustomer = new Customer
				{
					CustomerId = project.CustomerId,
					CustomerName = project.CustomerName,
					CustomerCode = project.CustomerCode
				},
				EndDate = project.EndingDate,
				ProjectName = project.ProjectName,
				OrganizationId = project.OrganizationId,
				ProjectId = project.ProjectId,
				ProjectCode = project.ProjectCode,
				StartDate = project.StartingDate,
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
				CustomerId = project.OwningCustomer.CustomerId,
				EndingDate = project.EndDate,
				ProjectName = project.ProjectName,
				OrganizationId = project.OrganizationId,
				ProjectId = project.ProjectId,
				ProjectCode = project.ProjectCode,
				StartingDate = project.StartDate,
				IsHourly = project.IsHourly,
				IsDefault = project.IsDefault
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
				OwningCustomer = new Customer
				{
					CustomerId = completeProject.CustomerId,
					CustomerName = completeProject.CustomerName,
					CustomerCode = completeProject.CustomerCode,
				},
				EndDate = completeProject.EndDate,
				IsCustomerActive = completeProject.IsCustomerActive,
				IsUserActive = completeProject.IsUserActive,
				IsDefault = completeProject.IsDefault,
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