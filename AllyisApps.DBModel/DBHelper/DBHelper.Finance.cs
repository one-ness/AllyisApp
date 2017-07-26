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
		/// <summary>
		/// Retrieves all acounts with a given parent account id.
		/// </summary>
		/// <param name="ParentAccountId">The id of the parent account</param>
		/// <returns>A collection of accounts.</returns>
		public IEnumerable<AccountDBEntity> GetAccounts(int ParentAccountId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ParentAccountId", ParentAccountId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<AccountDBEntity>("[Finance].[GetAccountsByParentId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable();
			}
		}

		/// <summary>
		/// list of valid countries
		/// </summary>
		public AccountDBEntity GetAccount(int AccountId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@AccountId", AccountId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<AccountDBEntity>("[Finance].[GetAccountsByAccountId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable().FirstOrDefault();
			}
		}
	}
}
