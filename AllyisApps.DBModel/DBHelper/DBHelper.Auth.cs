//------------------------------------------------------------------------------
// <copyright file="DBHelper.Auth.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// DBHelper Partial.
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// create a new user
		/// </summary>
		public async Task<int> CreateUserAsync(
			string email,
			string passwordHash,
			string firstName,
			string lastName,
			Guid emailConfirmationCode,
			DateTime dateOfBirth,
			string phoneNumber,
			string preferredLanguageId,
			string address1,
			string address2,
			string city,
			int? stateId,
			string postalCode,
			string countryCode)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@email", email);
			parameters.Add("@passwordHash", passwordHash);
			parameters.Add("@firstName", firstName);
			parameters.Add("@lastName", lastName);
			parameters.Add("@emailConfirmationCode", emailConfirmationCode);
			parameters.Add("@dateOfBirth", dateOfBirth);
			parameters.Add("@phoneNumber", phoneNumber);
			parameters.Add("@preferredLanguageId", preferredLanguageId);
			parameters.Add("@address1", address1);
			parameters.Add("@address2", address2);
			parameters.Add("@city", city);
			parameters.Add("@stateId", stateId);
			parameters.Add("@postalCode", postalCode);
			parameters.Add("@countryCode", countryCode);

			using (var con = new SqlConnection(SqlConnectionString))
			{
				return (await con.QueryAsync<int>("[Auth].[CreateUser]", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
			}
		}

		/// <summary>
		/// get user entity for the given email
		/// </summary>
		public async Task<UserDBEntity> GetUserByEmailAsync(string email)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return (await connection.QueryAsync<UserDBEntity>("[Auth].[GetUserByEmail] @a", new { a = email })).FirstOrDefault();
			}
		}

		/// <summary>
		/// get a list of organizations that user is member of. (note: the organization has to be active)
		/// </summary>
		public async Task<dynamic> GetUserOrganizationsAsync(int userId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var result = await connection.QueryAsync<dynamic>("[Auth].[GetUserOrganizations] @a", new { a = userId });
				return result.ToList();
			}
		}

		/// <summary>
		/// Gets the product role for a user.
		/// </summary>
		/// <param name="subscriptionId">The subscription that the user belongs to.</param>
		/// <param name="userId">User Id.</param>
		/// <returns>The product role id for the user in the subscription. Returns null if user is not in the subscription or the subscription is inactive.</returns>
		public async Task<int?> GetSubscriptionRoleForUser(int subscriptionId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@subscriptionId", subscriptionId);
			parameters.Add("@userId", userId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return (await connection.QueryAsync<int>("[Auth].[GetSubscriptionRoleForUser]", parameters, commandType: CommandType.StoredProcedure)).SingleOrDefault();
			}
		}

		/// <summary>
		/// Get user from the db
		/// </summary>
		public async Task<dynamic> GetUser(int userId)
		{
			dynamic result = new ExpandoObject();
			using (var con = new SqlConnection(SqlConnectionString))
			{
				var res = await con.QueryMultipleAsync("[Auth].[GetUser] @a", new { a = userId });
				result.User = res.Read().FirstOrDefault();
				result.Organizations = res.Read().ToList();
				result.Subscriptions = res.Read().ToList();
				result.Invitations = res.Read().ToList();
			}

			return result;
		}

		/// <summary>
		/// Get user from the db
		/// </summary>
		public async Task<UserDBEntity> GetUser2Async(int userId)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return (await con.QueryAsync<UserDBEntity>("[Auth].[GetUser2] @a", new { a = userId })).FirstOrDefault();
			}
		}

		/// <summary>
		///  update the given user profile
		/// </summary>
		public async Task UpdateUserProfile(
			int userId,
			string firstName,
			string lastName,
			DateTime? dateOfBirth,
			string phoneNumber,
			int? addressId,
			string address1,
			string address2,
			string city,
			int? stateId,
			string postalCode,
			string countryCode)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@firstName", firstName);
			parameters.Add("@lastName", lastName);
			parameters.Add("@dateOfBirth", dateOfBirth);
			parameters.Add("@phoneNumber", phoneNumber);
			parameters.Add("@addressId", addressId);
			parameters.Add("@address1", address1);
			parameters.Add("@address2", address2);
			parameters.Add("@city", city);
			parameters.Add("@stateId", stateId);
			parameters.Add("@postalCode", postalCode);
			parameters.Add("@countryCode", countryCode);

			using (var con = new SqlConnection(SqlConnectionString))
			{
				await con.ExecuteAsync("[Auth].[UpdateUserProfile]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// update employee id and org role for the given user in the given organization
		/// </summary>
		public async Task<int> UpdateEmployeeIdAndOrgRole(int orgId, int userId, string employeeId, int orgRoleId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgId);
			parameters.Add("@userId", userId);
			parameters.Add("@employeeId", employeeId);
			parameters.Add("@orgRoleId", orgRoleId);

			using (var con = new SqlConnection(SqlConnectionString))
			{
				return (await con.QueryAsync<int>("[Auth].[UpdateEmployeeIdAndOrgRole]", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
			}
		}

		/// <summary>
		/// Updates the preferred language for a user.
		/// </summary>
		/// <param name="userId">Target user's Id.</param>
		/// <param name="CultureName">Language Id.</param>
		public void UpdateUserLanguagePreference(int userId, string CultureName)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@id", userId);
			parameters.Add("@CultureName", CultureName);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
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
		public async Task<IEnumerable<UserDBEntity>> GetUsersWithSubscriptionToProductInOrganization(int organizationId, int productId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@productId", productId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.QueryAsync<UserDBEntity>("[Auth].[GetUsersWithSubscriptionToProductInOrganization]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates the max amount a user can approve of in a report.
		/// </summary>
		/// <param name="orgUser"></param>
		public async Task UpdateUserMaxAmount(OrganizationUserDBEntity orgUser)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", orgUser.UserId);
			parameters.Add("@orgId", orgUser.OrganizationId);
			parameters.Add("@maxAmount", orgUser.MaxAmount);

			using (var connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync("[Auth].[UpdateUserMaxAmount]", parameters, commandType: CommandType.StoredProcedure);
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
			parameters.Add("@userId", userId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
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
		public async Task UpdateUserPasswordAsync(int userId, string passwordHash)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@passwordHash", passwordHash);

			using (var connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync("[Auth].[UpdateUserPassword]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates new password in Auth.User table. Requires proper reset code. Returns the number of rows updated.
		/// </summary>
		public async Task<int> UpdateUserPasswordUsingCode(string passwordHash, Guid code)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return (await con.QueryAsync<int>("[Auth].[UpdateUserPasswordUsingCode] @a, @b", new { a = passwordHash, b = code })).FirstOrDefault();
			}
		}

		/// <summary>
		/// Updates password reset code in Auth.User table.
		/// </summary>
		/// <param name = "email">Target user's email address.</param>
		/// <param name = "resetCode">The resetCode.</param>
		/// <returns>number of rows updated.</returns>
		public async Task<int> UpdateUserPasswordResetCode(string email, string resetCode)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return (await con.QueryAsync<int>("[Auth].[UpdateUserPasswordResetCode] @a, @b", new { a = email, b = resetCode })).FirstOrDefault();
			}
		}

		/// <summary>
		/// create a new organization, add the user in the given role
		/// </summary>
		public async Task<int> SetupOrganization(
			int userId,
			int roleId,
			string employeeId,
			string organizationName,
			string phoneNumber,
			string faxNumber,
			string siteUrl,
			string subDomainName,
			string address1,
			string city,
			int? stateId,
			string postalCode,
			string countryCode)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@roleId", roleId);
			parameters.Add("@organizationName", organizationName);
			parameters.Add("@siteUrl", siteUrl);
			parameters.Add("@address", address1);
			parameters.Add("@city", city);
			parameters.Add("@stateID", stateId);
			parameters.Add("@countryCode", countryCode);
			parameters.Add("@postalCode", postalCode);
			parameters.Add("@phoneNumber", phoneNumber);
			parameters.Add("@faxNumber", faxNumber);
			parameters.Add("@subdomainName", subDomainName);
			parameters.Add("@employeeId", employeeId);

			using (var con = new SqlConnection(SqlConnectionString))
			{
				var results = await con.QueryAsync<int>("[Auth].[SetupOrganization]", parameters, commandType: CommandType.StoredProcedure);
				return results.SingleOrDefault();
			}
		}

		/// <summary>
		/// Updates the specified organization with new information.
		/// </summary>
		public async Task<int> UpdateOrganization(
			int organizationId,
			string organizationName,
			string siteUrl,
			int? addressId,
			string address1,
			string city,
			int? stateId,
			string countryCode,
			string postalCode,
			string phoneNumber,
			string faxNumber,
			string subDomain)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@organizationName", organizationName);
			parameters.Add("@siteUrl", siteUrl);
			parameters.Add("@addressId", addressId);
			parameters.Add("@address1", address1);
			parameters.Add("@city", city);
			parameters.Add("@stateId", stateId);
			parameters.Add("@countryCode", countryCode);
			parameters.Add("@postalCode", postalCode);
			parameters.Add("@phoneNumber", phoneNumber);
			parameters.Add("@faxNumber", faxNumber);
			parameters.Add("@subdomainName", subDomain);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.ExecuteAsync("[Auth].[UpdateOrganization]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Executes [Auth].[DeleteOrg].
		/// </summary>
		/// <param name="organizationId">Parameter @organizationId. .</param>
		public async Task DeleteOrganization(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", organizationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync("[Auth].[DeleteOrg]", parameters, commandType: CommandType.StoredProcedure);
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
			parameters.Add("@employeeTypeId", organizationUser.EmployeeTypeId);
			parameters.Add("@employeeId", organizationUser.EmployeeId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[Auth].[CreateOrganizationUser]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates an organization member's info.
		/// </summary>
		public async Task<int> UpdateMember(int userId, int orgId, string employeeId, int roleId, string firstName, string lastName, bool isInvited)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@orgId", orgId);
			parameters.Add("@employeeId", employeeId);
			parameters.Add("@employeeRoleId", roleId);
			parameters.Add("@firstName", firstName);
			parameters.Add("@lastName", lastName);
			parameters.Add("@isInvited", isInvited);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.QueryFirstOrDefaultAsync<int>("[Auth].[UpdateMember]", parameters, commandType: CommandType.StoredProcedure);
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

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
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
		public async Task<int> DeleteOrganizationUsers(List<int> userIds, int organizationId)
		{
			DataTable userIdsTable = new DataTable();
			userIdsTable.Columns.Add("userId", typeof(int));
			foreach (int userId in userIds) { userIdsTable.Rows.Add(userId); }

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userIds", userIdsTable.AsTableValuedParameter("[Auth].[UserTable]"));
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.QueryFirstAsync<int>("[Auth].[DeleteOrganizationUsers]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Returns the organization information.
		/// </summary>
		/// <param name="organizationId">The organization's Id.</param>
		/// <returns>The TableOrganizations containing the organization's information, null if an error is encountered.</returns>
		public async Task<dynamic> GetOrganization(int organizationId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.QueryFirstOrDefaultAsync<dynamic>("[Auth].[GetOrganization]", new { organizationId }, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves the list of members for the specified organization.
		/// </summary>
		/// <param name="organizationId">The organization's Id.</param>
		/// <returns>The collection of users in the organization, null on error.</returns>
		public IEnumerable<OrganizationUserDBEntity> GetOrganizationMemberList(int organizationId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<OrganizationUserDBEntity>("[Auth].[GetOrgUserList]", new { organizationId }, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// get the list of users in the given organization
		/// </summary>
		public async Task<List<dynamic>> GetOrganizationUsersAsync(int organizationId)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return (await con.QueryAsync<dynamic>("[Auth].[GetOrganizationUsers]", new { organizationId }, commandType: CommandType.StoredProcedure)).ToList();
			}
		}

		/// <summary>
		/// get the count of organization invitations
		/// </summary>
		public async Task<int> GetOrganizationInvitationCountAsync(int orgId, int statusMask)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return await con.QueryFirstOrDefaultAsync<int>("[Auth].[GetOrgInvitationCount]", new { orgId, statusMask }, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		///  Gets [Auth].[GetOrganizationsByUserId].
		/// </summary>
		/// <param name="userId">Parameter @userId.</param>
		/// <returns>A list of OrganizationDBEntities containing the organizations the user is subscribed to.</returns>
		public IEnumerable<dynamic> GetOrganizationsByUserId(int userId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<dynamic>("[Auth].[GetOrganizationsByUserId]", new { userId }, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// get the list of organization db entities for the given ids
		/// </summary>
		public async Task<List<OrganizationDBEntity>> GetActiveOrganizationsByIdsAsync(List<int> ids)
		{
			List<OrganizationDBEntity> result = new List<OrganizationDBEntity>();
			if (ids.Count > 0)
			{
				StringBuilder sb = new StringBuilder();
				for (int i = ids.Count - 1; i > 0; i--)
				{
					sb.Append(ids[i]);
					sb.Append(",");
				}

				sb.Append(ids[0]);

				using (SqlConnection con = new SqlConnection(SqlConnectionString))
				{
					result = (await con.QueryAsync<OrganizationDBEntity>("[Auth].[GetActiveOrganizationsByIds] @a", new { a = sb.ToString() })).ToList();
				}
			}

			return result;
		}

		/// <summary>
		/// get the name of the given organization
		/// </summary>
		public async Task<string> GetActiveOrganizationName(int orgId)
		{
			using (SqlConnection con = new SqlConnection(SqlConnectionString))
			{
				return (await con.QueryAsync<string>("[Auth].[GetActiveOrganizationName] @a", new { a = orgId })).FirstOrDefault();
			}
		}

		/// <summary>
		/// Adds an Invitation to the invitations table and invitation sub roles table.
		/// </summary>
		public async Task<int> CreateInvitation(
			string email,
			string firstName,
			string lastName,
			int organizationId,
			int organizationRoleId,
			int employeeTypeId,
			string employeedId,
			string prodJson)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@email", email);
			parameters.Add("@firstName", firstName);
			parameters.Add("@lastName", lastName);
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@organizationRole", organizationRoleId);
			parameters.Add("@employeeTypeId", employeeTypeId);
			parameters.Add("@employeeId", employeedId);
			parameters.Add("@prodJson", prodJson);
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return await con.QueryFirstOrDefaultAsync<int>("[Auth].[CreateInvitation]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Accepts a user invitation, creating records with appropriate roles for organization user,
		/// project user, and subscription user. Removes invitation and invitation sub roles on success.
		/// </summary>
		/// <param name="invitationId">Invitation Id.</param>
		/// <param name="userId">User Id for invited user.</param>
		/// <returns>true or false based on the number of rows affected</returns>
		public int AcceptInvitation(int invitationId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@invitationId", invitationId);
			parameters.Add("@callingUserId", userId);
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return con.QueryFirstOrDefault<int>("[Auth].[AcceptInvitation]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Removes a user invitation and related invitation sub roles.
		/// </summary>
		/// <param name="invitationId">Invitation Id.</param>
		/// <returns>True for success, false for error.</returns>
		public bool DeleteInvitation(int invitationId)
		{
			var result = false;
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@invitationId", invitationId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				int affected = connection.Execute(
					"[Auth].[DeleteInvitation]",
					parameters,
					commandType: CommandType.StoredProcedure);
				result = affected == 1;
			}

			return result;
		}

		/// <summary>
		/// Deletes the defined invitation.
		/// </summary>
		/// <param name="invitationId">The invitation's Id.</param>
		public async Task<int> RejectInvitation(int invitationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@invitationId", invitationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<int>("[Auth].[RejectInvitation]", parameters, commandType: CommandType.StoredProcedure);
				return results.FirstOrDefault();
			}
		}

		/// <summary>
		/// Gets all the invitations for the given organization
		/// </summary>
		public async Task<List<InvitationDBEntity>> GetInvitationsAsync(int organizationId, int statusMask)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return (await con.QueryAsync<InvitationDBEntity>("[Auth].[GetInvitations]", new { organizationId, statusMask }, commandType: CommandType.StoredProcedure)).ToList();
			}
		}

		/// <summary>
		/// get all the invitations for the given email
		/// </summary>
		public async Task<List<InvitationDBEntity>> GetInvitationsByEmailAsync(string email, int statusMask)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return (await con.QueryAsync<InvitationDBEntity>("[Auth].[GetInvitationsByEmail] @a, @b", new { a = email, b = statusMask })).ToList();
			}
		}

		/// <summary>
		/// Gets the roles for each subscription the organization has.
		/// </summary>
		/// <param name="inviteId">The invite Id.</param>
		/// <returns>List of all roles.</returns>
		public async Task<InvitationDBEntity> GetInvitation(int inviteId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<InvitationDBEntity>("[Auth].[GetInvitation]", new { inviteId }, commandType: CommandType.StoredProcedure);
				return results.FirstOrDefault();
			}
		}

		/// <summary>
		/// get context for the given user
		/// </summary>
		public dynamic GetUserContext(int userId)
		{
			dynamic result = new ExpandoObject();
			using (var con = new SqlConnection(SqlConnectionString))
			{
				var query = con.QueryMultiple("[Auth].[GetUserContext]", new { userId }, commandType: CommandType.StoredProcedure);
				result.User = query.Read<dynamic>().FirstOrDefault();
				result.OrganizationsAndRoles = query.Read<dynamic>().ToList();
				result.SubscriptionsAndRoles = query.Read<dynamic>().ToList();
			}

			return result;
		}

		/// <summary>
		/// Returns an OrganizationDBEntity for the given organization, along with a list of OrganizationUserDBEntities for the organization users
		/// in the organization, a list of SubscriptionDisplayDBEntities for any subscriptions for the organization, a list of InvitiationDBEntities
		/// for any invitations pending in the organization, the organization's billing stripe handle, and the complete list of products.
		/// </summary>
		/// <param name="organizationId">The organization Id.</param>
		public async Task<Tuple<dynamic, List<OrganizationUserDBEntity>, List<SubscriptionDisplayDBEntity>, List<InvitationDBEntity>, string>> GetOrganizationManagementInfo(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryMultipleAsync("[Auth].[GetOrgManagementInfo]", parameters, commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<dynamic>().SingleOrDefault(),
					results.Read<OrganizationUserDBEntity>().ToList(),
					results.Read<SubscriptionDisplayDBEntity>().ToList(),
					results.Read<InvitationDBEntity>().ToList(),
					results.Read<string>().SingleOrDefault());
			}
		}

		/// <summary>
		/// Updates user's IsEmailConfirmed to True in Auth.User table.
		/// </summary>
		public int UpdateEmailConfirmed(Guid code)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@emailConfirmCode", code);

			using (var con = new SqlConnection(SqlConnectionString))
			{
				return con.Execute("[Auth].[UpdateEmailConfirmed]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// get the max employee id from the invitation table
		/// </summary>
		public async Task<string> GetMaxEmployeeId(int organizationId)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return await con.QuerySingleOrDefaultAsync<string>("[Auth].[GetMaxEmployeeId]", new { organizationId }, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// get the product roles for the given org and given product
		/// </summary>
		public async Task<List<ProductRoleDBEntity>> GetProductRolesAsync(int orgId, int productId)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return (await con.QueryAsync<ProductRoleDBEntity>("[Auth].[GetProductRoles]", new { orgId, productId }, commandType: CommandType.StoredProcedure)).ToList();
			}
		}
	}
}
