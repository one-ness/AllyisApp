using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
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
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="submittedById">Thes submitting user id.</param>
		/// <param name="reportId">The report id.</param>
		/// <param name="submitType">The submission type.</param>
		/// <param name="files">List of files.</param>
		/// <param name="previousFiles">List of previous files.</param>
		/// <param name="items">List of items.</param>
		/// <returns>A redirect to the home page.</returns>
		[HttpPost]
		public ActionResult UpdateReport(ExpenseCreateModel model, int subscriptionId, int submittedById, int reportId, string submitType, IEnumerable<HttpPostedFileBase> files = null, IEnumerable<string> previousFiles = null, List<ExpenseItem> items = null)
		{
			var oldReport = AppService.GetExpenseReport(reportId);
			var userInfo = GetCookieData();
			if (reportId != -1)
			{
				if (oldReport.SubmittedById != userInfo.UserId
					|| ((ExpenseStatusEnum)oldReport.ReportStatus != ExpenseStatusEnum.Draft
					&& (ExpenseStatusEnum)oldReport.ReportStatus != ExpenseStatusEnum.Rejected))
				{
					string message = string.Format("action {0} denied", AppService.ExpenseTrackerAction.UpdateReport.ToString());
					throw new AccessViolationException(message);
				}

				AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.EditReport, subscriptionId);
			}
			else
			{
				AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Unmanaged, subscriptionId);
			}

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

			if (oldReport.ReportStatus == (int)ExpenseStatusEnum.Draft || oldReport.ReportStatus == (int)ExpenseStatusEnum.Rejected)
			{
				var report = new ExpenseReport()
				{
					ReportTitle = model.Report.ReportTitle,
					BusinessJustification = model.Report.BusinessJustification,
					ModifiedUtc = DateTime.UtcNow,
					SubmittedUtc = submittedUtc,
					SubmittedById = submittedById,
					OrganizationId = organizationId,
					ReportStatus = (int)reportStatus
				};

				UploadAttachments(reportId, files, previousFiles);

				if (UploadItems(reportId, items))
				{
					AppService.UpdateExpenseReport(report, reportId);
				}
				else
				{
					return RedirectToAction("Create", new { subscriptionId = subscriptionId, reportId = reportId });
				}
			}

			return RedirectToAction("Index");
		}

		private bool UploadItems(int reportId, List<ExpenseItem> items)
		{
			// this should be handled with client-side validation
			foreach (var item in items)
			{
				if (string.IsNullOrEmpty(item.ItemDescription) || string.IsNullOrEmpty(item.TransactionDate) || item.Amount == 0)
				{
					return false;
				}
			}

			IList<ExpenseItem> oldItems = AppService.GetExpenseItemsByReportId(reportId);
			List<int> itemIds = new List<int>();
			foreach (ExpenseItem oldItem in oldItems)
			{
				itemIds.Add(oldItem.ExpenseItemId);
			}

			foreach (var item in items)
			{
				item.ExpenseReportId = reportId;
				if (itemIds.Contains(item.ExpenseItemId))
				{
					AppService.UpdateExpenseItem(item);
					itemIds.Remove(item.ExpenseItemId);
				}
				else
				{
					AppService.CreateExpenseItem(item);
				}
			}

			foreach (int itemId in itemIds)
			{
				AppService.DeleteExpenseItem(itemId);
			}

			return true;
		}

		private void UploadAttachments(int reportId, IEnumerable<HttpPostedFileBase> files, IEnumerable<string> previousFiles)
		{
			foreach (string name in AzureFiles.GetReportAttachments(reportId))
			{
				if (previousFiles != null && !previousFiles.Contains(name))
				{
					AzureFiles.DeleteReportAttachment(reportId, name);
				}
			}

			List<string> empty = AzureFiles.GetReportAttachments(reportId);
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
		}
	}
}