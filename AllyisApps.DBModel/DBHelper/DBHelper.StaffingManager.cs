//------------------------------------------------------------------------------
// <copyright file="DBHelper.Lookup.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using AllyisApps.DBModel.StaffingManager;
using System;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// DBHelper Partial.
	/// </summary>
	public partial class DBHelper
	{
		////////////////////////////
		/*         CREATE         */
		////////////////////////////

		/// <summary>
		/// Adds an account to the DB if there is not already another account with the same StaffingManagerName.
		/// </summary>
		/// <param name="account">The account object to be added to the db.</param>
		/// <returns>The id of the created account or -1 if the account name is already taken.</returns>
		public int CreateStaffingManager(dynamic account)
		{
			if (account == null)
			{
				throw new System.ArgumentException("StaffingManager cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@accountName", account.StaffingManagerName);
			parameters.Add("@isActive", account.IsActive);
			parameters.Add("@accountTypeId", account.StaffingManagerTypeId);
			parameters.Add("@parentStaffingManagerId", account.ParentStaffingManagerId);
			parameters.Add("@returnValue", -1, DbType.Int32, direction: ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				connection.Execute("[Finance].[CreateStaffingManager]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@returnValue");
		}


        /// <summary>
        /// Updates the position with the given id.
        /// </summary>
        /// <param name="position">The account object to be updated.</param>
        /// <returns>Returns the number of rows updated.</returns>
        public int CreatePosition(PositionDBEntity position)
        {
            if (position == null)
            {
                throw new System.ArgumentException("Position cannot be null or empty.");
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OrganizationId", position.OrganizationId);
            parameters.Add("@AddressId", position.AddressId);
            parameters.Add("@StartDate", position.StartDate);
            parameters.Add("@PositionStatus", position.PositionStatus);
            parameters.Add("@PositionTitle", position.PositionTitle);
            parameters.Add("@BillingRateFrequency", position.BillingRateFrequency);
            parameters.Add("@BillingRateAmount", position.BillingRateAmount);
            parameters.Add("@DurationMonths", position.DurationMonths);
            parameters.Add("@EmploymentType", position.EmploymentType);
            parameters.Add("@PositionCount", position.PositionCount);
            parameters.Add("@RequiredSkills", position.RequiredSkills);
            parameters.Add("@JobResponsiblities", position.JobResponsibilities);
            parameters.Add("@DesiredSkills", position.DesiredSkills);
            parameters.Add("@PositionLevel", position.PositionLevel);
            parameters.Add("@HiringManager", position.HiringManager);
            parameters.Add("@TeamName", position.TeamName);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                return connection.Query<int>("[StaffingManager].[UpdatePosition]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
		/// Adds an Tag to the DB if there is not already another tag with the same name.
		/// </summary>
		/// <param name="name">The name of the tag to be added to the db.</param>
		/// <returns>The id of the created Tag or -1 if the tag name is already in use.</returns>
		public int CreateTag(string name)
        {
            if (name == null)
            {
                throw new System.ArgumentException("Name cannot be null or empty.");
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TagName", name);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                // default -1
                connection.Execute("[StaffingManager].[Tag]", parameters, commandType: CommandType.StoredProcedure);
            }

            return parameters.Get<int>("@returnValue");
        }



        ////////////////////////////
        /*          READ          */
        ////////////////////////////

        /// <summary>
        /// Retrieves all acounts with a given parent account id.
        /// </summary>
        /// <param name="parentStaffingManagerId">The id of the parent account.</param>
        /// <param name="isActive">Determines whether the account is "deleted" or not.  Most times we only want active accounts, so default to true.</param>
        /// <returns>A collection of accounts.</returns>
        public IEnumerable<dynamic> GetStaffingManagersByParentId(int parentStaffingManagerId, bool isActive = true)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@parentStaffingManagerId", parentStaffingManagerId);
			parameters.Add("@isActive", isActive);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<dynamic>("[Finance].[GetStaffingManagersByParentId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable();
			}
		}

		/// <summary>
		/// Retrieves all acounts with a given account type.
		/// </summary>
		/// <param name="accountTypeId">The id of the account type (TODO: what are the different account types?).</param>
		/// <param name="isActive">Determines whether the account is "deleted" or not.  Most times we only want active accounts, so default to true.</param>
		/// <returns>A collection of accounts.</returns>
		public IEnumerable<dynamic> GetStaffingManagersByStaffingManagerTypeId(int accountTypeId, bool isActive = true)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@accountTypeId", accountTypeId);
			parameters.Add("@isActive", isActive);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<dynamic>("[Finance].[GetStaffingManagersByStaffingManagerTypeId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable();
			}
		}

		/// <summary>
		/// Retrieves the acount with a given account id.
		/// </summary>
		/// <param name="accountId">The id of the account.</param>
		/// <returns>One account.</returns>
		public dynamic GetStaffingManagerByStaffingManagerId(int accountId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@accountId", accountId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<dynamic>("[Finance].[GetStaffingManager]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable().FirstOrDefault();
			}
		}
        
        /// <summary>
        /// Retrieves the Position with a given id.
        /// </summary>
        /// <param name="positionId">The id of the position.</param>
        /// <returns>One Position.</returns>
        public dynamic GetPositionByPositionId(int positionId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PositionId", positionId);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                return connection.Query<dynamic>("[StaffingManager].[GetPositionByPositionId]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retrieves the Positions from a given orgnizatin.
        /// </summary>
        /// <param name="organizationId">The id of the organization.</param>
        /// <returns>One Position.</returns>
        public IEnumerable<dynamic> GetPositionsByOrganizationId(int organizationId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OrganizationId", organizationId);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                return connection.Query<dynamic>("[StaffingManager].[GetPositionsByOrganizationId]", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        ////////////////////////////
        /*         UPDATE         */
        ////////////////////////////

        /// <summary>
        /// Updates the account with the given id, if another account doesn't already have the same StaffingManagerName.
        /// </summary>
        /// <param name="account">The account object to be updated.</param>
        /// <returns>-1 if the account wasn't updated (duplicate accountname), and the accountId if the account was updated.</returns>
        public int UpdateStaffingManager(dynamic account)
		{
			if (account == null)
			{
				throw new System.ArgumentException("StaffingManager cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@accountId", account.StaffingManagerId);
			parameters.Add("@accountName", account.StaffingManagerName);
			parameters.Add("@isActive", account.IsActive);
			parameters.Add("@accountTypeId", account.StaffingManagerTypeId);
			parameters.Add("@parentStaffingManagerId", account.ParentStaffingManagerId);
			parameters.Add("@returnValue", -1, DbType.Int32, direction: ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				connection.Execute("[Finance].[UpdateStaffingManager]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@returnValue");
		}

        /// <summary>
		/// Updates the position with the given id.
		/// </summary>
		/// <param name="position">The account object to be updated.</param>
		/// <returns>Returns the number of rows updated.</returns>
		public int UpdatePosition(dynamic position)
        {
            if (position == null)
            {
                throw new System.ArgumentException("Position cannot be null or empty.");
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PositionId", position.PositionId);
            parameters.Add("@OrganizationId", position.OrganizationId);
            parameters.Add("@AddressId", position.AddressId);
            parameters.Add("@StartDate", position.StartDate);
            parameters.Add("@PositionStatus", position.PositionStatus);
            parameters.Add("@PositionTitle", position.PositionTitle);
            parameters.Add("@BillingRateFrequency", position.BillingRateFrequency);
            parameters.Add("@BillingRateAmount", position.BillingRateAmount);
            parameters.Add("@DurationMonths", position.DurationMonths);
            parameters.Add("@EmploymentType", position.EmploymentType);
            parameters.Add("@PositionCount", position.PositionCount);
            parameters.Add("@RequiredSkills", position.RequiredSkills);
            parameters.Add("@JobResponsibilities", position.JobResponsibilities);
            parameters.Add("@DesiredSkills", position.DesiredSkills);
            parameters.Add("@PositionLevel", position.PositionLevel);
            parameters.Add("@HiringManager", position.HiringManager);
            parameters.Add("@TeamName", position.TeamName);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                // returns 0 if fails 
                return connection.Execute("[StaffingManager].[UpdatePosition]", parameters, commandType: CommandType.StoredProcedure);
            }
        }


        ////////////////////////////
        /*         DELETE         */
        ////////////////////////////

        /// <summary>
        /// Sets the given account (if exists) to inactive (IsActive == false).
        /// </summary>
        /// <param name="accountId">Parameter @organizationId. .</param>
        public void DeleteStaffingManager(int accountId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@accountId", accountId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Finance].[DeleteStaffingManager]", parameters, commandType: CommandType.StoredProcedure);
			}
        }

        /// <summary>
        /// Deletes a tag from the database
        /// </summary>
        /// <param name="tagId">Parameter @TagId. .</param>
        public void DeleteTag(int tagId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TagId", tagId);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                connection.Execute("[StaffingManager].[DeleteTag]", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Deletes a position tag from the database; Doesnt delete the tag, just removes it from the position
        /// </summary>
        /// <param name="tagId">Parameter @TagId. .</param>
        /// <param name="positionId">Parameter @TagId. .</param>
        public void DeleteTag(int tagId, int positionId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TagId", tagId);
            parameters.Add("@PositionId", positionId);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                connection.Execute("[StaffingManager].[DeletePositionTag]", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Removes a Position from the Database
        /// </summary>
        /// <param name="positionId">Parameter @PositionId. .</param>
        public void DeletePosition(int positionId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@positionId", positionId);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                connection.Execute("[StaffingManager].[DeletePosition]", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
