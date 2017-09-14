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

			string productName = AppService.GetProductNameBySubscriptionId(subInfo.SubscriptionId);

			IEnumerable<User> allUsers = AppService.GetUsersWithSubscriptionToProductInOrganization(subInfo.OrganizationId, (int)subInfo.ProductId);
			IEnumerable<User> specificUsers = allUsers.Where(u => AppService.GetProductRoleForUser(productName, u.UserId, subInfo.OrganizationId) == "User");
			List<UserMaxAmountViewModel> userViewModels = new List<UserMaxAmountViewModel>();
			foreach (User user in specificUsers)
			{
				userViewModels.Add(InitializeUserMaxAmout(user));
			}
			UserSettingsViewModel model = new UserSettingsViewModel()
			{
				SubscriptionId = subscriptionId,
				Users = userViewModels
			};

			return View(model);
		}

		private UserMaxAmountViewModel InitializeUserMaxAmout(User user)
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