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
		/// <param name="subscriptionId"></param>
		/// <param name="submittedById"></param>
		/// <param name="reportName"></param>
		/// <param name="businessJustification"></param>
		/// <param name="date"></param>
		/// <param name="items"></param>
		/// <returns></returns>
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
				item.AccountId = 1;
				item.ExpenseItemCreatedUtc = item.TransactionDate;
				item.ExpenseItemModifiedUtc = item.TransactionDate;
				item.ExpenseReportId = reportId;
				AppService.CreateExpenseItem(item);
			}

			return RedirectToAction("Index");
		}
	}
}