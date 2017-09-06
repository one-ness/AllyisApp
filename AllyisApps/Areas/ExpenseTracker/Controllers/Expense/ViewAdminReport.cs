using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using AllyisApps.Areas.ExpenseTracker.ViewModels.Expense;


namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
    public partial class ExpenseController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewDataButton"></param>
        /// <param name="model"></param>
        /// <param name="subscriptionId"></param>
        /// <param name="organizationId"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public ActionResult ViewAdminReport(string viewDataButton, AdminReportModel model, int subscriptionId, int organizationId, DateTime? StartDate, DateTime? EndDate)
        {
            AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.AdminReport, subscriptionId);


            var selectedUsers = model.Selection != null ? model.Selection.SelectedUsers : new List<int>();
            var selectedStatus = model.Selection != null ? model.Selection.Status : new List<int>();

            if (viewDataButton.Equals(Strings.Preview))
            {
                AdminReportSelectionModel adminRVMSelect = new AdminReportSelectionModel
                {
                    EndDate = EndDate,
                    StartDate = StartDate,
                    SelectedUsers = model.Selection.SelectedUsers != null ? model.Selection.SelectedUsers : new List<int>(),
                    Status = model.Selection.Status
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
                    dataVM = ConstructAdminDataExportViewModel(subscriptionId, organizationId, selectedUsers.ToList(), selectedStatus, StartDate, EndDate);
                }
                catch(Exception e)
                {
                    string message = Strings.CannotCreateReport;
                    if(e.Message != null)
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
            else if(viewDataButton.Equals(Strings.Export))
            {
                return this.ExportExpenseReport(subscriptionId, organizationId, model, StartDate, EndDate);
            }

            return RedirectToAction(ActionConstants.AdminReport);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        /// <param name="selectedStatus"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExpenseDataExportViewModel ConstructAdminDataExportViewModel(int subscriptionId, int organizationId, List<int> userId, List<int> selectedStatus, DateTime? startDate = null, DateTime? endDate = null)
        {
            List<ExpenseReport> expenses = new List<ExpenseReport>();

            DateTime start = startDate != null ? startDate.Value : DateTime.UtcNow;
            DateTime end = endDate != null ? endDate.Value : DateTime.UtcNow;

            foreach(var user in userId)
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