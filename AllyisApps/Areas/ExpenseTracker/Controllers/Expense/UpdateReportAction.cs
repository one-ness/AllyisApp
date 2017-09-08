using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Finance;
using AllyisApps.Lib;
using AllyisApps.Services;
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
		public ActionResult UpdateReport(ExpenseCreateModel model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Create", new { subscriptionId = model.SubscriptionId, reportId = model.Report.ExpenseReportId });
			}
			
			var oldReport = AppService.GetExpenseReport(model.Report.ExpenseReportId);
			var userInfo = GetCookieData();
			if (model.Report.ExpenseReportId != -1)
			{
				if (oldReport.SubmittedById != userInfo.UserId
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
				model.Items = new List<ExpenseItem>();
			}

			var subscription = AppService.GetSubscription(model.SubscriptionId);
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
				
				AppService.UpdateExpenseReport(report, model.Report.ExpenseReportId);
				UploadAttachments(model, report);
				UploadItems(model, report);
			}

			return RedirectToAction("Index");
		}
	}
}