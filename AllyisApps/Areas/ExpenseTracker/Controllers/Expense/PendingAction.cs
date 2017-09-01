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
using System.Linq;

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
		/// <returns>Returns an action result.</returns>
		public ActionResult Pending(int subscriptionId)
		{
			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Pending, subscriptionId);
			UserSubscription subInfo = this.AppService.UserContext.UserSubscriptions[subscriptionId];
			ViewBag.SubscriptionName = subInfo.SubscriptionName;
			ViewData["SubscriptionId"] = subscriptionId;

			ViewData["IsManager"] = subInfo.ProductRoleId == 2;

			if (subInfo.ProductRoleId != 2)
			{
				return RedirectToAction("Index");
			}

			var orgId = AppService.GetSubscription(subscriptionId).OrganizationId;
			int userId = GetCookieData().UserId;
			IEnumerable<ExpenseReport> reports = AppService.GetExpenseReportByOrgId(orgId);
			IEnumerable<ExpenseReport> pending = reports.ToList().Where(r => r.ReportStatus == (int)ExpenseStatusEnum.Pending);
			ExpensePendingModel model = InitializeViewModel(subscriptionId, userId, pending);
			return View(model);
		}

		/// <summary>
		/// Initializes the pending page view model.
		/// </summary>
		/// <param name="subId"></param>
		/// <param name="userId"></param>
		/// <param name="reports"></param>
		/// <returns></returns>
		public ExpensePendingModel InitializeViewModel(int subId, int userId, IEnumerable<ExpenseReport> reports)
		{
			List<ExpenseItemViewModel> reportModels = new List<ExpenseItemViewModel>();
			foreach (ExpenseReport report in reports)
			{
				var expItems = AppService.GetExpenseItemsByReportId(report.ExpenseReportId);

				var user = AppService.GetUser(report.SubmittedById);

				decimal totalAmount = expItems.Sum(x => x.Amount);

				reportModels.Add(new ExpenseItemViewModel()
				{
					Amount = totalAmount,
					Reason = report.BusinessJustification,
					ReportId = report.ExpenseReportId,
					ReportName = report.ReportTitle,
					Status = (ExpenseStatusEnum)report.ReportStatus,
					SubmittedDate = report.SubmittedUtc,
					UserId = user.userInfo.UserId,
					UserName = user.userInfo.FirstName + " " + user.userInfo.LastName
				});
			}
			ExpensePendingModel model = new ExpensePendingModel()
			{
				PendingReports = reportModels
			};
			return model;
		}
	}
}