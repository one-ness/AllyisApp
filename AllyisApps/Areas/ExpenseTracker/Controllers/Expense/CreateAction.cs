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

namespace AllyisApps.Areas.ExpenseTracker.Controllers.Expense
{
	/// <summary>
	/// Creates a new report
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// create expense report
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult Create(ExpenseIndexViewModel model)
		{
			return View(model);
		}
	}
}