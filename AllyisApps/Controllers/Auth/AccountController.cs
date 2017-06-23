//------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	[Authorize]
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Helper for ensuring a returnUrl is local and hasn't been tampered with.
		/// </summary>
		/// <param name="returnUrl">The returnUrl.</param>
		/// <returns>The redirection action, or a redirection to home if the url is bad.</returns>
		private ActionResult RedirectToLocal(string returnUrl = "")
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return this.Redirect(returnUrl);
			}
			else
			{
				return this.RouteHome();
			}
		}
	}
}
