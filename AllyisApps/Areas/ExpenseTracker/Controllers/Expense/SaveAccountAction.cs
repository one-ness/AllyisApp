using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Expense;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// The Expense Controller.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Action for saving a new account or updating an already existing one.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="model">The account model.</param>
		/// <returns>The account page.</returns>
		public ActionResult SaveAccount(int subscriptionId, CreateAccountViewModel model)
		{
			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Accounts, subscriptionId);

			var canDisable = CanDisableAccount(subscriptionId, model.AccountId, model.SelectedStatus);
			bool success = false;
			if (model != null)
			{
				Account acc = new Account()
				{
					AccountId = model.AccountId,
					AccountName = model.AccountName,
					SubscriptionId = subscriptionId,
					AccountTypeId = model.AccountTypeId,
					AccountTypeName = model.AccountTypeName,
					IsActive = !string.Equals(model.SelectedStatus, "0"),
					ParentAccountId = Convert.ToInt32(model.SelectedAccount) != 0 ? (int?)Convert.ToInt32(model.SelectedAccount) : null
				};

				if (CheckAccountParent(subscriptionId, acc))
				{
					if (acc.AccountId == 0)
					{
						success = AppService.CreateAccount(acc);
					}

					if (!success && canDisable)
					{
						if (AppService.UpdateAccount(acc))
						{
							Notifications.Add(new BootstrapAlert(string.Format("Account '{0}' was succesfully updated.", acc.AccountName), Variety.Success));
						}
						else
						{
							Notifications.Add(new BootstrapAlert(string.Format("Error occured when updating account.", acc.AccountName), Variety.Danger));
						}
					}
					else
					{
						if (!canDisable)
						{
							Notifications.Add(new BootstrapAlert(string.Format("Account '{0}' cannot be disabled due to use by active report(s).", acc.AccountName), Variety.Danger));
						}
						else
						{
							Notifications.Add(new BootstrapAlert(string.Format("Account '{0}' was succesfully created.", acc.AccountName), Variety.Success));
						}
					}
				}
			}

			return RedirectToAction("Accounts");
		}

		/// <summary>
		/// Checks to make sure a loop isn't created when adding a parent account to another account.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="childAcc">The child account</param>
		/// <returns></returns>
		public bool CheckAccountParent(int subscriptionId, Account childAcc)
		{
			bool results = true;

			var childId = childAcc.AccountId;

			var currentAccount = childAcc;

			List<Account> accounts = AppService.GetAccounts(subscriptionId).ToList();

			while (currentAccount.ParentAccountId != null && accounts.Count != 0)
			{
				currentAccount = accounts.Where(x => x.AccountId == currentAccount.ParentAccountId).FirstOrDefault();
				accounts.RemoveAll(x => x.AccountId == currentAccount.AccountId);

				if (currentAccount.AccountId == childAcc.AccountId)
				{
					return false;
				}
			}

			return results;
		}

		/// <summary>
		/// Check if an account is allowed to be disabled.
		/// </summary>
		/// <param name="subId">The subscription id.</param>
		/// <param name="accId">The acccount id.</param>
		/// <param name="selectedStatus">The new selcted status.</param>
		/// <returns></returns>
		public bool CanDisableAccount(int subId, int accId, string selectedStatus)
		{
			var results = true;

			var account = AppService.GetAccounts(subId).Where(x => x.AccountId == accId).FirstOrDefault();

			if (account != null && string.Equals(selectedStatus, "1"))
			{
				return results;
			}

			var orgReports = AppService.GetExpenseReportByOrgId(AppService.GetSubscription(subId).OrganizationId);

			foreach (var report in orgReports)
			{
				var reportItems = AppService.GetExpenseItemsByReportId(report.ExpenseReportId);
				var accItems = reportItems.Where(x => x.AccountId == accId);

				if (accItems.Count() > 0 && (ExpenseStatusEnum)report.ReportStatus != ExpenseStatusEnum.Paid)
				{
					return false;
				}
			}

			return results;
		}
	}
}