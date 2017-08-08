﻿using AllyisApps.DBModel;
using AllyisApps.DBModel.Finance;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all expense related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
        /// <summary>
        /// Gets an expense items by the user that submited them.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
		public IEnumerable<ExpenseItem> GetExpenseItemsByUserSubmitted(int userId)
		{
			var spResults = DBHelper.GetExpenseItemsByAccountId(userId);

			return spResults.Select(x => InitializeExpenseItem(x));
		}

        /// <summary>
        /// Gets an expense item by its id.
        /// </summary>
        /// <param name="expenseId">The expense id.</param>
        /// <returns></returns>
		public ExpenseItem GetExpenseItem(int expenseId)
		{
			return InitializeExpenseItem(DBHelper.GetExpenseItem(expenseId));
		}


        /// <summary>
        /// Gets an expense report by its id.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <returns></returns>
		public ExpenseItem GetExpenseReport(int reportId)
		{
			return InitializeExpenseItem(DBHelper.GetExpenseReport(reportId));
		}

        /// <summary>
        /// Get an expense report by its organization id.
        /// </summary>
        /// <param name="orgId">The organization id</param>
        /// <returns></returns>
		public IEnumerable<ExpenseItem> GetExpenseReportByOrgId(int orgId)
		{
			return DBHelper.GetExpenseReportsByOrganizationId(orgId).Select(x => InitializeExpenseItem(x));
		}

		public IEnumerable<ExpenseItem> GetExpenseReportBySubmittedId(int submitedId)
		{
			return DBHelper.GetExpenseReportsBySubmittedById(submitedId).Select(x => InitializeExpenseItem(x));
		}

        /// <summary>
        /// Initialize an ExpenseItem from an ExpenseItemDBEntity.
        /// </summary>
        /// <param name="entity">The ExpenseItemDBEntity</param>
        /// <returns></returns>
		public static ExpenseItem InitializeExpenseItem(ExpenseItemDBEntity entity)
		{

			if(entity == null)
			{
				return null;
			}

			return new ExpenseItem()
			{
				AccountId = entity.AccountId,
				Amount = entity.Amount,
				ExpenseItemCreatedUtc = entity.CreatedUtc,
				ExpenseItemId = entity.ExpenseItemId,
				ExpenseItemModifiedUtc = entity.ModifiedUtc,
				ExpenseReportId = entity.ExpenseReportId,
                ExpenseReportStatus = (ExpenseStatusEnum)entity.ReportStatus,
				IsBillableToCustomer = entity.IsBillableToCustomer,
				ItemDiscription = entity.ItemDescription,
				TransactionDate = entity.TransactionDate
			};

		}
	   
	}
}
