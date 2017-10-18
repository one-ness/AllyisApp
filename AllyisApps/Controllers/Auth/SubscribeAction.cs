//------------------------------------------------------------------------------
// <copyright file="SubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;
using System.Threading.Tasks;
using AllyisApps.Services.Cache;
using System.Collections.Generic;

namespace AllyisApps.Controllers.Auth
{
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
			var collection = await this.AppService.GetSubscriptionsAsync(id);
			var model = new SubscribeViewModel();
			foreach (var item in collection)
			{
				// is it an existing sku?
				if (item.SkuId == skuId)
				{
					// yes, already subscribed
					this.Notifications.Add(new BootstrapAlert(string.Format("You are already subscribed to {0}.", item.SkuName), Variety.Warning));
					return this.RedirectToAction(ActionConstants.Skus, ControllerConstants.Account, new { id = id });
				}
				else
				{
					// no, get the product of this subscription
					var pid = item.ProductId;
					Product product = null;
					if (!CacheContainer.ProductsCache.TryGetValue(pid, out product) || !product.IsActive)
					{
						// inactive or invalid product
						this.Notifications.Add(new BootstrapAlert("You selected an invalid product to subscribe to.", Variety.Danger));
						return this.RedirectToAction(ActionConstants.Skus, ControllerConstants.Account, new { id = id });
					}

					// is it a sku of this product?
					List<Sku> skus = null;
					if (CacheContainer.SkusCache.TryGetValue(pid, out skus))
					{
						var sku = skus.Where(x => x.IsActive && x.SkuId == skuId).FirstOrDefault();
						if (sku != null)
						{
							// yes, user is subscribing to another sku of an existing subscription (i.e., product)
							model.IsChanging = true;
							model.OrganizationId = id;
							model.ProductName = item.ProductName;
							model.SkuDescription = sku.SkuDescription;
							model.SkuIconUrl = sku.IconUrl;
							model.SkuName = sku.SkuName;
							model.SubscriptionName = item.SubscriptionName;

							// show the view
							return View(model);
						}
					}
				}
			}

			// reached here indicates user is subscribing to a new product with a new sku
			var selectedSku = CacheContainer.AllSkusCache.Where(x => x.IsActive && x.SkuId == skuId).FirstOrDefault();
			if (selectedSku == null)
			{
				// inactive or invalid sku
				this.Notifications.Add(new BootstrapAlert("You selected an invalid sku to subscribe to.", Variety.Danger));
				return this.RedirectToAction(ActionConstants.Skus, ControllerConstants.Account, new { id = id });
			}

			// get product for the sku
			Product selectedProduct = null;
			if (!CacheContainer.ProductsCache.TryGetValue(selectedSku.ProductId, out selectedProduct) || !selectedProduct.IsActive)
			{
				// inactive or invalid product
				this.Notifications.Add(new BootstrapAlert("You selected an invalid product to subscribe to.", Variety.Danger));
				return this.RedirectToAction(ActionConstants.Skus, ControllerConstants.Account, new { id = id });
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
			await this.AppService.Subscribe(model.OrganizationId, (SkuIdEnum)model.SkuId, model.SubscriptionName);
			Notifications.Add(new BootstrapAlert(string.Format("Your subscription: {0} was created successfully!", model.SubscriptionName), Variety.Success));
			return this.RedirectToAction(ActionConstants.OrganizationSubscriptions, new { id = model.OrganizationId });
		}
	}
}