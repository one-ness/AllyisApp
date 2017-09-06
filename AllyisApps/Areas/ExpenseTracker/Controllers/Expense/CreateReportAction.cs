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
		/// <param name="model">The expense create model.</param>
		/// <param name="submitType">The type of submission.</param>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="submittedById">The submitted by id.</param>
		/// <param name="files">The attachment files.</param>
        /// <param name="items">The expense items.</param>
		/// <returns>An action result.</returns>
		[HttpPost]
		public ActionResult CreateReport(ExpenseCreateModel model, string submitType, int subscriptionId, int submittedById, IEnumerable<HttpPostedFileBase> files = null, List<ExpenseItem> items = null)
		{
			var userInfo = GetCookieData();
			if (userInfo.UserId != submittedById)
			{
				string message = string.Format("action {0} denied", AppService.ExpenseTrackerAction.CreateReport.ToString());
				throw new AccessViolationException(message);
			}

			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Unmanaged, subscriptionId);

            if (items == null)
			{
				items = new List<ExpenseItem>();
			}

			var subscription = AppService.GetSubscription(subscriptionId);
			var organizationId = subscription.OrganizationId;
			ExpenseStatusEnum reportStatus; // = (ExpenseStatusEnum)Enum.Parse(typeof(ExpenseStatusEnum), Request.Form["Report.ReportStatus"]);
			DateTime? submittedUtc = null;

			if (submitType == "Submit")
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
				SubmittedById = submittedById,
				OrganizationId = organizationId,
				ReportStatus = (int)reportStatus
			};

			int reportId = AppService.CreateExpenseReport(report);

			foreach (var item in items)
			{
				item.ExpenseReportId = reportId;
				AppService.CreateExpenseItem(item);
			}

			foreach (string name in AzureFiles.GetReportAttachments(report.ExpenseReportId))
			{
				AzureFiles.DeleteReportAttachment(report.ExpenseReportId, name);
			}

			if (files != null)
			{
				foreach (var file in files)
				{
					if (file != null)
					{
						AzureFiles.SaveReportAttachments(reportId, file.InputStream, file.FileName);
					}
				}
			}

			return RedirectToAction("Index");
		}
	}
}