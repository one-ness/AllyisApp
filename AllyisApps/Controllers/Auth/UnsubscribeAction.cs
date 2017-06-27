//------------------------------------------------------------------------------
// <copyright file="UnsubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using System;
using System.Web.Mvc;

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
		/// <param name="id">The id of the product being unsubscribed from.</param>
		/// <returns>Removes selected subscription.</returns>
		[HttpGet]
		public ActionResult Unsubscribe(int id)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.UnsubscribeFromProduct, id);
			var infos = AppService.GetProductSubscriptionInfo(id);
			ProductSubscriptionViewModel model = this.ConstructProductSubscriptionViewModel(infos.Item1, infos.Item2, infos.Item3, infos.Item4);
			return this.View(model);
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
			this.AppService.CheckOrgAction(AppService.OrgAction.UnsubscribeFromProduct, model.OrganizationId);
			string notificationString = AppService.UnsubscribeAndRemoveBillingSubscription(model.SelectedSku, model.SubscriptionId);
			if (notificationString != null)
			{
				Notifications.Add(new BootstrapAlert(notificationString, Variety.Success));
			}

			return this.RedirectToAction(ActionConstants.Manage, new { id = model.OrganizationId });
		}
	}
}
