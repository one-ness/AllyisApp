using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Expense;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Creates a new report.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Create expense report.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <returns>Returns an action result.</returns>
		async public Task<ActionResult> Pending(int subscriptionId)
		{
			await SetNavData(subscriptionId);

			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Pending, subscriptionId);
			var results = await AppService.GetSubscription(subscriptionId);
			var orgId = results.OrganizationId;
			int userId = this.AppService.UserContext.UserId;
			IEnumerable<ExpenseReport> reports = await AppService.GetExpenseReportByOrgId(orgId);
			IEnumerable<ExpenseReport> pending = reports.ToList().Where(r => r.ReportStatus == (int)ExpenseStatusEnum.Pending);
			ExpensePendingModel model = await InitializeViewModel(subscriptionId, userId, pending);

			model.PendingReports = model.PendingReports.ToList().Where(r => r.Amount <= (decimal)ViewData["MaxAmount"]);
			return View(model);
		}

		/// <summary>
		/// Initializes the pending page view model.
		/// </summary>
		/// <param name="subId">The subscription id.</param>
		/// <param name="userId">The user id.</param>
		/// <param name="reports">A list of expense reports.</param>
		/// <returns>A Expense Pending model.</returns>
		async public Task<ExpensePendingModel> InitializeViewModel(int subId, int userId, IEnumerable<ExpenseReport> reports)
		{
			List<ExpenseItemViewModel> reportModels = new List<ExpenseItemViewModel>();
			foreach (ExpenseReport report in reports)
			{
				var expItemsTask = AppService.GetExpenseItemsByReportId(report.ExpenseReportId);
				var userTask = AppService.GetUser(report.SubmittedById);

				await Task.WhenAll(new Task[] { expItemsTask, userTask });

				var expItems = expItemsTask.Result;
				var user = userTask.Result;

				decimal totalAmount = expItems.Sum(x => x.Amount);

				reportModels.Add(new ExpenseItemViewModel()
				{
					Amount = totalAmount,
					Reason = report.BusinessJustification,
					ReportId = report.ExpenseReportId,
					ReportName = report.ReportTitle,
					Status = (ExpenseStatusEnum)report.ReportStatus,
					SubmittedDate = report.SubmittedUtc,
					UserId = user.UserId,
					UserName = user.FirstName + " " + user.LastName
				});
			}

			ExpensePendingModel model = new ExpensePendingModel()
			{
				PendingReports = reportModels
			};
			return model;
		}
	}
}