using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Expense controller.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Show the list of expense reports submitted by the logged in user.
		/// </summary>
        /// <returns>The action result.</returns>
		[HttpPost]
		public ActionResult UpdateUserSettings(UserSettingsViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("UserSettings", new { subscriptionId = model.SubscriptionId });
			}

			foreach (User user in model.Users)
			{
				AppService.UpdateUserMaxAmount(user);
			}
			return RedirectToAction("index");
		}
	}
}