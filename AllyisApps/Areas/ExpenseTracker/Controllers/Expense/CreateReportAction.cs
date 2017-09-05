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
			ExpenseStatusEnum reportStatus; // = (ExpenseStatusEnum)Enum.Parse(typeof(ExpenseStatusEnum), Request.Form["Report.ReportStatus"]);
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

			if (ValidateItems(model, report))
			{
				report.ExpenseReportId = AppService.CreateExpenseReport(report);
				UploadItems(model, report);
				UploadAttachments(model, report);
			}
			else
			{
				return RedirectToAction("Create", new { subscriptionId = model.SubscriptionId, reportId = model.Report.ExpenseReportId });
			}

			return RedirectToAction("Index");
		}

		private bool ValidateItems(ExpenseCreateModel model, ExpenseReport report)
		{
			// this should be handled with client-side validation
			foreach (var item in model.Items)
			{
				if (String.IsNullOrEmpty(item.ItemDescription) || String.IsNullOrEmpty(item.TransactionDate) || item.Amount == 0)
				{
					return false;
				}
			}
			return true;
		}

		private bool UploadItems(ExpenseCreateModel model, ExpenseReport report)
		{
			if (!ValidateItems(model, report))
			{
				return false;
			}

			IList<ExpenseItem> oldItems = AppService.GetExpenseItemsByReportId(report.ExpenseReportId);
			List<int> itemIds = new List<int>();
			foreach (ExpenseItem oldItem in oldItems)
			{
				itemIds.Add(oldItem.ExpenseItemId);
			}

			foreach (var item in model.Items)
			{
				item.ExpenseReportId = report.ExpenseReportId;
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

		private static void UploadAttachments(ExpenseCreateModel model, ExpenseReport report)
		{
			foreach (string name in AzureFiles.GetReportAttachments(report.ExpenseReportId))
			{
				AzureFiles.DeleteReportAttachment(report.ExpenseReportId, name);
			}
			if (model.Files != null)
			{
				foreach (var file in model.Files)
				{
					if (file != null)
					{
						AzureFiles.SaveReportAttachments(report.ExpenseReportId, file.InputStream, file.FileName);
					}
				}
			}
		}
	}
}