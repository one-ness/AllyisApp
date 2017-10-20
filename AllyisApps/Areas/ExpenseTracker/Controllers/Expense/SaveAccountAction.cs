﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
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
		async public Task<ActionResult> SaveAccount(int subscriptionId, CreateAccountViewModel model)
		{
			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Accounts, subscriptionId);
			var subInfoTask = AppService.GetSubscription(subscriptionId);
			var canDisableTask = CanDisableAccount(subscriptionId, model.AccountId, model.SelectedStatus);

			await Task.WhenAll(new Task[] { subInfoTask, canDisableTask });

			var subInfo = subInfoTask.Result;
			var canDisable = canDisableTask.Result;

			bool success = false;
			if (model != null)
			{
				Account acc = new Account()
				{
					AccountId = model.AccountId,
					AccountName = model.AccountName,
					OrganizationId = subInfo.OrganizationId,
					AccountTypeId = model.AccountTypeId,
					AccountTypeName = model.AccountTypeName,
					IsActive = !string.Equals(model.SelectedStatus, "0"),
					ParentAccountId = Convert.ToInt32(model.SelectedAccount) != 0 ? (int?)Convert.ToInt32(model.SelectedAccount) : null
				};

				if (await CheckAccountParent(subscriptionId, acc))
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
				else
				{
					Notifications.Add(new BootstrapAlert(string.Format("Account '{0}' cannot be have a child account as a parent.", acc.AccountName), Variety.Danger));
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
		async public Task<bool> CheckAccountParent(int subscriptionId, Account childAcc)
		{
			bool results = true;
			var subInfo = await AppService.GetSubscription(subscriptionId);
			var childId = childAcc.AccountId;

			var currentAccount = childAcc;
			var accountResult = await AppService.GetAccounts(subInfo.OrganizationId);
			List<Account> accounts = accountResult.ToList();

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
		async public Task<bool> CanDisableAccount(int subId, int accId, string selectedStatus)
		{
			var results = true;
			var subInfo = await AppService.GetSubscription(subId);
			var accResults = await AppService.GetAccounts(subInfo.OrganizationId);
			var account = accResults.Where(x => x.AccountId == accId).FirstOrDefault();

			if (account != null && string.Equals(selectedStatus, "1"))
			{
				return results;
			}

			var orgReports = await AppService.GetExpenseReportByOrgId(subInfo.OrganizationId);

			foreach (var report in orgReports)
			{
				var reportItems = await AppService.GetExpenseItemsByReportId(report.ExpenseReportId);
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