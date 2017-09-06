using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Areas.ExpenseTracker.ViewModels.Expense;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
    /// <summary>
    /// The Expense controller.
    /// </summary>
    public partial class ExpenseController : BaseController
    {
        /// <summary>
        /// The action to view a admin report.
        /// </summary>
        /// <param name="viewDataButton">The type of button clicked.</param>
        /// <param name="model">The view model.</param>
        /// <param name="subscriptionId">The subscription id.</param>
        /// <param name="organizationId">The oranization id.</param>
        /// <param name="startDate">The start date range.</param>
        /// <param name="endDate">The end date range.</param>
        /// <returns>Returns File or a Redirect to the admin report page.</returns>
        public ActionResult ViewAdminReport(string viewDataButton, AdminReportModel model, int subscriptionId, int organizationId, DateTime? startDate, DateTime? endDate)
        {
            AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.AdminReport, subscriptionId);

            var selectedUsers = model.Selection != null ? model.Selection.SelectedUsers : new List<int>() { GetCookieData().UserId };
            var selectedStatus = model.Selection != null ? model.Selection.Status : new List<int> { 1, 2, 3, 4 };

            if (viewDataButton.Equals(Strings.Preview))
            {
                AdminReportSelectionModel adminRVMSelect = new AdminReportSelectionModel
                {
                    EndDate = endDate,
                    StartDate = startDate,
                    SelectedUsers = selectedUsers,
                    Status = selectedStatus
                };

                var infos = AppService.GetReportInfo(subscriptionId);
                UserSubscription subInfo = null;
                AppService.UserContext.UserSubscriptions.TryGetValue(subscriptionId, out subInfo);

                AdminReportModel adminReportVM = CreateAdminReportModel(subscriptionId);
                adminReportVM.SubscriptionName = subInfo.SubscriptionName;
                adminReportVM.Selection = adminRVMSelect;

                ExpenseDataExportViewModel dataVM = null;
                try
                {
                    dataVM = ConstructAdminDataExportViewModel(subscriptionId, organizationId, selectedUsers.ToList(), selectedStatus, startDate, endDate);
                }
                catch (Exception e)
                {
                    string message = Strings.CannotCreateReport;
                    if (e.Message != null)
                    {
                        message = string.Format("{0} {1}", message, e.Message);
                    }

                    Notifications.Add(new BootstrapAlert(message, Variety.Danger));
                    return RedirectToAction(ActionConstants.AdminReport, ControllerConstants.Expense);
                }

                adminReportVM.PreviewReports = dataVM.PreviewData;
                this.TempData["ARVM"] = adminReportVM;
                return this.RedirectToAction(ActionConstants.AdminReport);
            }
            else if (viewDataButton.Equals(Strings.Export))
            {
                return this.ExportExpenseReport(subscriptionId, organizationId, model, startDate, endDate);
            }

            return RedirectToAction(ActionConstants.AdminReport);
        }

        /// <summary>
        /// Makes an admin report view from provided data.
        /// </summary>
        /// <param name="subscriptionId">The subscription id.</param>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="userId">The selected users..</param>
        /// <param name="selectedStatus">The selected statuses.</param>
        /// <param name="startDate">The start date range.</param>
        /// <param name="endDate">The end date range.</param>
        /// <returns>An ExpenseDataExportViewModel.</returns>
        public ExpenseDataExportViewModel ConstructAdminDataExportViewModel(int subscriptionId, int organizationId, List<int> userId, List<int> selectedStatus, DateTime? startDate = null, DateTime? endDate = null)
        {
            List<ExpenseReport> expenses = new List<ExpenseReport>();

            DateTime start = startDate != null ? startDate.Value : DateTime.UtcNow;
            DateTime end = endDate != null ? endDate.Value : DateTime.UtcNow;

            foreach (var user in userId)
            {
                var reports = AppService.GetExpenseReportBySubmittedId(user).Select(x => x).Where(x => DateTime.Compare(x.CreatedUtc, start) >= 0 && DateTime.Compare(x.CreatedUtc, end) <= 0);
                 reports = reports.Select(x => x).Where(y => selectedStatus.IndexOf(y.ReportStatus) != -1);
                expenses.AddRange(reports);
            }      

            ExpenseDataExportViewModel model = new ExpenseDataExportViewModel()
            {
                Data = expenses,
                PreviewData = expenses
            };

            return model;
        }
    }
}