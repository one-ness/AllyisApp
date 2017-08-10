using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;

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
        /// <param name="reportName">The report name.</param>
        /// <param name="businessJustification">The justification.</param>
        ///  <param name="items">The expense items.</param>
        /// <param name="date">The date.</param>
        /// <returns>An action result.</returns>
        public ActionResult ViewReport(int subscriptionId, int submittedById, string reportName, string businessJustification, List<ExpenseItem> items, DateTime date)
		{
			var subscription = AppService.GetSubscription(subscriptionId);
			var organizationId = subscription.OrganizationId;

			var report = new ExpenseReport()
			{
				ReportTitle = reportName,
				BusinessJustification = businessJustification,
				CreatedUtc = date,
				ModifiedUtc = date,
				ReportDate = date,
				SubmittedById = submittedById,
				OrganizationId = organizationId
			};

			var reportId = AppService.CreateExpenseReport(report);

			foreach (var item in items)
			{
				item.AccountId = submittedById;
				item.ExpenseItemCreatedUtc = item.TransactionDate;
				item.ExpenseItemModifiedUtc = item.TransactionDate;
				item.ExpenseReportId = reportId;
				AppService.CreateExpenseItem(item);
			}

			return RedirectToAction("Index");
		}
	}
}