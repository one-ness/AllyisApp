using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
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
		/// <param name="model">User settings information.</param>
		/// <returns>The action result.</returns>
		[HttpPost]
		public async Task<ActionResult> UpdateUserSettings(UserSettingsViewModel model)
		{
			UserContext.SubscriptionAndRole subInfo = AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId];
			if (!ModelState.IsValid)
			{
				return RedirectToAction("UserSettings", new { subscriptionId = model.SubscriptionId });
			}

			foreach (UserMaxAmountViewModel userMaxAmountViewModel in model.Users)
			{
				OrganizationUser userInfo = InitializeOrganizaionUser(userMaxAmountViewModel, subInfo.OrganizationId);
				await AppService.UpdateUserOrgMaxAmount(userInfo);
			}

			return RedirectToAction("index");
		}

		private OrganizationUser InitializeOrganizaionUser(UserMaxAmountViewModel user, int orgId)
		{
			return new OrganizationUser
			{
				MaxApprovalAmount = user.MaxAmount,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserId = user.UserId,
				OrganizationId = orgId
			};
		}
	}
}