﻿//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for home pages / site-wide functions.
	/// </summary>
	public partial class HomeController : BaseController
	{
		/// <summary>
		/// Displays the Home page.
		/// </summary>
		/// <returns>The users home route.</returns>
		public ActionResult Index()
		{
			return this.RouteUserHome();
		}
	}
}