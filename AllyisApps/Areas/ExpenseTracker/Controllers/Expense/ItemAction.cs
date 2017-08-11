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

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Adds an empty item to the create report view.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult AddItem(ExpenseCreateModel model)
		{
			//model.Items.Add(new ExpenseItem());
			return PartialView("_AjaxExpenseReportItems", model);
		}

		/// <summary>
		/// Removes the item from the model with the given ItemId.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="ItemId"></param>
		/// <returns></returns>
		public ActionResult RemoveItem(ExpenseCreateModel model, int ItemId)
		{
			model.Items.Remove(model.Items.Where(i => i.ExpenseItemId == ItemId).FirstOrDefault());
			return PartialView("_AjaxExpenseReportItems", model);
		}
	}
}