using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// The Expense Controller.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Action to delete an account.
		/// </summary>
		/// <returns></returns>
		public ActionResult DeleteAccount(int subscriptionId, int accountId)
		{
			List<Account> associatedAccounts = new List<Account>();

			if (AppService.CanDelete(subscriptionId, accountId, out associatedAccounts))
			{
				associatedAccounts.Reverse();
				foreach (var account in associatedAccounts)
				{
					AppService.DeleteAccount(account.AccountId);
				}
			}
			else
			{
				Notifications.Add(new BootstrapAlert("Error occured when deleting, the account or an associated account are still used by a report.", Variety.Danger));
			}

			return RedirectToAction("Accounts");
		}
	}
}