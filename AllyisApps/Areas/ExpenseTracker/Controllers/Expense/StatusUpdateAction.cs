using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Updates the status of a report to accepted or rejected depending on the clicked button.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="reportId">The reprot id.</param>
		/// <param name="btnAction">The button action.</param>
		/// <param name="reasonText">The reason text.</param>
		/// <returns>A redirect to the home page.</returns>
		public async Task<RedirectToRouteResult> StatusUpdate(int subscriptionId, int reportId, string btnAction, string reasonText)
		{
			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.StatusUpdate, subscriptionId);

			switch (btnAction)
			{
				case "Approve":
					await UpdateReport(reportId, "Approve", reasonText);
					break;

				case "Reject":
					await UpdateReport(reportId, "Reject", reasonText);
					break;

				default:
					break;
			}

			return RedirectToRoute("ExpenseTracker_Default", new { subscriptionId = subscriptionId, controller = "expense" });
		}

		/// <summary>
		/// Call functionality to update a report.
		/// </summary>
		/// <param name="reportId">The report id.</param>
		/// <param name="status">The new report status.</param>
		/// <param name="text">The reason text.</param>
		private async Task UpdateReport(int reportId, string status, string text)
		{
			ExpenseReport report = await AppService.GetExpenseReport(reportId);
			ExpenseHistory history = new ExpenseHistory()
			{
				CreatedUtc = DateTime.UtcNow,
				ModifiedUtc = DateTime.UtcNow,
				HistoryId = GetHashCode(),
				ReportId = reportId,
				Text = text,
				UserId = AppService.UserContext.UserId
			};

			if (string.Equals(status, "Approve"))
			{
				report.ReportStatus = (int)ExpenseStatusEnum.Approved;
				history.Status = report.ReportStatus;
			}
			else if (string.Equals(status, "Reject"))
			{
				report.ReportStatus = (int)ExpenseStatusEnum.Rejected;
				history.Status = report.ReportStatus;
			}
			else
			{
				throw new Exception("Unknown status given to status update.");
			}

			await AppService.UpdateExpenseReport(report, reportId);
			await AppService.CreateExpenseReportHistory(history);
		}
	}
}