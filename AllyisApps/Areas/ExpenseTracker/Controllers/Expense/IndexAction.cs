using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Expense controller.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Show the list of expense reports submitted by the logged in user.
		/// </summary>
        /// <param name="subscriptionId">The subscription id.</param>
        /// <returns>The action result.</returns>
		public ActionResult Index(int subscriptionId)
		{
            AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Unmanaged, subscriptionId);
            int userId = GetCookieData().UserId;

            UserContext.SubscriptionAndRole subInfo = this.AppService.UserContext.SubscriptionsAndRoles[subscriptionId];

            var items = AppService.GetExpenseReportBySubmittedId(userId).Select(x => x).Where(y => y.OrganizationId == subInfo.OrganizationId);

            ViewBag.SubscriptionName = AppService.getSubscriptionName(subscriptionId);

			ViewData["SubscriptionId"] = subInfo.SubscriptionId;

			ViewData["IsManager"] = subInfo.ProductRoleId == 2;

			return View(InitializeViewModel(subscriptionId, userId, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), items));
		}

        /// <summary>
        /// Initializes the home page view model.
        /// </summary>
        /// <param name="subId">The subscription id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="expenses">The expenses.</param>
        /// <returns>Returns the view model.</returns>
        public ExpenseIndexViewModel InitializeViewModel(int subId, int userId, DateTime startDate, DateTime endDate, IEnumerable<ExpenseReport> expenses)
        {
            List<ExpenseItemViewModel> items = new List<ExpenseItemViewModel>();

            foreach (var item in expenses)
            {
				var expItems = AppService.GetExpenseItemsByReportId(item.ExpenseReportId);
				
                var user = AppService.GetUser(item.SubmittedById);

                decimal totalAmount = expItems.Sum(x => x.Amount);

                items.Add(new ExpenseItemViewModel()
                {
                    Amount = totalAmount,
                    Reason = item.BusinessJustification,
                    ReportId = item.ExpenseReportId,
                    ReportName = item.ReportTitle,
                    Status = (ExpenseStatusEnum)item.ReportStatus,
                    SubmittedDate = item.SubmittedUtc,
                    UserId = user.userInfo.UserId,
                    UserName = user.userInfo.FirstName + " " + user.userInfo.LastName
                });
            }

            ExpenseIndexViewModel model = new ExpenseIndexViewModel()
            {
                CanManage = true,
                CurrentUser = userId,
                StartDate = startDate,
                Reports = items,
                EndDate = endDate,
            };

            return model;
        }
	}
}