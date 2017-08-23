﻿using AllyisApps.Controllers;
using AllyisApps.Services;
using System.Web.Mvc;
using System.Threading.Tasks;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Collections.Generic;
using System;
using System.Linq;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Finance;

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
		/// <param name="items"></param>
		/// <returns></returns>
		public ActionResult UpdateReport(int subscriptionId, int submittedById, int reportId, List<ExpenseItem> items, string reportName = "", string businessJustification = "")
		{
			var subscription = AppService.GetSubscription(subscriptionId);
			var organizationId = subscription.OrganizationId;

			var oldReport = AppService.GetExpenseReport(reportId);
			DateTime date = DateTime.UtcNow;

			if (oldReport.ReportStatus == (int)ExpenseStatusEnum.Draft)
			{
				var report = new ExpenseReport()
				{
					ReportTitle = reportName,
					BusinessJustification = businessJustification,
					CreatedUtc = date,
					ModifiedUtc = DateTime.UtcNow,
					ReportDate = date,
					SubmittedById = submittedById,
					OrganizationId = organizationId,
					//ReportStatus = (int)Enum.Parse(typeof(ExpenseStatusEnum), Request.Form["Report.ReportStatus"])
				};

				foreach (var item in items)
				{
					item.AccountId = submittedById;
					item.ExpenseItemCreatedUtc = Convert.ToDateTime(item.TransactionDate);
					item.ExpenseItemModifiedUtc = DateTime.UtcNow;
					item.ExpenseReportId = reportId;
					AppService.UpdateExpenseItem(item);
				}

				AppService.UpdateExpenseReport(report, reportId);
			}

			return RedirectToAction("Index");
		}
	}
}