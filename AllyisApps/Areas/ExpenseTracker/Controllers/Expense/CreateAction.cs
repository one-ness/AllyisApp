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
	/// Creates a new report
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// create expense report
		/// </summary>
		/// <param name="reportId"></param>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
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