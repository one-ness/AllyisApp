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
			var productInfo = AppService.GetProductSubscriptionInfo(subInfo.OrganizationId, (int)subInfo.SkuId);

			string productName = AppService.GetProductNameBySubscriptionId(subInfo.SubscriptionId);

			var allInfos = AppService.GetOrganizationManagementInfo(subInfo.OrganizationId).Item2;
			IEnumerable<OrganizationUserInfo> userInfos = allInfos.Where(u =>
				AppService.GetProductRoleForUser("Expense Tracker", u.UserId, subInfo.OrganizationId) == "Manager"
				|| AppService.GetProductRoleForUser("Expense Tracker", u.UserId, subInfo.OrganizationId) == "Admin");

			List<UserMaxAmountViewModel> userViewModels = new List<UserMaxAmountViewModel>();
			foreach (OrganizationUserInfo user in userInfos)
			{
				userViewModels.Add(InitializeUserMaxAmount(user));
			}

			UserSettingsViewModel model = new UserSettingsViewModel()
			{
				SubscriptionId = subscriptionId,
				Users = userViewModels
			};

			return View(model);
		}

		private UserMaxAmountViewModel InitializeUserMaxAmount(OrganizationUserInfo user)
		{
			return new UserMaxAmountViewModel()
			{
				MaxAmount = user.MaxAmount,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserId = user.UserId
			};
		}
	}
}