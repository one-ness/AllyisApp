//------------------------------------------------------------------------------
// <copyright file="UpdateOvertimeAction.cs" company="Allyis, Inc.">
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
		/// returns the view for overtime settings.
		/// </summary>
		/// <returns></returns>
		public async Task<ActionResult> SettingsOvertime(int subscriptionId)
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
			return View(new SettingsViewModel
			{
				Settings = new SettingsViewModel.SettingsInfoViewModel
				{
					OrganizationId = settings.OrganizationId,
					OvertimeHours = settings.OvertimeHours,
					OvertimeMultiplier = settings.OvertimeMultiplier,
					OvertimePeriod = settings.OvertimePeriod,
					StartOfWeek = settings.StartOfWeek,
					Today = System.DateTime.UtcNow.Date
				},
				PayClasses = infos.Item2.AsParallel().Select(payClass => new SettingsViewModel.PayClassViewModel
				{
					PayClassId = payClass.PayClassId,
					PayClassName = payClass.PayClassName
				}),
				Holidays = infos.Item3.AsParallel().Select(holiday => new SettingsViewModel.HolidayViewModel
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
		/// Updates the Overtime setting for an Organization.
		/// </summary>
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <param name="setting">Overtime available setting.</param>
		/// <param name="hours">Hours until overtime.</param>
		/// <param name="period">Time period for hours until overtime.</param>
		/// <param name="mult">Overtime pay multiplier.</param>
		/// <returns>Redirects to the settings view.</returns>
		[HttpPost]
		public async Task<ActionResult> UpdateOvertime(int subscriptionId, string setting, int hours = -1, string period = "", float mult = 1)
		{
			int actualHours = string.Equals(setting, "No") ? -1 : hours;

			if (await AppService.UpdateOvertime(subscriptionId, AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId, actualHours, period, mult))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.OvertimeUpdate, Variety.Success));
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}

			return RedirectToAction(ActionConstants.SettingsOvertime, new { subscriptionid = subscriptionId, id = AppService.UserContext.UserId });
		}
	}
}