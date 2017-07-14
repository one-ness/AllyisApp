
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
		public ActionResult EditSubscription(int id)
        {
			int orgId = AppService.UserContext.UserSubscriptions[id].OrganizationId;
			int skuId = AppService.UserContext.UserSubscriptions[id].SkuId;
			int productId = (int) AppService.UserContext.UserSubscriptions[id].ProductId;
			this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, orgId);
			SkuInfo sku = AppService.GetSkuDetails(skuId);
			sku.SkuIdNext = AppService.GetSkuDetailsForEditSubscription(skuId, productId);
			sku.NextName = AppService.GetSkuName(skuId, productId);
			EditSubscriptionViewModel model = new EditSubscriptionViewModel
			{
				SkuId = sku.SkuId,
				SkuIdNext = sku.SkuIdNext,
				Name = sku.Name,
				Description = sku.Description,
				SubscriptionId = id,
				OrganizationId = orgId,
				ProductId = sku.ProductId
			};
			return this.View(ViewConstants.EditSubscription, model);
		}
    }
}