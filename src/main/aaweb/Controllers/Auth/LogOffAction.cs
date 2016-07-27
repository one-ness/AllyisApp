//------------------------------------------------------------------------------
// <copyright file="LogOffAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/LogOff.
		/// Log off user and redirect to home page.
		/// </summary>
		/// <returns>The ActionResult.</returns>
		[HttpGet]
		public ActionResult LogOff()
		{
			Services.Account.AccountService.SignOut(Response);

			// display success message to user
			Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.LogOffSuccess, Variety.Success));

			// redirect to home
			// TODO: we shouldnt be hard coding http
			return this.Redirect(string.Format("http://{0}", GlobalSettings.WebRoot));
		}
	}
}
