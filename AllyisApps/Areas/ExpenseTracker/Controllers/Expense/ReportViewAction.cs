using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.Expense;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// The ExpenseController class with ReportView actions.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// The ReportView Action.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="reportId">The report id.</param>
		/// <returns>The view with selected report details.</returns>
		async public Task<ActionResult> ReportView(int subscriptionId, int reportId)
		{
			SetNavData(subscriptionId);

			var model = await InitializeReportViewModel(subscriptionId, reportId);

			return View(model);
		}

		/// <summary>
		/// Initializes the ReportViewModel.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="id">The report id.</param>
		/// <returns>The view model.</returns>
		async public Task<ReportViewModel> InitializeReportViewModel(int subscriptionId, int id)
		{
			SetNavData(subscriptionId);

			var reportTask = AppService.GetExpenseReport(id);
			var reportItemsTask = AppService.GetExpenseItemsByReportId(id);
			var historyTask = AppService.GetExpenseHistoryByReportId(id);

			await Task.WhenAll(new Task[] { reportTask, reportItemsTask, historyTask });

			var report = reportTask.Result;
			var reportItems = reportItemsTask.Result;
			var history = historyTask.Result;

			List<ExpenseItemCreateViewModel> itemViewModels = new List<ExpenseItemCreateViewModel>();
			foreach (ExpenseItem item in reportItems)
			{
				itemViewModels.Add(InitializeExpenseItemViewModel(item));
			}

			var user = await AppService.GetUser(report.SubmittedById);

			List<ExpenseHistoryViewModel> reportHistory = new List<ExpenseHistoryViewModel>();

			List<string> fileNames = AzureFiles.GetReportAttachments(id);

			foreach (var item in history)
			{
				var reviewer = await AppService.GetUser(item.UserId);
				reportHistory.Add(new ExpenseHistoryViewModel()
				{
					Reviewer = string.Format("{0} {1}", reviewer.FirstName, reviewer.LastName),
					Status = (ExpenseStatusEnum)item.Status,
					Submitted = item.CreatedUtc,
					Text = item.Text
				});
			}

			return new ReportViewModel()
			{
				ReprortTitle = report.ReportTitle,
				SubmittedBy = string.Format("{0} {1}", user.FirstName, user.LastName),
				CreatedUtc = report.CreatedUtc,
				ModifiedUtc = report.ModifiedUtc,
				SubmittedUtc = report.SubmittedUtc,
				Justification = report.BusinessJustification,
				Status = (ExpenseStatusEnum)report.ReportStatus,
				Expenses = itemViewModels,
				History = reportHistory,
				UserId = user.UserId,
				ReportId = report.ExpenseReportId,
				SubscriptionId = subscriptionId,
				Attachments = fileNames
			};
		}

		private ExpenseItemCreateViewModel InitializeExpenseItemViewModel(ExpenseItem item)
		{
			return new ExpenseItemCreateViewModel()
			{
				AccountId = item.AccountId,
				Amount = item.Amount,
				ExpenseItemCreatedUtc = item.ExpenseItemCreatedUtc,
				ExpenseItemId = item.ExpenseItemId,
				ExpenseItemModifiedUtc = item.ExpenseItemModifiedUtc,
				ExpenseReportId = item.ExpenseReportId,
				Index = item.Index,
				IsBillableToCustomer = item.IsBillableToCustomer,
				ItemDescription = item.ItemDescription,
				ToDelete = item.ToDelete,
				TransactionDate = item.TransactionDate
			};
		}
	}
}