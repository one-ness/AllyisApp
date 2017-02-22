//------------------------------------------------------------------------------
// <copyright file="CreateHolidayAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Creates a holiday for the current organization.
		/// </summary>
		/// <param name="newHolidayName">The name of the holiday.</param>
		/// <param name="newHolidayDate">The date of the holiday.</param>
		/// <returns>Redirects to the settings view.</returns>
		public ActionResult CreateHoliday(string newHolidayName, string newHolidayDate)
		{
			if (string.IsNullOrWhiteSpace(newHolidayName))
			{
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.CannotCreateHolidayWIthoutName, Variety.Warning));
			}

			DateTime holidayDate;
			if (!DateTime.TryParse(newHolidayDate, out holidayDate))
			{
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.CannotCreateHolidayWithInvalidDate, Variety.Warning));
			}

			if (TimeTrackerService.CreateHoliday(new HolidayInfo() { OrganizationId = UserContext.ChosenOrganizationId, HolidayName = newHolidayName, Date = holidayDate }))
			{
				Notifications.Add(new BootstrapAlert("Created holiday successfully.", Variety.Success));
			}
			else { 
				// This should only be a permissions failure
				Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Settings, new { OrganizationId = UserContext.ChosenOrganizationId }); // Same destination regardless of creation success
		}
	}
}