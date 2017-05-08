//------------------------------------------------------------------------------
// <copyright file="CreateHolidayAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using System;
using System.Web.Mvc;

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
			bool isValid = true;
			if (string.IsNullOrWhiteSpace(newHolidayName))
			{
				isValid = false;
				Notifications.Add(new BootstrapAlert(Resources.Strings.CannotCreateHolidayWIthoutName, Variety.Warning));
			}

			DateTime holidayDate;
			if (!DateTime.TryParse(newHolidayDate, out holidayDate))
			{
				isValid = false;
				Notifications.Add(new BootstrapAlert(Resources.Strings.CannotCreateHolidayWithInvalidDate, Variety.Warning));
			}
			if (isValid)
			{
				if (TimeTrackerService.CreateHoliday(new Holiday() { OrganizationId = UserContext.ChosenOrganizationId, HolidayName = newHolidayName, Date = holidayDate }))
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulCreateHoliday, Variety.Success));
				}
				else
				{
					// This should only be a permissions failure
					Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
				}
			}
			return this.RedirectToAction(ActionConstants.Settings, new { OrganizationId = UserContext.ChosenOrganizationId }); // Same destination regardless of creation success
		}
	}
}
