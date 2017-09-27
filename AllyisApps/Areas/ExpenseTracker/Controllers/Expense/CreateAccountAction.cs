using System;
using System.Collections.Generic;
using System.Linq;
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
		/// Default action for creating or editing an account.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="accountId">The Account Id.</param>
		/// <returns>A Model to the Create Account Page.</returns>
		public ActionResult CreateAccount(int subscriptionId, int? accountId = null)
		{
			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Accounts, subscriptionId);

			SetNavData(subscriptionId);

			var accounts = AppService.GetAccounts();
			var account = (accounts != null) && (accountId != null) ? accounts.Where(x => x.AccountId == accountId.Value).FirstOrDefault() : null;
			List<SelectListItem> parentList = new List<SelectListItem>() { new SelectListItem() { Text = "None", Value = "0" } };

			CreateAccountViewModel model = new CreateAccountViewModel();

			ViewBag.ButtonName = "Create";
			ViewBag.ButtonTitle = "Create a new Account";

			if (account != null)
			{
				ViewBag.ButtonName = "Edit";
				ViewBag.ButtonTitle = "Submit Edits on the account.";

				model.AccountId = account.AccountId;
				model.AccountName = account.AccountName;
				model.AccountTypeId = account.AccountTypeId;
				model.AccountTypeName = account.AccountTypeName;
				model.IsActive = account.IsActive;
				model.ParentAccountId = account.ParentAccountId;

				parentList.AddRange(accounts.Where(x => x.IsActive)
				.Select(x => new SelectListItem() { Text = x.AccountName, Value = x.AccountId.ToString(), Selected = x.AccountId == model.ParentAccountId })
				.Where(x => !string.Equals(x.Value, account.AccountId.ToString())).ToList());

				var selected = parentList.Where(x => x.Selected).FirstOrDefault();
				var selectedId = selected != null ? selected.Value : "0";

				model.ParentAccounts = new SelectList(parentList.AsEnumerable(), "Value", "Text", selectedId);

				List<SelectListItem> statuses = new List<SelectListItem>()
				{
					new SelectListItem() { Text = "Active", Value = "1", Selected = false },
					new SelectListItem() { Text = "Disabled", Value= "0", Selected = false }
				};

				string selectedStatus = model.IsActive ? "1" : "0";

				model.AccountStatuses = new SelectList(statuses.AsEnumerable(), "Value", "Text", selectedStatus);
			}
			else
			{
				var selected = parentList.Where(x => x.Selected).FirstOrDefault();
				var selectedId = selected != null ? selected.Value : "0";

				parentList.AddRange(accounts.Select(x => new SelectListItem() { Text = x.AccountName, Value = x.AccountId.ToString(), Selected = x.AccountId == model.ParentAccountId }).ToList());
				model.ParentAccounts = new SelectList(parentList.AsEnumerable(), "Value", "Text", selectedId);

				List<SelectListItem> statuses = new List<SelectListItem>()
				{
					new SelectListItem() { Text = "Active", Value = "1", Selected = false },
					new SelectListItem() { Text = "Disabled", Value= "0", Selected = false }
				};

				string selectedStatus = model.IsActive ? "1" : "0";

				model.AccountStatuses = new SelectList(statuses.AsEnumerable(), "Value", "Text", selectedStatus);
			}

			return View(model);
		}

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
					AccountTypeId = model.AccountTypeId,
					AccountTypeName = model.AccountTypeName,
					IsActive = !string.Equals(model.SelectedStatus, "0"),
					ParentAccountId = Convert.ToInt32(model.SelectedAccount) != 0 ? (int?)Convert.ToInt32(model.SelectedAccount) : null
				};

				if (CheckAccountParent(acc))
				{
					success = AppService.CreateAccount(acc);

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
		/// <param name="childAcc">The child account</param>
		/// <returns></returns>
		public bool CheckAccountParent(Account childAcc)
		{
			bool results = true;

			var childId = childAcc.AccountId;

			var currentAccount = childAcc;

			List<Account> accounts = AppService.GetAccounts().ToList();

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

			var account = AppService.GetAccounts().Where(x => x.AccountId == accId).FirstOrDefault();

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