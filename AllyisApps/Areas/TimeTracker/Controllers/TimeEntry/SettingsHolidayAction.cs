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
using AllyisApps.Services;
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

			string subName = await AppService.GetSubscriptionName(subscriptionId);
			int organizaionId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var holidays = AppService.GetAllSettings(organizaionId).Item3
				.AsParallel()
				.Select(holiday => new SettingsHolidayViewModel.HolidayViewModel
				{
					Date = holiday.Date,
					HolidayId = holiday.HolidayId,
					HolidayName = holiday.HolidayName
				});

			var model = new SettingsHolidayViewModel
			{
				Holidays = holidays,
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = AppService.UserContext.UserId
			};

			return View(model);
		}

		/// <summary>
		/// Creates a holiday for the current organization.
		/// </summary>
		/// <param name="newHolidayName">The name of the holiday.</param>
		/// <param name="newHolidayDate">The date of the holiday.</param>
		/// <param name="subscriptionId">The Id of the subscription.</param>
		/// <returns>Redirects to the settings view.</returns>
		public async Task<ActionResult> CreateHoliday(string newHolidayName, DateTime newHolidayDate, int subscriptionId)
		{
			int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

			await AppService.CreateHoliday(new Holiday { OrganizationId = orgId, HolidayName = newHolidayName, Date = newHolidayDate }, subscriptionId);

			Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulCreateHoliday, Variety.Success));

			return RedirectToAction(ActionConstants.SettingsHoliday, new { subscriptionId, id = AppService.UserContext.UserId }); // Same destination regardless of creation success
		}
	}
}