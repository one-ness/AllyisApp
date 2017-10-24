using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Expense;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Update expense report.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns> A redirect to index view.</returns>
		[HttpPost]
		async public Task<ActionResult> UpdateReport(ExpenseCreateModel model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Create", new { subscriptionId = model.SubscriptionId, reportId = model.Report.ExpenseReportId });
			}

			var oldReport = await AppService.GetExpenseReport(model.Report.ExpenseReportId);
			if (model.Report.ExpenseReportId != -1)
			{
				if (oldReport.SubmittedById != this.AppService.UserContext.UserId
					|| ((ExpenseStatusEnum)oldReport.ReportStatus != ExpenseStatusEnum.Draft
					&& (ExpenseStatusEnum)oldReport.ReportStatus != ExpenseStatusEnum.Rejected))
				{
					string message = string.Format("action {0} denied", AppService.ExpenseTrackerAction.UpdateReport.ToString());
					throw new AccessViolationException(message);
				}

				AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.EditReport, model.SubscriptionId);
			}
			else
			{
				AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Unmanaged, model.SubscriptionId);
			}

			if (model.Items == null)
			{
				model.Items = new List<ExpenseItemCreateViewModel>();
			}

			var subscription = await AppService.GetSubscription(model.SubscriptionId);
			var organizationId = subscription.OrganizationId;
			ExpenseStatusEnum reportStatus;
			DateTime? submittedUtc = null;

			if (model.SubmitType == "Submit")
			{
				reportStatus = ExpenseStatusEnum.Pending;
				submittedUtc = DateTime.UtcNow;
			}
			else
			{
				reportStatus = ExpenseStatusEnum.Draft;
			}

			if (oldReport.ReportStatus == (int)ExpenseStatusEnum.Draft || oldReport.ReportStatus == (int)ExpenseStatusEnum.Rejected)
			{
				var report = new ExpenseReport()
				{
					ExpenseReportId = model.Report.ExpenseReportId,
					ReportTitle = model.Report.ReportTitle,
					BusinessJustification = model.Report.BusinessJustification,
					ModifiedUtc = DateTime.UtcNow,
					SubmittedUtc = submittedUtc,
					SubmittedById = model.CurrentUser,
					OrganizationId = organizationId,
					ReportStatus = (int)reportStatus
				};

				await AppService.UpdateExpenseReport(report, model.Report.ExpenseReportId);
				UploadAttachments(model, report);
				await UploadItems(model, report);
			}

			return RedirectToAction("Index");
		}
	}
}