using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
    /// <summary>
    /// The expense controller.
    /// </summary>
    public partial class ExpenseController : BaseController
    {
        /// <summary>
        /// The Admin report action.
        /// </summary>
        /// <param name="subscriptionId">The subscription id.</param>
        /// <returns>A admin report view model.</returns>
        public ActionResult AdminReport(int subscriptionId)
        {
			ViewData["SubscriptionId"] = subscriptionId;

            ViewData["IsManager"] = AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.AdminReport, subscriptionId);

            UserContext.SubscriptionAndRole subInfo = this.AppService.UserContext.SubscriptionsAndRoles[subscriptionId];

            ViewData["SubscriptionName"] = AppService.getSubscriptionName(subInfo.SubscriptionId);

            AdminReportModel adminReportVM = null;

            string tempDataKey = "ARVM";

            if (this.TempData[tempDataKey] != null)
            {
                adminReportVM = (AdminReportModel)TempData[tempDataKey];
            }
            else
            {
                adminReportVM = CreateAdminReportModel(subscriptionId);
            }

            return View(adminReportVM);
        }

        /// <summary>
        /// Creates an admin report view model.
        /// </summary>
        /// <param name="subId">The subscription id.</param>
        /// <returns>An admin report model.</returns>
        public AdminReportModel CreateAdminReportModel(int subId)
        {
            var subInfo = AppService.GetSubscription(subId);
            var reportInfo = AppService.GetReportInfo(subId);
            var reports = AppService.GetExpenseReportByOrgId(subInfo.OrganizationId);
            var users = reportInfo.Item3;
            List<SelectListItem> enumList = new List<SelectListItem>();
            List<SelectListItem> userList = new List<SelectListItem>();

            foreach (var item in Enum.GetValues(typeof(ExpenseStatusEnum)))
            {
                int value = 0;
                switch (item.ToString())
                {
                    // We dont want reports on rough drafts for now.
                    case "Pending":
                        value = 1;
                        break;
                    case "Approved":
                        value = 2;
                        break;
                    case "Rejected":
                        value = 3;
                        break;
                    case "Paid":
                        value = 4;
                        break;
                    default:
                        break;
                }

                if (value != 0)
                {
                    enumList.Add(new SelectListItem()
                    {
                        Disabled = false,
                        Text = item.ToString(),
                        Value = value.ToString(),
                        Selected = string.Equals("1", value.ToString()) // Set pending as the default status.
                    });
                }
            }

            foreach (var user in users)
            {
                userList.Add(new SelectListItem()
                {
                    Disabled = false,
                    Text = string.Format("{0} {1}", user.FirstName, user.LastName),
                    Value = user.UserId.ToString(),
                    Selected = false
                });
            }

            AdminReportModel model = new AdminReportModel()
            {
                CanManage = true,
                OrganizationId = subInfo.OrganizationId,
                Reports = reports,
                PreviewReports = null,
                ShowExport = true,
                Statuses = enumList,
                SubscriptionId = subInfo.SubscriptionId,
                SubscriptionName = subInfo.Name,
                UserId = GetCookieData().UserId,
                Users = userList,
                Selection = new AdminReportSelectionModel
                {
                    EndDate = DateTime.Today.AddDays(7),
                    StartDate = DateTime.Today,
                    SelectedUsers = new List<int>(),
                    Status = new List<int>()
                }
            };

            return model;
        }
    }
}