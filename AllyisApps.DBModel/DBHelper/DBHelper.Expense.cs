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
		//=================//
		/*  Expense Items  */
		//=================//

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
            parameters.Add("@itemDescription", item.ItemDescription ?? "");
            parameters.Add("@transactionDate", item.TransactionDate);
            parameters.Add("@amount", item.Amount);
            parameters.Add("@expenseReportId", item.ExpenseReportId);
            parameters.Add("@isBillableToCustomer", item.IsBillableToCustomer);
			parameters.Add("@accountId", item.AccountId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Expense].[CreateExpenseItem]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds an ExpenseFile to the database.
		/// </summary>
		/// <param name="file"></param>
		public void CreateExpenseFile(ExpenseFileDBEntity file)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@fileName", file.FileName);
			parameters.Add("@url", file.Url);
			parameters.Add("@expenseReportId", file.ExpenseReportId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Expense].[CreateExpenseFile]", parameters, commandType: CommandType.StoredProcedure);
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
			parameters.Add("@accountId", AccountId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<ExpenseItemDBEntity>("[Expense].[GetExpenseItemsByAccountId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable();
			}
		}

        /// <summary>
        /// Gets expense items for a specified report.
        /// </summary>
        /// <param name="ReportId">The report id.</param>
        /// <returns>IEnumerable of Expense items.</returns>
        public IList<ExpenseItemDBEntity> GetExpenseItemsByReportId(int ReportId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@reportId", ReportId);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                return connection.Query<ExpenseItemDBEntity>("[Expense].[GetExpenseItemsByExpenseReportId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable().ToList();
            }
        }

		/// <summary>
		/// Gets expense files for a specified report
		/// </summary>
		/// <param name="ReportId"></param>
		/// <returns></returns>
		public IList<ExpenseFileDBEntity> GetExpenseFilesByReportId(int ReportId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@reportId", ReportId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<ExpenseFileDBEntity>("[Expense].[GetExpenseFilesByExpenseReportId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable().ToList();
			}
		}

        /// <summary>
        /// Gets the Expense Item with the corresponding ID
        /// </summary>
        /// <param name="ExpenseItemId"> the ID of the Expense Item</param>
        /// <returns>A collection of accounts.</returns>
        public ExpenseItemDBEntity GetExpenseItem(int ExpenseItemId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@expenseItemId", ExpenseItemId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<ExpenseItemDBEntity>("[Expense].[GetExpenseItemsByExpenseItemId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable().FirstOrDefault();
			}
		}


        /// <summary>
        /// Updates the Expense Item with the specified ID.
        /// </summary>
        /// <param name="item">The expense item object that will replace the previous entry.</param>
        public int UpdateExpenseItem(ExpenseItemDBEntity item)
        {
            if (item == null)
            {
                throw new System.ArgumentException("Item cannot be null, must already exist");
            }

            DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@expenseItemId", item.ExpenseItemId);
            parameters.Add("@itemDescription", item.ItemDescription);
            parameters.Add("@transactionDate", item.TransactionDate);
			parameters.Add("@modifiedUtc", item.ModifiedUtc);
            parameters.Add("@amount", item.Amount);
            parameters.Add("@expenseReportId", item.ExpenseReportId);
            parameters.Add("@isBillableToCustomer", item.IsBillableToCustomer);
			parameters.Add("@accountId", item.AccountId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                return connection.Execute("[Expense].[UpdateExpenseItem]", parameters, commandType: CommandType.StoredProcedure);
            }
        }


        /// <summary>
        /// Deletes an Expense Item with a given id.
        /// </summary>
        /// <param name="ExpenseItemId">The id of the expense item to be deleted</param>
        /// <returns>Boolean to determine if the process succeeded in finding the item</returns>
        public bool DeleteExpenseItem(int ExpenseItemId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@expenseItemId", ExpenseItemId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>("[Expense].[DeleteExpenseItem]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault() == 1;
			}
		}

		//=================//
		/* Expense Reports */
		//=================//

        /// <summary>
        /// Adds an ExpenseReport to the database.
        /// </summary>
        /// <param name="report">The ExpenseReportDBEntity to create.</param>
        /// <returns>TODO: Should this return success/failuer codes? ? ?</returns>
        public int CreateExpenseReport(ExpenseReportDBEntity report)
        {
            if (report.OrganizationId == 0 || report.SubmittedById == 0) //check if user and organization exists already
            {
                throw new System.ArgumentException("An Error Creating Report");
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@reportTitle", report.ReportTitle);
			parameters.Add("@organizationId", report.OrganizationId);
			parameters.Add("@submittedById", report.SubmittedById);
            parameters.Add("@reportStatus", report.ReportStatus);
            parameters.Add("@businessJustification", report.BusinessJustification);
            parameters.Add("@createdUtc", report.ExpenseReportCreatedUtc);
            parameters.Add("@modifiedUtc", report.ExpenseReportModifiedUtc);
			parameters.Add("@submittedUtc", report.ExpenseReportSubmittedUtc);
			parameters.Add("@reportId");

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
				int reportId = connection.ExecuteScalar<int>("[Expense].[CreateExpenseReport]", parameters, commandType: CommandType.StoredProcedure);
				return reportId;
			}
		}

        /// <summary>
        /// Retrieves the Expense Reportss with specific id.
        /// </summary>
        /// <param name="ExpenseReportId">The id of the Report</param>
        /// <returns>An Expense Reports.</returns>
        public ExpenseReportDBEntity GetExpenseReport(int ExpenseReportId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@expenseReportId", ExpenseReportId);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                var entity = connection.Query<ExpenseReportDBEntity>("[Expense].[GetExpenseReportByExpenseReportId]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
				return entity;
            }
        }

        /// <summary>
        /// Retrieves all Expense Reportss created by a user id.
        /// </summary>
        /// <param name="SubmittedById">The id of the parent account</param>
        /// <returns>A collection of Expense Reports.</returns>
        public IEnumerable<ExpenseReportDBEntity> GetExpenseReportsBySubmittedById(int SubmittedById)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@submittedById", SubmittedById);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                return connection.Query<ExpenseReportDBEntity>("[Expense].[GetExpenseReportsBySubmittedById]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable();
            }
        }

        /// <summary>
        /// Retrieves all Expense Reportss created by an organization.
        /// </summary>
        /// <param name="OrganizationId">The id of the parent organization</param>
        /// <returns>A collection of Expense Reports.</returns>
        public IEnumerable<ExpenseReportDBEntity> GetExpenseReportsByOrganizationId(int OrganizationId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@organizationId", OrganizationId);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                return connection.Query<ExpenseReportDBEntity>("[Expense].[GetExpenseReportsByOrganizationId]", parameters, commandType: CommandType.StoredProcedure).AsEnumerable();
            }
        }

		/// <summary>
		/// Updates the Expense Report with the specified ID.
		/// </summary>
		/// <param name="report">The expense report object that will replace the previous entry.</param>
		public void UpdateExpenseReport(ExpenseReportDBEntity report)
		{
			if (report == null)
			{
				throw new System.ArgumentException("Report cannot be null, must already exist");
			}

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@expenseReportId", report.ExpenseReportId);
            parameters.Add("@reportTitle", report.ReportTitle);
            parameters.Add("@organizationId", report.OrganizationId);
            parameters.Add("@submittedById", report.SubmittedById);
            parameters.Add("@reportStatus", report.ReportStatus);
            parameters.Add("@businessJustification", report.BusinessJustification);
			parameters.Add("@submittedUtc", report.ExpenseReportSubmittedUtc);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Expense].[UpdateExpenseReport]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

        /// <summary>
        /// Deletes an Expense Report with a given id.
        /// </summary>
        /// <param name="ExpenseReportId">The id of the expense report to be deleted</param>
        /// <returns>Boolean to determine if the process succeeded in finding the item</returns>
        public bool DeleteExpenseReport(int ExpenseReportId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@expenseReportId", ExpenseReportId);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                return connection.Query<int>("[Expense].[DeleteExpenseReport]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault() == 1;
            }
        }

        //=================//
        /* Expense History */
        //=================//

        /// <summary>
        /// Creates an expense history item.
        /// </summary>
        /// <param name="entity">An ExpenseHistoryDBEntity</param>
        public void CreateExpenseHistory(ExpenseHistoryDBEntity entity)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@historyId", entity.HistoryId);
            parameters.Add("@reportId", entity.ExpenseReportId);
            parameters.Add("@userId", entity.UserId);
            parameters.Add("@status", entity.Status);
            parameters.Add("@text", entity.Text);
            parameters.Add("@createdUtc", entity.CreatedUtc);
            parameters.Add("@modifiedUtc", entity.ModifiedUtc);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                connection.Execute("[Expense].[CreateExpenseHistory]", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Gets an expense report history.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <returns></returns>
        public IEnumerable<ExpenseHistoryDBEntity> GetExpenseHistory(int reportId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(@"reportId", reportId);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                return connection.Query<ExpenseHistoryDBEntity>("[Expense].[GetExpenseHistory]", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Updates an expense history item.
        /// </summary>
        /// <param name="entity">The expense history entity.</param>
        public void UpdateExpenseHistory(ExpenseHistoryDBEntity entity)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@historyId", entity.HistoryId);
            parameters.Add("@text", entity.Text);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                connection.Execute("[Expense].[UpdateExpenseHistory]", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
