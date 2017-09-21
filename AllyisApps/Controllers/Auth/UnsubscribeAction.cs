//------------------------------------------------------------------------------
// <copyright file="UnsubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Removes the selected subscription from the database.
		/// </summary>
		/// <param name="id"> Subscription Id.</param>
		/// <param name="idTwo">The id of the sku being unsubscribed from.</param>
		/// <returns>Removes selected subscription.</returns>
		[HttpGet]
		public ActionResult Unsubscribe(int id, int idTwo)
		{
			UserContext.SubscriptionAndRole userSub = null;
			AppService.UserContext.SubscriptionsAndRoles.TryGetValue(id, out userSub);
			int orgId = userSub.OrganizationId;
			int productId = (int)userSub.ProductId;

			this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, orgId);
			var infos = AppService.GetProductSubscriptionInfo(orgId, idTwo);
			ProductSubscriptionViewModel model = this.ConstructProductSubscriptionViewModel(infos, orgId);

			this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, model.OrganizationId);
			string notificationString = AppService.UnsubscribeAndRemoveBillingSubscription(model.SelectedSku, model.CurrentSubscription.SubscriptionId);
			if (notificationString != null)
			{
				Notifications.Add(new BootstrapAlert(notificationString, Variety.Success));
			}

			return this.RedirectToAction(ActionConstants.ManageOrg, new { id = model.OrganizationId });
		}

		/// <summary>
		/// Removes the selected subscription from the database.
		/// </summary>
		/// <param name="model">Model representing the current state of the change of subscription.</param>
		/// <returns>Removes selected subscription.</returns>
		[HttpPost]
		[CLSCompliant(false)]
		public ActionResult Unsubscribe(ProductSubscriptionViewModel model)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, model.OrganizationId);
			string notificationString = AppService.UnsubscribeAndRemoveBillingSubscription(model.SelectedSku, model.CurrentSubscription.SubscriptionId);
			if (notificationString != null)
			{
				Notifications.Add(new BootstrapAlert(notificationString, Variety.Success));
			}

			return this.RedirectToAction(ActionConstants.ManageOrg, new { id = model.OrganizationId });
		}
	}
}