using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.Billing;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/EditSubscription.
		/// </summary>
		/// <param name="id">Subscription id.</param>
		/// <returns>The result of this action.</returns>
		[HttpGet]
		public async Task<ActionResult> EditSubscription(int id)
		{
			// TODO: below call needs org id
			await this.AppService.CheckPermissionAsync(Services.Billing.ProductIdEnum.AllyisApps, AppService.UserAction.Edit, AppService.AppEntity.Subscription, id);
			var sub = await AppService.GetSubscription(id);
			var model = new EditSubscriptionViewModel();
			model.OrganizationId = sub.OrganizationId;
			model.ProductName = sub.ProductName;
			model.SkuDescription = sub.SkuDescription;
			model.SkuIconUrl = sub.SkuIconUrl;
			model.SkuName = sub.SkuName;
			model.SubscriptionId = id;
			model.SubscriptionName = sub.SubscriptionName;

			return View(model);
		}

		/// <summary>
		/// POST: /Account/EditSubscription.
		/// </summary>
		/// <param name="model">Edit subscription view model, containing the form info to edit the subscription.</param>
		/// <returns>
		/// Redirects to unsubscribe, if we're unsubscribing
		/// Redirects to manage org if editing
		/// Goes back to edit page if there's an error.
		/// .</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditSubscription(EditSubscriptionViewModel model)
		{
			if (ModelState.IsValid)
			{
				await AppService.UpdateSubscriptionName(model.SubscriptionId, model.SubscriptionName);
				Notifications.Add(new BootstrapAlert(string.Format("{0} updated successfully!", model.SubscriptionName), Variety.Success));
				return RedirectToAction(ActionConstants.OrganizationSubscriptions, new { id = model.OrganizationId });
			}
			// error
			return View(model);
		}
	}
}