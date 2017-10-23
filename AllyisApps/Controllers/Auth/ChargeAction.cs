//------------------------------------------------------------------------------
// <copyright file="ChargeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.Services.Common.Types;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Edits or creates billing information.
		/// </summary>
		/// <param name="id">The organization this billing information belongs to.</param>
		/// <param name="token">The billing services token being used for this charge.</param>
		/// <param name="billingServicesEmail">The email associated with this customer.</param>
		/// <returns>A page.</returns>
		[CLSCompliant(false)]
		async public Task<ActionResult> Charge(int id, BillingServicesToken token, string billingServicesEmail)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditBilling, id);
			AppService.UpdateBillingInfo(billingServicesEmail, token, id);
			Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Strings.Billing, Core.Alert.Variety.Success));
			await Task.Yield();
			return this.RedirectToAction(ActionConstants.ManageOrg);
		}
	}
}