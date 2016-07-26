//------------------------------------------------------------------------------
// <copyright file="BusinessObjectsHelper.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

////using System;
////using System.Collections.Generic;
////using System.Data;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using System.Data.SqlClient;
////using System.Configuration;
////using Dapper;
////using System.Diagnostics;

////namespace AllyisApps.DBModel.BusinessObjects
////{
//    /// <summary>
//    /// helper class containing functionality for manipulating business objects
//    /// </summary>
//    public class BusinessObjectsHelper
//    {
//        /// <summary>
//        /// Singleton instance.
//        /// </summary>
//        public static readonly BusinessObjectsHelper Instance = new BusinessObjectsHelper();

////        /// <summary>
//		/// Gets the connection string to the backing database.
//		/// </summary>
//        public string SqlConnectionString { get; private set; }

////        /// <summary>
//        /// private constructor
//        /// </summary>
//        private BusinessObjectsHelper()
//        {
//        }

////        /// <summary>
//        /// Initializes the business objects helper.
//        /// </summary>
//        /// <param name="key">The key of the connection strings configuration.</param>
//        public void Init(string key)
//        {
//            SqlConnectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
//        }

////        /// <summary>
//        /// Gets a list of membership information for the given user.
//        /// </summary>
//        public List<UserMembershipInfo> GetUserMembershipInfoList(int userId)
//        {
//            using (var conn = new SqlConnection(SqlConnectionString))
//            {
//                return conn.Query<UserMembershipInfo>("[Auth].GetUserMembershipInfoList @a", new { a = userId }).ToList();
//            }
//        }

////        /// <summary>
//		/// Gets a project from its id.
//		/// </summary>
//		/// <param name="projectId">The project's Id.</param>
//		/// <returns>Info about the requested project.</returns>
//		public ProjectInfo GetProjectById(int projectId)
//        {
//            DynamicParameters parameters = new DynamicParameters();
//            parameters.Add("@ProjectId", projectId);

////            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                return connection.Query<ProjectInfo>(
//                    "[Crm].[GetProjectById]",
//                    parameters,
//                    commandType: CommandType.StoredProcedure).SingleOrDefault();
//            }
//        }

////        /// <summary>
//		/// Gets all the projects a user can use in an organization.
//		/// </summary>
//		/// <param name="userId">The User's Id</param>
//		/// <param name="orgId">The organization's Id</param>
//		/// <returns>A collection of ProjectInfo objects for each project the user has access to within the organization.</returns>
//		public IEnumerable<ProjectInfo> GetProjectsByUserAndOrganization(int userId, int orgId)
//        {
//            return GetProjectsByUserAndOrganization(userId, orgId, 1);
//        }

////        /// <summary>
//		/// Gets all the projects a user can use in an organization.
//		/// </summary>
//		/// <param name="userId">The user's Id</param>
//		/// <param name="orgId">The organization's Id.</param>
//		/// <param name="activity">The level of activity you wish to allow. Specifying 0 includes inactive projects.</param>
//		/// <returns>A collection of ProjectInfo objects for each project the user has access to within the organization.</returns>
//		public IEnumerable<ProjectInfo> GetProjectsByUserAndOrganization(int userId, int orgId, int activity)
//        {
//            DynamicParameters parameters = new DynamicParameters();
//            parameters.Add("@UserId", userId);
//            parameters.Add("@OrgId", orgId);
//            parameters.Add("@Activity", activity);
//            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                return connection.Query<ProjectInfo>(
//                    "[Crm].[GetProjectsByUserAndOrganization]",
//                    parameters,
//                     commandType: CommandType.StoredProcedure);
//            }
//        }

////        /// <summary>
//		/// Gets all projects from all customers in an organization.
//		/// </summary>
//		/// <param name="orgId">The organization's Id.</param>
//		/// <param name="activity">The level of activity you wish to allow. Specifying 0 includes inactive projects.</param>
//		/// <returns>A collection of ProjectInfo objects for each project within the organization.</returns>
//		public IEnumerable<ProjectInfo> GetProjectsByOrgId(int orgId, int activity = 1)
//        {
//            DynamicParameters parameters = new DynamicParameters();
//            parameters.Add("@OrgId", orgId);
//            parameters.Add("@Activity", activity);
//            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                return connection.Query<ProjectInfo>(
//                    "[Crm].[GetProjectsByOrgId]",
//                    parameters,
//                    commandType: CommandType.StoredProcedure);
//            }
//        }

////        /// <summary>
//		/// Gets a Collection of project data that the user can use.
//		/// </summary>
//		/// <param name="userId">The User's Id.</param>
//		/// <returns>Collection of project data</returns>
//		public IEnumerable<ProjectInfo> GetProjectsByUserId(int userId)
//        {
//            DynamicParameters parameters = new DynamicParameters();
//            parameters.Add("@UserId", userId);
//            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                return connection.Query<ProjectInfo>("[Crm].[GetProjectsByUserId]",
//                                             parameters,
//                                             commandType: CommandType.StoredProcedure);
//            }
//        }

////        /// <summary>
//		/// Returns the organization information crossed with subscription and role information, from a user id.
//		/// </summary>
//		public IEnumerable<SubscriptionDisplay> GetUserSubscriptionOrganizationList(int userId)
//        {
//            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                //default null
//                return (IEnumerable<SubscriptionDisplay>)connection.Query<SubscriptionDisplay>(
//                    "[Billing].[GetSubscriptionDetailsByUser]",
//                    new
//                    {
//                        userId = userId
//                    },
//                    commandType: CommandType.StoredProcedure);
//            }
//        }

////        /// <summary>
//		/// Executes [Billing].[GetSubscriptionsDisplayByOrg].
//		/// </summary>
//		/// <param name="organizationId">Sets OrganizationId.</param>
//		/// <returns>List of SubscriptionDisplay's.</returns>
//		public IEnumerable<SubscriptionDisplay> GetSubscriptionsDisplayByOrg(int organizationId)
//        {
//            DynamicParameters parameters = new DynamicParameters();
//            parameters.Add("@OrganizationId", organizationId);
//            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                //default empty list
//                return connection.Query<SubscriptionDisplay>("[Billing].[GetSubscriptionsDisplayByOrg]", parameters, commandType: CommandType.StoredProcedure);
//            }
//        }

////        /// <summary>
//		/// Executes Billing.GetProductRolesFromSubscription.
//		/// </summary>
//		/// <param name="subscriptionId">Sets SubscriptionId.</param>
//		/// <returns>List of SubscriptionRole.</returns>
//		public IEnumerable<SubscriptionRole> GetProductRolesFromSubscription(int subscriptionId)
//        {
//            DynamicParameters parameters = new DynamicParameters();
//            parameters.Add("@SubscriptionId", subscriptionId);
//            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                //default null
//                return connection.Query<SubscriptionRole>("[Billing].[GetProductRolesFromSubscription]", parameters, commandType: CommandType.StoredProcedure);
//            }
//        }

////        /// <summary>
//		/// Gets all of the subscription permissions for the defined user.
//		/// </summary>
//		/// <param name="userId">The defined user's Id.</param>
//		/// <returns>A collection of subscription permissions for the requested user.</returns>
//		public IEnumerable<TableSubscriptionPermissions> GetUserSubscriptionPermissions(int userId)
//        {
//            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                DynamicParameters parameters = new DynamicParameters();
//                parameters.Add("@UserId", userId);
//                return connection.Query<TableSubscriptionPermissions>("[Auth].[GetUserSubscriptionPermissions]", parameters, commandType: CommandType.StoredProcedure);
//            }
//        }

////        /// <summary>
//		/// Gets a collection of project permissions for a given user. 
//		/// </summary>
//		/// <param name="userId">The id of the user in question.</param>
//		/// <returns>A list of permissions. </returns>
//		public IEnumerable<TableSubscriptionPermissions> GetUserProjectPermissions(int userId)
//        {
//            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                DynamicParameters parameters = new DynamicParameters();
//                parameters.Add("@UserId", userId);
//                return connection.Query<TableSubscriptionPermissions>("[Auth].[GetUserProjectPermissions]", parameters, commandType: CommandType.StoredProcedure);
//            }
//        }

////        /// <summary>
//		/// Gets a collection of organization permissions for a given user. 
//		/// </summary>
//		/// <param name="userId">The id of the user in question.</param>
//		/// <returns>A list of permissions. </returns>
//		public IEnumerable<TableOrganizationPermissions> GetUserOrganizationPermissions(int userId)
//        {
//            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                DynamicParameters parameters = new DynamicParameters();
//                parameters.Add("@UserId", userId);
//                return connection.Query<TableOrganizationPermissions>("[Auth].[GetUserOrganizationPermissions]", parameters, commandType: CommandType.StoredProcedure);
//            }
//        }

////        /// <summary>
//		/// Gets the user roles
//		/// </summary>
//		/// <param name="orgid">The id of the relevant organization</param>
//		/// <returns>An UserRoles object</returns>
//		public IEnumerable<UserRoles> GetRoles(int orgid)
//        {
//            DynamicParameters parameters = new DynamicParameters();
//            parameters.Add("@OrgId", orgid);
//            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                return connection.Query<UserRoles>("[Auth].[GetRolesAndPermissions]",
//                                             parameters,
//                                             commandType: CommandType.StoredProcedure);
//            }
//        }

////        /// <summary>
//		/// Gets last used organization and product off of email.
//		/// </summary>
//		/// <param name="email">User email.</param>
//		/// <returns>TableLast used object.</returns>
//		public LastUsed GetLast(string email)
//        {
//            Trace.WriteLine("GetLast");
//            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
//            {
//                DynamicParameters parameters = new DynamicParameters();
//                parameters.Add("@Email", email);
//                //default null
//                return connection.Query<LastUsed>("[Auth].[GetLast]", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
//            }
//        }
//    }
////}
