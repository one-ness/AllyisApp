//------------------------------------------------------------------------------
// <copyright file="SetLockDateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
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
			Setting settings = await AppService.GetSettingsByOrganizationId(organizationId);
			string subName = await AppService.GetSubscriptionName(subscriptionId);
			dynamic payPeriodInfo = JsonConvert.DeserializeObject(settings.PayPeriod);

			var model = new SettingsPayPeriodViewModel();
			model.SubscriptionId = subscriptionId;
			model.OrganizationId = organizationId;
			model.SubscriptionName = subName;
			model.UserId = AppService.UserContext.UserId;
			model.PayPeriodTypeId = payPeriodInfo.type == PayPeriodType.Duration.GetEnumName() ? (int)PayPeriodType.Duration : (int)PayPeriodType.Dates;
			model.Duration = payPeriodInfo.duration ?? 14;
			model.StartDate = (DateTime?)payPeriodInfo.startDate;
			model.Dates = payPeriodInfo.dates == null ? "" : string.Join(",", payPeriodInfo.dates);
			return View(model);
		}

		/// <summary>
		/// POST for updating the time tracker pay period settings
		/// </summary>
		/// <param name="model">Model containing updated data</param>
		/// <returns>Either return to same page if invalid data, or refresh the page with the updated data.</returns>
		[HttpPost]
		public async Task<ActionResult> SettingsPayPeriod(SettingsPayPeriodViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			switch ((PayPeriodType)model.PayPeriodTypeId)
			{
				case PayPeriodType.Duration:
					await AppService.UpdateDurationPayPeriod(model.Duration.Value, model.StartDate.Value, model.OrganizationId);
					break;

				case PayPeriodType.Dates:
					await AppService.UpdateDatesPayPeriod(model.Dates.Trim(' ').Split(',').Select(int.Parse).ToList(), model.OrganizationId);
					break;

				default:
					throw new InvalidEnumArgumentException(nameof(model.PayPeriodTypeId));
			}

			Notifications.Add(new BootstrapAlert(Strings.UpdatePayPeriodSuccess, Variety.Success));
			return RedirectToAction(ActionConstants.SettingsPayPeriod, new { subscriptionId = model.SubscriptionId });
		}
	}
}