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
		public ActionResult DisplayItems(ExpenseItemModel model)
		{
			model.AccountList = AppService.GetAccounts();
			return PartialView("_AjaxExpenseReportItems", model);
		}

		/// <summary>
		/// Adds a new item to the model and displays the items in the create report view.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult AddItem(ExpenseItemModel model)
		{
			if (model == null)
			{
				model.Items = new List<ExpenseItem>();
			}
			model.Items.Add(new ExpenseItem()
			{
				Index = model.Items.Count
			});
			//return PartialView("_AjaxExpenseReportItems", model);
			return PartialView("_AjaxExpenseReportItems", model);
		}

		/// <summary>
		/// Removes the item from the model with the given ItemId.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public ActionResult RemoveItem(ExpenseItemModel model, int index)
		{
			model.Items.Remove(model.Items.Last());
			//model.Items.Remove(model.Items.Where(i => i.Index == index).FirstOrDefault());
			return PartialView("_AjaxExpenseReportItems", model);
		}
	}
}