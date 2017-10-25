//------------------------------------------------------------------------------
// <copyright file="CreateHolidayAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Returns the settings holiday view.
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public async Task<ActionResult> SettingsHoliday(int subscriptionId)
		{
			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);
			int organizaionID = this.AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var infos = AppService.GetAllSettings(organizaionID);
			UserContext.SubscriptionAndRole subInfo = null;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			string subName = await AppService.GetSubscriptionName(subscriptionId);
			var infoOrg = await AppService.GetTimeEntryIndexInfo(subInfo.OrganizationId, null, null);
			ViewBag.WeekStart = Utility.GetDaysFromDateTime(AppService.SetStartingDate(null, infoOrg.Item1.StartOfWeek));
			ViewBag.WeekEnd = Utility.GetDaysFromDateTime(SetEndingDate(null, infoOrg.Item1.StartOfWeek));
			Setting settings = infos.Item1;
			DateTime formatDateTime = DateTime.Now;
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
					StartOfWeek = settings.StartOfWeek,
					Today = formatDateTime
				},
				PayClasses = infos.Item2.AsParallel().Select(payClass => new SettingsViewModel.PayClassViewModel()
				{
					PayClassId = payClass.PayClassId,
					PayClassName = payClass.PayClassName
				}),
				Holidays = infos.Item3.AsParallel().Select(holiday => new SettingsViewModel.HolidayViewModel()
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
		/// Creates a holiday for the current organization.
		/// </summary>
		/// <param name="newHolidayName">The name of the holiday.</param>
		/// <param name="newHolidayDate">The date of the holiday.</param>
		/// <param name="subscriptionId">The Id of the subscription.</param>
		/// <returns>Redirects to the settings view.</returns>
		public async Task<ActionResult> CreateHoliday(string newHolidayName, string newHolidayDate, int subscriptionId)
		{
			bool isValid = true;
			if (string.IsNullOrWhiteSpace(newHolidayName))
			{
				isValid = false;
				Notifications.Add(new BootstrapAlert(Resources.Strings.CannotCreateHolidayWIthoutName, Variety.Warning));
			}

			int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			DateTime holidayDate;
			if (!DateTime.TryParse(newHolidayDate, out holidayDate))
			{
				isValid = false;
				Notifications.Add(new BootstrapAlert(Resources.Strings.CannotCreateHolidayWithInvalidDate, Variety.Warning));
			}

			if (isValid)
			{
				if (await AppService.CreateHoliday(new Holiday() { OrganizationId = orgId, HolidayName = newHolidayName, Date = holidayDate }, subscriptionId))
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulCreateHoliday, Variety.Success));
				}
				else
				{
					// This should only be a permissions failure
					Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
				}
			}

			return this.RedirectToAction(ActionConstants.SettingsHoliday, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId }); // Same destination regardless of creation success
		}
	}
}