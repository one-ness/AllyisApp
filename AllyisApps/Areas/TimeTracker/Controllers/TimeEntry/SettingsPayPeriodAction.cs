//------------------------------------------------------------------------------
// <copyright file="SetLockDateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using Newtonsoft.Json;

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
			int organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var settings = await AppService.GetSettingsByOrganizationId(organizationId);
			string subName = await AppService.GetSubscriptionName(subscriptionId);
			dynamic payPeriodInfo = JsonConvert.DeserializeObject(settings.PayPeriod);

			var model = new SettingsPayPeriodViewModel
			{
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = AppService.UserContext.UserId,
				PayPeriodTypeId = payPeriodInfo.type == "duration" ? 0 : 1,
				Duration = payPeriodInfo.duration ?? 0,
				StartDate = (DateTime?)payPeriodInfo.startDate,
				Dates = payPeriodInfo.dates
			};
			return View(model);
		}

		/// <summary>
		/// POST for updating the time tracker pay period settings
		/// </summary>
		/// <param name="model">Model containing updated data</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult SettingsPayPeriod(SettingsPayPeriodViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Notifications.Add(new BootstrapAlert(Strings.UpdatePayPeriodSuccess, Variety.Success));
			return RedirectToAction(ActionConstants.SettingsPayPeriod, new { subscriptionId = model.SubscriptionId });
		}
	}
}