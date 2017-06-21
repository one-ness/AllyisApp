//------------------------------------------------------------------------------
// <copyright file="SettingAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
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
        /// <param name="subscriptionId">The subscription Id</param>
		/// <returns>The settings page.</returns>
		public ActionResult Settings(int subscriptionId)
		{
            int orgId = AppService.GetSubscription(subscriptionId).OrganizationId;
			var infos = AppService.GetAllSettings(orgId);

			if (AppService.Can(Actions.CoreAction.TimeTrackerEditOthers, false, orgId, subscriptionId))
			{
                return this.View(new SettingsViewModel()
                {
                    Settings = infos.Item1,
                    PayClasses = infos.Item2,
                    Holidays = infos.Item3,
                    SubscriptionId = subscriptionId,
                    UserId = UserContext.UserId
				});
			}

			// Permissions failure
			Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));           
			return this.RouteHome(subscriptionId);
		}
	}
}
