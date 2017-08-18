﻿using System;
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
            int userId = GetCookieData().UserId;

            var items = AppService.GetExpenseReportBySubmittedId(userId);

            UserSubscription subInfo = null;
            this.AppService.UserContext.OrganizationSubscriptions.TryGetValue(subscriptionId, out subInfo);

            ViewBag.SubscriptionName = subInfo.SubscriptionName;

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
				var expItems = AppService.GetExpenseItemsByUserSubmitted(item.SubmittedById).Select(x => x).Where(x => x.ExpenseReportId == item.ExpenseReportId);
				
                var user = AppService.GetUser(item.SubmittedById);

                decimal totalAmount = expItems.Sum(x => x.Amount);

                items.Add(new ExpenseItemViewModel()
                {
                    Amount = totalAmount,
                    Reason = item.BusinessJustification,
                    ReportId = item.ExpenseReportId,
                    ReportName = item.ReportTitle,
                    Status = (ExpenseStatusEnum)item.ReportStatus,
                    SubmittedDate = item.CreatedUtc,
                    UserId = user.UserId,
                    UserName = user.FirstName + " " + user.LastName
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