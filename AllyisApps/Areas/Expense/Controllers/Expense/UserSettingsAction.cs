using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <inheritdoc />
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
		public async Task<ActionResult> UserSettings(int subscriptionId)
		{
			await SetNavData(subscriptionId);

			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.UserSettings, subscriptionId);

			int organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

			var users = (await AppService.GetOrganizationManagementInfo(organizationId)).Users;

			var userInfos = new List<UserMaxAmountViewModel>();
			foreach (OrganizationUser user in users)
			{
				int? userSubRole = await AppService.GetSubscriptionRoleForUser(subscriptionId, user.UserId);
				if (userSubRole == (int)ExpenseTrackerRole.Manager || userSubRole == (int)ExpenseTrackerRole.Manager)
				{
					userInfos.Add(InitializeUserMaxAmount(user));
				}
			}

			var model = new UserSettingsViewModel
			{
				SubscriptionId = subscriptionId,
				Users = userInfos
			};

			return View(model);
		}

		private static UserMaxAmountViewModel InitializeUserMaxAmount(OrganizationUser user)
		{
			return new UserMaxAmountViewModel
			{
				MaxAmount = user.ExpenseApprovalLimit,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserId = user.UserId
			};
		}
	}
}