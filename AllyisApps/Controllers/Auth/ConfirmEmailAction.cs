//------------------------------------------------------------------------------
// <copyright file="ConfirmEmailAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
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
		/// <param name = "userId" > The current user's id.</param>
		/// <param name = "code" > The authentication code.</param>
		/// <returns>The async task responsible for confirming an e-mail.</returns>
		[AllowAnonymous]
		public ActionResult ConfirmEmail(string userId, string code)
		{
			if (this.Service.ConfirmUserEmail(int.Parse(userId), code))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.NotifyYourEmailIsConfirmed, Variety.Success));
				return this.RedirectToAction(ActionConstants.LogOn, ControllerConstants.Account);
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.WarnYourEmailHasAlreadyBeenConfirmed, Variety.Warning));
				return this.RouteHome();
			}
		}
	}
}
