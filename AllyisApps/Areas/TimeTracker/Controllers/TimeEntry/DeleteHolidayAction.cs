//------------------------------------------------------------------------------
// <copyright file="DeleteHolidayAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Deletes a holiday from an org.
		/// </summary>
		/// <param name="holidayId">The id of the holiday to remove.</param>
		/// <returns>Redirects to the settings view.</returns>
		public ActionResult DeleteHoliday(int holidayId)
		{
			if (TimeTrackerService.DeleteHoliday(holidayId))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulDeleteHoliday, Variety.Success));
			}
			else
			{
				// premissions handled in service level
				// Should only get here on permission failure
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Settings);
		}
	}
}
