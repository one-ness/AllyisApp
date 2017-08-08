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

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// view/export expense report
		/// </summary>
		/// <param name="reportName"></param>
		/// <param name="businessJustification"></param>
		/// <param name="date"></param>
		/// <param name="items"></param>
		/// <returns></returns>
		public ActionResult ViewReport(string reportName, string businessJustification, List<ExpenseItem> items, DateTime date)
		{
			var report = new ExpenseReport()
			{
				ReportTitle = reportName,
				BusinessJustification = businessJustification,
				CreatedUtc = date,
				ModifiedUtc = date,
				ReportDate = date,
				SubmittedById = 1,
				OrganizationId = 1
			};

			AppService.CreateExpenseReport(report);

			foreach (var item in items)
			{
				item.AccountId = 1;
				item.ExpenseItemCreatedUtc = item.TransactionDate;
				item.ExpenseItemModifiedUtc = item.TransactionDate;
				item.ExpenseReportId = report.ExpenseReportId;
				AppService.CreateExpenseItem(item);
			}

			return RedirectToAction("Index");
		}
	}
}