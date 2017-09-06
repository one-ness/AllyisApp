using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Displays the items in the create report view.
		/// </summary>
		/// <param name="data">A list of expense items.</param>
		/// <param name="model">Reprot view model.</param>
		/// <returns>Returns the partial view of report items.</returns>
		public ActionResult DisplayItem(ExpenseItem[] data, IEnumerable<ExpenseItem> model)
		{
			Session["AccountList"] = AppService.GetAccounts();
			return PartialView("_AjaxExpenseReportItems", model);
		}

        /// <summary>
        /// Adds a new item to the model and displays the items in the create report view.
        /// </summary>
        /// <param name="items">List of expense items.</param>
        /// <returns>A new partial view of expense itmes.</returns>
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
		/// <param name="items">A list of report items.</param>
		/// <returns>A new partial view of expense itmes.</returns>
		[HttpPost]
		public ActionResult DeleteItem(List<ExpenseItem> items)
		{
			items.Remove(items.Where(i => i.ToDelete).FirstOrDefault());
			Session["AccountList"] = AppService.GetAccounts();
			return PartialView("_AjaxExpenseReportItems", items);
		}
	}
}