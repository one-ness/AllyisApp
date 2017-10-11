//------------------------------------------------------------------------------
// <copyright file="ReviewEntries.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Lib;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Returns a new view for approving time entries
		/// </summary>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="startDate">The start date of the time entries to pull.</param>
		/// <param name="endDate">The start date of the time entries to pull.</param>
		/// <returns>The action result.</returns>
		public ActionResult Review(int subscriptionId, int? startDate = null, int? endDate = null)
		{
			int calculatedStartDate = startDate ?? Utility.GetDaysFromDateTime(DateTime.Now.AddMonths(-1));
			int calculatedEndDate = endDate ?? Utility.GetDaysFromDateTime(DateTime.Now);
			var organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

			var model = new ReviewViewModel
			{
				UserId = AppService.UserContext.UserId,
				SubscriptionId = subscriptionId,
				OrganizationId = organizationId,
				SubscriptionName = AppService.GetSubscription(subscriptionId).SubscriptionName,
				PayClasses = AppService.GetPayClassesByOrganizationId(organizationId).Select(x => new PayClassInfoViewModel(x)).ToList(),
				TimeEntries = AppService.GetTimeEntriesOverDateRange(organizationId, Utility.GetDateTimeFromDays(calculatedStartDate), Utility.GetDateTimeFromDays(calculatedEndDate)).Select(x => new TimeEntryViewModel(x)).ToList(),
				StartDate = DateTime.Now.AddMonths(-1),
				EndDate = DateTime.Now
			};
			return View(model);
		}

		/*
		/// <summary>
		/// Returns a new view for approving time entries
		/// </summary>
		/// <param name="subscriptionId">The subscription we're on.</param>
		/// <param name="startDate">The start date of the time entries to pull.</param>
		/// <param name="endDate">The start date of the time entries to pull.</param>
		/// <returns>The action result.</returns>
		public ActionResult ReviewChangeDateRange(int subscriptionId, int startDate, int endDate)
		{
			return RedirectToAction("Review", new { subscriptionId = subscriptionId, startDate = startDate, endDate = endDate });
		}*/
	}
}