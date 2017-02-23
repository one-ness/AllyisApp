//------------------------------------------------------------------------------
// <copyright file="HelpAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for home pages / site-wide functions.
	/// </summary>
	public partial class HomeController : BaseController
	{
		/// <summary>
		/// Shows the Help page.
		/// </summary>
		/// <returns>
		/// Returns view for help.
		/// </returns>
		public ActionResult Help()
		{
			return this.View();
		}
	}
}
