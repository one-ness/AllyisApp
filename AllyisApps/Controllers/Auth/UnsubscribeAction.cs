//------------------------------------------------------------------------------
// <copyright file="UnsubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using System;
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
		/// Removes the selected subscription from the database.
		/// </summary>
		/// <param name="id">The id of the product being unsubscribed from.</param>
		/// <returns>Removes selected subscription.</returns>
		[HttpGet]
		public ActionResult Unsubscribe(int id)
		{
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				var infos = Service.GetProductSubscriptionInfo(id);

				ProductSubscriptionViewModel model = this.ConstructProductSubscriptionViewModel(infos.Item1, infos.Item2, infos.Item3, infos.Item4);

				return this.View(model);
			}

			Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));

			return this.RedirectToAction(ActionConstants.Manage);
		}

		//TODO: Test this more systematically when stripe billing is reinstated.
		/// <summary>
		/// Removes the selected subscription from the database.
		/// </summary>
		/// <param name="model">Model representing the current state of the change of subscription.</param>
		/// <returns>Removes selected subscription.</returns>
		[HttpPost]
		[CLSCompliant(false)]
		public ActionResult Unsubscribe(ProductSubscriptionViewModel model)
		{
			if (model == null)
			{
				return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Errors.ModelNullMessage), ControllerConstants.Subscription, ActionConstants.Unsubscribe));
			}

			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				try
				{
					model.Billing.Customer = Service.RetrieveCustomer(Service.GetOrgBillingServicesCustomerId());
					if (model.Billing.Customer != null)
					{
						int? subId = null;
						if (model.CurrentSubscription != null)
						{
							subId = model.CurrentSubscription.SubscriptionId;
						}
						string notificationString = Service.UnsubscribeAndRemoveBillingSubscription(model.SelectedSku, subId);

						if (notificationString != null)
						{
							Notifications.Add(new BootstrapAlert(notificationString, Variety.Success));
						}

						//string subscriptionId = Service.GetSubscriptionId(model.Billing.Customer.Id);
						//if (subscriptionId != null)
						//{
						//	Service.DeleteSubscription(model.Billing.Customer.Id, subscriptionId.Trim());
						//	Service.DeleteSubscriptionPlan(subscriptionId);
						//	Service.AddBillingHistory("Unsubscribing from product", model.SelectedSku);
						//}
					}
				}
				catch (Exception e)
				{
					Notifications.Add(new BootstrapAlert(e.ToString(), Variety.Warning));
				}

				//if (model.CurrentSubscription != null)
				//{
					//Service.Unsubscribe(model.CurrentSubscription.SubscriptionId);
					//string formattedNotificationString = string.Format("{0} has been unsubscribed from the license {1}.", Service.GetOrganization(model.OrganizationId).Name, Service.GetSkuDetails(model.PreviousSku).Name);
					//Notifications.Add(new BootstrapAlert(formattedNotificationString, Variety.Success));
				//}

				return this.RedirectToAction(ActionConstants.Manage);
			}

			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Errors.CannotEditSubscriptionsMessage), ControllerConstants.Subscription, ActionConstants.Subscribe));
		}
	}
}
