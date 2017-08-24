using AllyisApps.Controllers;
using AllyisApps.Services;
using System.Web.Mvc;
using System.Threading.Tasks;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Collections.Generic;
using System;
using System.Linq;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Finance;
using System.Web;
using AllyisApps.Lib;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// update expense report
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <param name="submittedById"></param>
		/// <param name="reportName"></param>
		/// <param name="businessJustification"></param>
		/// <param name="reportId"></param>
		/// <param name="submitType"></param>
		/// <param name="files"></param>
		/// <param name="items"></param>
		/// <returns></returns>
		public ActionResult UpdateReport(int subscriptionId, int submittedById, int reportId, string submitType, IEnumerable<HttpPostedFileBase> files = null, List<ExpenseItem> items = null, string reportName = "", string businessJustification = "")
		{
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

			var oldReport = AppService.GetExpenseReport(reportId);

			if (oldReport.ReportStatus == (int)ExpenseStatusEnum.Draft)
			{
				var report = new ExpenseReport()
				{
					ReportTitle = reportName,
					BusinessJustification = businessJustification,
					ModifiedUtc = DateTime.UtcNow,
					SubmittedUtc = submittedUtc,
					SubmittedById = submittedById,
					OrganizationId = organizationId,
					ReportStatus = (int)reportStatus
				};

				foreach (var item in items)
				{
					item.AccountId = submittedById;
					item.ExpenseItemCreatedUtc = Convert.ToDateTime(item.TransactionDate);
					item.ExpenseItemModifiedUtc = DateTime.UtcNow;
					item.ExpenseReportId = reportId;
					AppService.UpdateExpenseItem(item);
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

				AppService.UpdateExpenseReport(report, reportId);
			}

			return RedirectToAction("Index");
		}
	}
}