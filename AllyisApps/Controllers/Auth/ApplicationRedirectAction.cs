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
				this.AppService.UpdateActiveSubscription(id);

				// throw exception for trying to access some other subscription
				throw new AccessViolationException("invalid subscription");
			}

			// todo: get the product for the subscription and route to the product

			return this.RouteHome();
		}
	}
}
