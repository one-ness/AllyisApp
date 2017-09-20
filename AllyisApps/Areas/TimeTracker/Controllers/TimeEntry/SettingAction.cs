//------------------------------------------------------------------------------
// <copyright file="SettingAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
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
		public ActionResult Settings(int subscriptionId)
		{
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);
			var infos = AppService.GetAllSettings(subscriptionId);
			UserContext.SubscriptionAndRole subInfo = null;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			string subName = AppService.GetSubscription(subscriptionId).Name;
			var infoOrg = AppService.GetTimeEntryIndexInfo(subInfo.OrganizationId, null, null);
			ViewBag.WeekStart = AppService.GetDaysFromDateTime(AppService.SetStartingDate(null, infoOrg.Item1.StartOfWeek));
			ViewBag.WeekEnd = AppService.GetDaysFromDateTime(SetEndingDate(null, infoOrg.Item1.StartOfWeek));
			Services.TimeTracker.Setting settings = infos.Item1;
			return this.View(new SettingsViewModel()
			{
				Settings = new SettingsViewModel.SettingsInfoViewModel()
				{
					IsLockDateUsed = settings.IsLockDateUsed,
					LockDatePeriod = settings.LockDatePeriod,
					LockDateQuantity = settings.LockDateQuantity,
					OrganizationId = settings.OrganizationId,
					OvertimeHours = settings.OvertimeHours,
					OvertimeMultiplier = settings.OvertimeMultiplier,
					OvertimePeriod = settings.OvertimePeriod,
					StartOfWeek = settings.StartOfWeek
				},
				PayClasses = infos.Item2.AsParallel().Select(payClass => new SettingsViewModel.PayClassViewModel()
				{
					PayClassId = payClass.PayClassId,
					PayClassName = payClass.PayClassName
				}),
				Holidays = infos.Item3.AsParallel().Select(holiday => new SettingsViewModel.HolidayViewModel()
				{
				}),
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = this.AppService.UserContext.UserId
			});
		}
	}
}