using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Areas.ExpenseTracker.ViewModels.Expense;
using AllyisApps.Controllers;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// The expense controller.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Handles Expense tracker accounts actions.
		/// </summary>
		/// <param name="subscriptionId">The Subscription Id.</param>
		/// <returns>The account view page.</returns>
		public ActionResult Accounts(int subscriptionId)
		{
			SetNavData(subscriptionId);

			AccountPageViewModel model = new AccountPageViewModel();
			var accounts = AppService.GetAccounts();
			var results = accounts.Where(x => x.IsActive)
				.Select(x =>
				new AccountManagementViewModel()
				{
					AccountId = x.AccountId,
					AccountName = x.AccountName,
					AccountTypeId = x.AccountTypeId,
					AccountTypeName = x.AccountTypeName,
					IsActive = x.IsActive,
					ParentAccountId = x.ParentAccountId
				}).ToList();

			for (int i = 0; i < results.Count; i++)
			{
				var parentAccount = accounts.Where(x => x.AccountId == results[i].ParentAccountId).FirstOrDefault();
				results[i].ParentAccountName = parentAccount != null ? parentAccount.AccountName : null;
			}

			model.Accounts = results;

			return View(model);
		}
	}
}