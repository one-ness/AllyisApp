//------------------------------------------------------------------------------
// <copyright file="ChargeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;

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
		/// <param name="stripeToken">The stripe token being used for this charge.</param>
		/// <param name="stripeEmail">The email associated with this customer.</param>
		/// <returns>A page.</returns>
		public ActionResult Charge(string stripeToken, string stripeEmail)
		{
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization))
			{
				if (stripeToken == null)
				{
                    Notifications.Add(new Core.Alert.BootstrapAlert("Token was not assigned properly.", Core.Alert.Variety.Warning));

                    return this.RedirectToAction("OrgIndex");
				}

				CrmService.UpdateBillingInfo(stripeToken, stripeEmail);

                Notifications.Add(new Core.Alert.BootstrapAlert("Billing information accepted", Core.Alert.Variety.Success));
                return this.RedirectToAction("Manage");
			}

			// Incorrect permissions
			Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			return this.RedirectToAction("OrgIndex");
		}
	}
}
