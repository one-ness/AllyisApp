//------------------------------------------------------------------------------
// <copyright file="UpdateOvertimeAction.cs" company="Allyis, Inc.">
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
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <inheritdoc />
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
			string subName = await AppService.GetSubscriptionName(subscriptionId);
			int organizaionId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var settings = await AppService.GetSettingsByOrganizationId(organizaionId);

			var model = new SettingsOvertimeViewModel
			{
				OvertimeHours = settings.OvertimeHours,
				OvertimePeriod = settings.OvertimePeriod,
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = AppService.UserContext.UserId,
				IsOvertimeUsed = settings.OvertimeHours != null,
				OvertimePeriodOptions = ModelHelper.GetOvertimePeriodOptions()
			};

			return View(model);
		}

		/// <summary>
		/// Updates the Overtime setting for an Organization.
		/// </summary>
		/// <param name="model">Data from the view form.</param>
		/// <returns>Redirects to the settings view.</returns>
		[HttpPost]
		public async Task<ActionResult> UpdateOvertime(SettingsOvertimeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction(ActionConstants.SettingsOvertime, new { subscriptionid = model.SubscriptionId, id = AppService.UserContext.UserId });
			}

			int organizationId = AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId;
			var result = await AppService.UpdateOvertime(model.SubscriptionId, organizationId, model.OvertimeHours, model.OvertimePeriod, model.IsOvertimeUsed);

			switch (result.Enum)
			{
				case OvertimeResult.InvalidPeriodValue:
					Notifications.Add(new BootstrapAlert(Strings.OvertimeResultInvalidPeriod, Variety.Warning));
					break;
				case OvertimeResult.Success:
					Notifications.Add(new BootstrapAlert(Strings.OvertimeUpdate, Variety.Success));
					break;
				case OvertimeResult.NoHoursValue:
					Notifications.Add(new BootstrapAlert(Strings.OvertimeResultNoHours, Variety.Warning));
					break;
				case OvertimeResult.InvalidHours:
					Notifications.Add(new BootstrapAlert(Strings.OvertimeResultInvalidHours, Variety.Warning));
					break;
				case OvertimeResult.SettingsNotFound:
					Notifications.Add(new BootstrapAlert(Strings.OvertimeResultSettingsNotFound, Variety.Danger));
					break;
				case OvertimeResult.SuccessAndRecalculatedOvertime:
					Notifications.Add(new BootstrapAlert(Strings.OvertimeResultSuccessAndRecalculatedOvertime, Variety.Success));
					break;
				case OvertimeResult.SuccessAndDeletedOvertime:
					Notifications.Add(new BootstrapAlert(Strings.OvertimeResultSuccessAndDeletedOvertime, Variety.Success));
					break;
				case OvertimeResult.InvalidLockDate:
					Notifications.Add(new BootstrapAlert(string.Format(Strings.OvertimeResultInvalidLockDate.Replace("\\n", "</br>"), result.SuggestedLockDate.ToShortDateString()), Variety.Danger));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(result), result, "");
			}

			return RedirectToAction(ActionConstants.SettingsOvertime, new { subscriptionid = model.SubscriptionId, id = AppService.UserContext.UserId });
		}
	}
}