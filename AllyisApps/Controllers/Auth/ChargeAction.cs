//------------------------------------------------------------------------------
// <copyright file="ChargeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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
        /// <param name="id">the organization this billing information belongs to</param>
        /// <param name="token">The billing services token being used for this charge.</param>
        /// <param name="billingServicesEmail">The email associated with this customer.</param>
        /// <returns>A page.</returns>
        [CLSCompliant(false)]
		public ActionResult Charge(int id, BillingServicesToken token, string billingServicesEmail)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditBilling, id);
			AppService.UpdateBillingInfo(billingServicesEmail, token, id);
			Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Strings.Billing, Core.Alert.Variety.Success));
			return this.RedirectToAction(ActionConstants.ManageOrg);
		}
	}
}
