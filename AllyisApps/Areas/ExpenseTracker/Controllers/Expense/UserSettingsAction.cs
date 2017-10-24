using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
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
		async public Task<ActionResult> UserSettings(int subscriptionId)
		{
			await SetNavData(subscriptionId);

			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.UserSettings, subscriptionId);

			int userId = this.AppService.UserContext.UserId;

			UserContext.SubscriptionAndRole subInfo = this.AppService.UserContext.SubscriptionsAndRoles[subscriptionId];

			var productNameTask = AppService.GetProductNameBySubscriptionId(subInfo.SubscriptionId);
			var allInfosTask = AppService.GetOrganizationManagementInfo(subInfo.OrganizationId);

			await Task.WhenAll(new Task[] { productNameTask, allInfosTask });

			string productName = productNameTask.Result;
			var allInfos = allInfosTask.Result;

			IEnumerable<OrganizationUser> userInfos = allInfos.Users.Where(u =>
				AppService.GetProductRoleForUser("Expense Tracker", u.UserId, subInfo.OrganizationId) == "Manager"
				|| AppService.GetProductRoleForUser("Expense Tracker", u.UserId, subInfo.OrganizationId) == "SuperUser");

			List<UserMaxAmountViewModel> userViewModels = new List<UserMaxAmountViewModel>();
			foreach (OrganizationUser user in userInfos)
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

		private UserMaxAmountViewModel InitializeUserMaxAmount(OrganizationUser user)
		{
			return new UserMaxAmountViewModel()
			{
				MaxAmount = user.MaxApprovalAmount,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserId = user.UserId
			};
		}
	}
}