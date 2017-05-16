//------------------------------------------------------------------------------
// <copyright file="LogOffAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using System.Web.Mvc;

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
			this.SignOut(Response);

			// display success message to user
			Notifications.Add(new BootstrapAlert(Resources.Strings.LogOffSuccess, Variety.Success));

			// redirect to home
			// TODO: we shouldnt be hard coding http
			return this.Redirect(string.Format("http://{0}", GlobalSettings.WebRoot));
		}
	}
}
