//------------------------------------------------------------------------------
// <copyright file="DeleteHolidayAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// Deletes a holiday from an org.
		/// </summary>
		/// <param name="holidayName">The name of the holiday to remove.</param>
		/// <param name="date">The holiday's date.</param>
		/// <returns>Redirects to the settings view.</returns>
		public ActionResult DeleteHoliday(string holidayName, DateTime date)
		{
			if (!TimeTrackerService.DeleteHoliday(holidayName, date))
			{
				// premissions handled in service level
				// Should only get here on permission failure
				Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction("Settings");
		}
	}
}