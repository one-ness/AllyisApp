//------------------------------------------------------------------------------
// <copyright file="LogOffAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
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
			this.SignOut();

			// display success message to user
			Notifications.Add(new BootstrapAlert(Resources.Strings.LogOffSuccess, Variety.Success));

			// redirect to home
			return Redirect(this.ApplicationRootUrl);
		}
	}
}