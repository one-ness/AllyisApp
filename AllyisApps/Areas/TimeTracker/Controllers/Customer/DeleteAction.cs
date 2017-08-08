//------------------------------------------------------------------------------
// <copyright file="DeleteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// GET: Customer/SubscriptionId/Delete/CustomerId.
		/// </summary>
		/// <param name="subscriptionId">The Subscription Id.</param>
		/// <param name="userId">The Customer id.</param>
		/// <returns>The Customer index.</returns>
		[HttpGet]
		public ActionResult Delete(int subscriptionId, string userId)
		{
			int numValue;
			bool parsed = Int32.TryParse(userId, out numValue);

			if (!parsed)
			{
				return this.RedirectToAction(ActionConstants.Index, new { subscriptionId = subscriptionId });
			}
			else
			{
				var result = AppService.DeleteCustomer(subscriptionId, numValue);
				
				// if deleted successfully
				if (!string.IsNullOrEmpty(result))
				{
					Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", result, Resources.Strings.CustomerDeleteNotification), Variety.Success));
				}
				
				// Permission failure
				else if (result == null)
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
				}

				return this.RedirectToAction(ActionConstants.Index, new { subscriptionId = subscriptionId });
			}
		}

		/*
        /// <summary>
		/// POST: Customer/Delete.
		/// </summary>
		/// <param name="id">The Customer id.</param>
		/// <returns>The Customer index.</returns>
		public ActionResult Delete(int id)
		{
			Customer customer = AppService.GetCustomer(id);

			if (customer != null)
			{
				if (AppService.DeleteCustomer(id))
				{
					Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", customer.Name, Resources.Strings.CustomerDeleteNotification), Variety.Success));

					return this.RedirectToAction(ActionConstants.Index);
				}

				// Permission failure
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Index);
		}
    */
	}
}
