//------------------------------------------------------------------------------
// <copyright file="ChargeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.Common.Types;
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
		/// Edits or creates billing information.
		/// </summary>
		/// <param name="token">The billing services token being used for this charge.</param>
		/// <param name="billingServicesEmail">The email associated with this customer.</param>
		/// <returns>A page.</returns>
		[CLSCompliant(false)]
		public ActionResult Charge(BillingServicesToken token, string billingServicesEmail)
		{
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				if (token == null)
				{
					Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Strings.Token, Core.Alert.Variety.Warning));

					return this.RedirectToAction(ActionConstants.Manage);
				}
				else
				{
					Service.UpdateBillingInfo(billingServicesEmail, token);

					Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Strings.Billing, Core.Alert.Variety.Success));
					return this.RedirectToAction(ActionConstants.Manage);
				}
			}
			else
			{
				// Incorrect permissions
				Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
				return this.RedirectToAction(ActionConstants.Index);
			}
		}
	}
}
