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
        /// <param name="subscriptionId">The subscription id.</param>
        /// <returns>The action result.</returns>
		public ActionResult UserSettings(int subscriptionId)
		{
			SetNavData(subscriptionId);

            AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.UserSettings, subscriptionId);

            int userId = GetCookieData().UserId;

            UserContext.SubscriptionAndRole subInfo = this.AppService.UserContext.SubscriptionsAndRoles[subscriptionId];

			IEnumerable<User> allUsers = AppService.GetUsersWithSubscriptionToProductInOrganization(subInfo.OrganizationId, (int)subInfo.ProductId);

			string productName = AppService.GetProductNameBySubscriptionId(subInfo.SubscriptionId);
			
			UserSettingsViewModel model = new UserSettingsViewModel()
			{
				SubscriptionId = subscriptionId,
				Users = allUsers.Where(u => AppService.GetProductRoleForUser(productName, u.UserId, subInfo.OrganizationId) == "User")
			};

			return View(model);
		}
	}
}