﻿using System;
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
		/// <param name="submitType"></param>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="submittedById">The submitted by id.</param>
		///  <param name="items">The expense items.</param>
		/// <param name="date">The date.</param>
		/// <param name="files"></param>
		/// <param name="reportName">The report name.</param>
		/// <param name="businessJustification">The justification.</param>
		/// <returns>An action result.</returns>
		[HttpPost]
		public ActionResult CreateReport(string submitType, int subscriptionId, int submittedById, string date, IEnumerable<HttpPostedFileBase> files = null, List<ExpenseItem> items = null, string reportName = "", string businessJustification = "")
		{
			if (items == null)
			{
				items = new List<ExpenseItem>();
			}
			var subscription = AppService.GetSubscription(subscriptionId);
			var organizationId = subscription.OrganizationId;
			ExpenseStatusEnum reportStatus; // = (ExpenseStatusEnum)Enum.Parse(typeof(ExpenseStatusEnum), Request.Form["Report.ReportStatus"]);
			if (submitType == "Submit")
			{
				reportStatus = ExpenseStatusEnum.Pending;
			}
			else
			{
				reportStatus = ExpenseStatusEnum.Draft;
			}

			var report = new ExpenseReport()
			{
				ReportTitle = reportName,
				BusinessJustification = businessJustification,
				CreatedUtc = DateTime.UtcNow,
				ModifiedUtc = DateTime.UtcNow,
				ReportDate = Convert.ToDateTime(date),
				SubmittedById = submittedById,
				OrganizationId = organizationId,
				ReportStatus = (int)reportStatus
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