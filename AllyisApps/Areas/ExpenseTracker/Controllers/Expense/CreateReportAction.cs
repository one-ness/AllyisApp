﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using System.Web;
using AllyisApps.Services.Expense;
using System.Linq;

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
		/// <param name="file"></param>
		/// <param name="files"></param>
		/// <param name="reportName">The report name.</param>
		/// <param name="businessJustification">The justification.</param>
		/// <returns>An action result.</returns>
		public ActionResult CreateReport(int subscriptionId, int submittedById, string date, HttpPostedFileBase file, IEnumerable<HttpPostedFileBase> files = null, List<ExpenseItem> items = null, string reportName = "", string businessJustification = "")
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
				item.AccountId = submittedById;
				item.ExpenseItemCreatedUtc = Convert.ToDateTime(item.TransactionDate);
				item.ExpenseItemModifiedUtc = Convert.ToDateTime(item.TransactionDate);
				item.ExpenseReportId = reportId;
				AppService.CreateExpenseItem(item);
			}

			var thing = file;

			if (files != null && files.ToList().Count > 0)
			{
				//return RedirectToAction("Import", new { subscriptionId = subscriptionId, files = files, reportId = reportId });
			}
			return RedirectToAction("Index");
		}
	}
}