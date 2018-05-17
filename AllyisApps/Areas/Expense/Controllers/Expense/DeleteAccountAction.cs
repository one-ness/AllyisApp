using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Auth;

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
		public async Task<ActionResult> DeleteAccount(int subscriptionId, int accountId)
		{
			
			var canDelete = await AppService.CanDelete(subscriptionId, accountId);
			if (canDelete.canDelete)
			{
				canDelete.associatedAccounts.Reverse();
				foreach (var account in canDelete.associatedAccounts)
				{
					await AppService.DeleteAccount(account.AccountId);
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