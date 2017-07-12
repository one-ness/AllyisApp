﻿
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;
using System;
using System.Collections.Generic;
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
		public ActionResult EditSubscription(int id, int skuId)
        {
			int orgId = AppService.UserContext.UserSubscriptions[id].OrganizationId;
			this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, orgId);
			SkuInfo sku = AppService.GetSkuDetails(skuId);
			EditSubscriptionViewModel model = new EditSubscriptionViewModel
			{
				SkuId = sku.SkuId,
				Name = sku.Name,
				Description = sku.Description,
				SubscriptionId = sku.SubscriptionId
			};
			return this.View(ViewConstants.EditSubscription, model);
		}
    }
}