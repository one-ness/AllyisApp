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
		/// <param name="data"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult DisplayItem(ExpenseItem[] data, IEnumerable<ExpenseItem> model)
		{
			Session["AccountList"] = AppService.GetAccounts();
			return PartialView("_AjaxExpenseReportItems", model);
		}

		/// <summary>
		/// Adds a new item to the model and displays the items in the create report view.
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult AddItem(List<ExpenseItem> items)
		{
			if (items == null)
			{
				items = new List<ExpenseItem>();
			}
			Session["AccountList"] = AppService.GetAccounts();
			items.Add(new ExpenseItem());
			return PartialView("_AjaxExpenseReportItems", items);
		}

		/// <summary>
		/// Adds a new item to the model and displays the items in the create report view.
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteItem(List<ExpenseItem> items)
		{
			items.Remove(items.Where(i => i.ToDelete).FirstOrDefault());
			Session["AccountList"] = AppService.GetAccounts();
			return PartialView("_AjaxExpenseReportItems", items);
		}
	}
}