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
		/// <param name="itemCount"></param>
		/// <returns>Returns an action result.</returns>
		public ActionResult Create(int subscriptionId, int reportId = -1, int itemCount = 0)
		{
			ExpenseReport report = reportId == -1 ? null : AppService.GetExpenseReport(reportId);
			IList<ExpenseItem> items = new List<ExpenseItem>();
			if (report == null)
			{
				items = new List<ExpenseItem>();
				for (int i = 0; i < itemCount; i++)
				{
					items.Add(new ExpenseItem());
				}
			}
			else
			{
				items = AppService.GetExpenseItemsByReportId(reportId);
			}
			//IList<ExpenseItem> items = report == null ? new List<ExpenseItem>() : AppService.GetExpenseItemsByReportId(reportId);

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