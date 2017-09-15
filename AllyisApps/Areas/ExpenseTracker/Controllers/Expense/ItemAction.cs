using System.Web.Mvc;
using AllyisApps.Areas.ExpenseTracker.ViewModels.Expense;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Collections.Generic;
using AllyisApps.DBModel.Finance;

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
		/// <param name="index">The item index.</param>
		/// <returns>A partial view.</returns>
		public ActionResult AddItem(int index)
		{
			IList<AccountDBEntity> accountEntities = AppService.GetAccounts();
			List<AccountViewModel> accountViewModels = new List<AccountViewModel>();
			foreach (AccountDBEntity entity in accountEntities)
			{
				accountViewModels.Add(InitializeAccountViewModel(entity));
			}

			return PartialView("_AjaxExpenseReportItems", new ExpenseItemModel()
			{
				Item = new ExpenseItemCreateViewModel()
				{
					Index = index,
				},
				AccountList = accountViewModels
			});
		}
	}
}
