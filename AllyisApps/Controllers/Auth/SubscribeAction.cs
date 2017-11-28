//------------------------------------------------------------------------------
// <copyright file="SubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Cache;
using AllyisApps.ViewModels.Billing;

namespace AllyisApps.Controllers.Auth
{
	/// <inheritdoc />
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Subscription/Subscribe/skuid.
		/// </summary>
		/// <param name="id">Organization id.</param>
		/// <param name="skuId">The id of the SKU being subscribed to.</param>
		/// <returns>The result of this action.</returns>
		[HttpGet]
		public async Task<ActionResult> Subscribe(int id, SkuIdEnum skuId)
		{
			// get all subscriptions of the given organization
			var subscriptions = await AppService.GetSubscriptionsAsync(id);
			var model = new SubscribeViewModel();
			foreach (var subscription in subscriptions)
			{
				// is it an existing sku?
				if (subscription.SkuId == skuId)
				{
					// yes, already subscribed
					Notifications.Add(new BootstrapAlert($"You are already subscribed to {subscription.SkuName}.", Variety.Warning));
					return RedirectToAction(ActionConstants.Skus, ControllerConstants.Account, new { id });
				}

				// no, get the product of this subscription
				var pid = subscription.ProductId;
				if (!CacheContainer.ProductsCache.TryGetValue(pid, out Product product) || !product.IsActive)
				{
					// inactive or invalid product
					Notifications.Add(new BootstrapAlert("You selected an invalid product to subscribe to.", Variety.Danger));
					return RedirectToAction(ActionConstants.Skus, ControllerConstants.Account, new { id });
				}

				// is it a sku of this product?
				if (!CacheContainer.SkusCache.TryGetValue(pid, out var skus)) continue;
				Sku sku = skus.FirstOrDefault(x => x.IsActive && x.SkuId == skuId);
				if (sku == null) continue;

				// yes, user is subscribing to another sku of an existing subscription (i.e., product)
				model.IsChanging = true;
				model.OrganizationId = id;
				model.ProductName = subscription.ProductName;
				model.SkuDescription = sku.SkuDescription;
				model.SkuIconUrl = sku.IconUrl;
				model.SkuName = sku.SkuName;
				model.SubscriptionName = subscription.SubscriptionName;

				// show the view
				return View(model);
			}

			// reached here indicates user is subscribing to a new product with a new sku
			var selectedSku = CacheContainer.AllSkusCache.FirstOrDefault(x => x.IsActive && x.SkuId == skuId);
			if (selectedSku == null)
			{
				// inactive or invalid sku
				Notifications.Add(new BootstrapAlert("You selected an invalid sku to subscribe to.", Variety.Danger));
				return RedirectToAction(ActionConstants.Skus, ControllerConstants.Account, new { id });
			}

			// get product for the sku
			if (!CacheContainer.ProductsCache.TryGetValue(selectedSku.ProductId, out Product selectedProduct) || !selectedProduct.IsActive)
			{
				// inactive or invalid product
				Notifications.Add(new BootstrapAlert("You selected an invalid product to subscribe to.", Variety.Danger));
				return RedirectToAction(ActionConstants.Skus, ControllerConstants.Account, new { id });
			}

			// fill model
			model.OrganizationId = id;
			model.ProductName = selectedProduct.ProductName;
			model.SkuDescription = selectedSku.SkuDescription;
			model.SkuIconUrl = selectedSku.IconUrl;
			model.SkuName = selectedSku.SkuName;
			return View(model);
		}

		/// <summary>
		/// Subscribe to a product.
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Subscribe(SubscribeViewModel model)
		{
			await AppService.Subscribe(model.OrganizationId, (SkuIdEnum)model.SkuId, model.SubscriptionName);
			Notifications.Add(new BootstrapAlert($"Your subscription: {model.SubscriptionName} was created successfully!", Variety.Success));
			return RedirectToAction(ActionConstants.OrganizationSubscriptions, new { id = model.OrganizationId });
		}
	}
}