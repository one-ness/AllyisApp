//------------------------------------------------------------------------------
// <copyright file="AboutAction.cs" company="Allyis, Inc.">
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
		/// Displays the About page.
		/// </summary>
		/// <returns>
		/// The result of this action.
		/// </returns>
		[AllowAnonymous]
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";
			return this.View();
		}
	}
}
