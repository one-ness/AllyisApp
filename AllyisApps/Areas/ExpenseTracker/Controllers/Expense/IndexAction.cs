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
                    new ExpenseReportModel() {Amount = 250.01, Status = ReportStatusEnum.Pending, SubmittedDate = DateTime.UtcNow, ReportName = "Test Report"}
                }
            });
		}

		///// <summary>
		///// return create expense report page
		///// </summary>
		///// <returns></returns>
		//public ActionResult Create()
		//{
		//	return View();
		//}

		/// <summary>
		/// return expense report
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult Create(ExpenseReportsViewModel model)
		{
			return View(model);
		}
	}
}
