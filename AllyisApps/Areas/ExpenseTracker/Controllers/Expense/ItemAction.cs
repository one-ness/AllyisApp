using AllyisApps.Controllers;
using AllyisApps.Services;
using System.Web.Mvc;
using System.Threading.Tasks;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Collections.Generic;
using System;
using System.Linq;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Finance;
using AllyisApps.Areas.ExpenseTracker.ViewModels.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Displays the items in the create report view.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult DisplayItem(IEnumerable<ExpenseItem> model)
		{
			ViewBag.AccountList = AppService.GetAccounts();
			return PartialView("_AjaxExpenseReportItems", model);
		}

		/// <summary>
		/// Adds a new item to the model and displays the items in the create report view.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public ActionResult AddItem(ExpenseItemModel model, int index)
		{
			if (model == null)
			{
				model.Item = new ExpenseItem()
				{
					Index = index
				};
			}
			return PartialView("_AjaxExpenseReportItems", model);
		}
	}
}