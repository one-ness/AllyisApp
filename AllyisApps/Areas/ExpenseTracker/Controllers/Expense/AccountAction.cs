using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
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
		async public Task<ActionResult> Accounts(int subscriptionId)
		{
			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Accounts, subscriptionId);

			SetNavData(subscriptionId);

			AccountPageViewModel model = new AccountPageViewModel();
			var subInfo = await AppService.GetSubscription(subscriptionId);
			var accounts = await AppService.GetAccounts(subInfo.OrganizationId);
			var results = accounts.Select(x =>
				new AccountManagementViewModel()
				{
					AccountId = x.AccountId,
					AccountName = x.AccountName,
					AccountTypeId = x.AccountTypeId,
					AccountTypeName = x.AccountTypeName,
					IsActive = x.IsActive,
					Status = x.IsActive ? "Active" : "Disabled",
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