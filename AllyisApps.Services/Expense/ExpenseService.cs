using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.DBModel.Finance;
using AllyisApps.Services.Expense;

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
		/// Get expense report items by report id.
		/// </summary>
		/// <param name="reportId">The reports id</param>
		/// <returns>A IEnumerabe of ExpenseItems for the report.</returns>
		public async Task<IList<ExpenseItem>> GetExpenseItemsByReportId(int reportId)
		{
			var results = await DBHelper.GetExpenseItemsByReportId(reportId);
			return results.Select(InitializeExpenseItem).AsEnumerable().ToList();
		}

		/// <summary>
		/// Gets an expense report by its id.
		/// </summary>
		/// <param name="reportId">The report id.</param>
		/// <returns></returns>
		public async Task<ExpenseReport> GetExpenseReport(int reportId)
		{
			return InitializeExpenseReport(await DBHelper.GetExpenseReport(reportId));
		}

		/// <summary>
		/// Updates expense report with given reportId.
		/// </summary>
		/// <param name="report"></param>
		/// <param name="reportId"></param>
		public async Task UpdateExpenseReport(ExpenseReport report, int reportId)
		{
			ExpenseReportDBEntity reportEntity = new ExpenseReportDBEntity
			{
				BusinessJustification = report.BusinessJustification,
				ExpenseReportCreatedUtc = report.CreatedUtc,
				ExpenseReportId = reportId,
				ExpenseReportModifiedUtc = report.ModifiedUtc,
				ExpenseReportSubmittedUtc = report.SubmittedUtc,
				OrganizationId = report.OrganizationId,
				ReportStatus = report.ReportStatus,
				ReportTitle = report.ReportTitle,
				SubmittedById = report.SubmittedById
			};
			await DBHelper.UpdateExpenseReport(reportEntity);
		}

		/// <summary>
		/// Updates Expense Item with given itemId.
		/// </summary>
		/// <param name="item"></param>
		public void UpdateExpenseItem(ExpenseItem item)
		{
			ExpenseItemDBEntity itemEntity = new ExpenseItemDBEntity
			{
				AccountId = item.AccountId,
				Amount = Decimal.Parse(String.Format("{0:c}", item.Amount)),
				ExpenseItemId = item.ExpenseItemId,
				ExpenseReportId = item.ExpenseReportId,
				IsBillableToCustomer = item.IsBillableToCustomer,
				ItemDescription = item.ItemDescription,
				TransactionDate = Convert.ToDateTime(item.TransactionDate),
			};

			DBHelper.UpdateExpenseItem(itemEntity);
		}

		/// <summary>
		/// Get an expense report by its organization id.
		/// </summary>
		/// <param name="orgId">The organization id</param>
		/// <returns></returns>
		public async Task<IEnumerable<ExpenseReport>> GetExpenseReportByOrgId(int orgId)
		{
			var results = await DBHelper.GetExpenseReportsByOrganizationId(orgId);
			return results.Select(x => InitializeExpenseReport(x));
		}

		/// <summary>
		/// Get an expense report by the submitted id
		/// </summary>
		/// <param name="submittedId">The submitting users id.</param>
		/// <returns>An IEnumerabe of expense reports.</returns>
		public async Task<IEnumerable<ExpenseReport>> GetExpenseReportBySubmittedId(int submittedId)
		{
			var results = await DBHelper.GetExpenseReportsBySubmittedById(submittedId);
			return results.Select(x => InitializeExpenseReport(x));
		}

		public async Task<IEnumerable<ExpenseHistory>> GetExpenseHistoryByReportId(int reportId)
		{
			var results = await DBHelper.GetExpenseHistory(reportId);
			return results.Select(x => InitializeExpenseHistory(x));
		}

		/// <summary>
		/// Updates an expense report history.
		/// </summary>
		/// <param name="history">An expense history object.</param>
		public void UpdateExpenseReportHistory(ExpenseHistory history)
		{
			ExpenseHistoryDBEntity expHistory = new ExpenseHistoryDBEntity
			{
				HistoryId = history.HistoryId,
				CreatedUtc = history.CreatedUtc,
				ModifiedUtc = history.ModifiedUtc,
				ExpenseReportId = history.ReportId,
				Status = history.Status,
				Text = history.Text,
				UserId = history.UserId
			};

			DBHelper.UpdateExpenseHistory(expHistory);
		}

		/// <summary>
		/// Updates an expense report history.
		/// </summary>
		/// <param name="history">An expense history object.</param>
		public async Task CreateExpenseReportHistory(ExpenseHistory history)
		{
			ExpenseHistoryDBEntity expHistory = new ExpenseHistoryDBEntity
			{
				HistoryId = history.HistoryId,
				CreatedUtc = history.CreatedUtc,
				ModifiedUtc = history.ModifiedUtc,
				ExpenseReportId = history.ReportId,
				Text = history.Text,
				Status = history.Status,
				UserId = history.UserId
			};

			await DBHelper.CreateExpenseHistory(expHistory);
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

			return new ExpenseReport
			{
				OrganizationId = entity.OrganizationId,
				ExpenseReportId = entity.ExpenseReportId,
				SubmittedById = entity.SubmittedById,
				CreatedUtc = entity.ExpenseReportCreatedUtc,
				ModifiedUtc = entity.ExpenseReportModifiedUtc,
				SubmittedUtc = entity.ExpenseReportSubmittedUtc,
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
			if (entity == null)
			{
				return null;
			}

			return new ExpenseItem
			{
				AccountId = entity.AccountId,
				Amount = entity.Amount,
				ExpenseItemCreatedUtc = entity.CreatedUtc,
				ExpenseItemId = entity.ExpenseItemId,
				ExpenseItemModifiedUtc = entity.ModifiedUtc,
				ExpenseReportId = entity.ExpenseReportId,
				IsBillableToCustomer = entity.IsBillableToCustomer,
				ItemDescription = entity.ItemDescription,
				TransactionDate = entity.TransactionDate.ToString()
			};
		}

		public ExpenseHistory InitializeExpenseHistory(ExpenseHistoryDBEntity entity)
		{
			if (entity == null)
			{
				return null;
			}

			return new ExpenseHistory
			{
				UserId = entity.UserId,
				HistoryId = entity.HistoryId,
				ReportId = entity.ExpenseReportId,
				Text = entity.Text,
				Status = entity.Status,
				CreatedUtc = entity.CreatedUtc,
				ModifiedUtc = entity.ModifiedUtc
			};
		}

		public void CreateExpenseItem(ExpenseItem item)
		{
			ExpenseItemDBEntity itemEntity = new ExpenseItemDBEntity
			{
				AccountId = item.AccountId,
				Amount = Decimal.Parse(String.Format("{0}", item.Amount)),
				CreatedUtc = item.ExpenseItemCreatedUtc,
				ExpenseItemId = item.ExpenseItemId,
				ExpenseReportId = item.ExpenseReportId,
				IsBillableToCustomer = item.IsBillableToCustomer,
				ItemDescription = item.ItemDescription,
				ModifiedUtc = item.ExpenseItemModifiedUtc,
				TransactionDate = Convert.ToDateTime(item.TransactionDate)
			};
			DBHelper.CreateExpenseItem(itemEntity);
		}

		public int CreateExpenseReport(ExpenseReport report)
		{
			ExpenseReportDBEntity reportEntity = new ExpenseReportDBEntity
			{
				BusinessJustification = report.BusinessJustification,
				ExpenseReportCreatedUtc = report.CreatedUtc,
				ExpenseReportId = report.ExpenseReportId,
				ExpenseReportModifiedUtc = report.ModifiedUtc,
				ExpenseReportSubmittedUtc = report.SubmittedUtc,
				OrganizationId = report.OrganizationId,
				ReportStatus = report.ReportStatus,
				ReportTitle = report.ReportTitle,
				SubmittedById = report.SubmittedById
			};
			return DBHelper.CreateExpenseReport(reportEntity);
		}
	}
}