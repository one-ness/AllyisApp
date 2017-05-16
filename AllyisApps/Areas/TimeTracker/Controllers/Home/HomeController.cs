//------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Home Controller.
	/// </summary>
	[Authorize]
	public class HomeController : BaseController
	{
		private static readonly int TimeTrackerID = Service.GetProductIdByName(ProductNameKeyConstants.TimeTracker);

		#region default constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="HomeController" /> class.
		/// </summary>
		public HomeController() : base(TimeTrackerID)
		{
		}

		#endregion default constructor

		/// <summary>
		/// Index page.
		/// </summary>
		/// <returns>The view.</returns>
		public ActionResult Index()
		{
			if (Service.Can(Actions.CoreAction.TimeTrackerEditSelf))
			{
				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.TimeEntry);
			}

			Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Home, new { area = string.Empty });
		}
	}
}
