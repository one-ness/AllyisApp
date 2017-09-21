using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/EditSubscription.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <returns>The result of this action.</returns>
		[HttpGet]
		public ActionResult EditSubscription(int subscriptionId)
		{
			int orgId = AppService.GetSubscription(subscriptionId).OrganizationId;
			this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, orgId);
			int skuId = (int)AppService.GetSubscription(subscriptionId).SkuId;

			int productId = (int)AppService.UserContext.SubscriptionsAndRoles[subscriptionId].ProductId;
			string subscriptionName = AppService.getSubscriptionName(subscriptionId);

			SkuInfo sku = GetNextName(subscriptionId, skuId, productId);
			EditSubscriptionViewModel model = new EditSubscriptionViewModel
			{
				SkuId = sku.SkuId,
				SkuIdNext = sku.SkuIdNext,
				Name = sku.SkuName,
				NextName = sku.NextName,
				Description = sku.Description,
				SubscriptionId = subscriptionId,
				OrganizationId = orgId,
				ProductId = sku.ProductId,
				SubscriptionName = subscriptionName
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
					// TODO: Go straight to unsubscribe action from the view -- create a seperate button for unsubscribing
					if (model.ActionType == "Unsubscribe")
					{
						// Unsubscribe
						return this.RedirectToAction(ActionConstants.Unsubscribe, new { id = model.SubscriptionId, idTwo = model.SkuId });
					}
					else if (model.ActionType == model.Name)
					{
						// Nothing happening in "change subscription" dropdown, just changing subscription name

						model.SkuIdNext = model.SkuId;
						model.NextName = model.Name;
					}
					else
					{
						// Upgrade or downgrade
						// TODO: Pass SKU ids from the db to the view dropdown values.  No hardcoding here plz

						model.NextName = model.ActionType;
						switch (model.ActionType)
						{
							case "Time Tracker":
								model.SkuIdNext = 200001;
								break;

							case "Expense Tracker":
								model.SkuIdNext = 300001;
								break;

							case "Staffing Manager":
								model.SkuIdNext = 400001;
								break;

							default:
								break;
						}
					}

					ProductSubscription infos = AppService.GetProductSubscriptionInfo(model.OrganizationId, model.SkuIdNext);

					var id = infos.StripeTokenCustId;
					var customerId = new BillingServicesCustomerId(id);
					var token = new BillingServicesToken(customerId.ToString());

					AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, model.OrganizationId);

					// TODO: Seperate edit subscription and create subscription
					AppService.Subscribe(model.ProductId, model.ProductName, model.SkuIdNext, model.SubscriptionName, model.SkuId, 0, token, false, null, null, model.OrganizationId);
					Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.SubscribedSuccessfully, model.NextName), Variety.Success));
					return this.RedirectToAction(ActionConstants.ManageOrg, new { id = model.OrganizationId });
				}
				catch (Exception ex)
				{
					var thing = ex;
					Notifications.Add(new BootstrapAlert(@Resources.Strings.CannotEditSubscriptionsMessage, Variety.Warning));
					return this.RedirectToAction(ActionConstants.ManageOrg, ControllerConstants.Account, new { id = model.OrganizationId });
				}
			}

			return this.View(model);
		}

		private SkuInfo GetNextName(int id, int skuId, int productId)
		{
			var infos = AppService.GetProductSubscriptionInfo(id, skuId);
			SkuInfo sku = AppService.GetSkuDetails(skuId);
			SkuInfo skuNext = infos.List.Where(s => s.SkuId != skuId && s.ProductId == productId).SingleOrDefault();
			sku.SkuIdNext = skuNext == null ? 0 : skuNext.SkuId;
			sku.NextName = skuNext == null ? null : skuNext.SkuName;
			switch (sku.SkuName)
			{
				case "Time Tracker":
					sku.NextName = "Time Tracker Pro";
					sku.SkuIdNext = 300001;
					break;

				case "Time Tracker Pro":
					sku.NextName = "Time Tracker";
					sku.SkuIdNext = 200001;
					break;

				default:
					break;
			}

			return sku;
		}
	}
}
