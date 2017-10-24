//------------------------------------------------------------------------------
// <copyright file="UnsubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;
using System.Threading.Tasks;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Removes the selected subscription from the database.
		/// </summary>
		[HttpGet]
		async public Task<ActionResult> Unsubscribe(int id)
		{
			int orgId = await this.AppService.DeleteSubscription(id);
			Notifications.Add(new BootstrapAlert("Your subscription was deleted successfully.", Variety.Success));
			return this.RedirectToAction(ActionConstants.OrganizationSubscriptions, new { id = orgId });
		}
	}
}
