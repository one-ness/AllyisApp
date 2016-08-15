//------------------------------------------------------------------------------
// <copyright file="UnsubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.ViewModels;

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
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization))
			{
				ProductSubscriptionViewModel model = this.ConstructProductSubscriptionViewModel(id);

				return this.View(model);	
			}

			Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));

			return this.RedirectToAction(ActionConstants.OrgIndex);
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
			if (model == null)
			{
				return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Errors.ModelNullMessage), ControllerConstants.Subscription, ActionConstants.Unsubscribe));
			}

			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization))
			{
				try
				{
					model.Billing.Customer = CrmService.RetrieveCustomer(CrmService.GetOrgBillingServicesCustomerId());
					if (model.Billing.Customer != null)
					{
						string subscriptionId = CrmService.GetSubscriptionId(model.Billing.Customer.Id);
						if (subscriptionId != null)
						{
							CrmService.DeleteSubscription(model.Billing.Customer.Id, subscriptionId.Trim());
							CrmService.DeleteSubscriptionPlan(subscriptionId);
							CrmService.AddBillingHistory("Unsubscribing from product", model.SelectedSku);
						}
					}
				}
				catch (Exception e)
				{
					Notifications.Add(new BootstrapAlert(e.ToString(), Variety.Warning)); 
				}

				if (model.CurrentSubscription != null)
				{
					CrmService.Unsubscribe(model.CurrentSubscription.SubscriptionId);
					string formattedNotificationString = string.Format("{0} has been unsubscribed from the license {1}.", OrgService.GetOrganization(model.OrganizationId).Name, CrmService.GetSkuDetails(model.PreviousSku).Name);
					Notifications.Add(new BootstrapAlert(formattedNotificationString, Variety.Success));
				}

				return this.RedirectToAction(ActionConstants.Manage);
			}

			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Errors.CannotEditSubscriptionsMessage), ControllerConstants.Subscription, ActionConstants.Subscribe));
		}
	}
}
