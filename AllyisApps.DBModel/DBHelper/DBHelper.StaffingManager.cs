//------------------------------------------------------------------------------
// <copyright file="DBHelper.StaffingManager.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

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
	}
}
