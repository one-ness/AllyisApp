//------------------------------------------------------------------------------
// <copyright file="UpdateStartOfWeekAction.cs" company="Allyis, Inc.">
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
			Services.TimeTracker.Setting settings = AppService.GetAllSettings(organizaionId).Item1;

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