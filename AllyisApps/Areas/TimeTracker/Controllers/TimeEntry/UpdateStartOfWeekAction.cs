//------------------------------------------------------------------------------
// <copyright file="UpdateStartOfWeekAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
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
		public async Task<ActionResult> SettingsStartOfWeek(int subscriptionId)
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
			return View(new SettingsWeekStartViewModel
			{
				Settings = new SettingsWeekStartViewModel.SettingsInfoViewModel
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
				PayClasses = infos.Item2.AsParallel().Select(payClass => new SettingsWeekStartViewModel.PayClassViewModel
				{
					PayClassId = payClass.PayClassId,
					PayClassName = payClass.PayClassName
				}),
				Holidays = infos.Item3.AsParallel().Select(holiday => new SettingsWeekStartViewModel.HolidayViewModel
				{
					Date = holiday.Date,
					HolidayId = holiday.HolidayId,
					HolidayName = holiday.HolidayName
				}),
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = AppService.UserContext.UserId
			});
		}

		/// <summary>
		/// Updates the start of week for an Organization.
		/// </summary>
		/// <param name="subscriptionId">The subscription's id.</param>
		/// <param name="startOfWeek">Start of week selected by Organization admin.</param>
		/// <returns>Action result.</returns>
		public async Task<ActionResult> UpdateStartOfWeek(int subscriptionId, int startOfWeek)
		{
			System.Diagnostics.Debug.WriteLine("New Start Date: " + startOfWeek);
			var upWeek = await AppService.UpdateStartOfWeek(AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId, subscriptionId, startOfWeek);
			if (startOfWeek < 0 || startOfWeek > 6)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.InvalidSOW, Variety.Warning));
			}
			else if (!upWeek)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulSOW, Variety.Success));
			}

			return RedirectToAction(ActionConstants.SettingsStartOfWeek, new { subscriptionid = subscriptionId, id = AppService.UserContext.UserId });
		}
	}
}