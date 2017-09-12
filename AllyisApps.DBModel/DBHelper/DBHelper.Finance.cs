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
		/// Adds an account to the DB if there is not already another account with the same AccountName.
		/// </summary>
		/// <param name="account">The account object to be added to the db.</param>
		/// <returns>The id of the created account or -1 if the account name is already taken.</returns>
		public int CreateAccount(AccountDBEntity account)
		{
			if (account == null)
			{
				throw new System.ArgumentException("Account cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@accountName", account.AccountName);
			parameters.Add("@isActive", account.IsActive);
			parameters.Add("@accountTypeId", account.AccountTypeId);
			parameters.Add("@parentAccountId", account.ParentAccountId);
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
		/// <param name="parentAccountId">The id of the parent account.</param>
		/// <param name="isActive">Determines whether the account is "deleted" or not.  Most times we only want active accounts, so default to true.</param>
		/// <returns>A collection of accounts.</returns>
		public IEnumerable<AccountDBEntity> GetAccountsByParentId(int parentAccountId, bool isActive = true)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@parentAccountId", parentAccountId);
			parameters.Add("@isActive", isActive);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<AccountDBEntity>("[Finance].[GetAccountsByParentId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable();
			}
		}

		/// <summary>
		/// Retrieves all acounts with a given account type.
		/// </summary>
		/// <param name="accountTypeId">The id of the account type (TODO: what are the different account types?).</param>
		/// <param name="isActive">Determines whether the account is "deleted" or not.  Most times we only want active accounts, so default to true.</param>
		/// <returns>A collection of accounts.</returns>
		public IEnumerable<AccountDBEntity> GetAccountsByAccountTypeId(int accountTypeId, bool isActive = true)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@accountTypeId", accountTypeId);
			parameters.Add("@isActive", isActive);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<AccountDBEntity>("[Finance].[GetAccountsByAccountTypeId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable();
			}
		}

		/// <summary>
		/// Retrieves the acount with a given account id.
		/// </summary>
		/// <param name="accountId">The id of the account.</param>
		/// <returns>One account.</returns>
		public AccountDBEntity GetAccountByAccountId(int accountId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@accountId", accountId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<AccountDBEntity>("[Finance].[GetAccount]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable().FirstOrDefault();
			}
		}

		/// <summary>
		/// Retrieves all of the accounts in the database.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AccountDBEntity> GetAccounts()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<AccountDBEntity>("[Finance].[GetAccounts]", commandType: CommandType.StoredProcedure).AsEnumerable();
			}
		}

		////////////////////////////
		/*         UPDATE         */
		////////////////////////////

		/// <summary>
		/// Updates the account with the given id, if another account doesn't already have the same AccountName.
		/// </summary>
		/// <param name="account">The account object to be updated.</param>
		/// <returns>-1 if the account wasn't updated (duplicate accountname), and the accountId if the account was updated.</returns>
		public int UpdateAccount(AccountDBEntity account)
		{
			if (account == null)
			{
				throw new System.ArgumentException("Account cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@accountId", account.AccountId);
			parameters.Add("@accountName", account.AccountName);
			parameters.Add("@isActive", account.IsActive);
			parameters.Add("@accountTypeId", account.AccountTypeId);
			parameters.Add("@parentAccountId", account.ParentAccountId);
			parameters.Add("@returnValue", -1, DbType.Int32, direction: ParameterDirection.Output);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				connection.Execute("[Finance].[UpdateAccount]", parameters, commandType: CommandType.StoredProcedure);
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
		public void DeleteAccount(int accountId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@accountId", accountId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Finance].[DeleteAccount]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Returns the Owners of a given organization.
		/// </summary>
		/// <param name="organizationId">.</param>
		/// <returns>.</returns>
		public IEnumerable<dynamic> GetOrgOwnerEmails(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query("[Auth].[GetOrganizationOwnerEmails]", parameters, commandType: CommandType.StoredProcedure);
			}
		}
	}
}
