//------------------------------------------------------------------------------
// <copyright file="SubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
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
		/// <param name="prodId">The id of the SKU being subscribed to.</param>
		/// <returns>The result of this action.</returns>
		[HttpGet]
		public async Task<ActionResult> Subscribe(int id, int prodId)
		{
			// get all subscriptions of the given organization
			var subscriptions = await AppService.GetSubscriptionsAsync(id);
			var model = new SubscribeViewModel();
            ViewBag.productId = prodId;
			foreach (var subscription in subscriptions)
			{
                // is it an existing subscription?
                if (subscription.ProductId == (ProductIdEnum)prodId)
				{
					// yes, already subscribed
					Notifications.Add(new BootstrapAlert($"You are already subscribed to {subscription.ProductName}.", Variety.Warning));
					return RedirectToAction(ActionConstants.Skus, ControllerConstants.Account, new { id });
				}
			}
            model.OrganizationId = id;
            model.ProductName = AppService.GetProductById(prodId).ProductName;

            // show the view
            return View(model);
		}

		/// <summary>
		/// Subscribe to a product.
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Subscribe(SubscribeViewModel model)
		{
            var prodsidser = model.ProductID;
			await AppService.Subscribe(model.OrganizationId, (ProductIdEnum) model.ProductID, model.SubscriptionName);
			Notifications.Add(new BootstrapAlert($"Your subscription: {model.SubscriptionName} was created successfully!", Variety.Success));
			return RedirectToAction(ActionConstants.OrganizationSubscriptions, new { id = model.OrganizationId });
		}
	}
}