//------------------------------------------------------------------------------
// <copyright file="AboutAction.cs" company="Allyis, Inc.">
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
		/// Displays the About page.
		/// </summary>
		/// <returns>
		/// The result of this action.
		/// </returns>
		[AllowAnonymous]
		public ActionResult AboutOLD()
		{
			ViewBag.Message = "Your application description page.";
			return this.View();
		}
	}
}
