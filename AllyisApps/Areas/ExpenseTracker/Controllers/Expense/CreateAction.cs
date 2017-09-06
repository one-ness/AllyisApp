using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Web;
using AllyisApps.Services.Expense;
using System.IO;
using AllyisApps.Lib;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Linq.Expressions;

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
			UserSubscription subInfo = this.AppService.UserContext.UserSubscriptions[subscriptionId];
			ViewBag.SubscriptionName = subInfo.SubscriptionName;
			ViewData["SubscriptionId"] = subscriptionId;

			ViewData["IsManager"] = subInfo.ProductRoleId == 2;

			ExpenseReport report = reportId == -1 ? null : AppService.GetExpenseReport(reportId);
            var userInfo = GetCookieData();
            if (reportId != -1)
            {
                if(report != null
					&& (report.SubmittedById != userInfo.UserId 
					|| ((ExpenseStatusEnum)report.ReportStatus != ExpenseStatusEnum.Draft 
					&& (ExpenseStatusEnum)report.ReportStatus != ExpenseStatusEnum.Rejected)))
				{
					string message = string.Format("action {0} denied", AppService.ExpenseTrackerAction.EditReport.ToString());
					throw new AccessViolationException(message);
				}
				AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.EditReport, subscriptionId);
			}
            else
            {
                AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Unmanaged, subscriptionId);
            }
            

            
			IList<ExpenseItem> items = new List<ExpenseItem>();
			if (report != null)
			{
				items = AppService.GetExpenseItemsByReportId(reportId);
			}

			var model = new ExpenseCreateModel()
			{
				CurrentUser = GetCookieData().UserId,
				Items = items,
				StartDate = DateTime.UtcNow,
				SubscriptionId = subscriptionId,
				Report = report,
				Files = null
			};
			return View(model);
		}
	}
}