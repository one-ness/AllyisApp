using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
    /// <summary>
    /// The ExpenseController class with ReportView actions.
    /// </summary>
    public partial class ExpenseController : BaseController
    {
        /// <summary>
        /// The ReportView Action.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <returns>The view with selected report details.</returns>
        public ActionResult ReportView(int reportId)
        {
            var model = InitializeReportViewModel(reportId);

            return View(model);
        }

        /// <summary>
        /// Initializes the ReportViewModel.
        /// </summary>
        /// <param name="id">The report id.</param>
        /// <returns>The view model.</returns>
        public ReportViewModel InitializeReportViewModel(int id)
        {
            var report = AppService.GetExpenseReport(id);
            var reportItems = AppService.GetExpenseItemsByReportId(id);
            var user = AppService.GetUser(report.SubmittedById);

            return new ReportViewModel()
            {
                ReprortTitle = report.ReportTitle,
                SubmittedBy = string.Format("{0} {1}", user.FirstName, user.LastName),
                CreatedUtc = report.CreatedUtc,
                ModifiedUtc = report.ModifiedUtc,
                Justification = report.BusinessJustification,
                ReportDate = report.ReportDate,
                Status = (ExpenseStatusEnum)report.ReportStatus,
                Expenses = reportItems
            };
        }
    }
}