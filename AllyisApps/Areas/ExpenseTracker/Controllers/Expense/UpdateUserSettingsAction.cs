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
		/// <param name="model">User settings infoamtion.</param>
		/// <returns>The action result.</returns>
		[HttpPost]
		public ActionResult UpdateUserSettings(UserSettingsViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("UserSettings", new { subscriptionId = model.SubscriptionId });
			}

			foreach (UserMaxAmountViewModel userMaxAmountViewModel in model.Users)
			{
				User user = InitializeUser(userMaxAmountViewModel);
				AppService.UpdateUserMaxAmount(user);
			}

			return RedirectToAction("index");
		}

		private User InitializeUser(UserMaxAmountViewModel user)
		{
			return new User()
			{
				MaxAmount = user.MaxAmount,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserId = user.UserId
			};
		}
	}
}