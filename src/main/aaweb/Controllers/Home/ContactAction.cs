//------------------------------------------------------------------------------
// <copyright file="ContactAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for home pages / site-wide functions.
	/// </summary>
	public partial class HomeController : BaseController
	{
		/// <summary>
		/// Displays the Contact page.
		/// </summary>
		/// <returns>The result of this action.</returns>
		[AllowAnonymous]
		public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return this.View();
		}
	}
}
