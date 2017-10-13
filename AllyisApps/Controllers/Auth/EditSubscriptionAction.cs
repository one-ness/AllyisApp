using System;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;

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
		public ActionResult EditSubscription(int id)
		{
			var sub = AppService.GetSubscription(id);
			this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, sub.OrganizationId);
			ProductSubscription infos = AppService.GetProductSubscriptionInfo(id, sub.SkuId);
			SkuInfo sku = AppService.GetSkuDetails(sub.SkuId);

			EditSubscriptionViewModel model = new EditSubscriptionViewModel
			{
				SkuId = sku.SkuId,
				SkuIdNext = sku.SkuId,
				Name = sku.SkuName,
				NextName = sku.NextName,
				Description = sku.Description,
				SubscriptionId = id,
				OrganizationId = sub.OrganizationId,
				ProductId = sku.ProductId,
				SubscriptionName = sub.SubscriptionName,
				SkuIconUrl = sku.IconUrl,
				OtherSkus = infos.SkuList.Where(sk => sku.SkuId != sk.SkuId).Select(sk => sk.SkuId)
			};

			return this.View(ViewConstants.EditSubscription, model);
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
		public ActionResult EditSubscription(EditSubscriptionViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, model.OrganizationId);

					if (model.SelectedNewSkuEnum.HasValue)
					{
						model.SkuIdNext = model.SelectedNewSkuEnum.Value;
					}
					else
					{
						model.SkuIdNext = model.SkuId;
					}

					ProductSubscription oldProduct = AppService.GetProductSubscriptionInfo(model.OrganizationId, model.SkuId);
					ProductSubscription infos = AppService.GetProductSubscriptionInfo(model.OrganizationId, model.SkuIdNext);

					var id = infos.StripeTokenCustId;
					var customerId = new BillingServicesCustomerId(id);
					var token = new BillingServicesToken(customerId.ToString());

					AppService.UpdateSubscription(model.OrganizationId, (int)model.SkuIdNext, model.SubscriptionId, model.SubscriptionName);
					Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.SubscribedSuccessfully, model.NextName), Variety.Success));
					return this.RedirectToAction(ActionConstants.OrganizationSubscriptions, new { id = model.OrganizationId });
				}
				catch (Exception ex)
				{
					var thing = ex;
					Notifications.Add(new BootstrapAlert(@Resources.Strings.CannotEditSubscriptionsMessage, Variety.Warning));
					return this.RedirectToAction(ActionConstants.OrganizationSubscriptions, ControllerConstants.Account, new { id = model.OrganizationId });
				}
			}

			return this.View(model);
		}
	}
}