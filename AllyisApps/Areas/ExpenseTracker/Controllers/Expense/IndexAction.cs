using AllyisApps.Controllers;
using System.Web.Mvc;
using System.Threading.Tasks;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Collections.Generic;
using System;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// expense controller
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// show the list of expense reports submitted by the logged in user
		/// </summary>
		public ActionResult Index(int subscriptionId)
		{
			return View(new ExpenseReportsViewModel()
            {
                SubscriptionId = 111111,
                CanManage = true,
                CurrentUser = 1111111,
                EndDate = 1111111,
                StartDate = 01111111,
                ProductRole = 2,
                Reports= new List<ExpenseReportModel>()
                {
                    new ExpenseReportModel() {ReportId = 000001, Amount = 250.01, Status = ReportStatusEnum.Rejected, SubmittedDate = DateTime.UtcNow, ReportName = "Kichenette Supplies"},
                    new ExpenseReportModel() {ReportId = 000002, Amount = 10000.92, Status = ReportStatusEnum.Pending, SubmittedDate = DateTime.UtcNow, ReportName = "Company Scooter"},
                    new ExpenseReportModel() {ReportId = 000003, Amount = 7356.11, Status = ReportStatusEnum.Accepted, SubmittedDate = DateTime.UtcNow, ReportName = "Laptop Order"},
                    new ExpenseReportModel() {ReportId = 000004, Amount = 20.33, Status = ReportStatusEnum.Pending, SubmittedDate = DateTime.UtcNow, ReportName = "New Batteries"},
                    new ExpenseReportModel() {ReportId = 000005, Amount = 92.31, Status = ReportStatusEnum.Pending, SubmittedDate = DateTime.UtcNow, ReportName = "IT Lunch Meeting"},
                    new ExpenseReportModel() {ReportId = 000006, Amount = 2566355.00, Status = ReportStatusEnum.Rejected, SubmittedDate = DateTime.UtcNow, ReportName = "Toga Party"},
                    new ExpenseReportModel() {ReportId = 000007, Amount = 477.63, Status = ReportStatusEnum.Accepted, SubmittedDate = DateTime.UtcNow, ReportName = "Exec Lunch Meeting"},
                }
            });
		}
	}
}
