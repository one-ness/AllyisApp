//------------------------------------------------------------------------------
// <copyright file="SettingAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Areas.TimeTracker.Models;
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
		/// GET /TimeTracker/TimeEntry/Settings.
		/// </summary>
		/// <returns>The settings page.</returns>
		public ActionResult Settings()
		{
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers))
			{
				return this.View(new SettingsViewModel()
				{
					OrganizationId = UserContext.ChosenOrganizationId,
					StartOfWeek = TimeTrackerService.GetStartOfWeek(UserContext.ChosenOrganizationId),
					Settings = TimeTrackerService.GetSettings(UserContext.ChosenOrganizationId),
					PayClasses = TimeTrackerService.GetPayClasses(UserContext.ChosenOrganizationId),
					Holidays = TimeTrackerService.GetHolidays()
				});
			}

			// Permissions failure
			Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));

			return this.RedirectToAction("Index", "Home");
		}
	}
}