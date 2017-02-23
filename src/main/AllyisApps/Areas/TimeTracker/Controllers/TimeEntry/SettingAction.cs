//------------------------------------------------------------------------------
// <copyright file="SettingAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// GET /TimeTracker/TimeEntry/Settings.
		/// </summary>
		/// <returns>The settings page.</returns>
		public ActionResult Settings()
		{
			if (Service.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				return this.View(new SettingsViewModel()
				{
					StartOfWeek = TimeTrackerService.GetStartOfWeek(),
					Settings = TimeTrackerService.GetSettings(),
					PayClasses = TimeTrackerService.GetPayClasses(),
					Holidays = TimeTrackerService.GetHolidays()
				});
			}

			// Permissions failure
			Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));

			return this.RouteHome();
		}
	}
}
