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
using AllyisApps.Services.Billing;

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
		public async Task<ActionResult> Charge(int id, BillingServicesToken token, string billingServicesEmail)
		{
			await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Update, AppService.AppEntity.Billing, id);
			await AppService.UpdateBillingInfo(billingServicesEmail, token, id);
			Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Strings.Billing, Core.Alert.Variety.Success));
			await Task.Yield();
			return RedirectToAction(ActionConstants.Index, ControllerConstants.Account);
		}
	}
}