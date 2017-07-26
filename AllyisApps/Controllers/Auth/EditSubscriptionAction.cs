
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
				ProductId = sku.ProductId,
				SubscriptionName = "" // TODO: make this available in infos via stored procedure.
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
						return this.RedirectToAction(ActionConstants.Unsubscribe, new { id = model.SubscriptionId, idTwo = model.SkuId });
					}
					var infos = AppService.GetProductSubscriptionInfo(model.OrganizationId, model.SkuIdNext);

					//ProductSubscriptionViewModel mod = new ProductSubscriptionViewModel()
					//{
					//	IsValid = true,
					//	OrganizationId = model.OrganizationId,
					//	OrganizationName = model.NextName,
					//	ProductId = model.ProductId,
					//	ProductName = model.ProductName,
					//	ProductDescription = model.Description,
					//	CurrentSubscription = infos.Item2,
					//	Skus = infos.Item3,
					//	SelectedSku = model.SkuIdNext,
					//	SelectedSkuName = model.SkuIdNext > 0 ? infos.Item3.Where(s => s.SkuId == model.SkuIdNext).SingleOrDefault().Name : string.Empty,
					//	PreviousSku = model.SkuId
					//};
					var id = infos.Item4;
					var customerId = new BillingServicesCustomerId(id);
					var token = new BillingServicesToken(customerId.ToString());

					this.AppService.CheckOrgAction(AppService.OrgAction.SubscribeToProduct, model.OrganizationId);

					if (AppService.Subscribe(model.ProductId, model.ProductName, model.SkuIdNext, model.SubscriptionName, model.SkuId, 0, token, false, null, null, model.OrganizationId))
					{
						Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.SubscribedSuccessfully, model.NextName), Variety.Success));
						return this.RedirectToAction(ActionConstants.ManageOrg, new { id = model.OrganizationId });
					}
					else
					{
						Notifications.Add(new BootstrapAlert(Resources.Strings.ReduceNumberOfUsers, Variety.Danger));
						return this.RedirectToAction(ActionConstants.Subscribe, new { productId = model.ProductId });
					}
				}
				catch (Exception)
				{
					Notifications.Add(new BootstrapAlert(@Resources.Strings.CannotEditSubscriptionsMessage, Variety.Warning));
					return this.RedirectToAction(ActionConstants.ManageOrg, ControllerConstants.Account, new { id = model.OrganizationId });
				}
			}
			return this.View(model);
		}
	}
}