//------------------------------------------------------------------------------
// <copyright file="DBHelper.Lookup.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Finance;
using Dapper;
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
		////////////////////////////
		/*         CREATE         */
		////////////////////////////

		/// <summary>
		/// Adds an account to the DB if there is not already another account with the same AccountName
		/// </summary>
		/// <param name="Account">The account object to be added to the db</param>
		/// <returns>The id of the created account or -1 if the account name is already taken.</returns>
		public int CreateAccount(AccountDBEntity Account)
		{
			if (Account == null)
			{
				throw new System.ArgumentException("Account cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@AccountName", Account.AccountName);
			parameters.Add("@IsActive", Account.IsActive);
			parameters.Add("@AccountTypeId", Account.AccountTypeId);
			parameters.Add("@ParentAccountId", Account.ParentAccountId);
			parameters.Add("@returnValue", -1, DbType.Int32, direction: ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				connection.Execute("[Finance].[CreateAccount]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@returnValue");
		}


		////////////////////////////
		/*          READ          */
		////////////////////////////

		/// <summary>
		/// Retrieves all acounts with a given parent account id.
		/// </summary>
		/// <param name="ParentAccountId">The id of the parent account</param>
		/// <param name="IsActive">Determines whether the account is "deleted" or not.  Most times we only want active accounts, so default to true</param>
		/// <returns>A collection of accounts.</returns>
		public IEnumerable<AccountDBEntity> GetAccountsByParentId(int ParentAccountId, bool IsActive = true)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ParentAccountId", ParentAccountId);
			parameters.Add("@IsActive", IsActive);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<AccountDBEntity>("[Finance].[GetAccountsByParentId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable();
			}
		}

		/// <summary>
		/// Retrieves all acounts with a given account type.
		/// </summary>
		/// <param name="AccountTypeId">The id of the account type (TODO: what are the different account types?)</param>
		/// <param name="IsActive">Determines whether the account is "deleted" or not.  Most times we only want active accounts, so default to true</param>
		/// <returns>A collection of accounts.</returns>
		public IEnumerable<AccountDBEntity> GetAccountsByAccountTypeId(int AccountTypeId, bool IsActive = true)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@AccountTypeId", AccountTypeId);
			parameters.Add("@IsActive", IsActive);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<AccountDBEntity>("[Finance].[GetAccountsByAccountTypeId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable();
			}
		}

		/// <summary>
		/// Retrieves the acount with a given account id.
		/// </summary>
		/// <param name="AccountId">The id of the account</param>
		/// <returns>One account.</returns>
		public AccountDBEntity GetAccountByAccountId(int AccountId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@AccountId", AccountId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<AccountDBEntity>("[Finance].[GetAccountByAccountId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable().FirstOrDefault();
			}
		}


		////////////////////////////
		/*         UPDATE         */
		////////////////////////////

		/// <summary>
		/// Updates the account with the given id, if another account doesn't already have the same AccountName.
		/// </summary>
		/// <param name="Account">The account object to be updated</param>
		/// <returns>-1 if the account wasn't updated, and the accountId if the account was updated</returns>
		public int UpdateAccount(AccountDBEntity Account)
		{
			if (Account == null)
			{
				throw new System.ArgumentException("Account cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@AccountId", Account.AccountId);
			parameters.Add("@AccountName", Account.AccountName);
			parameters.Add("@IsActive", Account.IsActive);
			parameters.Add("@AccountTypeId", Account.AccountTypeId);
			parameters.Add("@ParentAccountId", Account.ParentAccountId);
			parameters.Add("@returnValue", -1, DbType.Int32, direction: ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				connection.Execute("[Finance].[UpdateAccount]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@returnValue");
		}
	}
}
