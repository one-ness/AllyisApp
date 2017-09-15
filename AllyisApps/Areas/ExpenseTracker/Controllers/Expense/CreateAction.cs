using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.DBModel.Finance;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Creates a new report.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Create expense report.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="reportId">The report id.</param>
		/// <returns>Returns an action result.</returns>
		public ActionResult Create(int subscriptionId, int reportId = -1)
		{
			SetNavData(subscriptionId);

			IList<AccountDBEntity> accountEntities = AppService.GetAccounts();
			List<AccountViewModel> accountViewModels = new List<AccountViewModel>();
			foreach (AccountDBEntity entity in accountEntities)
			{
				accountViewModels.Add(InitializeAccountViewModel(entity));
			}

			if (accountEntities.Count == 0)
			{
				throw new InvalidOperationException("Cannot create a report if no accounts exist.");
			}

			ExpenseReport report = reportId == -1 ? null : AppService.GetExpenseReport(reportId);
			var userInfo = GetCookieData();
			if (reportId != -1)
			{
				if (report != null
					&& (report.SubmittedById != userInfo.UserId
					|| ((ExpenseStatusEnum)report.ReportStatus != ExpenseStatusEnum.Draft
					&& (ExpenseStatusEnum)report.ReportStatus != ExpenseStatusEnum.Rejected)))
				{
					string message = string.Format("action {0} denied", AppService.ExpenseTrackerAction.EditReport.ToString());
					throw new AccessViolationException(message);
				}

				AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.EditReport, subscriptionId);
			}
			else
			{
				AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Unmanaged, subscriptionId);
			}

			IList<ExpenseItem> items = report == null ? null : AppService.GetExpenseItemsByReportId(reportId);
			List<ExpenseItemCreateViewModel> itemViewModels = new List<ExpenseItemCreateViewModel>();
			if (items != null)
			{
				foreach (ExpenseItem item in items)
				{
					itemViewModels.Add(InitializeExpenseItemViewModel(item));
				}
			}

			var model = new ExpenseCreateModel()
			{
				CurrentUser = GetCookieData().UserId,
				Items = itemViewModels,
				StartDate = DateTime.UtcNow,
				SubscriptionId = subscriptionId,
				Report = InitializeExpenseReportViewModel(report),
				Files = null,
				AccountList = accountViewModels
			};

			return View(model);
		}

		private AccountViewModel InitializeAccountViewModel(AccountDBEntity entity)
		{
			return new AccountViewModel()
			{
				AccountId = entity.AccountId,
				AccountName = entity.AccountName
			};
		}
	}
}