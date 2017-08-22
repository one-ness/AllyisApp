using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Lib;
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
		/// View/export expense report.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="submittedById">The submitted by id.</param>
		///  <param name="items">The expense items.</param>
		/// <param name="date">The date.</param>
		/// <param name="files"></param>
		/// <param name="reportName">The report name.</param>
		/// <param name="businessJustification">The justification.</param>
		/// <returns>An action result.</returns>
		public ActionResult CreateReport(int subscriptionId, int submittedById, string date, IEnumerable<HttpPostedFileBase> files = null, List<ExpenseItem> items = null, string reportName = "", string businessJustification = "")
		{
			if (items == null)
			{
				items = new List<ExpenseItem>();
			}
			var subscription = AppService.GetSubscription(subscriptionId);
			var organizationId = subscription.OrganizationId;
			
			var report = new ExpenseReport()
			{
				ReportTitle = reportName,
				BusinessJustification = businessJustification,
				CreatedUtc = Convert.ToDateTime(date),
				ModifiedUtc = Convert.ToDateTime(date),
				ReportDate = Convert.ToDateTime(date),
				SubmittedById = submittedById,
				OrganizationId = organizationId
			};

			int reportId = AppService.CreateExpenseReport(report);

			foreach (var item in items)
			{
				item.AccountId = item.AccountId;
				item.ExpenseItemCreatedUtc = Convert.ToDateTime(item.TransactionDate);
				item.ExpenseItemModifiedUtc = Convert.ToDateTime(item.TransactionDate);
				item.ExpenseReportId = reportId;
				AppService.CreateExpenseItem(item);
			}

			if (files != null && files.ToList().Count > 0 && files.First() != null)
			{
				foreach (var upload in files)
				{
                    AzureFiles.SaveReportAttachments(reportId, upload.InputStream, upload.FileName);
				}
			}
			return RedirectToAction("Index");
		}
	}
}