//------------------------------------------------------------------------------
// <copyright file="ApplicationRedirectAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using System.Web.Mvc;
using System;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for home pages / site-wide functions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Redirects to the correct application given the productId.
		/// </summary>
		public ActionResult ApplicationRedirect(int id)
		{
			// check if the user is member of the given subscription
			UserSubscriptionInfo subInfo = null;
			if (!this.UserContext.UserSubscriptions.TryGetValue(id, out subInfo))
			{
				// set active subscrption to none
				this.AppService.UpdateActiveSubscription(0);

				// throw exception for trying to access some other subscription
				throw new AccessViolationException("invalid subscription");
			}

            // todo: get the product for the subscription and route to the product
            if (Request.IsAuthenticated)
            {
                Product product = AppService.GetProductById(AppService.GetProductIdByName(subInfo.ProductName));
                if (product != null && !string.IsNullOrWhiteSpace(product.ProductName))
                {
                    return RedirectToAction(ActionConstants.Index, ControllerConstants.TimeEntry, new { area = subInfo.ProductName, userId = this.UserContext.UserId });
                }
            }

            return this.RouteHome();
		}
	}
}
