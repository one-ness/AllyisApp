using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Web;
using AllyisApps.Services.Expense;
using System.IO;

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
		/// <param name="fileCount"></param>
		/// <returns>Returns an action result.</returns>
		public ActionResult Create(int subscriptionId, int reportId = -1, int itemCount = 0, int fileCount = 0)
		{
			ExpenseReport report = reportId == -1 ? null : AppService.GetExpenseReport(reportId);
			IList<ExpenseItem> items = new List<ExpenseItem>();
			IList<ExpenseFile> files = new List<ExpenseFile>();
			if (report == null)
			{
				items = new List<ExpenseItem>();
				for (int i = 0; i < itemCount; i++)
				{
					items.Add(new ExpenseItem());
				}
				files = new List<ExpenseFile>();
				for (int i = 0; i < fileCount; i++)
				{
					files.Add(new ExpenseFile(null, "", ""));
				}
			}
			else
			{
				items = AppService.GetExpenseItemsByReportId(reportId);
				files = AppService.GetExpenseFilesByReportId(reportId);
			}
			//IList<ExpenseItem> items = report == null ? new List<ExpenseItem>() : AppService.GetExpenseItemsByReportId(reportId);

			var model = new ExpenseCreateModel()
			{
				CurrentUser = GetCookieData().UserId,
				Items = items,
				StartDate = DateTime.UtcNow,
				SubscriptionId = subscriptionId,
				Report = report,
				Files = files
			};
			return View(model);
		}
	}
}