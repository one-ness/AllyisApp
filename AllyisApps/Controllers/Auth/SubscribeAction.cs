//------------------------------------------------------------------------------
// <copyright file="SubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Subscription/Subscribe/ProductId=#.
		/// </summary>
		/// <param name="id">organization id</param>
		/// <param name="skuId">The id of the SKU being subscribed to.</param>
		/// <returns>The result of this action.</returns>
		[HttpGet]
		public ActionResult Subscribe(int id, int skuId)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.SubscribeToProduct, id);
			var infos = AppService.GetProductSubscriptionInfo(id, skuId);

			ProductSubscriptionViewModel model = this.ConstructProductSubscriptionViewModel(infos.Item1, infos.Item2, infos.Item3, infos.Item4, id);
			model.SelectedSku = skuId;
			model.SelectedSkuName = infos.Item3.Where(s => s.SkuId == skuId).SingleOrDefault().Name;
			model.SelectedSkuDescription = infos.Item3.Where(s => s.SkuId == skuId).SingleOrDefault().Description;

			if (!model.IsValid)
			{
				return this.View(ViewConstants.Details, id);
			}

			model.CurrentUsers = infos.Item5;
			return this.View(ViewConstants.Subscribe, model);
		}

		/// <summary>
		/// Uses services to populate a <see cref="ProductSubscriptionViewModel"/> and returns it.
		/// </summary>
		/// <param name="productInfo">Product for product.</param>
		/// <param name="currentSubscription">SubscriptionInfo for this org's subscription to the product.</param>
		/// <param name="skus">List of SkuInfos for this product's skus.</param>
		/// <param name="stripeToken">This org's billing stripe token.</param>
		/// <param name="orgId"></param>
		/// <returns>The ProductSubscriptionViewModel.</returns>
		[CLSCompliant(false)]
		public ProductSubscriptionViewModel ConstructProductSubscriptionViewModel(Product productInfo, SubscriptionInfo currentSubscription, List<SkuInfo> skus, string stripeToken, int orgId)
		{
			if (productInfo != null)
			{
				int selectedSku = currentSubscription == null ? 0 : currentSubscription.SkuId;
				BillingServicesCustomerId customerId = new BillingServicesCustomerId(stripeToken);

				return new ProductSubscriptionViewModel
				{
					IsValid = true,
					OrganizationId = orgId,
					ProductId = productInfo.ProductId,
					ProductName = productInfo.ProductName,
					AreaUrl = productInfo.AreaUrl,
					ProductDescription = productInfo.ProductDescription,
					CurrentSubscription = currentSubscription,
					Skus = skus,
					SelectedSku = selectedSku,
					SelectedSkuName = selectedSku > 0 ? skus.Where(s => s.SkuId == selectedSku).SingleOrDefault().Name : string.Empty,
					PreviousSku = selectedSku,
					CustomerId = customerId,
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
			this.AppService.CheckOrgAction(AppService.OrgAction.SubscribeToProduct, model.OrganizationId);

			if (AppService.Subscribe(model.ProductId, model.ProductName, model.SelectedSku, model.SubscriptionName, model.PreviousSku, 0, model.Token, false, null, null, model.OrganizationId))
			{
				Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.SubscribedSuccessfully, model.SelectedSkuName), Variety.Success));
				return this.RedirectToAction(ActionConstants.ManageOrg, new { id = model.OrganizationId });
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.ReduceNumberOfUsers, Variety.Danger));
				return this.RedirectToAction(ActionConstants.Subscribe, new { productId = model.ProductId });
			}
		}

		/*
		/// <summary>
		/// Subscribe to a product.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="token">Response from the billing service.</param>
		/// <param name="billingServicesEmail">The billing email.</param>
		/// <returns>A page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[CLSCompliant(false)]
		public ActionResult Subscribe(ProductSubscriptionViewModel model, BillingServicesToken token, string billingServicesEmail)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.SubscribeToProduct, model.OrganizationId);

			if (model.Billing.Amount > 0 && token == null && model.Token == null)
			{
				return this.View(ViewConstants.AddBillingToSubscribe, model);
			}

			bool addingNewBilling = token != null && model.Token == null;
			
			if (AppService.Subscribe(model.ProductId, model.ProductName, model.SelectedSku, model.PreviousSku, model.Billing.Amount, model.Token, addingNewBilling, billingServicesEmail, token, model.OrganizationId))
			{
				Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.SubscribedSuccessfully, model.SelectedSkuName), Variety.Success));
				return this.RedirectToAction(ActionConstants.ManageOrg, new { id = model.OrganizationId });
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.ReduceNumberOfUsers, Variety.Danger));
				return this.RedirectToAction(ActionConstants.Subscribe, new { productId = model.ProductId });
			}
		}
		*/
	}
}
