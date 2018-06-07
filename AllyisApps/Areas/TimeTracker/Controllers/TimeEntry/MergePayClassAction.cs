//------------------------------------------------------------------------------
// <copyright file="MergePayClassAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Hrm;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System.Collections.Generic;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Merge a pay class with another one.
		/// </summary>
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <param name="userId"> The payclass Id.</param>
		/// <returns>The merge pay class view.</returns>
		[HttpGet]
		public async Task<ActionResult> MergePayClass(int subscriptionId, int userId)
		{
			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);

			int payClassId = userId; //TODO: poor name because of poor routing; rename route params
			var allPayClasses = (await AppService.GetPayClassesBySubscriptionId(subscriptionId)).ToList();
			var destPayClasses = allPayClasses.Where(pc => pc.PayClassId != payClassId);
			var sourcePayClass = allPayClasses.First(pc => pc.PayClassId == payClassId);

			string subscriptionName = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;

			// Built-in, non-editable pay classes cannot be merged
			if (sourcePayClass.BuiltInPayClassId != BuiltinPayClassEnum.Custom)
			{
				Notifications.Add(new BootstrapAlert(Strings.CannotMergePayClass, Variety.Warning));
				return RedirectToAction(ActionConstants.SettingsPayClass, new { subscriptionId });
			}

			var model = new MergePayClassViewModel
			{
				SourcePayClassId = payClassId,
				SourcePayClassName = sourcePayClass.PayClassName,
				SubscriptionId = subscriptionId,
				SubscriptionName = this.AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName,
				DestinationPayClasses = destPayClasses.Where(payc => payc.BuiltInPayClassId != BuiltinPayClassEnum.Overtime).Select(payclass => new PayClassInfoViewModel(payclass))
			};

			return View(ViewConstants.MergePayClass, model);
		}

		/// <summary>
		/// Merge a pay class into another one: delete the old pay class, change all of its time entries' payclassId to the new one.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="destPayClass">The destination pay class' id.</param>
		/// <returns>A page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[CLSCompliant(false)]
		public async Task<ActionResult> MergePayClass(MergePayClassViewModel model, int destPayClass)
		{
			try
			{
				var paylcasses = (await AppService.GetPayClassesBySubscriptionId(model.SubscriptionId)).ToDictionary(pc => pc.PayClassId);

				if (paylcasses[destPayClass].BuiltInPayClassId == BuiltinPayClassEnum.Overtime)
				{
					Notifications.Add(new BootstrapAlert("Cannont merge into overtime Over time has specail meaning suggest regular"));
				}
				// change all of the entries with old payclass to destPayClass and delete the old payclass
				if (await AppService.DeletePayClass(model.SourcePayClassId, AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId, model.SubscriptionId, destPayClass))
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulMergePayClass, Variety.Success));
				}
				else
				{
					// Should only be here because of permission failures
					Notifications.Add(new BootstrapAlert("Succssfuly changd all editalbe records but payclass could not be deleted as it has locked Time entries"));
					Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
				}
				if (paylcasses[destPayClass].BuiltInPayClassId == BuiltinPayClassEnum.Regular)
				{
					//upadate over time 
					var orgId = AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId;
					var users = await AppService.GetOrganizationUsersAsync(orgId);
					var settings = await AppService.GetSettingsByOrganizationId(orgId); //updated
					if (settings.OvertimeHours != null)
					{
						var tasks = new List<Task>();
						foreach (var user in users)
						{
							tasks.Add(AppService.RecalculateOvertimeForUserAfterLockDate(orgId, user.UserId, settings));
						}
						await Task.WhenAll(tasks);
					}
				}
			}
			catch
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}

			return RedirectToAction(ActionConstants.SettingsPayClass, new { subscriptionId = model.SubscriptionId });
		}
	}
}