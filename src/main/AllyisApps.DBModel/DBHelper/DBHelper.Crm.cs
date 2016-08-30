//------------------------------------------------------------------------------
// <copyright file="DBHelper.Crm.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Crm;
using Dapper;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// DBHelper Partial.
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// Method to Create a new project.
		/// </summary>
		/// <param name="organizationId">The organization associated with the project.</param>
		/// <param name="customerId">The id of the customer the project is for.</param>
		/// <param name="name">The name of project.</param>
		/// <param name="type">The type of project pricing type.</param>
		/// <param name="start">The start date of the project.</param>
		/// <param name="end">The end date of the project.</param>
		/// <returns>Returns the id of the created project, else returns -1.</returns>
		public int CreateProject(int organizationId, int customerId, string name, string type, DateTime start, DateTime end)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Name cannot be null, empty, or whitespace.");
			}

			////string type = "Fixed";
			////DateTime start = DateTime.MinValue;
			////DateTime end = DateTime.MaxValue;

			DynamicParameters parameters = new DynamicParameters();

			////parameters.Add("@OrganizationId", organizationId);
			parameters.Add("@customerID", customerId);
			parameters.Add("@Name", name);
			parameters.Add("@PriceType", type);
			parameters.Add("@StartingDate", start.ToShortDateString());
			parameters.Add("@EndingDate", end.ToShortDateString());
			parameters.Add("@retId", -1, DbType.Int32, direction: ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute(
					"[Crm].[CreateProject]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@retId");
		}

		/// <summary>
		/// Method to Create a new project from the customer Id.
		/// </summary>
		/// <param name="customerId">The id of the customer the project is for.</param>
		/// <param name="name">The name of project.</param>
		/// <param name="type">The type of project pricing type.</param>
		/// <param name="start">The start date of the project.</param>
		/// <param name="end">The end date of the project.</param>
		/// <returns>Returns the id of the created project, else returns -1.</returns>
		public int CreateProjectFromCustomerIdOnly(int customerId, string name, string type, DateTime start, DateTime end)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Name cannot be null, empty, or whitespace.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerID", customerId);
			parameters.Add("@Name", name);
			parameters.Add("@PriceType", type);
			parameters.Add("@StartingDate", start.ToShortDateString());
			parameters.Add("@EndingDate", end.ToShortDateString());
			parameters.Add("@retId", -1, DbType.Int32, direction: ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute(
					"[Crm].[CreateProjectFromCustomerIdOnly]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@retId");
		}

		/// <summary>
		/// Deletes a project.
		/// </summary>
		/// <param name="projectId">The id of the project to be deleted.</param>
		public void DeleteProject(int projectId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ProjectId", projectId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute(
					"[Crm].[DeleteProject]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets all the projects associated with a specific customer.
		/// </summary>
		/// <param name="customerID">The id of the customer.</param>
		/// <returns>A collection of projects with the definied customer.</returns>
		public IEnumerable<ProjectDBEntity> GetProjectsByCustomer(int customerID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerID", customerID);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<ProjectDBEntity>(
					"[Crm].[GetProjectsByCustomer]",
					parameters,
				   commandType: CommandType.StoredProcedure);
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
			parameters.Add("@ProjectId", projectId);
			parameters.Add("@UserId", userId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Query<int>(
					"[Crm].[CreateProjectUser]",
					parameters,
					commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Updates a ProjectUser entry in the database, isActive must be specified as 0 or 1.
		/// </summary>
		/// <param name="projectId">The project ID.</param>
		/// <param name="userId">The User ID.</param>
		/// <param name="isActive">Is active.</param>
		/// <returns>The number of rows successfully updated.</returns>C:\Users\v-trsan\Desktop\AllyisApps\aa\src\main\aadb\StoredProcedures\TimeTracker
		public int UpdateProjectUser(int projectId, int userId, int isActive)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ProjectId", projectId);
			parameters.Add("@UserId", userId);
			parameters.Add("@IsActive", isActive);
			parameters.Add("@RowsUpdated", 0);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				int rows = 1;
				rows = connection.Query<int>(
					"[Crm].[UpdateProjectUser]",
					parameters,
					commandType: CommandType.StoredProcedure).SingleOrDefault();

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
			parameters.Add("@ProjectId", projectId);
			parameters.Add("@UserId", userId);
			parameters.Add("@ret", 0);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>(
					"[Crm].[DeleteProjectUser]",
					parameters,
					commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Gets a collection of users that currently can use the project.
		/// </summary>
		/// <param name="projectId">The specified project's Id.</param>
		/// <returns>Collection of userIds.</returns>
		public IEnumerable<UserDBEntity> GetUsersByProjectId(int projectId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ProjectId", projectId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				IEnumerable<UserDBEntity> result = connection.Query<UserDBEntity>(
					"[Crm].[GetProjectUsersByProjectId]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return result;
			}
		}

		/// <summary>
		/// Updates the customer with the information specified in the customer table.
		/// </summary>
		/// <param name="customer">The table with the customer to create.</param>
		/// <returns>The ID of the customer if one was created -1 if not.</returns>
		public int CreateCustomerInfo(CustomerDBEntity customer)
		{
			Trace.WriteLine("CreateUserInfo");

			if (customer == null)
			{
				throw new ArgumentException("customer cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ContactEmail", customer.ContactEmail);
			parameters.Add("@Name", customer.Name);
			parameters.Add("@Address", customer.Address);
			parameters.Add("@City", customer.City);
			parameters.Add("@State", customer.State);
			parameters.Add("@Country", customer.Country);
			parameters.Add("@PostalCode", customer.PostalCode);
			parameters.Add("@ContactPhoneNumber", customer.ContactPhoneNumber);
			parameters.Add("@FaxNumber", customer.FaxNumber);
			parameters.Add("@Website", customer.Website);
			parameters.Add("@EIN", customer.EIN);
			parameters.Add("@OrganizationID", customer.OrganizationId);
			parameters.Add("@retId", -1, DbType.Int32, direction: ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				connection.Execute("[Crm].[CreateCustomerInfo]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@retId");
		}

		/// <summary>
		/// Updates the customer with the specified ID.
		/// </summary>
		/// <param name="customer">The table with the customer to create.</param>
		public void UpdateCustomer(CustomerDBEntity customer)
		{
			Trace.WriteLine("UpdateUser");
			if (customer == null)
			{
				throw new ArgumentException("customer cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@CustomerId", customer.CustomerId);
			parameters.Add("@ContactEmail", customer.ContactEmail);
			parameters.Add("@Name", customer.Name);
			parameters.Add("@Address", customer.Address);
			parameters.Add("@City", customer.City);
			parameters.Add("@State", customer.State);
			parameters.Add("@Country", customer.Country);
			parameters.Add("@PostalCode", customer.PostalCode);
			parameters.Add("@ContactPhoneNumber", customer.ContactPhoneNumber);
			parameters.Add("@FaxNumber", customer.FaxNumber);
			parameters.Add("@Website", customer.Website);
			parameters.Add("@EIN", customer.EIN);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Crm].[UpdateCustomerInfo]", parameters, commandType: CommandType.StoredProcedure);
			}

			Trace.WriteLine("UpdateCusomterFinished");
		}

		/// <summary>
		/// Retrieves the customer's information from the database.
		/// </summary>
		/// <param name="orgId">The organization's ID.</param>
		/// <returns>The CustomerDBEntity containing the customer's information, null if call fails.</returns>
		public IEnumerable<CustomerDBEntity> GetCustomerList(int orgId)
		{
			Trace.WriteLine("GetCustomers");

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				return connection.Query<CustomerDBEntity>(
					"[Crm].[GetCustomersByOrgId]",
					new { OrgId = orgId },
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves the customer's information from the database.
		/// </summary>
		/// <param name="customerID">The customer's ID.</param>
		/// <returns>The CustomerDBEntity containing the customer's information, null if call fails.</returns>
		public CustomerDBEntity GetCustomerInfo(int customerID)
		{
			////Trace.WriteLine("GetCustomerInfo");

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				return connection.Query<CustomerDBEntity>("[Crm].[GetCustomerInfo]", new { CustomerId = customerID }, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Delete the specified customer.
		/// </summary>
		/// <param name="customerID">The customer's Id.</param>
		/// <returns>True if successful.</returns>
		public bool DeleteCustomer(int customerID)
		{
			Trace.WriteLine("DeleteCustomer");
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				connection.Query<CustomerDBEntity>("[Crm].[DeleteCustomer]", new { CustomerId = customerID }, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}

			return true;
		}

		/// <summary>
		/// Gets all the projects a user can use in an organization.
		/// </summary>
		/// <param name="userId">The user's Id.</param>
		/// <param name="orgId">The organization's Id.</param>
		/// <param name="activity">The level of activity you wish to allow. Specifying 0 includes inactive projects.</param>
		/// <returns>A collection of CompleteProjectDBEntity objects for each project the user has access to within the organization.</returns>
		public IEnumerable<CompleteProjectDBEntity> GetProjectsByUserAndOrganization(int userId, int orgId, int activity)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserId", userId);
			parameters.Add("@OrgId", orgId);
			parameters.Add("@Activity", activity);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<CompleteProjectDBEntity>(
					"[Crm].[GetProjectsByUserAndOrganization]",
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
		public IEnumerable<CompleteProjectDBEntity> GetProjectsByOrgId(int orgId, int activity)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrgId", orgId);
			parameters.Add("@Activity", activity);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<CompleteProjectDBEntity>(
					"[Crm].[GetProjectsByOrgId]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets a project from its id.
		/// </summary>
		/// <param name="projectId">The project's Id.</param>
		/// <returns>Info about the requested project.</returns>
		public CompleteProjectDBEntity GetProjectById(int projectId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ProjectId", projectId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<CompleteProjectDBEntity>(
					"[Crm].[GetProjectById]",
					parameters,
					commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Gets a Collection of project data that the user can use.
		/// </summary>
		/// <param name="userId">The User's Id.</param>
		/// <returns>Collection of project data.</returns>
		public IEnumerable<CompleteProjectDBEntity> GetProjectsByUserId(int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserId", userId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<CompleteProjectDBEntity>(
					"[Crm].[GetProjectsByUserId]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates all of a project's properties and its users in one go.
		/// </summary>
		/// <param name="projectId">The Project's Id.</param>
		/// <param name="name">The new name of the project.</param>
		/// <param name="type">The pricing type of the project.</param>
		/// <param name="start">The start date assigned to the project.</param>
		/// <param name="end">The end date assigned to the project.</param>
		/// <param name="userIDs">The updated list of project users, by their IDs.</param>
		public void UpdateProjectAndUsers(int projectId, string name, string type, DateTime start, DateTime end, IEnumerable<int> userIDs)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Name cannot be null, empty, or whitespace.");
			}

			DataTable userIDsTable = new DataTable();
			userIDsTable.Columns.Add("userId", typeof(int));
			foreach (int userID in userIDs)
			{
				userIDsTable.Rows.Add(userID);
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ProjectId", projectId);
			parameters.Add("@Name", name);
			parameters.Add("@PriceType", type);
			parameters.Add("@StartingDate", start.ToShortDateString());
			parameters.Add("@EndingDate", end.ToShortDateString());
			parameters.Add("@UserIDs", userIDsTable.AsTableValuedParameter("[Auth].[UserTable]"));

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Query(
					"[Crm].[UpdateProjectAndUsers]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}
	}
}