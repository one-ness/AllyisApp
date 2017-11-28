//------------------------------------------------------------------------------
// <copyright file="UpdateStartOfWeekAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
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
		/// GET /TimeTracker/TimeEntry/subscriptionId/Settings.
		/// </summary>
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <returns>The settings page.</returns>
		public async Task<ActionResult> SettingsStartOfWeek(int subscriptionId)
		{
			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);

			string subName = await AppService.GetSubscriptionName(subscriptionId);
			int organizaionId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			Setting settings = AppService.GetAllSettings(organizaionId).Item1;

			var model = new SettingsWeekStartViewModel
			{
				StartOfWeek = settings.StartOfWeek,
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = AppService.UserContext.UserId
			};

			return View(model);
		}

		/// <summary>
		/// Updates the start of week for an Organization.
		/// </summary>
		/// <param name="subscriptionId">The subscription's id.</param>
		/// <param name="startOfWeek">Start of week selected by Organization admin.</param>
		/// <returns>Action result.</returns>
		public async Task<ActionResult> UpdateStartOfWeek(int subscriptionId, int startOfWeek)
		{
			if (startOfWeek < 0 || startOfWeek > 6)
			{
				Notifications.Add(new BootstrapAlert(Strings.InvalidSOW, Variety.Warning));
			}
			else
			{
				int organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
				var success = await AppService.UpdateStartOfWeek(organizationId, subscriptionId, startOfWeek);

				Notifications.Add(success
					? new BootstrapAlert(Strings.SuccessfulSOW, Variety.Success)
					: new BootstrapAlert(Strings.ActionUnauthorizedMessage, Variety.Warning));
			}

			return RedirectToAction(ActionConstants.SettingsStartOfWeek, new { subscriptionId });
		}
	}
}