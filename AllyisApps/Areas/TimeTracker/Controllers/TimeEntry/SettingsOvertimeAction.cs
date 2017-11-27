//------------------------------------------------------------------------------
// <copyright file="UpdateOvertimeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
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
			var settings = AppService.GetAllSettings(organizaionId).Item1;

			var model = new SettingsOvertimeViewModel
			{
				OvertimeHours = settings.OvertimeHours,
				OvertimePeriod = settings.OvertimePeriod,
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = AppService.UserContext.UserId
			};

			return View(model);
		}

		/// <summary>
		/// Updates the Overtime setting for an Organization.
		/// </summary>
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <param name="setting">Overtime available setting.</param>
		/// <param name="hours">Hours until overtime.</param>
		/// <param name="period">Time period for hours until overtime.</param>
		/// <returns>Redirects to the settings view.</returns>
		[HttpPost]
		public async Task<ActionResult> UpdateOvertime(int subscriptionId, string setting, int hours = -1, string period = "")
		{
			int actualHours = string.Equals(setting, "No") ? -1 : hours;

			if (await AppService.UpdateOvertime(subscriptionId, AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId, actualHours, period))
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