using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Lib;
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
		/// View/export expense report.
		/// </summary>
		/// <param name="model"></param>
		/// <returns>An action result.</returns>
		[HttpPost]
		public ActionResult CreateReport(ExpenseCreateModel model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Create", new { subscriptionId = model.SubscriptionId, reportId = model.Report.ExpenseReportId });
			}

			var userInfo = GetCookieData();
			if (userInfo.UserId != model.CurrentUser)
			{
				string message = string.Format("action {0} denied", AppService.ExpenseTrackerAction.CreateReport.ToString());
				throw new AccessViolationException(message);
			}


			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Unmanaged, model.SubscriptionId);

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

			var report = new ExpenseReport()
			{
				ReportTitle = model.Report.ReportTitle,
				BusinessJustification = model.Report.BusinessJustification,
				CreatedUtc = DateTime.UtcNow,
				ModifiedUtc = DateTime.UtcNow,
				SubmittedUtc = submittedUtc,
				SubmittedById = model.CurrentUser,
				OrganizationId = organizationId,
				ReportStatus = (int)reportStatus
			};
			
			report.ExpenseReportId = AppService.CreateExpenseReport(report);
			UploadItems(model, report);
			UploadAttachments(model, report);

			return RedirectToAction("Index");
		}
	}
}