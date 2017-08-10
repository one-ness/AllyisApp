using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Creates a new report.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Create expense report.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
        /// <param name="reportId">The report id.</param>
		/// <returns>Returns an action result.</returns>
		public ActionResult Create(int subscriptionId, int reportId = -1)
		{
			ExpenseReport report = reportId == -1 ? null : AppService.GetExpenseReport(reportId);
			List<ExpenseItem> items = report == null ? new List<ExpenseItem>() : AppService.GetExpenseItemsByReportId(reportId);

			var model = new ExpenseCreateModel()
			{
				CurrentUser = GetCookieData().UserId,
				Items = items,
				StartDate = DateTime.UtcNow,
				SubscriptionId = subscriptionId,
				Report = report
			};
			return View(model);
		}
	}
}