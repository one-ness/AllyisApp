//------------------------------------------------------------------------------
// <copyright file="SetLockDateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

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
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <returns>The settings page.</returns>
		public async Task<ActionResult> SettingsPayPeriod(int subscriptionId)
		{
			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);
			int organizaionID = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var infos = AppService.GetAllSettings(organizaionID);
			UserContext.SubscriptionAndRole subInfo = null;
			AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			string subName = await AppService.GetSubscriptionName(subscriptionId);
			var infoOrg = await AppService.GetTimeEntryIndexInfo(subInfo.OrganizationId, null, null);
			ViewBag.WeekStart = Utility.GetDaysFromDateTime(AppService.SetStartingDate(null, infoOrg.Item1.StartOfWeek));
			ViewBag.WeekEnd = Utility.GetDaysFromDateTime(SetEndingDate(infoOrg.Item1.StartOfWeek));
			Services.TimeTracker.Setting settings = infos.Item1;

			return View(new SettingsPayPeriodViewModel
			{
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = AppService.UserContext.UserId
			});
		}
	}
}