//------------------------------------------------------------------------------
// <copyright file="DBHelper.Auth.cs" company="Allyis, Inc.">
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
using AllyisApps.DBModel.Cache;
using Dapper;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// DBHelper Partial.
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// Updates the user with the information specified in the user table.
		/// </summary>
		/// <param name="user">The table with the user to create.</param>
		/// <returns>The ID of the user if one was created -1 if not.</returns>
		public int CreateUser(UserDBEntity user)
		{
			if (user == null)
			{
				throw new ArgumentException("user cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();

			parameters.Add("@Email", user.Email);
			parameters.Add("@FirstName", user.FirstName);
			parameters.Add("@LastName", user.LastName);
			parameters.Add("@Address", user.Address);
			parameters.Add("@City", user.City);
			parameters.Add("@State", user.State);
			parameters.Add("@Country", user.Country);
			parameters.Add("@PostalCode", user.PostalCode);
			parameters.Add("@PhoneNumber", user.PhoneNumber);
			parameters.Add("@DateOfBirth", user.DateOfBirth);
			parameters.Add("@UserName", user.UserName);
			parameters.Add("@EmailConfirmed", user.EmailConfirmed);
			parameters.Add("@PasswordHash", user.PasswordHash);
			parameters.Add("@TwoFactorEnabled", user.TwoFactorEnabled);
			parameters.Add("@AccessFailedCount", user.AccessFailedCount);
			parameters.Add("@LockoutEnabled", user.LockoutEnabled);
			parameters.Add("@LockoutEndDateUtc", user.LockoutEndDateUtc);
			parameters.Add("@LanguageID", user.LanguagePreference);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				user.UserId = (int)connection.Query<int>("[Auth].[CreateUserInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
			
			return user.UserId;
		}

		/// <summary>
		/// Executes [Auth].[GetUserFromEmail].
		/// </summary>
		/// <param name="email">Parameter @email.</param>
		/// <returns>UserDBEntity obj.</returns>
		public UserDBEntity GetUserByEmail(string email)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var result = connection.Query<UserDBEntity>("[Auth].[GetUserFromEmail] @a", new { a = email });
				return result.FirstOrDefault();
			}
		}

		/// <summary>
		/// Get a list of all organizations.
		/// </summary>
		/// <returns>A list of all organizations.</returns>
		public List<OrganizationDBEntity> GetOrganizationList()
		{
			using (var conn = new SqlConnection(this.SqlConnectionString))
			{
				return conn.Query<OrganizationDBEntity>("Auth.GetOrganizationList").ToList();
			}
		}

		/// <summary>
		/// Get a list of all organization users.
		/// </summary>
		/// <returns>A list of all organization users.</returns>
		public List<OrganizationUserDBEntity> GetOrganizationUserList()
		{
			using (var conn = new SqlConnection(this.SqlConnectionString))
			{
				return conn.Query<OrganizationUserDBEntity>("Auth.GetOrganizationUserList").ToList();
			}
		}

		///// <summary>
		///// Gets the Organization User Database entry.
		///// </summary>
		///// <param name="orgId">The Organization ID.</param>
		///// <param name="userId">The User ID.</param>
		///// <returns>The Organization User Database entry.</returns>
		//public OrganizationUserDBEntity GetOrganizationUser(int orgId, int userId)
		//{
		//	using (SqlConnection con = new SqlConnection(this.SqlConnectionString))
		//	{
		//		DynamicParameters parameters = new DynamicParameters();
		//		parameters.Add("@OrgId", orgId);
		//		parameters.Add("@UserId", userId);
		//		return con.Query<OrganizationUserDBEntity>("[Auth].[GetOrganizationUser]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
		//	}
		//}

		///// <summary>
		///// Gets the org id by the access code.
		///// </summary>
		///// <param name="accessCode">The access code.</param>
		///// <returns>The org id.</returns>
		//public int GetOrgFromAccessCode(string accessCode)
		//{
		//	DynamicParameters parameters = new DynamicParameters();
		//	parameters.Add("@AccessCode", accessCode);

		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		return connection.Query<int>(
		//			"[Auth].[GetOrgFromAccessCode]",
		//			parameters,
		//		   commandType: CommandType.StoredProcedure).SingleOrDefault();
		//	}
		//}

		///// <summary>
		///// Gets the product roles for a specific product.
		///// </summary>
		///// <param name="productName">The name of the product to get roles for.</param>
		///// <returns>The Product roles.</returns>
		//public IEnumerable<ProductRoleDBEntity> GetProductRoles(string productName)
		//{
		//	DynamicParameters parameters = new DynamicParameters();
		//	parameters.Add("@productName", productName);
		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		return connection.Query<ProductRoleDBEntity>("[Auth].[GetProductRoles]", parameters, commandType: CommandType.StoredProcedure);
		//	}
		//}

		/// <summary>
		/// Get all product roles from db.
		/// </summary>
		/// <returns>A list of product roles.</returns>
		public List<ProductRoleDBEntity> GetProductRoleList()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<ProductRoleDBEntity>("[Auth].[GetProductRoleList]", null, commandType: CommandType.StoredProcedure).ToList();
			}
		}

		/// <summary>
		/// Gets the product role for a user for a product.
		/// </summary>
		/// <param name="productName">The name of the product.</param>
		/// <param name="organizationID">The organization.</param>
		/// <param name="userID">The user.</param>
		/// <returns>The product rold for the user.</returns>
		public string GetProductRoleForUser(string productName, int organizationID, int userID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@productName", productName);
			parameters.Add("@organizationID", organizationID);
			parameters.Add("@userID", userID);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>("[Auth].[GetActiveProductRoleForUser]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault() ?? "None";
			}
		}

		/// <summary>
		/// Executes Auth.getUsersByOrganization.
		/// </summary>
		/// <param name="organizationID">Organization Id.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns>List of Subscription Users.</returns>
		public IEnumerable<SubscriptionUserDBEntity> GetUsersByOrganization(int organizationID, int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", organizationID);
			parameters.Add("@SubscriptionId", subscriptionId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default empty list
				return connection.Query<SubscriptionUserDBEntity>("[Auth].[GetUsersByOrganization]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		///// <summary>
		///// Gets the organizations where the user Is the admin.
		///// </summary>
		///// <param name="userId">The User's Id.</param>
		///// <returns>A list of the organizations the user is admin of.</returns>
		//public IEnumerable<OrganizationDBEntity> GetOrganizationsWhereUserIsAdmin(int userId)
		//{
		//	DynamicParameters parameters = new DynamicParameters();
		//	parameters.Add("@userID", userId);

		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		// default empty list
		//		return connection.Query<OrganizationDBEntity>("[Auth].[GetOrganizationsWhereUserIsAdmin]", parameters, commandType: CommandType.StoredProcedure);
		//	}
		//}

		/// <summary>
		/// Updates the user with the specified ID.
		/// </summary>
		/// <param name="user">The table with the user to create.</param>
		public void UpdateUser(UserDBEntity user)
		{
			if (user == null)
			{
				throw new ArgumentException("user cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();

			parameters.Add("@Id", user.UserId);
			parameters.Add("@FirstName", user.FirstName);
			parameters.Add("@LastName", user.LastName);
			parameters.Add("@Address", user.Address);
			parameters.Add("@City", user.City);
			parameters.Add("@State", user.State);
			parameters.Add("@Country", user.Country);
			parameters.Add("@PostalCode", user.PostalCode);
			parameters.Add("@PhoneNumber", user.PhoneNumber);
			parameters.Add("@DateOfBirth", user.DateOfBirth);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[UpdateUserInfo]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves the user's information from the database.
		/// </summary>
		/// <param name="userId">The user's ID.</param>
		/// <returns>The TableUsers containing the user's information, null if call fails.</returns>
		public UserDBEntity GetUserInfo(int userId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				return connection.Query<UserDBEntity>("[Auth].[GetUserInfo]", new { UserId = userId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Populates a user's last used subscription.
		/// </summary>
		/// <param name = "userId">Target user's ID.</param>
		/// <param name = "subId">The subscription's ID.</param>
		public void UpdateActiveSubscription(int userId, int? subId = null)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userID", userId);
			parameters.Add("@SubscriptionId", subId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute(
					"[Auth].[UpdateUserActiveSub]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Populates a user's last used organization.
		/// </summary>
		/// <param name = "userId">Target user's ID.</param>
		/// <param name = "organizationID">The organization's ID.</param>
		public void UpdateActiveOrganization(int userId, int organizationID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userID", userId);
			parameters.Add("@organizationID", organizationID);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute(
					"[Auth].[UpdateUserActiveOrg]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates the preferred language for a user.
		/// </summary>
		/// <param name="userID">Target user's ID.</param>
		/// <param name="languageID">Language ID.</param>
		public void UpdateUserLanguagePreference(int userID, int languageID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", userID);
			parameters.Add("@LanguageID", languageID);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute(
					"[Auth].[UpdateUserLanguagePreference]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets the list of organizations to which the user belongs, as well as
		///		the user's role within each organization.
		/// </summary>
		/// <param name="userId">The user's ID.</param>
		/// <returns>The collection of organizations this user is a part of.</returns>
		public List<OrganizationUserDBEntity> GetUserOrganizationList(int userId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				return connection.Query<OrganizationUserDBEntity>("[Auth].[GetOrgListByUserId]", new { UserId = userId }, commandType: CommandType.StoredProcedure).ToList();
			}
		}

		/// <summary>
		/// Gets the list of subscriptions to which the user belongs, as well as
		///		the user's role within each subscription.
		/// </summary>
		/// <param name="userId">The user's ID.</param>
		/// <param name="orgId">The organization's ID.</param>
		/// <returns>The collection of subscriptions this user is a part of for the given organization.</returns>
		public List<SubscriptionUserDBEntity> GetUserSubscriptionList(int userId, int orgId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				return connection.Query<SubscriptionUserDBEntity>("[Auth].[GetSubscriptionsByUserAndOrg]", new { UserId = userId, OrgId = orgId }, commandType: CommandType.StoredProcedure).ToList();
			}
		}

		///// <summary>
		///// Gets a list of organizations for which the given user is a member of.
		///// </summary>
		///// <param name="userId">The user ID.</param>
		///// <returns>A list of organizations.</returns>
		//public List<OrganizationUserDBEntity> GetOrganizationUserListByUserId(int userId)
		//{
		//	return OrganizationUserDBEntityCache.Instance.Items().Where(x => x.UserId == userId).ToList();
		//}

		/// <summary>
		/// Returns the permissions all user's in an organization have for a product.
		/// </summary>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="productId">The product's Id.</param>
		/// <returns>A collection of user data.</returns>
		public IEnumerable<UserDBEntity> GetUsersWithSubscriptionToProductInOrganization(int organizationId, int productId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", organizationId);
			parameters.Add("@ProductId", productId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<UserDBEntity>("[Auth].[GetUsersWithSubscriptionToProductInOrganization]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates new password in Auth.User table.
		/// </summary>
		/// <param name = "userId">Target user's ID.</param>
		/// <param name = "password">The subscription's ID.</param>
		public void UpdateUserPassword(int userId, string password)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userID", userId);
			parameters.Add("@PasswordHash", password);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute(
					"[Auth].[UpdateUserPassword]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates new password in Auth.User table. Requires proper reset code.
		/// </summary>
		/// <param name = "userId">Target user's ID.</param>
		/// <param name = "password">The new password hash.</param>
		/// <param name = "code">The password reset code.</param>
		/// <returns>An int...</returns>
		public int UpdateUserPasswordUsingCode(int userId, string password, string code)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserID", userId);
			parameters.Add("@PasswordHash", password);
			parameters.Add("@PasswordResetCode", code);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>(
					"[Auth].[UpdateUserPasswordUsingCode]",
					parameters,
					commandType: CommandType.StoredProcedure).FirstOrDefault<int>();
			}
		}

		/// <summary>
		/// Updates password reset code in Auth.User table.
		/// </summary>
		/// <param name = "userId">Target user's ID.</param>
		/// <param name = "resetCode">The resetCode.</param>
		public void UpdateUserPasswordResetCode(int userId, string resetCode)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserId", userId);
			parameters.Add("@PasswordResetCode", resetCode);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute(
					"[Auth].[UpdateUserPasswordResetCode]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds an organization to the database.
		/// </summary>
		/// <param name="org">The OrganizationDBEntity to create.</param>
		/// <param name="ownerId">The owner's user ID.</param>
		/// <param name="roleId">The role associated with the creator of the organization.</param>
        /// <param name="employeeId">The employee ID for the user creating the organization.</param>
		/// <returns>The id of the created organization or -1.</returns>
		public int CreateOrganization(OrganizationDBEntity org, int ownerId, int roleId, string employeeId)
		{
			if (org == null)
			{
				throw new ArgumentException("organizationID cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userID", ownerId);
			parameters.Add("@RoleId", roleId);
			parameters.Add("@Name", org.Name);
			parameters.Add("@SiteUrl", org.SiteUrl);
			parameters.Add("@Address", org.Address);
			parameters.Add("@City", org.City);
			parameters.Add("@State", org.State);
			parameters.Add("@Country", org.Country);
			parameters.Add("@PostalCode", org.PostalCode);
			parameters.Add("@PhoneNumber", org.PhoneNumber);
			parameters.Add("@FaxNumber", org.FaxNumber);
			parameters.Add("@Subdomain", org.Subdomain);
			parameters.Add("@retId", -1, DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@EmployeeId", employeeId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				connection.Execute("[Auth].[CreateOrg]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@retId");
		}

		/// <summary>
		/// Updates the specified organization with new information.
		/// </summary>
		/// <param name="org">The organization table with updates.</param>
		public void UpdateOrganization(OrganizationDBEntity org)
		{
			if (org == null)
			{
				throw new ArgumentException("org cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", org.OrganizationId);
			parameters.Add("@Name", org.Name);
			parameters.Add("@SiteUrl", org.SiteUrl);
			parameters.Add("@Address", org.Address);
			parameters.Add("@City", org.City);
			parameters.Add("@State", org.State);
			parameters.Add("@Country", org.Country);
			parameters.Add("@PostalCode", org.PostalCode);
			parameters.Add("@PhoneNumber", org.PhoneNumber);
			parameters.Add("@FaxNumber", org.FaxNumber);
			parameters.Add("@SubdomainName", org.Subdomain);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[UpdateOrg]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Executes [Auth].[DeleteOrg].
		/// </summary>
		/// <param name="organizationId">Parameter @organizationID. </param>
		public void DeleteOrganization(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrgId", organizationId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[DeleteOrg]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds user to an organization.
		/// </summary>
		/// <param name="orgUser">The organization's ID.</param>
		public void CreateOrganizationUser(OrganizationUserDBEntity orgUser)
		{
			if (orgUser == null)
			{
				throw new ArgumentException("orgUser cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", orgUser.OrganizationId);
			parameters.Add("@userID", orgUser.UserId);
			parameters.Add("@RoleId", orgUser.OrgRoleId);
			parameters.Add("@employeeID", orgUser.EmployeeId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[CreateOrgUser]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets the employee id for a user for an org.
		/// </summary>
		/// <param name="userID">The Id of the user.</param>
		/// <param name="organizationID">The Id of the organization.</param>
		/// <returns>The employee id.</returns>
		public string GetEmployeeId(int userID, int organizationID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userID", userID);
			parameters.Add("@organizationID", organizationID);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>("[Auth].[GetOrgUserEmployeeId]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		///// <summary>
		///// Sets the employee id for a user for an org.
		///// </summary>
		///// <param name="userID">The Id of the user.</param>
		///// <param name="organizationID">The Id of the organization.</param>
		///// <param name="employeeID">The value to set the employeeID to.</param>
		//public void SetEmployeeId(int userID, int organizationID, string employeeID)
		//{
		//	DynamicParameters parameters = new DynamicParameters();
		//	parameters.Add("@userID", userID);
		//	parameters.Add("@organizationID", organizationID);
		//	parameters.Add("@employeeID", employeeID);

		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		connection.Execute("[Auth].[UpdateOrgUserEmployeeId]", parameters, commandType: CommandType.StoredProcedure);
		//	}
		//}

		/// <summary>
		/// Updates the specified user within the specified organization with a new role.
		/// </summary>
		/// <param name="orgUser">The orgUser table with updates.</param>
		public void UpdateOrganizationUser(OrganizationUserDBEntity orgUser)
		{
			if (orgUser == null)
			{
				throw new ArgumentException("orgUser cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", orgUser.OrganizationId);
			parameters.Add("@userID", orgUser.UserId);
			parameters.Add("@RoleId", orgUser.OrgRoleId);
			parameters.Add("@employeeID", orgUser.EmployeeId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[UpdateOrgUser]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Removes the specified user from the organization.
		/// </summary>
		/// <param name="organizationId">The organization's ID.</param>
		/// <param name="userId">The user's ID.</param>
		public void RemoveOrganizationUser(int organizationId, int userId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("@organizationID", organizationId);
				parameters.Add("@userID", userId);

				connection.Execute("[Auth].[DeleteOrgUser]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves the specified user's permissions in the specified organization.
		/// </summary>
		/// <param name="organizationId">The organization's ID.</param>
		/// <param name="userId">The user's ID.</param>
		/// <returns>The TableOrganizationRole related to the user for the specified organization.</returns>
		public OrgRoleDBEntity GetPermissionLevel(int organizationId, int userId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				return connection.Query<OrgRoleDBEntity>("[Auth].[GetOrgUserRole]", new { OrganizationId = organizationId, UserId = userId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Returns the organization information.
		/// </summary>
		/// <param name="organizationId">The organization's ID.</param>
		/// <returns>The TableOrganizations containing the organization's information, null if an error is encountered.</returns>
		public OrganizationDBEntity GetOrganization(int organizationId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<OrganizationDBEntity>("[Auth].[GetOrg] @a", new { a = organizationId }).FirstOrDefault();
			}
		}

		/// <summary>
		/// Retrieves the list of members for the specified organization.
		/// </summary>
		/// <param name="organizationId">The organization's ID.</param>
		/// <returns>The collection of users in the organization, null on error.</returns>
		public IEnumerable<OrganizationUserDBEntity> GetOrganizationMemberList(int organizationId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				return connection.Query<OrganizationUserDBEntity>(
					"[Auth].[GetOrgUserList]",
					new
					{
						OrganizationId = organizationId
					},
				commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retreives the id of the organization that registered the given subdomain.
		/// </summary>
		/// <param name="subdomain">The organization's subdomain.</param>
		/// <returns>The id of the organization.</returns>
		public int GetIdBySubdomain(string subdomain)
		{
			DynamicParameters param = new DynamicParameters();
			param.Add("@Subdomain", subdomain);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>("[Auth].[GetOrgIdBySubdomain]", param, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Retrieves the subdomain that is registerd with the organzation with the provided id.
		/// </summary>
		/// <param name="id">The organization's Id.</param>
		/// <returns>The subdomain registered with the organization.</returns>
		public string GetSubdomainById(int id)
		{
			DynamicParameters param = new DynamicParameters();
			param.Add("@OrgId", id);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>("[Auth].[GetSubdomainByOrgId]", param, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		///// <summary>
		///// Executes [Auth].[GetNumberOfOrgSubscriptions].
		///// </summary>
		///// <param name="organizationID">Parameter @organizationID.</param>
		///// <returns>Count of Organization subscriptions.</returns>
		//public int GetOrganizationSubscriptionCount(int organizationID)
		//{
		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		// default -1
		//		return (int)connection.ExecuteScalar("[Auth].[GetNumberOfOrgSubscriptions]", new { OrgId = organizationID }, commandType: CommandType.StoredProcedure);
		//	}
		//}

		/// <summary>
		///  Gets [Auth].[GetOrganizationsByUserId].
		/// </summary>
		/// <param name="userId">Parameter @userId.</param>
		/// <returns>A list of OrganizationDBEntities containing the organizations the user is subscribed to.</returns>
		public IEnumerable<OrganizationDBEntity> GetOrganizationsByUserId(int userId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default empty list
				return connection.Query<OrganizationDBEntity>(
					"[Auth].[GetOrganizationsByUserId]",
					new
					{
						UserId = userId
					},
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds a user Invitation to the invitations table.
		/// </summary>
		/// <param name="invitation">A representation of the invitation to create.</param>
		/// <returns>The id of the newly created invitation.</returns>
		public int CreateUserInvitation(InvitationDBEntity invitation)
		{
			if (invitation == null)
			{
				throw new ArgumentException("invitation cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Email", invitation.Email);
			parameters.Add("@FirstName", invitation.FirstName);
			parameters.Add("@LastName", invitation.LastName);
			parameters.Add("@DateOfBirth", invitation.DateOfBirth);
			parameters.Add("@organizationID", invitation.OrganizationId);
			parameters.Add("@AccessCode", invitation.AccessCode);
			parameters.Add("@OrgRole", invitation.OrgRole);
			parameters.Add("@ProjectId", invitation.ProjectId);
			parameters.Add("@retId", -1, DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@EmployeeId", invitation.EmployeeId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[CreateUserInvitation]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@retId");
		}

		/// <summary>
		/// Gets all of the invitations based off of user data.
		/// </summary>
		/// <param name="user">A representation of the User's data.</param>
		/// <returns>A list of invitations the user is a part of.</returns>
		public IEnumerable<InvitationDBEntity> GetUserInvitationsByUserData(UserDBEntity user)
		{
			if (user == null)
			{
				throw new ArgumentException("user cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Email", user.Email);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default empty list
				return connection.Query<InvitationDBEntity>("[Auth].[GetUserInvitationsByUserData]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		///// <summary>
		///// Gets all of the invitations for the organization.
		///// </summary>
		///// <param name="organizationID">The organization's Id.</param>
		///// <returns>Returns a list of invitations associated with the organization.</returns>
		//public IEnumerable<InvitationDBEntity> GetUserInvitationsByOrganizationId(int organizationID)
		//{
		//	DynamicParameters parameters = new DynamicParameters();
		//	parameters.Add("@organizationID", organizationID);

		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		// default empty list
		//		return connection.Query<InvitationDBEntity>("[Auth].[GetUserInvitationsByOrganizationId]", parameters, commandType: CommandType.StoredProcedure);
		//	}
		//}

		/// <summary>
		/// Deletes the defined invitation.
		/// </summary>
		/// <param name="invitationId">The invitation's Id.</param>
		public void RemoveUserInvitation(int invitationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@InvitationId", invitationId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[DeleteUserInvitation]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds a single entry to the InvitationSubRole database.
		/// </summary>
		/// <param name="invitationId">The invitation Id.</param>
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <param name="productRoleId">The Id of the product role.</param>
		public void CreateInvitationSubRole(int invitationId, int subscriptionId, int productRoleId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@InvitationId", invitationId);
			parameters.Add("@SubscriptionId", subscriptionId);
			parameters.Add("@ProductRoleId", productRoleId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[CreateInvitationSubRole]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a single entry from the InvitationSubRole database.
		/// </summary>
		/// <param name="invitationId">The invitation Id.</param>
		/// <param name="subscriptionId">The subscription Id.</param>
		public void DeleteInvitationSubRole(int invitationId, int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@InvitationId", invitationId);
			parameters.Add("@SubscriptionId", subscriptionId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[DeleteInvitationSubRole]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets the roles for each subscription the invited user has.
		/// </summary>
		/// <param name="invitationId">The invitation Id.</param>
		/// <returns>List of all roles.</returns>
		public IEnumerable<InvitationSubRoleDBEntity> GetInvitationSubRolesByInvitationId(int invitationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@InvitationId", invitationId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<InvitationSubRoleDBEntity>("[Auth].[GetInvitationSubRolesByInvitationId]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets the first name of a user if they are in that organizaiton.
		/// </summary>
		/// <param name="organizationID">The organization id.</param>
		/// <param name="email">The user email.</param>
		/// <returns>The user first name.</returns>
		public string GetOrgUserFirstName(int organizationID, string email)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Email", email);
			parameters.Add("@OrgID", organizationID);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>("[Auth].[GetOrgUserByEmail]", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Gets the roles for each subscription the organization has.
		/// </summary>
		/// <param name="organizationId">The organization Id.</param>
		/// <returns>List of all roles.</returns>
		public IEnumerable<InvitationDBEntity> GetUserInvitationsByOrgId(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", organizationId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<InvitationDBEntity>("[Auth].[GetUserInvitationsByOrgId]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets the roles for each subscription the organization has.
		/// </summary>
		/// <param name="organizationId">The organization Id.</param>
		/// <returns>List of all roles.</returns>
		public IEnumerable<InvitationSubRoleDBEntity> GetInvitationSubRolesByOrganizationId(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", organizationId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<InvitationSubRoleDBEntity>("[Auth].[GetUserInvitationsByOrgId]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets the user roles.
		/// </summary>
		/// <param name="orgid">The id of the relevant organization.</param>
		/// <returns>An UserRoles object.</returns>
		public IEnumerable<UserRolesDBEntity> GetRoles(int orgid)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrgId", orgid);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<UserRolesDBEntity>(
					"[Auth].[GetRolesAndPermissions]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets all roles accross all organizations.
		/// </summary>
		/// <returns>A list of OrgRoleDBEntity entities.</returns>
		public List<OrgRoleDBEntity> GetOrgRoleList()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<OrgRoleDBEntity>("[Auth].[GetOrgRoleList]", null, commandType: CommandType.StoredProcedure).ToList();
			}
		}

		/// <summary>
		/// Gets a user List.
		/// </summary>
		/// <returns>A list of UserDBEntity entities.</returns>
		internal List<UserDBEntity> GetUserList()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<UserDBEntity>("[Auth].[GetUserList]", null, commandType: CommandType.StoredProcedure).ToList();
			}
		}

		/// <summary>
		/// Gets the user context information. The returned results will be for a single user, but will have a separate row for each subscription
		/// the user is a part of. User information is repeated on each row, and organizaiton information is repeated in some rows when the user 
		/// is a part of mulitple subscriptions in the same organization.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public List<UserContextDBEntity> GetUserContextInfo(int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserId", userId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<UserContextDBEntity>(
					"[Auth].[GetUserContextInfo]",
					parameters,
					commandType: CommandType.StoredProcedure).ToList();
			}
		}

		/// <summary>
		/// Returns a UserDBEntity for the given user, along with a list of OrganizationDBEntities for the organizations that
		/// the user is a member of, and a list of InvititationDBEntities for any invitations for that user.
		/// </summary>
		/// <param name="userId">The User Id</param>
		public Tuple<UserDBEntity, List<OrganizationDBEntity>, List<InvitationDBEntity>> GetUserOrgsAndInvitations(int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[Auth].[GetUserOrgsAndInvitationInfo]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create<UserDBEntity, List<OrganizationDBEntity>, List<InvitationDBEntity>>(
					results.Read<UserDBEntity>().SingleOrDefault(),
					results.Read<OrganizationDBEntity>().ToList(),
					results.Read<InvitationDBEntity>().ToList());
			}
		}
	}
}