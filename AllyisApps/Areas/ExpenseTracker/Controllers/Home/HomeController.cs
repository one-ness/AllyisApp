//------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using System.Web.Mvc;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
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
		public ActionResult Index(int subscriptionId)
		{
			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Expense);
		}
	}
}
