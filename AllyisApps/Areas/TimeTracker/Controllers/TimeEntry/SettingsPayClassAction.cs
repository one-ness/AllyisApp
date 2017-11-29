//------------------------------------------------------------------------------
// <copyright file="CreatePayClassAction.cs" company="Allyis, Inc.">
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
		public async Task<ActionResult> SettingsPayClass(int subscriptionId)
		{
			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);

			string subName = await AppService.GetSubscriptionName(subscriptionId);
			int organizaionId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var payClasses = AppService.GetAllSettings(organizaionId).Item2.AsParallel().Select(p => new SettingsPayClassesViewModel.PayClassViewModel(p.PayClassName, p.PayClassId));

			return View(new SettingsPayClassesViewModel
			{
				PayClasses = payClasses,
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = AppService.UserContext.UserId
			});
		}

		/// <summary>
		/// Creates a payclass.
		/// </summary>
		/// <param name="newPayClass">The pay class to create.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns>Redirects to the settings view.</returns>
		public async Task<ActionResult> CreatePayClass(string newPayClass, int subscriptionId)
		{
			if (string.IsNullOrWhiteSpace(newPayClass))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.CannotCreateBlankPayClass, Variety.Warning));
			}
			else
			{
				int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

				// should put try catch in 'else'. Creating a blank pay class results in Two alerts: "Cannot create blank pay class" and "pay class already exists"
				try
				{
					if (await AppService.CreatePayClass(newPayClass, orgId, subscriptionId) != 0 )
					{
						Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulCreatePayClass, Variety.Success));
					}
					else
					{
						// Should only get here on permissions failure
						Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
					}
				}
				catch (ArgumentException)
				{
					// Pay class already exists
					Notifications.Add(new BootstrapAlert(Resources.Strings.FailureCreatePayClassAlreadyExists, Variety.Danger));
				}
			}

			return RedirectToAction(ActionConstants.SettingsPayClass, new { subscriptionId, id = AppService.UserContext.UserId });
		}
	}
}