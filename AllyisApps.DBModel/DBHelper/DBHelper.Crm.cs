//------------------------------------------------------------------------------
// <copyright file="DBHelper.Crm.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Crm;
using AllyisApps.DBModel.Lookup;
using Dapper;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// DBHelper Partial.
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// Method to Create a new project then update its user list.
		/// </summary>
		/// <param name="project">ProjectDBEntity with new project info.</param>
		/// <param name="userIds">List of users to be assigned to this project.</param>
		/// <returns>Returns the id of the created project, else returns -1.</returns>
		public async Task<int> CreateProjectAndUpdateItsUserList(ProjectDBEntity project, IEnumerable<int> userIds)
		{
			if (string.IsNullOrWhiteSpace(project.ProjectName))
			{
				throw new ArgumentException("Name cannot be null, empty, or whitespace.");
			}

			DataTable userIdsTable = new DataTable();
			userIdsTable.Columns.Add("userId", typeof(int));
			foreach (int userId in userIds)
			{
				userIdsTable.Rows.Add(userId);
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerId", project.CustomerId);
			parameters.Add("@projectName", project.ProjectName);
			parameters.Add("@isHourly", project.IsHourly);
			parameters.Add("@projectOrgId", project.ProjectOrgId);
			parameters.Add("@startingDate", project.StartingDate?.ToShortDateString());
			parameters.Add("@endingDate", project.EndingDate?.ToShortDateString());
			parameters.Add("@userIds", userIdsTable.AsTableValuedParameter("[Auth].[UserTable]"));
			parameters.Add("@retId", -1, DbType.Int32, ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync(
					"[Pjm].[CreateProjectAndUpdateItsUserList]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@retId");
		}

		/// <summary>
		/// Method to Create a new project.
		/// </summary>
		/// <param name="project">ProjectDBEntity with new project info.</param>
		/// <returns>Returns the id of the created project, else returns -1.</returns>
		public async Task<int> CreateProject(ProjectDBEntity project)
		{
			if (string.IsNullOrWhiteSpace(project.ProjectName))
			{
				throw new ArgumentException("Name cannot be null, empty, or whitespace.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerId", project.CustomerId);
			parameters.Add("@projectName", project.ProjectName);
			parameters.Add("@isHourly", project.IsHourly);
			parameters.Add("@projectOrgId", project.ProjectOrgId);
			parameters.Add("@startingDate", project.StartingDate?.ToShortDateString());
			parameters.Add("@endingDate", project.EndingDate?.ToShortDateString());
			parameters.Add("@retId", -1, DbType.Int32, ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync(
					"[Pjm].[CreateProject]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@retId");
		}

		/// <summary>
		/// Deletes a project.
		/// </summary>
		/// <param name="projectId">The id of the project to be deleted.</param>
		/// <returns>Returns project name if successful, return empty string if not found.</returns>
		public async Task<string> DeleteProject(int projectId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var resultGet = await connection.QueryAsync<string>("[Pjm].[DeleteProject]", new { ProjectId = projectId, DeactivateDate = DateTime.Now }, commandType: CommandType.StoredProcedure);
				var result = resultGet.SingleOrDefault();
				if (result == null) { return ""; }
				return result;
			}
		}

		/*
        /// <summary>
        /// Deletes a project.
        /// </summary>
        /// <param name="projectId">The id of the project to be deleted.</param>
        public void DeleteProject(int projectId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectId", projectId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute(
					"[Crm].[DeleteProject]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}*/

		/// <summary>
		/// Reactivate a project.
		/// </summary>
		/// <param name="projectId">The id of the project to be reactivated.</param>
		public void ReactivateProject(int projectId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectId", projectId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute(
					"[Pjm].[ReactivateProject]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates project properties.
		/// </summary>
		/// <param name="project">The ProjectDBEntity with the updated properties.</param>
		public async void UpdateProject(ProjectDBEntity project)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectId", project.ProjectId);
			parameters.Add("@projectName", project.ProjectName);
			parameters.Add("@isHourly", project.IsHourly);
			parameters.Add("@projectOrgId", project.ProjectOrgId);
			parameters.Add("@startingDate", project.StartingDate?.ToShortDateString());
			parameters.Add("@endingDate", project.EndingDate?.ToShortDateString());

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync(
					"[Pjm].[UpdateProject]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets all the projects associated with a specific customer.
		/// </summary>
		/// <param name="customerId">The id of the customer.</param>
		/// <returns>A collection of projects with the definied customer.</returns>
		public async Task<IEnumerable<ProjectDBEntity>> GetProjectsByCustomerAsync(int customerId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerId", customerId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.QueryAsync<ProjectDBEntity>(
					"[Pjm].[GetProjectsByCustomer]",
					parameters,
				   commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets all the projects associated with a specific customer.
		/// </summary>
		/// <param name="customerId">The id of the customer.</param>
		/// <returns>A collection of projects with the definied customer.</returns>
		public async Task<IEnumerable<ProjectDBEntity>> GetInactiveProjectsByCustomer(int customerId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerId", customerId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.QueryAsync<ProjectDBEntity>(
					"[Pjm].[GetInactiveProjectsByCustomer]",
					parameters,
				   commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Returns a CompleteProjectDBEntity for the project, a list of UserDBEntities for the project's assigned
		/// users, and a list of SubscriptionUserDBEntities for all users of the given subscription.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns>.</returns>
		public Tuple<ProjectDBEntity, List<UserDBEntity>, List<SubscriptionUserDBEntity>> GetProjectEditInfo(int projectId, int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectId", projectId);
			parameters.Add("@subscriptionId", subscriptionId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[Pjm].[GetProjectEditInfo]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<ProjectDBEntity>().SingleOrDefault(),
					results.Read<UserDBEntity>().ToList(),
					results.Read<SubscriptionUserDBEntity>().ToList());
			}
		}

		/// <summary>
		/// Returns the alphanumericaly topmost project id for the given customer and a list of SubscriptionUserDBEntities
		/// for all users of the given subscription.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns>.</returns>
		public async Task<Tuple<string, List<SubscriptionUserDBEntity>>> GetNextProjectIdAndSubUsers(int customerId, int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerId", customerId);
			parameters.Add("@subscriptionId", subscriptionId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryMultipleAsync(
					"[Pjm].[GetNextProjectIdAndSubUsers]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<string>().SingleOrDefault(),
					results.Read<SubscriptionUserDBEntity>().ToList());
			}
		}

		/// <summary>
		/// Updates a ProjectUser entry in the database.
		/// </summary>
		/// <param name="projectId">The project's Id.</param>
		/// <param name="userId">The User's Id.</param>
		public void CreateProjectUser(int projectId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectId", projectId);
			parameters.Add("@userId", userId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Query<int>(
					"[Pjm].[CreateProjectUser]",
					parameters,
					commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Updates a ProjectUser entry in the database, isActive must be specified as 0 or 1.
		/// </summary>
		/// <param name="projectId">The project Id.</param>
		/// <param name="userId">The User Id.</param>
		/// <param name="isActive">Is active.</param>
		/// <returns>The number of rows successfully updated.</returns>C:\Users\v-trsan\Desktop\AllyisApps\aa\src\main\aadb\StoredProcedures\TimeTracker
		public async Task<int> UpdateProjectUser(int projectId, int userId, int isActive)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectId", projectId);
			parameters.Add("@userId", userId);
			parameters.Add("@isActive", isActive);
			parameters.Add("@rowsUpdated", 0);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				int rows = 1;
				var result = await connection.QueryAsync<int>(
					"[Pjm].[UpdateProjectUser]",
					parameters,
					commandType: CommandType.StoredProcedure);
				rows = result.SingleOrDefault();

				return rows;
			}
		}

		/// <summary>
		/// Deletes a ProjectUser entry in the database.
		/// </summary>
		/// <param name="projectId">The project's Id.</param>
		/// <param name="userId">The User's Id.</param>
		/// <returns>A value of 1 if successful, else 0.</returns>
		public int DeleteProjectUser(int projectId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectId", projectId);
			parameters.Add("@userId", userId);
			parameters.Add("@ret", 0);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<int>(
					"[Pjm].[DeleteProjectUser]",
					parameters,
					commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Updates the customer with the information specified in the customer table.
		/// </summary>
		/// <param name="customerInfo">The table with the customer to create.</param>
		/// <returns>The Id of the customer if one was created -1 if not.</returns>
		public async Task<int> CreateCustomerInfoAsync(Tuple<CustomerDBEntity, AddressDBEntity> customerInfo)
		{
			if (customerInfo == null)
			{
				throw new ArgumentException("customer cannot be null.");
			}
			CustomerDBEntity customer = customerInfo.Item1;
			AddressDBEntity address = customerInfo.Item2;

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@contactEmail", customer.ContactEmail);
			parameters.Add("@customerName", customer.CustomerName);
			parameters.Add("@address", address?.Address1);
			parameters.Add("@city", address?.City);
			parameters.Add("@stateId", address?.StateId);
			parameters.Add("@countryCode", address?.CountryCode);
			parameters.Add("@postalCode", address?.PostalCode);
			parameters.Add("@contactPhoneNumber", customer.ContactPhoneNumber);
			parameters.Add("@faxNumber", customer.FaxNumber);
			parameters.Add("@website", customer.Website);
			parameters.Add("@eIN", customer.EIN);
			parameters.Add("@organizationId", customer.OrganizationId);
			parameters.Add("@customerOrgId", customer.CustomerOrgId);
			parameters.Add("@retId", -1, DbType.Int32, ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default null
				await connection.ExecuteAsync("[Crm].[CreateCustomerInfo]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@retId");
		}

		/// <summary>
		/// Updates the customer with the specified Id.
		/// </summary>
		/// <param name="customerInfo">The table with the customer to create.</param>
		/// <return>1 if succeed, -1 if fail because CustOrgId is not unique.</return>
		public async Task<int> UpdateCustomerAsync(Tuple<CustomerDBEntity, AddressDBEntity> customerInfo)
		{
			if (customerInfo == null)
			{
				throw new ArgumentException("customer cannot be null.");
			}
			CustomerDBEntity customer = customerInfo.Item1;
			AddressDBEntity address = customerInfo.Item2;

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerId", customer.CustomerId);
			parameters.Add("@contactEmail", customer.ContactEmail);
			parameters.Add("@customerName", customer.CustomerName);
			parameters.Add("@addressId", address?.AddressId);
			parameters.Add("@address", address?.Address1);
			parameters.Add("@city", address?.City);
			parameters.Add("@stateId", address?.StateId);
			parameters.Add("@countryCode", address?.CountryCode);
			parameters.Add("@postalCode", address?.PostalCode);
			parameters.Add("@contactPhoneNumber", customer.ContactPhoneNumber);
			parameters.Add("@faxNumber", customer.FaxNumber);
			parameters.Add("@website", customer.Website);
			parameters.Add("@eIN", customer.EIN);
			parameters.Add("@orgId", customer.CustomerOrgId);
			parameters.Add("@retId", -1, DbType.Int32, ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync("[Crm].[UpdateCustomerInfo]", parameters, commandType: CommandType.StoredProcedure);
			}
			return parameters.Get<int>("@retId");
		}

		/*
        /// <summary>
        /// Updates the customer with the specified Id.
        /// </summary>
        /// <param name="customer">The table with the customer to create.</param>
        public void UpdateCustomer(CustomerDBEntity customer)
		{
			if (customer == null)
			{
				throw new ArgumentException("customer cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerId", customer.CustomerId);
			parameters.Add("@contactEmail", customer.ContactEmail);
			parameters.Add("@name", customer.Name);
			parameters.Add("@address", customer.Address);
			parameters.Add("@city", customer.City);
			parameters.Add("@state", customer.State);
			parameters.Add("@country", customer.Country);
			parameters.Add("@postalCode", customer.PostalCode);
			parameters.Add("@contactPhoneNumber", customer.ContactPhoneNumber);
			parameters.Add("@faxNumber", customer.FaxNumber);
			parameters.Add("@website", customer.Website);
			parameters.Add("@eIN", customer.EIN);
			parameters.Add("@orgId", customer.CustomerOrgId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Crm].[UpdateCustomerInfo]", parameters, commandType: CommandType.StoredProcedure);
			}
		} */

		/// <summary>
		/// Retrieves the customer's information from the database.
		/// </summary>
		/// <param name="orgId">The organization's Id.</param>
		/// <returns>The CustomerDBEntity containing the customer's information, null if call fails.</returns>
		public IEnumerable<dynamic> GetCustomerList(int orgId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default null
				return connection.Query<dynamic>(
					"[Crm].[GetCustomersByOrgId]",
					new { OrgId = orgId },
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves the customer's information from the database.
		/// </summary>
		/// <param name="customerId">The customer's Id.</param>
		/// <returns>The CustomerDBEntity containing the customer's information, null if call fails.</returns>
		public dynamic GetCustomerInfo(int customerId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default null
				return connection.Query<dynamic>("[Crm].[GetCustomerInfo]", new { CustomerId = customerId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Gets the alphanumerically topmost customer id for the given organization and a list of valid
		/// country names.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>.</returns>
		public async Task<Tuple<string>> GetNextCustId(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryMultipleAsync(
					"[Crm].[GetNextCustId]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<string>().SingleOrDefault()
					);
			}
		}

		/// <summary>
		/// Gets a CustomerDBEntity for the given customer and a list of valid
		/// country names.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>.</returns>
		public async Task<Tuple<dynamic, AddressDBEntity>> GetCustomerProfile(int customerId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerId", customerId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryMultipleAsync(
					"[Crm].[GetCustomerProfile]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<dynamic>().SingleOrDefault(),
					results.Read<AddressDBEntity>().SingleOrDefault());
			}
		}

		/// <summary>
		/// Delete the specified customer.
		/// </summary>
		/// <param name="customerId">The customer's Id.</param>
		/// <returns>Customer's name if successful, empty string if not found.</returns>
		public async Task<string> DeleteCustomer(int customerId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var resultGet = await connection.QueryAsync<string>("[Crm].[DeleteCustomer]", new { CustomerId = customerId }, commandType: CommandType.StoredProcedure);
				var result = resultGet.SingleOrDefault();
				if (result == null) { return ""; }
				return result;
			}
		}

		/*
        /// <summary>
		/// Delete the specified customer.
		/// </summary>
		/// <param name="customerId">The customer's Id.</param>
		/// <returns>True if successful.</returns>
		public bool DeleteCustomer(int customerId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				connection.Query<CustomerDBEntity>("[Crm].[DeleteCustomer]", new { CustomerId = customerId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}

			return true;
		}
    */

		/// <summary>
		/// Delete the specified customer.
		/// </summary>
		/// <param name="customerId">The customer's Id.</param>
		/// <returns>True if successful.</returns>
		public string ReactivateCustomer(int customerId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var resultGet = connection.Query<string>("[Crm].[ReactivateCustomer]", new { CustomerId = customerId }, commandType: CommandType.StoredProcedure);
				var result = resultGet.SingleOrDefault();
				if (result == null) { return ""; }
				return result;
			}
		}

		/// <summary>
		/// Gets all the projects a user can use in an organization.
		/// </summary>
		/// <param name="userId">The user's Id.</param>
		/// <param name="orgId">The organization's Id.</param>
		/// <param name="activity">The level of activity you wish to allow. Specifying 0 includes inactive projects.</param>
		/// <returns>A collection of CompleteProjectDBEntity objects for each project the user has access to within the organization.</returns>
		public async Task<IEnumerable<ProjectDBEntity>> GetProjectsByUserAndOrganization(int userId, int orgId, int activity)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@orgId", orgId);
			parameters.Add("@activity", activity);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.QueryAsync<ProjectDBEntity>(
					"[Pjm].[GetProjectsByUserAndOrganization]",
					parameters,
					 commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets all projects from all customers in an organization.
		/// </summary>
		/// <param name="orgId">The organization's Id.</param>
		/// <param name="activity">The level of activity you wish to allow. Specifying 0 includes inactive projects.</param>
		/// <returns>A collection of CompleteProjectInfo objects for each project within the organization.</returns>
		public IEnumerable<ProjectDBEntity> GetProjectsByOrgId(int orgId, int activity)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgId);
			parameters.Add("@activity", activity);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<ProjectDBEntity>(
					"[Pjm].[GetProjectsByOrgId]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets a project from its id.
		/// </summary>
		/// <param name="projectId">The project's Id.</param>
		/// <returns>Info about the requested project.</returns>
		public ProjectDBEntity GetProjectById(int projectId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectId", projectId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<ProjectDBEntity>(
					"[Pjm].[GetProjectById]",
					parameters,
					commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}
		
		/// <summary>
		/// Gets a project from its id.
		/// </summary>
		/// <param name="projectOrgId">The project's Id.</param>
		/// <returns>Info about the requested project.</returns>
		public ProjectDBEntity GetProjectByProjectOrgId(string projectOrgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectOrgId", projectOrgId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var entity = connection.Query<ProjectDBEntity>(
					"[Pjm].[GetProjectByProjectOrgId]",
					parameters,
					commandType: CommandType.StoredProcedure).SingleOrDefault();
				return entity;
			}
		}

		/// <summary>
		/// Gets a project from its id and a user id, with the IsProjectUser field filled out
		/// for that user.
		/// </summary>
		/// <param name="projectId">The project's Id.</param>
		/// <param name="userId">The user Id.</param>
		/// <returns>Info about the requested project.</returns>
		public async Task<ProjectDBEntity> GetProjectByIdAndUser(int projectId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectId", projectId);
			parameters.Add("@userId", userId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<ProjectDBEntity>(
					"[Pjm].[GetProjectByIdAndUser]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return results.SingleOrDefault();
			}
		}

		/// <summary>
		/// Updates all of a project's properties and its users in one go.
		/// </summary>
		/// <param name="projectId">The Project's Id.</param>
		/// <param name="projectName">The new name of the project.</param>
		/// <param name="orgId">The new orgId of the project.</param>
		/// <param name="isHourly">The pricing type of the project.  True == hourly, false == fixed. TODO: use this parameter to update the project's isHourly column.  Currently disabled attribute.</param>
		/// <param name="start">The start date assigned to the project.</param>
		/// <param name="end">The end date assigned to the project.</param>
		/// <param name="userIds">The updated list of project users, by their Ids.</param>
		public async void UpdateProjectAndUsers(int projectId, string projectName, string orgId, bool isHourly, DateTime? start, DateTime? end, IEnumerable<int> userIds)
		{
			if (string.IsNullOrWhiteSpace(projectName))
			{
				throw new ArgumentException("Name cannot be null, empty, or whitespace.");
			}

			if (string.IsNullOrWhiteSpace(orgId))
			{
				throw new ArgumentException("OrgId cannot be null, empty, or whitespace.");
			}

			DataTable userIdsTable = new DataTable();
			userIdsTable.Columns.Add("userId", typeof(int));
			foreach (int userId in userIds)
			{
				userIdsTable.Rows.Add(userId);
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectId", projectId);
			parameters.Add("@projectName", projectName);
			parameters.Add("@orgId", orgId);
			parameters.Add("@isHourly", isHourly);
			parameters.Add("@startingDate", start?.ToShortDateString());
			parameters.Add("@endingDate", end?.ToShortDateString());
			parameters.Add("@userIds", userIdsTable.AsTableValuedParameter("[Auth].[UserTable]"));

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync(
					"[Pjm].[UpdateProjectAndUsers]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Returns a list of ProjectDBEntities for projects the given user is assigned to in the given organization,
		/// another list of ProjectDBEntities for all projects in the given organization, and a UserDBEntity with the
		/// name and email of the user.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>.</returns>
		public async Task<Tuple<List<ProjectDBEntity>, List<ProjectDBEntity>, UserDBEntity>> GetProjectsForOrgAndUser(int userId, int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@orgId", orgId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryMultipleAsync(
					"[Pjm].[GetProjectsForOrgAndUser]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<ProjectDBEntity>().ToList(),
					results.Read<ProjectDBEntity>().ToList(),
					results.Read<UserDBEntity>().SingleOrDefault());
			}
		}

		/// <summary>
		/// Returns a list of CompleteProjectDBEntities for the given organization with the IsProjectUser field filled
		/// out for the given user, and a list of CustomerDBEntities for the organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="userId">User Id.</param>
		/// <returns>.</returns>
		public async Task<Tuple<List<ProjectDBEntity>, List<dynamic>>> GetProjectsAndCustomersForOrgAndUser(int orgId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@orgId", orgId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryMultipleAsync(
					"[Pjm].[GetProjectsAndCustomersForOrgAndUser]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<ProjectDBEntity>().ToList(),
					results.Read<dynamic>().ToList());
			}
		}

		/// <summary>
		/// Returns a list of CompleteProjectDBEntities for the given organization with the IsProjectUser field filled
		/// out for the given user, and a list of CustomerDBEntities for the organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="userId">User Id.</param>
		/// <returns>.</returns>
		public Tuple<List<ProjectDBEntity>, List<dynamic>> GetInactiveProjectsAndCustomersForOrgAndUser(int orgId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@orgId", orgId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[Crm].[GetInactiveProjectsAndCustomersForOrgAndUser]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<ProjectDBEntity>().ToList(),
					results.Read<dynamic>().ToList());
			}
		}
	}
}