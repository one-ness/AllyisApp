//------------------------------------------------------------------------------
// <copyright file="ErrorAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

namespace AllyisApps.Controllers.Home
{
	/// <summary>
	/// Controller for home pages / site-wide functions.
	/// </summary>
	public partial class HomeController : BaseController
	{
		/// <summary>
		/// Shows the Error page.
		/// </summary>
		/// <returns>The action.</returns>
		public ActionResult Error()
		{
			return View("~/Views/Shared/ErrorQuiet.cshtml");
		}
	}
}