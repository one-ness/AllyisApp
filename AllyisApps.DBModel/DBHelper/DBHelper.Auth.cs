//------------------------------------------------------------------------------
// <copyright file="DBHelper.Auth.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Crm;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
		/// <param name="emailConfirmationCode">The email confirmation code to put in for the user.</param>
		/// <returns>The ID of the user if one was created -1 if not.</returns>
		public Tuple<int, int> CreateUser(UserDBEntity user, Guid emailConfirmationCode)
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
			parameters.Add("@EmailConfirmationCode", emailConfirmationCode);
			parameters.Add("@PasswordHash", user.PasswordHash);
			parameters.Add("@TwoFactorEnabled", user.TwoFactorEnabled);
			parameters.Add("@LockoutEnabled", user.LockoutEnabled);
			parameters.Add("@LockoutEndDateUtc", user.LockoutEndDateUtc);
			parameters.Add("@LanguageID", user.LanguagePreference);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				//user.UserId = (int)connection.Query<int>("[Auth].[CreateUserInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

				var results = connection.QueryMultiple(
					"[Auth].[CreateUserInfo]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<int>().FirstOrDefault(),
					results.Read<int>().FirstOrDefault());
			}

			//return user.UserId;
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
		/// Returns the password hash for the given user.
		/// </summary>
		/// <param name="userId">User id.</param>
		/// <returns>Password hash.</returns>
		public string GetPasswordHashById(int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserId", userId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>(
					"[Auth].[GetPasswordHashFromUserId]",
					parameters,
					commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Updates new password in Auth.User table.
		/// </summary>
		/// <param name = "userId">Target user's ID.</param>
		/// <param name = "passwordHash">The new password hash.</param>
		/// <returns>The password hash retreived independently after the update, for verification.</returns>
		public void UpdateUserPassword(int userId, string passwordHash)
		{
			using (var con = new SqlConnection(this.SqlConnectionString))
			{
				con.Execute("[Auth].[UpdateUserPassword] @a, @b", new { a = userId, b = passwordHash });
			}
		}

		/// <summary>
		/// Updates new password in Auth.User table. Requires proper reset code.
		/// </summary>
		/// <param name = "userId">Target user's ID.</param>
		/// <param name = "password">The new password hash.</param>
		/// <param name = "code">The password reset code.</param>
		/// <returns>An int...</returns>
		public int UpdateUserPasswordUsingCode(int userId, string password, Guid code)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserID", userId);
			parameters.Add("@PasswordHash", password);
			parameters.Add("@PasswordResetCode", code.ToString());

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
		/// <param name = "email">Target user's email address.</param>
		/// <param name = "resetCode">The resetCode.</param>
		/// <returns>UserId of updated account, or -1 if no account is found for the given email.</returns>
		public int UpdateUserPasswordResetCode(string email, string resetCode)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Email", email);
			parameters.Add("@PasswordResetCode", resetCode);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>(
					"[Auth].[UpdateUserPasswordResetCode]",
					parameters,
					commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Adds an organization to the database and sets the owner's chosen organization to the new org.
		/// </summary>
		/// <param name="org">The OrganizationDBEntity to create.</param>
		/// <param name="ownerId">The owner's user ID.</param>
		/// <param name="roleId">The role associated with the creator of the organization.</param>
		/// <param name="employeeId">The employee ID for the user creating the organization.</param>
		/// <returns>The id of the created organization or -1 if the subdomain name is taken.</returns>
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
			parameters.Add("@EmployeeTypeId", 1);   //assuming Org's owner is always a salaried employee

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
		/// <returns>True for success, false if the subdomain name is changed to something already taken.</returns>
		public bool UpdateOrganization(OrganizationDBEntity org)
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
				return connection.Query<int>("[Auth].[UpdateOrg]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault() == 1;
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
			parameters.Add("employeeTypeId", orgUser.EmployeeTypeId);

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

		/// <summary>
		/// Updates an organization member's info
		/// </summary>
		/// <param name="employeeId">The member's id</param>
		/// <param name="employeeTypeId">The member's type (Salary=1/Hourly=2)</param>
		/// <param name="employeeRoleId">The member's role (Member=1/Owner=2)</param>
		/// <param name="isInvited">Is the member invited or already a member?</param>
		/// <param name="orgId">The org id</param>
		/// <param name="userId">The user id</param>
		/// <returns>A 1 or 0 based on if the employeeId already exists or not</returns>
		public int UpdateMember(string employeeId, int employeeTypeId, int employeeRoleId, bool isInvited, int userId, int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@orgId", orgId);
			parameters.Add("@employeeId", employeeId);
			parameters.Add("@employeeTypeId", employeeTypeId);
			parameters.Add("@employeeRoleId", employeeRoleId);
			parameters.Add("@isInvited", isInvited);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Execute("[Auth].[UpdateMember]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Sets the employee id for a user for an org.
		/// </summary>
		/// <param name="userID">The Id of the user.</param>
		/// <param name="organizationID">The Id of the organization.</param>
		/// <param name="employeeID">The value to set the employeeID to.</param>
		public int SetEmployeeId(int userID, int organizationID, string employeeID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userID", userID);
			parameters.Add("@organizationID", organizationID);
			parameters.Add("@employeeID", employeeID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>("[Auth].[UpdateOrgUserEmployeeId]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Sets the Employee Id for a invitation in a org
		/// </summary>
		/// <param name="invitationID">Id of the invitation</param>
		/// <param name="organizationID">Id of the organization</param>
		/// <param name="employeeID">The value to set the employeeID to</param>
		public int SetInvitationEmployeeId(int invitationID, int organizationID, string employeeID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@invitationID", invitationID);
			parameters.Add("@organizationID", organizationID);
			parameters.Add("@employeeID", employeeID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>("[Auth].[UpdateOrgInvitationEmployeeId]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Sets the employee type id for a user for an org.
		/// </summary>
		/// <param name="userID">The Id of the user.</param>
		/// <param name="organizationID">The Id of the organization.</param>
		/// <param name="employeeTypeID">The value to set the employeeTypeId to.</param>
		public void SetEmployeeTypeId(int userID, int organizationID, int employeeTypeID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userID", userID);
			parameters.Add("@organizationID", organizationID);
			parameters.Add("@employeeTypeID", employeeTypeID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[UpdateOrgUserEmployeeTypeId]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Sets the Employee Type Id for a invitation in a org
		/// </summary>
		/// <param name="invitationID">Id of the invitation</param>
		/// <param name="organizationID">Id of the organization</param>
		/// <param name="employeeTypeID">The value to set the employeeID to</param>
		public void SetInvitationEmployeeTypeId(int invitationID, int organizationID, int employeeTypeID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@invitationID", invitationID);
			parameters.Add("@organizationID", organizationID);
			parameters.Add("@employeeTypeID", employeeTypeID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[UpdateOrgInvitationEmployeeTypeId]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

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
		/// Assigns an organization role to a list of users.
		/// </summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="organizationId">The Organization Id.</param>
		/// <param name="orgRoleId">Organization role to assign (or -1 to remove from organization).</param>
		/// <returns>The number of affected users.</returns>
		public int EditOrganizationUsers(List<int> userIds, int organizationId, int orgRoleId)
		{
			DataTable userIdsTable = new DataTable();
			userIdsTable.Columns.Add("userId", typeof(int));
			foreach (int userId in userIds)
			{
				userIdsTable.Rows.Add(userId);
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserIDs", userIdsTable.AsTableValuedParameter("[Auth].[UserTable]"));
			parameters.Add("@OrganizationId", organizationId);
			parameters.Add("@OrgRole", orgRoleId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>(
					"[Auth].[EditOrgUsers]",
					parameters,
					commandType: CommandType.StoredProcedure).SingleOrDefault();
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
		/// Retreives the id of the given employee type.
		/// </summary>
		/// <param name="employeeType">The employee type's name.</param>
		/// <returns>The id of the employee type.</returns>
		public int GetEmployeeTypeIdByTypeName(string employeeType)
		{
			DynamicParameters param = new DynamicParameters();
			param.Add("@EmployeeType", employeeType);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>("[Auth].[GetEmployeeTypeId]", param, commandType: CommandType.StoredProcedure).SingleOrDefault();
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
			parameters.Add("@OrgRole", invitation.OrgRoleId);
			parameters.Add("@retId", -1, DbType.Int32, direction: ParameterDirection.Output);
			parameters.Add("@EmployeeId", invitation.EmployeeId);
			parameters.Add("@EmployeeTypeId", invitation.EmployeeTypeId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[CreateUserInvitation]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@retId");
		}

		/// <summary>
		/// Adds an Invitation to the invitations table and invitation sub roles table.
		/// </summary>
		/// <param name="invitingUserId">The id of the user sending the invitation.</param>
		/// <param name="invitation">A representation of the invitation to create.</param>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="productRoleId">The product role id.</param>
		/// <returns>The id of the newly created invitation (or -1 if the employee id is taken), and the
		/// first and last name of the inviting user</returns>
		public Tuple<int, string, string> CreateInvitation(int invitingUserId, InvitationDBEntity invitation, int? subscriptionId, int? productRoleId)
		{
			if (invitation == null)
			{
				throw new ArgumentException("invitation cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserId", invitingUserId);
			parameters.Add("@Email", invitation.Email);
			parameters.Add("@FirstName", invitation.FirstName);
			parameters.Add("@LastName", invitation.LastName);
			parameters.Add("@organizationID", invitation.OrganizationId);
			parameters.Add("@AccessCode", invitation.AccessCode);
			parameters.Add("@OrgRole", invitation.OrgRoleId);
			parameters.Add("@retId", -1, DbType.Int32, direction: ParameterDirection.Output);
			parameters.Add("@EmployeeId", invitation.EmployeeId);
			parameters.Add("@SubscriptionId", subscriptionId);
			parameters.Add("@SubRoleId", productRoleId);
			parameters.Add("@EmployeeType", invitation.EmployeeTypeId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple("[Auth].[InviteUser]", parameters, commandType: CommandType.StoredProcedure);
				int inviteId = results.Read<int>().FirstOrDefault();
				if (inviteId < 0)
				{
					return new Tuple<int, string, string>(inviteId, inviteId == -1 ? "User is already in organization." : "Employee Id is taken.", null);
				}
				return Tuple.Create(
					inviteId,
					results.Read<string>().FirstOrDefault(),
					results.Read<string>().FirstOrDefault());
			}
		}

		/// <summary>
		/// Accepts a user invitation, creating records with appropriate roles for organization user,
		/// project user, and subscription user. Removes invitation and invitation sub roles on success.
		/// </summary>
		/// <param name="invitationId">Invitation Id.</param>
		/// <param name="userId">User Id for invited user.</param>
		/// <returns>On success, returns the name of the organization and the name of the organization role.
		/// If an error occurred, returns null.</returns>
		public Tuple<string, string> AcceptInvitation(int invitationId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@InvitationId", invitationId);
			parameters.Add("@CallingUserId", userId);
			using (var con = new SqlConnection(this.SqlConnectionString))
			{
				var results = con.QueryMultiple(
					"[Auth].[AcceptInvitation]",
					parameters,
					commandType: CommandType.StoredProcedure);
				if (results == null) return null;
				else
				{
					return Tuple.Create(
						results.Read<string>().FirstOrDefault(),
						results.Read<string>().FirstOrDefault());
				}
			}
		}

		/// <summary>
		/// Removes a user invitation and related invitation sub roles.
		/// </summary>
		/// <param name="invitationId">Invitation Id.</param>
		/// <param name="userId">User Id for invited user, or -1 to skip that check.</param>
		/// <returns>True for success, false for error.</returns>
		public bool RemoveInvitation(int invitationId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@InvitationId", invitationId);
			parameters.Add("@CallingUserId", userId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				int success = connection.Query<int>(
					"[Auth].[RemoveInvitation]",
					parameters,
					commandType: CommandType.StoredProcedure).FirstOrDefault();
				return success == 1;
			}
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

		///// <summary>
		///// Adds a single entry to the InvitationSubRole database.
		///// </summary>
		///// <param name="invitationId">The invitation Id.</param>
		///// <param name="subscriptionId">The subscription Id.</param>
		///// <param name="productRoleId">The Id of the product role.</param>
		//public void CreateInvitationSubRole(int invitationId, int subscriptionId, int productRoleId)
		//{
		//    DynamicParameters parameters = new DynamicParameters();
		//    parameters.Add("@InvitationId", invitationId);
		//    parameters.Add("@SubscriptionId", subscriptionId);
		//    parameters.Add("@ProductRoleId", productRoleId);

		//    using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//    {
		//        connection.Execute("[Auth].[CreateInvitationSubRole]", parameters, commandType: CommandType.StoredProcedure);
		//    }
		//}

		///// <summary>
		///// Deletes a single entry from the InvitationSubRole database.
		///// </summary>
		///// <param name="invitationId">The invitation Id.</param>
		///// <param name="subscriptionId">The subscription Id.</param>
		//public void DeleteInvitationSubRole(int invitationId, int subscriptionId)
		//{
		//    DynamicParameters parameters = new DynamicParameters();
		//    parameters.Add("@InvitationId", invitationId);
		//    parameters.Add("@SubscriptionId", subscriptionId);

		//    using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//    {
		//        connection.Execute("[Auth].[DeleteInvitationSubRole]", parameters, commandType: CommandType.StoredProcedure);
		//    }
		//}

		///// <summary>
		///// Gets the roles for each subscription the invited user has.
		///// </summary>
		///// <param name="invitationId">The invitation Id.</param>
		///// <returns>List of all roles.</returns>
		//public IEnumerable<InvitationSubRoleDBEntity> GetInvitationSubRolesByInvitationId(int invitationId)
		//{
		//    DynamicParameters parameters = new DynamicParameters();
		//    parameters.Add("@InvitationId", invitationId);
		//    using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//    {
		//        return connection.Query<InvitationSubRoleDBEntity>("[Auth].[GetInvitationSubRolesByInvitationId]", parameters, commandType: CommandType.StoredProcedure);
		//    }
		//}

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
		/// Gets the user roles.
		/// </summary>
		/// <param name="orgid">The id of the relevant organization.</param>
		/// <returns>An UserRoles object.</returns>
		public IEnumerable<UserRolesDBEntity> GetRoles(int orgid)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrgId", orgid);
			using (var con = new SqlConnection(this.SqlConnectionString))
			{
				return con.Query<UserRolesDBEntity>(
					"[Auth].[GetRolesAndPermissions]",
					parameters,
					commandType: CommandType.StoredProcedure);
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
				return Tuple.Create(
					results.Read<UserDBEntity>().SingleOrDefault(),
					results.Read<OrganizationDBEntity>().ToList(),
					results.Read<InvitationDBEntity>().ToList());
			}
		}

		/// <summary>
		/// Returns an OrganizationDBEntity for the given organization, along with a list of OrganizationUserDBEntities for the organization users
		/// in the organization, a list of SubscriptionDisplayDBEntities for any subscriptions for the organization, a list of InvitiationDBEntities
		/// for any invitations pending in the organization, the organization's billing stripe handle, and the complete list of products.
		/// </summary>
		/// <param name="organizationId">The organization Id</param>
		public Tuple<OrganizationDBEntity, List<OrganizationUserDBEntity>, List<SubscriptionDisplayDBEntity>, List<InvitationDBEntity>, string, List<ProductDBEntity>> GetOrganizationManagementInfo(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", organizationId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[Auth].[GetOrgManagementInfo]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<OrganizationDBEntity>().SingleOrDefault(),
					results.Read<OrganizationUserDBEntity>().ToList(),
					results.Read<SubscriptionDisplayDBEntity>().ToList(),
					results.Read<InvitationDBEntity>().ToList(),
					results.Read<string>().SingleOrDefault(),
					results.Read<ProductDBEntity>().ToList());
			}
		}

		/// <summary>
		/// Returns an OrganizationDBEntity, the list of valid countries, and the employee id of the given employee in
		/// the given organization.
		/// </summary>
		/// <param name="orgId">Organization id.</param>
		/// <param name="userId">User id.</param>
		/// <returns></returns>
		public Tuple<OrganizationDBEntity, List<string>, string> GetOrgWithCountriesAndEmployeeId(int orgId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", orgId);
			parameters.Add("@UserId", userId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[Auth].[GetOrgWithCountriesAndEmployeeId]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<OrganizationDBEntity>().SingleOrDefault(),
					results.Read<string>().ToList(),
					results.Read<string>().SingleOrDefault());
			}
		}

		/// <summary>
		/// Returns the next recommended employee id by existing employees, a list of SubscriptionDisplayDBEntities for subscriptions in
		/// the organization, a list of SubscriptionRoleDBEntities for roles within the subscriptions of the organization,
		/// a list of CompleteProjectDBEntityies for TimeTracker projects in the organization, and the next recommended employee id by
		/// invitations.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns></returns>
		public Tuple<string, List<SubscriptionDisplayDBEntity>, List<SubscriptionRoleDBEntity>, List<CompleteProjectDBEntity>, string> GetAddMemberInfo(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", orgId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[Auth].[GetAddMemberInfo]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<string>().SingleOrDefault(),
					results.Read<SubscriptionDisplayDBEntity>().ToList(),
					results.Read<SubscriptionRoleDBEntity>().ToList(),
					results.Read<CompleteProjectDBEntity>().ToList(),
					results.Read<string>().SingleOrDefault());
			}
		}

		/// <summary>
		/// Returns a list of UserRolesDBEntities for users in the organization and their roles/subscription roles,
		/// and a list of SubscriptionDBEntites (with only SubscriptionId, ProductId, and ProductName populated) for
		/// all subscriptions in the organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns></returns>
		public Tuple<List<UserRolesDBEntity>, List<SubscriptionDisplayDBEntity>> GetOrgAndSubRoles(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", orgId);
			using (var con = new SqlConnection(this.SqlConnectionString))
			{
				var results = con.QueryMultiple(
					"[Auth].[GetOrgAndSubRoles]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<UserRolesDBEntity>().ToList(),
					results.Read<SubscriptionDisplayDBEntity>().ToList());
			}
		}

		/// <summary>
		/// Updates user's EmailConfirmed to True in Auth.User table.
		/// </summary>
		/// <param name = "userId">Target user's ID.</param>
		/// <param name = "code">The email confirmation code.</param>
		/// <returns>Return true for success, false if userId is not found or code does not match or other error</returns>
		public bool UpdateEmailConfirmed(int userId, string code)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userID", userId);
			parameters.Add("@confirmCode", code);
			string result = null;
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				result = connection.Query<string>(
					"[Auth].[UpdateEmailConfirmed]",
					parameters,
					commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
			return result == null ? false : true;
		}
	}
}
