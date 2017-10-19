using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Expense;
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

			IList<Account> accounts = AppService.GetAccounts(subscriptionId).ToList();
			List<AccountViewModel> accountViewModels = new List<AccountViewModel>();
			foreach (Account account in accounts)
			{
				if (account.IsActive)
				{
					accountViewModels.Add(InitializeAccountViewModel(account));
				}
			}

			if (accounts.Count == 0)
			{
				Notifications.Add(new BootstrapAlert("Cannot create a report if no accounts exist.", Variety.Danger));
				return RedirectToAction("Index");
			}

			ExpenseReport report = reportId == -1 ? null : AppService.GetExpenseReport(reportId);
			if (reportId != -1)
			{
				if (report != null
					&& (report.SubmittedById != this.AppService.UserContext.UserId)
					|| ((ExpenseStatusEnum)report.ReportStatus != ExpenseStatusEnum.Draft
					&& (ExpenseStatusEnum)report.ReportStatus != ExpenseStatusEnum.Rejected))
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
				CurrentUser = this.AppService.UserContext.UserId,
				Items = itemViewModels,
				StartDate = DateTime.UtcNow,
				SubscriptionId = subscriptionId,
				Report = InitializeExpenseReportViewModel(report),
				Files = null,
				AccountList = accountViewModels
			};

			return View(model);
		}

		private AccountViewModel InitializeAccountViewModel(Account entity)
		{
			return new AccountViewModel()
			{
				AccountId = entity.AccountId,
				AccountName = entity.AccountName
			};
		}
	}
}