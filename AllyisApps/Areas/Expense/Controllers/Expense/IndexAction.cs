using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Expense;
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
		public async Task<ActionResult> Index(int subscriptionId)
		{
			await SetNavData(subscriptionId);

			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Unmanaged, subscriptionId);

			int userId = AppService.UserContext.UserId;

			UserContext.SubscriptionAndRole subInfo = AppService.UserContext.SubscriptionsAndRoles[subscriptionId];
			var results = await AppService.GetExpenseReportBySubmittedId(userId);
			var items = results.Select(x => x).Where(y => y.OrganizationId == subInfo.OrganizationId);

			return View(await InitializeViewModel(subscriptionId, userId, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), items));
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
		public async Task<ExpenseIndexViewModel> InitializeViewModel(int subId, int userId, DateTime startDate, DateTime endDate, IEnumerable<ExpenseReport> expenses)
		{
			List<ExpenseItemViewModel> items = new List<ExpenseItemViewModel>();

			foreach (var item in expenses)
			{
				var expItemsTask = AppService.GetExpenseItemsByReportId(item.ExpenseReportId);
				var userTask = AppService.GetUserOldAsync(item.SubmittedById);

				await Task.WhenAll(new Task[] { expItemsTask, userTask });

				var expItems = expItemsTask.Result;
				var user = userTask.Result;

				decimal totalAmount = expItems.Sum(x => x.Amount);

				items.Add(new ExpenseItemViewModel
				{
					Amount = totalAmount,
					Reason = item.BusinessJustification,
					ReportId = item.ExpenseReportId,
					ReportName = item.ReportTitle,
					Status = (ExpenseStatusEnum)item.ReportStatus,
					SubmittedDate = item.SubmittedUtc,
					UserId = user.UserId,
					UserName = user.FirstName + " " + user.LastName
				});
			}

			ExpenseIndexViewModel model = new ExpenseIndexViewModel
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