//------------------------------------------------------------------------------
// <copyright file="SetLockDateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using System.Linq;
using AllyisApps.Lib;
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
		public ActionResult LockDate(int subscriptionId)
		{
			this.AppService.CheckTimeTrackerAction((AppService.TimeTrackerAction.EditOthers), subscriptionId);
			int organizaionID = this.AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var infos = AppService.GetAllSettings(organizaionID);
			UserContext.SubscriptionAndRole subInfo = null;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			string subName = AppService.GetSubscription(subscriptionId).SubscriptionName;
			var infoOrg = AppService.GetTimeEntryIndexInfo(subInfo.OrganizationId, null, null);
			ViewBag.WeekStart = Utility.GetDaysFromDateTime(AppService.SetStartingDate(null, infoOrg.Item1.StartOfWeek));
			ViewBag.WeekEnd = Utility.GetDaysFromDateTime(SetEndingDate(null, infoOrg.Item1.StartOfWeek));
			Services.TimeTracker.Setting settings = infos.Item1;
			return this.View(new SettingsLockDateViewModel()
			{
				Settings = new SettingsLockDateViewModel.SettingsInfoViewModel()
				{
					IsLockDateUsed = settings.IsLockDateUsed,
					LockDatePeriod = settings.LockDatePeriod,
					LockDateQuantity = settings.LockDateQuantity,
					OrganizationId = settings.OrganizationId,
					OvertimeHours = settings.OvertimeHours,
					OvertimeMultiplier = settings.OvertimeMultiplier,
					OvertimePeriod = settings.OvertimePeriod,
					StartOfWeek = settings.StartOfWeek,
					Today = System.DateTime.UtcNow.Date
				},
				PayClasses = infos.Item2.AsParallel().Select(payClass => new SettingsLockDateViewModel.PayClassViewModel()
				{
					PayClassId = payClass.PayClassId,
					PayClassName = payClass.PayClassName
				}),
				Holidays = infos.Item3.AsParallel().Select(holiday => new SettingsLockDateViewModel.HolidayViewModel()
				{
					Date = holiday.Date,
					HolidayId = holiday.HolidayId,
					HolidayName = holiday.HolidayName
				}),
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = this.AppService.UserContext.UserId
			});
		}

	/// <summary>
	/// Sets the lock date for an organization.
	/// </summary>
	/// <param name="subscriptionId">The subscription's Id.</param>
	/// <param name="LDsetting">Whether or not to use a lock date.</param>
	/// <param name="LDperiod">The currently-selected period (days/weeks/months).</param>
	/// <param name="LDquantity">The quantity of the selected period.</param>
	/// <returns>Provides the view for the user.</returns>
	[HttpPost]
		public ActionResult SetLockDate(int subscriptionId, bool LDsetting, int LDperiod, int LDquantity)
		{
			this.AppService.CheckTimeTrackerAction(Services.AppService.TimeTrackerAction.EditOthers, subscriptionId);
			int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			try
			{
				if (AppService.UpdateLockDate(LDsetting, LDperiod, LDquantity, orgId))
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.LockDateUpdate, Variety.Success));
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.LockDateUpdateFail, Variety.Warning));
				}
			}
			catch (System.ArgumentException ex)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.LockDateUpdateFail + " " + ex.Message, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
		}
	}
}