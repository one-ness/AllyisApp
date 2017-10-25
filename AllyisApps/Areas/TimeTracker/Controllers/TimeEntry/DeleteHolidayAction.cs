﻿//------------------------------------------------------------------------------
// <copyright file="DeleteHolidayAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;

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
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <returns>Redirects to the settings view.</returns>
		public async Task<ActionResult> DeleteHoliday(int holidayId, int subscriptionId)
		{
			if (await AppService.DeleteHoliday(holidayId, AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId, subscriptionId))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulDeleteHoliday, Variety.Success));
			}
			else
			{
				// premissions handled in service level
				// Should only get here on permission failure
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}

			return RedirectToAction(ActionConstants.SettingsHoliday, new { subscriptionId = subscriptionId, id = AppService.UserContext.UserId });
		}
	}
}