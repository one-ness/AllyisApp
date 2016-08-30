//------------------------------------------------------------------------------
// <copyright file="ConfirmEmailAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

/*
This partial is no longer needed; we are not authenticating via Owin anymore
*/

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using System.Threading.Tasks;
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
		public async Task<ActionResult> ConfirmEmail(string userId, string code)
		{
			if (await this.AccountService.ConfirmEmailAsync(int.Parse(userId), code))
			{
				Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.NotifyYourEmailIsConfirmed, Variety.Success));
				return this.RedirectToAction(ActionConstants.LogOn, ControllerConstants.Account);
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.WarnYourEmailHasAlreadyBeenConfirmed, Variety.Warning));
				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Home);
			}
		}
	}
}