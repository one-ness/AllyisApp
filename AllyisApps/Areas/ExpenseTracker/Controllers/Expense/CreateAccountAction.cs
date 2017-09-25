using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
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
			var parentList = new List<SelectListItem>() { new SelectListItem() { Text = "None", Value = "0" } };

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
			}
			else
			{
				var selected = parentList.Where(x => x.Selected).FirstOrDefault();
				var selectedId = selected != null ? selected.Value : "0";

				parentList.AddRange(accounts.Select(x => new SelectListItem() { Text = x.AccountName, Value = x.AccountId.ToString(), Selected = x.AccountId == model.ParentAccountId }).ToList());
				model.ParentAccounts = new SelectList(parentList.AsEnumerable(), "Value", "Text", selectedId);
			}

			return View(model);
		}

		/// <summary>
		/// Action for saving a new account or updating an already existing one.
		/// </summary>
		/// <param name="subscriptionId">Teh subscription id.</param>
		/// <param name="model">The account model.</param>
		/// <returns>The account page.</returns>
		public ActionResult SaveAccount(int subscriptionId, CreateAccountViewModel model)
		{
			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Accounts, subscriptionId);

			bool success = false;
			if (model != null)
			{
				Account acc = new Account()
				{
					AccountId = model.AccountId,
					AccountName = model.AccountName,
					AccountTypeId = model.AccountTypeId,
					AccountTypeName = model.AccountTypeName,
					IsActive = true,
					ParentAccountId = model.SelectedAccount != null ? (int?)Convert.ToInt32(model.SelectedAccount) : null
				};

				success = AppService.CreateAccount(acc);

				if (!success)
				{
					AppService.UpdateAccount(acc);
				}
			}

			return RedirectToAction("Accounts");
		}

		/// <summary>
		/// Deletes an account.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="accountId">The Id of an account to be deleted.</param>
		/// <returns>Redirect to the Accounts page.</returns>
		public ActionResult DeleteAccount(int subscriptionId, int accountId)
		{
			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Accounts, subscriptionId);

			AppService.DeleteAccount(accountId);

			return RedirectToAction("Accounts");
		}
	}
}