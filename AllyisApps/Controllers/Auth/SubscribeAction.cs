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

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Subscription/Subscribe/ProductId=#.
		/// </summary>
		/// <param name="id">Organization id.</param>
		/// <param name="skuId">The id of the SKU being subscribed to.</param>
		/// <returns>The result of this action.</returns>
		[HttpGet]
		public async Task<ActionResult> Subscribe(int id, SkuIdEnum skuId)
		{
			var collection = await this.AppService.GetSubscriptionsAsync(id);
			foreach (var item in collection)
			{
				if (item.SkuId == skuId)
				{
					// already subscribed
					this.Notifications.Add(new BootstrapAlert(string.Format("You are already subscribed to {0}", item.SkuName), Variety.Warning));
					return this.RedirectToAction(ActionConstants.OrganizationSubscriptions, ControllerConstants.Account, new { id = id });
				}
				else
				{
					// get a list of all active skus
					var activeSkus = CacheContainer.AllSkusCache.Where(x => x.IsActive).ToList();

					
				}
			}


			var infos = AppService.GetProductSubscriptionInfo(id, skuId);

			ProductSubscriptionViewModel model = this.ConstructProductSubscriptionViewModel(infos, id);
			model.SelectedSku = skuId;
			model.SelectedSkuName = infos.SkuList.Where(s => s.SkuId == skuId).SingleOrDefault().SkuName;
			model.SelectedSkuDescription = infos.SkuList.Where(s => s.SkuId == skuId).SingleOrDefault().Description;

			if (!model.IsValid)
			{
				return this.View(ViewConstants.Details, id);
			}

			model.CurrentUsers = infos.UserCount;
			return this.View(ViewConstants.Subscribe, model);
		}

		/// <summary>
		/// Uses services to populate a <see cref="ProductSubscriptionViewModel"/> and returns it.
		/// </summary>

		/// <param name="productSubscription"></param>
		/// <param name="organizationId">Organization id.</param>
		/// <returns>The ProductSubscriptionViewModel.</returns>
		[CLSCompliant(false)]
		public ProductSubscriptionViewModel ConstructProductSubscriptionViewModel(ProductSubscription productSubscription, int organizationId)
		{
			if (productSubscription.Product != null)
			{
				SkuIdEnum selectedSku = productSubscription.SubscriptionInfo == null ? 0 : productSubscription.SubscriptionInfo.SkuId;

				// TODO: orgName MUST be obtained in the service call AppService.GetProductSubscriptionInfo (it gets the orgId)
				string SubscriptionName = "SubScription Name";
				BillingServicesCustomerId customerId = new BillingServicesCustomerId(productSubscription.StripeTokenCustId);

				return new ProductSubscriptionViewModel
				{
					IsValid = true,
					OrganizationId = organizationId,
					OrganizationName = SubscriptionName,
					ProductId = productSubscription.Product.ProductId,
					ProductName = productSubscription.Product.ProductName,
					AreaUrl = productSubscription.Product.AreaUrl,
					ProductDescription = productSubscription.Product.ProductDescription,
					CurrentSubscription = productSubscription.SubscriptionInfo,
					Skus = productSubscription.SkuList,
					SelectedSku = selectedSku,
					SelectedSkuName = selectedSku > 0 ? productSubscription.SkuList.Where(s => s.SkuId == selectedSku).SingleOrDefault().SkuName : string.Empty,
					PreviousSku = selectedSku,
					CustomerId = customerId.Id,
					Token = new BillingServicesToken(customerId.ToString()) // TODO: Does this just convert back to the stripeToken string?? Investigate.
				};
			}

			return new ProductSubscriptionViewModel
			{
				IsValid = false
			};
		}

		/// <summary>
		/// Subscribe to a product.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>A page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[CLSCompliant(false)]
		public ActionResult Subscribe(ProductSubscriptionViewModel model)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, model.OrganizationId);
			AppService.Subscribe(model.ProductId, model.ProductName, model.SelectedSku, model.SubscriptionName, 0, model.Token, false, null, null, model.OrganizationId);
			Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.SubscribedSuccessfully, model.SelectedSkuName), Variety.Success));
			return this.RedirectToAction(ActionConstants.OrganizationSubscriptions, new { id = model.OrganizationId });
		}
	}
}