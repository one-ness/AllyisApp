using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Web;
using AllyisApps.Services.Expense;
using System.IO;
using AllyisApps.Lib;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Creates a new report.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Create expense report.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <returns>Returns an action result.</returns>
		public ActionResult Pending(int subscriptionId)
		{
			var model = new ExpensePendingModel()
			{

			};
			return View(model);
		}
	}
}