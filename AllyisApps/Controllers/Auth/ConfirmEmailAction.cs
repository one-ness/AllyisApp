//------------------------------------------------------------------------------
// <copyright file="ConfirmEmailAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Class which manages user accounts.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/ConfirmEmail.
		/// </summary>
		/// <param name="id">The guid to confirm the user email.</param>
		/// <returns>The route for the user's home page.</returns>
		[AllowAnonymous]
		async public Task<ActionResult> ConfirmEmail(Guid id)
		{
			if (this.AppService.ConfirmUserEmail(id))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.NotifyYourEmailIsConfirmed, Variety.Success));
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.WarnYourEmailHasAlreadyBeenConfirmed, Variety.Warning));
			}
			await Task.Delay(1);
			return this.RouteUserHome();
		}
	}
}