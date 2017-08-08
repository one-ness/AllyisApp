using AllyisApps.Controllers;
using AllyisApps.Services;
using System.Web.Mvc;
using System.Threading.Tasks;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Collections.Generic;
using System;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Finance;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// expense controller
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Show the list of expense reports submitted by the logged in user
		/// </summary>
		public ActionResult Index(int subscriptionId)
		{
            int userId = GetCookieData().UserId;

            var items = AppService.GetExpenseReportBySubmittedId(userId);


			return View();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subId"></param>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pRole"></param>
        /// <param name="expenses"></param>
        /// <returns></returns>
        public ExpenseIndexViewModel InitializeViewModel(int subId, int userId, int startDate, int endDate, int pRole, IEnumerable<ExpenseItem> expenses)
        {
            List<ExpenseItemViewModel> items = new List<ExpenseItemViewModel>();

            foreach( var item in expenses)
            {
                var compInfo = AppService.GetExpenseItem(item.ExpenseItemId);
                var user = AppService.GetUser(item.AccountId);
                items.Add(new ExpenseItemViewModel()
                {
                    Amount = compInfo.Amount,
                    ReportId = item.ExpenseReportId,
                    Reason = item.ItemDiscription,
                    ReportName = item.ExpenseItemName,
                    SubmittedDate = item.TransactionDate,
                    UserId = item.AccountId,
                    Status = item.ExpenseReportStatus,
                    UserName = user.FirstName + " " + user.LastName
                });
            }


            ExpenseIndexViewModel model = new ExpenseIndexViewModel()
            {
                CanManage = true,
                CurrentUser = userId,
                StartDate = startDate,
                EndDate = endDate,
                ProductRole = pRole
            };
            return model;
        }
		/// <summary>
		/// create expense report
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult Create(ExpenseIndexViewModel model)
		{
			return View(model);
		}

		/// <summary>
		/// view/export expense report
		/// </summary>
		/// <param name="reportName"></param>
		/// <param name="businessJustification"></param>
		/// <param name="date"></param>
		/// <param name="items"></param>
		/// <returns></returns>
		public ActionResult ViewReport(string reportName, string businessJustification, List<ExpenseItem> items, DateTime? date)
		{
			foreach (var item in items)
			{
				AppService.CreateExpenseItem(item);
			}

			var report = new ExpenseItemViewModel()
			{
				ReportName = reportName,
				SubmittedDate = date == null ? DateTime.Now.Date : (DateTime)date,
				Reason = businessJustification,
				Status = ExpenseStatusEnum.New,
				Items = items
			};

			//AppService.CreateExpenseReport(report);

			List<ExpenseItemViewModel> reports = new List<ExpenseItemViewModel>();

			reports.Add(report);

			ExpenseIndexViewModel model = new ExpenseIndexViewModel()
			{
				Reports = reports
			};
			return RedirectToAction("Index");
		}
	}
}