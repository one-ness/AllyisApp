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
			var subInfo = AppService.GetSubscription(subscriptionId);
			var accounts = AppService.GetAccounts(subInfo.OrganizationId);
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

				string selectedStatus = "1";

				model.AccountStatuses = new SelectList(statuses.AsEnumerable(), "Value", "Text", selectedStatus);
			}

			return View(model);
		}
	}
}