﻿//------------------------------------------------------------------------------
// <copyright file="MergePayClassAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Merge a pay class with another one
		/// </summary>
		/// <param name="subscriptionId">The subscription Id</param>
		/// <param name="userId"> The payclass Id</param>
		[HttpGet]
		public ActionResult MergePayClass(int subscriptionId, int userId)
		{
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);
			var allPayClasses = AppService.GetPayClasses(subscriptionId);
			var destPayClasses = allPayClasses.Where(pc => pc.PayClassId != userId);
			string sourcePayClassName = allPayClasses.Where(pc => pc.PayClassId == userId).ElementAt(0).Name;

			//Built-in, non-editable pay classes cannot be merged
			if (sourcePayClassName == "Regular" || sourcePayClassName == "Overtime" || sourcePayClassName == "Holiday" || sourcePayClassName == "Paid Time Off" || sourcePayClassName == "Unpaid Time Off")
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.CannotMergePayClass, Variety.Warning));
				return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId });
			}

			MergePayClassViewModel model = ConstructMergePayClassViewModel(userId, sourcePayClassName, subscriptionId, destPayClasses);
			return this.View(ViewConstants.MergePayClass, model);
		}

		/// <summary>
		/// Uses services to populate a <see cref="MergePayClassViewModel"/> and returns it.
		/// </summary>
		/// <param name="sourcePayClassId">The id of the pay class being merged</param>
		/// <param name="destPayClasses">List of all PayClass that can be merged into</param>
		/// <param name="subscriptionId">The subscription's Id</param>
		/// <param name="sourcePayClassName">The name of the pay class being merged</param>
		/// <returns>The MergePayClassViewModel.</returns>
		[CLSCompliant(false)]
		public MergePayClassViewModel ConstructMergePayClassViewModel(int sourcePayClassId, string sourcePayClassName, int subscriptionId, IEnumerable<PayClass> destPayClasses)
		{
			return new MergePayClassViewModel
			{
				sourcePayClassId = sourcePayClassId,
				sourcePayClassName = sourcePayClassName,
				SubscriptionId = subscriptionId,
				destinationPayClasses = destPayClasses
			};
		}

		/// <summary>
		/// Merge a pay class into another one: delete the old pay class, change all of its time entries' payclassId to the new one
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="destPayClass">The destination pay class' id.</param>
		/// <returns>A page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[CLSCompliant(false)]
		public ActionResult MergePayClass(MergePayClassViewModel model, int destPayClass)
		{
			//change all of the entries with old payclass to destPayClass and delete the old payclass
			if (AppService.DeletePayClass(model.sourcePayClassId, AppService.UserContext.UserSubscriptions[model.SubscriptionId].OrganizationId, model.SubscriptionId, destPayClass))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulMergePayClass, Variety.Success));
			}
			else
			{
				// Should only be here because of permission failures
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}
			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = model.SubscriptionId });
		}
	}
}
