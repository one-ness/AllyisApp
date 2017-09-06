//------------------------------------------------------------------------------
// <copyright file="CreateHolidayAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using AllyisApps.Controllers;
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
		/// <param name="subscriptionId">The Id of the subscription.</param>
		/// <returns>Redirects to the settings view.</returns>
		public ActionResult CreateHoliday(string newHolidayName, string newHolidayDate, int subscriptionId)
		{
			bool isValid = true;
			if (string.IsNullOrWhiteSpace(newHolidayName))
			{
				isValid = false;
				Notifications.Add(new BootstrapAlert(Resources.Strings.CannotCreateHolidayWIthoutName, Variety.Warning));
			}

			int orgId = AppService.UserContext.UserSubscriptions[subscriptionId].OrganizationId;
			DateTime holidayDate;
			if (!DateTime.TryParse(newHolidayDate, out holidayDate))
			{
				isValid = false;
				Notifications.Add(new BootstrapAlert(Resources.Strings.CannotCreateHolidayWithInvalidDate, Variety.Warning));
			}

			if (isValid)
			{
				if (AppService.CreateHoliday(new Holiday() { OrganizationId = orgId, HolidayName = newHolidayName, Date = holidayDate }, subscriptionId))
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulCreateHoliday, Variety.Success));
				}
				else
				{
					// This should only be a permissions failure
					Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
				}
			}

			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId }); // Same destination regardless of creation success
		}
	}
}
