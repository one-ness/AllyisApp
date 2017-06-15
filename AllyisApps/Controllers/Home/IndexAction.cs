//------------------------------------------------------------------------------
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
		/// <returns>The result of this action.</returns>
		public ActionResult Index()
		{
			// TODO: change this later, for now everyone must login and go to account home page.
			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Account);
		}
	}
}
