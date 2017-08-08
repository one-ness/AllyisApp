using AllyisApps.Controllers;
using AllyisApps.Services;
using System.Web.Mvc;
using System.Threading.Tasks;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Collections.Generic;
using System;
using System.Linq;
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

			return View(InitializeViewModel(subscriptionId, userId, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), items));
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subId"></param>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="expenses"></param>
        /// <returns></returns>
        public ExpenseIndexViewModel InitializeViewModel(int subId, int userId, DateTime startDate, DateTime endDate, IEnumerable<ExpenseReport> expenses)
        {
            List<ExpenseItemViewModel> items = new List<ExpenseItemViewModel>();

            foreach( var item in expenses)
            {
                var compInfo = AppService.GetExpenseItemsByUserSubmitted(item.SubmittedById).Select(x => x).Where(x => x.ExpenseReportId == item.ExpenseReportId).First();
                var user = AppService.GetUser(item.SubmittedById);
                items.Add(new ExpenseItemViewModel()
                {
                    Amount = compInfo.Amount,
                    Reason = item.BusinessJustification,
                    ReportId = item.ExpenseReportId,
                    ReportName = item.ReportTitle,
                    Status = (ExpenseStatusEnum)item.ReportStatus,
                    SubmittedDate = item.CreatedUtc,
                    UserId = compInfo.AccountId,
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