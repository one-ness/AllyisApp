//------------------------------------------------------------------------------
// <copyright file="DBHelper.Auth.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Crm;
using AllyisApps.DBModel.Lookup;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
		public int CreateUser(UserDBEntity user)
		{
			if (user == null)
			{
				throw new ArgumentException("user cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@email", user.Email);
			parameters.Add("@firstName", user.FirstName);
			parameters.Add("@lastName", user.LastName);
			parameters.Add("@address", user.Address);
			parameters.Add("@city", user.City);
			parameters.Add("@state", user.State);
			parameters.Add("@country", user.Country);
			parameters.Add("@postalCode", user.PostalCode);
			parameters.Add("@phoneNumber", user.PhoneNumber);
			parameters.Add("@dateOfBirth", user.DateOfBirth);
			parameters.Add("@emailConfirmationCode", user.EmailConfirmationCode);
			parameters.Add("@passwordHash", user.PasswordHash);
			parameters.Add("@isTwoFactorEnabled", user.IsTwoFactorEnabled);
			parameters.Add("@isLockoutEnabled", user.IsLockoutEnabled);
			parameters.Add("@lockoutEndDateUtc", user.LockoutEndDateUtc);
			parameters.Add("@languageId", user.PreferredLanguageId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>("Auth.CreateUser", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
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
		/// <param name="organizationId">The organization.</param>
		/// <param name="userId">The user.</param>
		/// <returns>The product rold for the user.</returns>
		public string GetProductRoleForUser(string productName, int organizationId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@productName", productName);
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@userId", userId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>("[Auth].[GetActiveProductRoleForUser]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault() ?? "None";
			}
		}

		/// <summary>
		/// Updates the user with the specified Id.
		/// </summary>
		/// <param name="user">The table with the user to create.</param>
		public void UpdateUser(UserDBEntity user)
		{
			if (user == null)
			{
				throw new ArgumentException("user cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();

			parameters.Add("@userId", user.UserId);
			parameters.Add("@addressId", user.AddressId);
			parameters.Add("@firstName", user.FirstName);
			parameters.Add("@lastName", user.LastName);
			parameters.Add("@address", user.Address);
			parameters.Add("@city", user.City);
			parameters.Add("@state", user.State);
			parameters.Add("@country", user.Country);
			parameters.Add("@postalCode", user.PostalCode);
			parameters.Add("@phoneNumber", user.PhoneNumber);
			parameters.Add("@dateOfBirth", user.DateOfBirth);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[UpdateUserInfo]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves the user's information from the database.
		/// </summary>
		/// <param name="userId">The user's Id.</param>
		/// <returns>The TableUsers containing the user's information, null if call fails.</returns>
		public Tuple<UserDBEntity, AddressDBEntity> GetUserInfo(int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserId", userId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple("[Auth].[GetUserInfo]", parameters, commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<UserDBEntity>().FirstOrDefault(),
					results.Read<AddressDBEntity>().FirstOrDefault());
			}
		}

		/// <summary>
		/// Populates a user's last used subscription.
		/// </summary>
		/// <param name = "userId">Target user's Id.</param>
		/// <param name = "subId">The subscription's Id.</param>
		public void UpdateActiveSubscription(int userId, int? subId = null)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@subscriptionId", subId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[UpdateUserActiveSub]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Populates a user's last used organization.
		/// </summary>
		/// <param name = "userId">Target user's Id.</param>
		/// <param name = "organizationId">The organization's Id.</param>
		public void UpdateActiveOrganization(int userId, int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[UpdateUserActiveOrg]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates the preferred language for a user.
		/// </summary>
		/// <param name="userId">Target user's Id.</param>
		/// <param name="languageId">Language Id.</param>
		public void UpdateUserLanguagePreference(int userId, int languageId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@id", userId);
			parameters.Add("@languageId", languageId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[UpdateUserLanguagePreference]", parameters, commandType: CommandType.StoredProcedure);
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
			parameters.Add("@organizationId", organizationId);
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
		/// <param name = "userId">Target user's Id.</param>
		/// <param name = "passwordHash">The new password hash.</param>
		/// <returns>The password hash retreived independently after the update, for verification.</returns>
		public void UpdateUserPassword(int userId, string passwordHash)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@passwordHash", passwordHash);

			using (var connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[UpdateUserPassword]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates new password in Auth.User table. Requires proper reset code. Returns the number of rows updated.
		/// </summary>
		public int UpdateUserPasswordUsingCode(string passwordHash, Guid code)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@passwordHash", passwordHash);
			parameters.Add("@code", code);

			using (var connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Execute("[Auth].[UpdateUserPasswordUsingCode]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates password reset code in Auth.User table.
		/// </summary>
		/// <param name = "email">Target user's email address.</param>
		/// <param name = "resetCode">The resetCode.</param>
		/// <returns>number of rows updated</returns>
		public int UpdateUserPasswordResetCode(string email, string resetCode)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@email", email);
			parameters.Add("@resetCode", resetCode);

			using (var connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Execute("[Auth].[UpdateUserPasswordResetCode]", parameters, commandType: CommandType.StoredProcedure);
			}
		}
	
		/// <summary>
		/// Adds an organization to the database and sets the owner's chosen organization to th
		/// </summary>
		/// <param name="organization">The OrganizationDBEntity to create.</param>
		/// <param name="ownerId">The owner's user Id.</param>
		/// <param name="roleId">The role associated with the creator of the organization.</param>
		/// <param name="employeeId">The employee Id for the user creating the organization.</param>
		/// <returns>The id of the created organization</returns>
		public int SetupOrganization(OrganizationDBEntity organization, int ownerId, int roleId, string employeeId)
		{
			if (organization == null)
			{
				throw new ArgumentException("organizationId cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", ownerId);
			parameters.Add("@roleId", roleId);
			parameters.Add("@organizationName", organization.OrganizationName);
			parameters.Add("@siteUrl", organization.SiteUrl);
			parameters.Add("@address", organization.Address);
			parameters.Add("@city", organization.City);
			parameters.Add("@state", organization.State);
			parameters.Add("@country", organization.Country);
			parameters.Add("@postalCode", organization.PostalCode);
			parameters.Add("@phoneNumber", organization.PhoneNumber);
			parameters.Add("@faxNumber", organization.FaxNumber);
			parameters.Add("@subdomain", organization.Subdomain);
			parameters.Add("@employeeId", employeeId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>("[Auth].[SetupOrganization]", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Updates the specified organization with new information.
		/// </summary>
		/// <param name="organization">The organization table with updates.</param>
		/// <returns>Number of rows changed</returns>
		public int UpdateOrganization(OrganizationDBEntity organization)
		{
			if (organization == null)
			{
				throw new ArgumentException("organization cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organization.OrganizationId);
			parameters.Add("@organizationName", organization.OrganizationName);
			parameters.Add("@siteUrl", organization.SiteUrl);
			parameters.Add("@addressId", organization.AddressId);
			parameters.Add("@address", organization.Address);
			parameters.Add("@city", organization.City);
			parameters.Add("@state", organization.State);
			parameters.Add("@country", organization.Country);
			parameters.Add("@postalCode", organization.PostalCode);
			parameters.Add("@phoneNumber", organization.PhoneNumber);
			parameters.Add("@faxNumber", organization.FaxNumber);
			parameters.Add("@subdomainName", organization.Subdomain);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Execute("[Auth].[UpdateOrganization]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Executes [Auth].[DeleteOrg].
		/// </summary>
		/// <param name="organizationId">Parameter @organizationId. </param>
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
		/// <param name="organizationUser">The organization's Id.</param>
		public void CreateOrganizationUser(OrganizationUserDBEntity organizationUser)
		{
			if (organizationUser == null)
			{
				throw new ArgumentException("orgUser cannot be null.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationUser.OrganizationId);
			parameters.Add("@userId", organizationUser.UserId);
			parameters.Add("@roleId", organizationUser.OrganizationRoleId);
			parameters.Add("@employeeId", organizationUser.EmployeeId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Auth].[CreateOrganizationUser]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets the employee id for a user for an org.
		/// </summary>
		/// <param name="userId">The Id of the user.</param>
		/// <param name="organizationId">The Id of the organization.</param>
		/// <returns>The employee id.</returns>
		public string GetEmployeeId(int userId, int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>("[Auth].[GetOrgUserEmployeeId]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Updates an organization member's info
		/// </summary>
		/// <param name="modelData">The data from the form that the service passed in</param>
		/// <returns>A 1 or 0 based on if the employeeId already exists or not</returns>
		public int UpdateMember(Dictionary<string, dynamic> modelData)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", modelData["userId"]);
			parameters.Add("@orgId", modelData["orgId"]);
			parameters.Add("@employeeId", modelData["employeeId"]);
			parameters.Add("@employeeRoleId", modelData["employeeRoleId"]);
			parameters.Add("@firstName", modelData["firstName"]);
			parameters.Add("@lastName", modelData["lastName"]);
			parameters.Add("@isInvited", modelData["isInvited"] ? 1 : 0);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>("[Auth].[UpdateMember]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Removes the specified user from the organization.
		/// </summary>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="userId">The user's Id.</param>
		public void RemoveOrganizationUser(int organizationId, int userId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("@organizationId", organizationId);
				parameters.Add("@userId", userId);

				connection.Execute("[Auth].[DeleteOrgUser]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Assigns an organization role to a list of users.
		/// </summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="organizationId">The Organization Id.</param>
		/// <param name="organizationRoleId">Organization role to assign (or -1 to remove from organization).</param>
		/// <returns>The number of affected users.</returns>
		public int UpdateOrganizationUsersRole(List<int> userIds, int organizationId, int organizationRoleId)
		{
			DataTable userIdsTable = new DataTable();
			userIdsTable.Columns.Add("userId", typeof(int));
			foreach (int userId in userIds) { userIdsTable.Rows.Add(userId); }

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userIds", userIdsTable.AsTableValuedParameter("[Auth].[UserTable]"));
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@organizationRole", organizationRoleId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Execute("[Auth].[UpdateOrganizationUsersRole]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Assigns an organization role to a list of users.
		/// </summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="organizationId">The Organization Id.</param>
		/// <returns>The number of affected users.</returns>
		public int DeleteOrganizationUsers(List<int> userIds, int organizationId)
		{
			DataTable userIdsTable = new DataTable();
			userIdsTable.Columns.Add("userId", typeof(int));
			foreach (int userId in userIds) { userIdsTable.Rows.Add(userId); }

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userIds", userIdsTable.AsTableValuedParameter("[Auth].[UserTable]"));
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Execute("[Auth].[DeleteOrganizationUsers]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves the specified user's permissions in the specified organization.
		/// </summary>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="userId">The user's Id.</param>
		/// <returns>The TableOrganizationRole related to the user for the specified organization.</returns>
		public OrganizationRoleDBEntity GetPermissionLevel(int organizationId, int userId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				return connection.Query<OrganizationRoleDBEntity>("[Auth].[GetOrgUserRole]", new { OrganizationId = organizationId, UserId = userId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Returns the organization information.
		/// </summary>
		/// <param name="organizationId">The organization's Id.</param>
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
		/// <param name="organizationId">The organization's Id.</param>
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
			parameters.Add("@organizationId", invitation.OrganizationId);
			parameters.Add("@AccessCode", invitation.AccessCode);
			parameters.Add("@OrganizationRole", invitation.OrganizationRoleId);
			parameters.Add("@retId", -1, DbType.Int32, direction: ParameterDirection.Output);
			parameters.Add("@EmployeeId", invitation.EmployeeId);
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
			parameters.Add("@organizationId", invitation.OrganizationId);
			parameters.Add("@AccessCode", invitation.AccessCode);
			parameters.Add("@OrganizationRole", invitation.OrganizationRoleId);
			parameters.Add("@retId", -1, DbType.Int32, direction: ParameterDirection.Output);
			parameters.Add("@EmployeeId", invitation.EmployeeId);
			parameters.Add("@SubscriptionId", subscriptionId);
			parameters.Add("@SubRoleId", productRoleId);
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
		/// <param name="organizationId">The organization id.</param>
		/// <param name="email">The user email.</param>
		/// <returns>The user first name.</returns>
		public string GetOrgUserFirstName(int organizationId, string email)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Email", email);
			parameters.Add("@OrgId", organizationId);

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
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<InvitationDBEntity>("[Auth].[GetUserInvitationsByOrgId]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets the roles for each subscription the organization has.
		/// </summary>
		/// <param name="inviteId">The invite Id.</param>
		/// <returns>List of all roles.</returns>
		public IEnumerable<InvitationDBEntity> GetUserInvitationsByInviteId(int inviteId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@InviteId", inviteId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<InvitationDBEntity>("[Auth].[GetUserInvitationsByInviteId]", parameters, commandType: CommandType.StoredProcedure);
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
		public Tuple<UserDBEntity, List<OrganizationDBEntity>, List<InvitationDBEntity>, AddressDBEntity> GetUserOrgsAndInvitations(int userId)
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
					results.Read<InvitationDBEntity>().ToList(),
					results.Read<AddressDBEntity>().SingleOrDefault());
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
		public Tuple<string, List<SubscriptionDisplayDBEntity>, List<SubscriptionRoleDBEntity>, List<ProjectDBEntity>, string> GetAddMemberInfo(int orgId)
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
					results.Read<ProjectDBEntity>().ToList(),
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
		/// Updates user's IsEmailConfirmed to True in Auth.User table.
		/// </summary>
		public int UpdateEmailConfirmed(Guid code)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@emailConfirmCode", code);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Execute("[Auth].[UpdateEmailConfirmed]", parameters, commandType: CommandType.StoredProcedure);
			}
		}
	}
}
