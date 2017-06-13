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
		/// Verifies that the subdomain in question has not been used.
		/// </summary>
		/// <param name="subdomainName">The requested subdomain name. </param>
		/// <returns>True if the subdomain has not been used, else false.</returns>
		public bool IsSubdomainNameUnique(string subdomainName)
		{
			int id = AppService.GetIdBySubdomain(subdomainName);
			return id == 0;
		}

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
