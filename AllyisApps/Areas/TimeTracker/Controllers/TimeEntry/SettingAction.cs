//------------------------------------------------------------------------------
// <copyright file="SettingAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
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
		/// GET /TimeTracker/TimeEntry/subscriptionId/Settings.
		/// </summary>
		/// <param name="subscriptionId">The subscription Id</param>
		/// <returns>The settings page.</returns>
		public ActionResult Settings(int subscriptionId)
		{
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);
			var infos = AppService.GetAllSettings(subscriptionId);
            UserSubscription subInfo = null;
            this.AppService.UserContext.UserSubscriptions.TryGetValue(subscriptionId, out subInfo);
            return this.View(new SettingsViewModel()
			{
				Settings = infos.Item1,
				PayClasses = infos.Item2,
				Holidays = infos.Item3,
				SubscriptionId = subscriptionId,
                SubscriptionName = subInfo.SubscriptionName,
				UserId = this.AppService.UserContext.UserId
			});
		}
	}
}
