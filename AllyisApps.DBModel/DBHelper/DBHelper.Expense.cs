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
        /// Adds an ExpenseItem to the database.
        /// </summary>
        /// <param name="item">The ExpenseDBEntity to create.</param>
        /// <returns>TODO: Should this return success/failuer codes? ? ?</returns>
        public void CreateExpenseItem(ExpenseItemDBEntity item)
        {
            if (item.AccountId == 0) //check if account exists already
            {
                throw new System.ArgumentException("An Error");
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ItemDescription", item.ItemDescription);
            parameters.Add("@TransactionDate", item.TransactionDate);
            parameters.Add("@Amount", item.Amount);
            parameters.Add("@ExpenseReportId", item.ExpenseReportId);
            parameters.Add("@IsBillableToCustomer", item.IsBillableToCustomer);
            parameters.Add("@CreatedUtc", item.CreatedUtc);
            parameters.Add("@ModifiedUtc", item.ModifiedUtc);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                connection.Execute("[Expense].[CreateExpenseItem]", parameters, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// Retrieves all Expense Items with a given account id.
        /// </summary>
        /// <param name="AccountId">The id of the parent account</param>
        /// <returns>A collection of accounts.</returns>
        public IEnumerable<ExpenseItemDBEntity> GetExpenseItemsByAccountId(int AccountId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@AccountId", AccountId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<ExpenseItemDBEntity>("[Expense].[GetExpenseItemsByAccountId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable();
			}
		}

        /// <summary>
        /// Gets the Expense Item with the corresponding ID
        /// </summary>
        /// <param name="ExpenseItemId">The id of the parent account</param>
        /// <returns>A collection of accounts.</returns>
        public ExpenseItemDBEntity GetExpenseItemByExpenseItemId(int ExpenseItemId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ExpenseItemId", ExpenseItemId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<ExpenseItemDBEntity>("[Expense].[GetExpenseItemByExpenseItemId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable().FirstOrDefault();
			}
		}


        /// <summary>
        /// Updates the Expense Item with the specified ID.
        /// </summary>
        /// <param name="item">The table with the user to create.</param>
        public void UpdateExpenseItem(ExpenseItemDBEntity item)
        {
            if (item == null)
            {
                throw new System.ArgumentException("Item cannot be null, must already exist");
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ExpenseItemId", item.ExpenseItemId);
            parameters.Add("@ItemDescription", item.ItemDescription);
            parameters.Add("@TransactionDate", item.TransactionDate);
            parameters.Add("@Amount", item.Amount);
            parameters.Add("@ExpenseReportId", item.ExpenseReportId);
            parameters.Add("@IsBillableToCustomer", item.IsBillableToCustomer);
            parameters.Add("@CreatedUtc", item.CreatedUtc);
            parameters.Add("@ModifiedUtc", item.ModifiedUtc);


            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                connection.Execute("[Expense].[UpdateExpenseItem]", parameters, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
