using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Adds a new item to the model and displays the items in the create report view.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="index">The item index.</param>
		/// <returns>A partial view.</returns>
		public async Task<ActionResult> AddItem(int subscriptionId, int index)
		{
			var organizationId = (AppService.UserContext.SubscriptionsAndRoles[subscriptionId]).OrganizationId;
			IEnumerable<Account> accountEntities = await AppService.GetAccounts(organizationId);
			List<AccountViewModel> accountViewModels = new List<AccountViewModel>();
			foreach (Account account in accountEntities)
			{
				accountViewModels.Add(InitializeAccountViewModel(account));
			}

			return PartialView(
				"_AjaxExpenseReportItems",
				new ExpenseItemCreateViewModel
				{
					Index = index,
					AccountList = accountViewModels
				});
		}
	}
}