using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

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
		public ActionResult Create(int subscriptionId)
		{
			var model = new ExpenseCreateModel()
			{
				CurrentUser = GetCookieData().UserId,
				Items = new List<ExpenseItemViewModel>(),
				StartDate = DateTime.UtcNow,
				SubscriptionId = subscriptionId
			};
			return View(model);
		}
	}
}