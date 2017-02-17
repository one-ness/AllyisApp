//------------------------------------------------------------------------------
// <copyright file="UpdateOvertimeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Updates the Overtime setting for an Organization.
		/// </summary>
		/// <param name="setting">Overtime available setting.</param>
		/// <param name="hours">Hours until overtime.</param>
		/// <param name="period">Time period for hours until overtime.</param>
		/// <param name="mult">Overtime pay multiplier.</param>
		/// <returns>Redirects to the settings view.</returns>
		[HttpPost]
		public ActionResult UpdateOvertime(string setting, int hours = -1, string period = "", float mult = 1)
		{
			int actualHours = string.Equals(setting, "No") ? -1 : hours;

			if (TimeTrackerService.UpdateOvertime(actualHours, period, mult))
			{
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.OvertimeUpdate, Variety.Success));
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Settings);
		}
	}
}