﻿
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
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
		/// GET: /Account/EditSubscription.
		/// </summary>
		/// <returns>The result of this action.</returns>
		[HttpGet]
		public ActionResult EditSubscription(int id)
        {
			int orgId = AppService.UserContext.UserSubscriptions[id].OrganizationId;
			this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, orgId);
			int skuId = AppService.UserContext.UserSubscriptions[id].SkuId;
			int productId = (int) AppService.UserContext.UserSubscriptions[id].ProductId;
			string subName = AppService.UserContext.UserSubscriptions[id].SubscriptionName;
			var infos = AppService.GetProductSubscriptionInfo(id, skuId);
			SkuInfo sku = AppService.GetSkuDetails(skuId);
			sku.SkuIdNext = infos.Item3.Where(s => s.SkuId != skuId && s.ProductId == productId).SingleOrDefault().SkuId;
			sku.NextName = infos.Item3.Where(s => s.SkuId != skuId && s.ProductId == productId).SingleOrDefault().Name;
			EditSubscriptionViewModel model = new EditSubscriptionViewModel
			{
				SkuId = sku.SkuId,
				SkuIdNext = sku.SkuIdNext,
				Name = sku.Name,
				NextName = sku.NextName,
				Description = sku.Description,
				SubscriptionId = id,
				OrganizationId = orgId,
				ProductId = sku.ProductId,
				SubscriptionName = subName
			};
			return this.View(ViewConstants.EditSubscription, model);
		}
    }
}