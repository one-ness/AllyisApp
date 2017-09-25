//------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Home Controller.
	/// </summary>
	[Authorize]
	public class HomeController : BaseController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HomeController" /> class.
		/// </summary>
		public HomeController()
		{
		}

		/// <summary>
		/// Index page.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <returns>The view.</returns>
		public ActionResult Index(int subscriptionId)
		{
			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.TimeEntry);
		}
	}
}