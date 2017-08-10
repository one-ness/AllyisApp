using AllyisApps.DBModel;
using AllyisApps.DBModel.Finance;
using System;
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
        /// Get an expense reports items by it id.
        /// </summary>
        /// <param name="reportId">The reports id</param>
        /// <returns>A IEnumerabe of ExpenseItems for the report.</returns>
        public IEnumerable<ExpenseItem> GetExpenseItemsByReportId(int reportId)
        {
            return DBHelper.GetExpenseItemsByReportId(reportId).Select(x => InitializeExpenseItem(x)).AsEnumerable(); ;
        }

		/// <summary>
		/// Gets an expense report by its id.
		/// </summary>
		/// <param name="reportId">The report id.</param>
		/// <returns></returns>
		public ExpenseReport GetExpenseReport(int reportId)
		{
			return InitializeExpenseReport(DBHelper.GetExpenseReport(reportId));
		}

		/// <summary>
		/// Updates expense report with given reportId.
		/// </summary>
		/// <param name="report"></param>
		/// <param name="reportId"></param>
		public void UpdateExpenseReport(ExpenseReport report, int reportId)
		{
			ExpenseReportDBEntity reportEntity = new ExpenseReportDBEntity()
			{
				BusinessJustification = report.BusinessJustification,
				ExpenseReportCreatedUtc = report.CreatedUtc,
				ExpenseReportId = reportId,
				ExpenseReportModifiedUtc = report.ModifiedUtc,
				OrganizationId = report.OrganizationId,
				ReportDate = report.ReportDate,
				ReportStatus = report.ReportStatus,
				ReportTitle = report.ReportTitle,
				SubmittedById = report.SubmittedById
			};
			DBHelper.UpdateExpenseReport(reportEntity);
		}

		/// <summary>
		/// Updates Expense Item with given itemId.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="itemId"></param>
		public void UpdateExpenseItem(ExpenseItem item, int itemId)
		{
			ExpenseItemDBEntity itemEntity = new ExpenseItemDBEntity()
			{
				AccountId = item.AccountId,
				Amount = item.Amount,
				CreatedUtc = item.ExpenseItemCreatedUtc,
				ExpenseItemId = itemId,
				ExpenseReportId = item.ExpenseReportId,
				IsBillableToCustomer = item.IsBillableToCustomer,
				ItemDescription = item.ExpenseItemName,
				ModifiedUtc = item.ExpenseItemModifiedUtc,
				TransactionDate = item.TransactionDate,
			};

			DBHelper.UpdateExpenseItem(itemEntity);
		}

        /// <summary>
        /// Get an expense report by its organization id.
        /// </summary>
        /// <param name="orgId">The organization id</param>
        /// <returns></returns>
		public IEnumerable<ExpenseReport> GetExpenseReportByOrgId(int orgId)
		{
			return DBHelper.GetExpenseReportsByOrganizationId(orgId).Select(x => InitializeExpenseReport(x));
		}

        /// <summary>
        /// Get an expense report by the submitted id
        /// </summary>
        /// <param name="submittedId">The submitting users id.</param>
        /// <returns>An IEnumerabe of expense reports.</returns>
		public IEnumerable<ExpenseReport> GetExpenseReportBySubmittedId(int submittedId)
		{
			return DBHelper.GetExpenseReportsBySubmittedById(submittedId).Select(x => InitializeExpenseReport(x));
		}

        /// <summary>
        /// Initializes an ExpenseReport from and ExpenseReportDbEntity.
        /// </summary>
        /// <param name="entity">The ExpenseReportDBEntity</param>
        /// <returns></returns>
        public static ExpenseReport InitializeExpenseReport(ExpenseReportDBEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

			return new ExpenseReport()
			{
				OrganizationId = entity.OrganizationId,
				ExpenseReportId = entity.ExpenseReportId,
				SubmittedById = entity.SubmittedById,
				CreatedUtc = entity.ExpenseReportCreatedUtc,
				ModifiedUtc = entity.ExpenseReportModifiedUtc,
				ReportDate = entity.ReportDate,
				ReportStatus = entity.ReportStatus,
				ReportTitle = entity.ReportTitle,
				BusinessJustification = entity.BusinessJustification
            };
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
                ExpenseItemName = entity.ExpenseItemName,
				ExpenseReportId = entity.ExpenseReportId,
				IsBillableToCustomer = entity.IsBillableToCustomer,
				ItemDiscription = entity.ItemDescription,
				TransactionDate = entity.TransactionDate
			};
		}
		
		public void CreateExpenseItem(ExpenseItem item)
		{
			ExpenseItemDBEntity itemEntity = new ExpenseItemDBEntity()
			{
				AccountId = item.AccountId,
                ExpenseItemName = item.ExpenseItemName,
				Amount = item.Amount,
				CreatedUtc = item.ExpenseItemCreatedUtc,
				ExpenseItemId = item.ExpenseItemId,
				ExpenseReportId = item.ExpenseReportId,
				IsBillableToCustomer = item.IsBillableToCustomer,
				ItemDescription = item.ExpenseItemName,
				ModifiedUtc = item.ExpenseItemModifiedUtc,
				TransactionDate = item.TransactionDate,
			};
			DBHelper.CreateExpenseItem(itemEntity);
		}

		public int CreateExpenseReport(ExpenseReport report)
		{
			ExpenseReportDBEntity reportEntity = new ExpenseReportDBEntity()
			{
				BusinessJustification = report.BusinessJustification,
				ExpenseReportCreatedUtc = report.CreatedUtc,
				ExpenseReportId = report.ExpenseReportId,
				ExpenseReportModifiedUtc = report.ModifiedUtc,
				OrganizationId = report.OrganizationId,
				ReportDate = report.ReportDate,
				ReportStatus = report.ReportStatus,
				ReportTitle = report.ReportTitle,
				SubmittedById = report.SubmittedById
			};
			return DBHelper.CreateExpenseReport(reportEntity);
		}
	}
}
