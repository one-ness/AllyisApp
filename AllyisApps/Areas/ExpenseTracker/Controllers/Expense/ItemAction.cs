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
		public ActionResult DisplayItem(ExpenseCreateModel model)
		{
			Session["AccountList"] = AppService.GetAccounts();
			return PartialView("_AjaxExpenseReportItems", model);
		}

		/// <summary>
		/// Adds a new item to the model and displays the items in the create report view.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult AddItem(ExpenseCreateModel model)
		{
			if (model.Items == null)
			{
				model.Items = new List<ExpenseItem>();
			}
			Session["AccountList"] = AppService.GetAccounts();
			model.Items.Add(new ExpenseItem());
			return PartialView("_AjaxExpenseReportItems", model);
		}

		/// <summary>
		/// Adds a new item to the model and displays the items in the create report view.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult DeleteItem(ExpenseCreateModel model)
		{
			model.Items.Remove(model.Items.Where(i => i.ToDelete).FirstOrDefault());
			Session["AccountList"] = AppService.GetAccounts();
			return PartialView("_AjaxExpenseReportItems", model);
		}
	}
}