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
        /// <param name="subscriptionId"></param>
        /// <param name="organizationId"></param>
        /// <param name="model"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ActionResult ExportExpenseReport(int subscriptionId, int organizationId, AdminReportModel model, DateTime? startDate = null, DateTime? endDate = null)
        {
            List<ExpenseReport> expenses = new List<ExpenseReport>();

            DateTime start = startDate != null ? startDate.Value : DateTime.UtcNow;
            DateTime end = endDate != null ? endDate.Value : DateTime.UtcNow;

            foreach (var user in model.Selection.SelectedUsers)
            {
                var reports = AppService.GetExpenseReportBySubmittedId(user).Select(x => x).Where(x => DateTime.Compare(x.CreatedUtc, start) >= 0 && DateTime.Compare(x.CreatedUtc, end) <= 0);
                reports = reports.Select(x => x).Where(y => model.Selection.Status.IndexOf(y.ReportStatus) != -1);
                expenses.AddRange(reports);
            }


            return File(AppService.PrepareExpenseCSVExport(organizationId, expenses, start, end).BaseStream, "text/csv", "export.csv");
        }
    }
}