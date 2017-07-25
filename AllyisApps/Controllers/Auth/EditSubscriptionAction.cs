
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
				ProductId = sku.ProductId
			};
			return this.View(ViewConstants.EditSubscription, model);
		}

		/// <summary>
		/// POST: /Account/EditSubscription.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditSubscription(EditSubscriptionViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (model.ActionType == "Unsubscribe")
					{
						return this.RedirectToAction(ActionConstants.Unsubscribe, new { });
					}

					return this.RedirectToAction(ActionConstants.Subscribe, new { id = model.OrganizationId, skuId = model.SkuIdNext });
				}
				catch (Exception)
				{
					Notifications.Add(new BootstrapAlert(@Resources.Strings.CannotEditSubscriptionsMessage, Variety.Success));
					return this.RedirectToAction(ActionConstants.ManageOrg, ControllerConstants.Account, new { id = model.OrganizationId });
				}
				//return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Strings.CannotEditSubscriptionsMessage), ControllerConstants.Organization, ActionConstants.Edit));
			}
			return this.View(model);
		}
	}
}