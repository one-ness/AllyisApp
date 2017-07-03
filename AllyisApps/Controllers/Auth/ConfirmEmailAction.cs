//------------------------------------------------------------------------------
// <copyright file="ConfirmEmailAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using System;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Class which manages user accounts.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/ConfirmEmail.
		/// </summary>
		[AllowAnonymous]
		public ActionResult ConfirmEmail(int id, Guid code)
		{
			if (this.AppService.ConfirmUserEmail(id, code))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.NotifyYourEmailIsConfirmed, Variety.Success));
				return this.RedirectToAction(ActionConstants.LogOn, ControllerConstants.Account);
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.WarnYourEmailHasAlreadyBeenConfirmed, Variety.Warning));
				return this.RouteUserHome();
			}
		}
	}
}
