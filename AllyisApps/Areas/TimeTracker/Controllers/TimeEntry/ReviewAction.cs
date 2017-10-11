//------------------------------------------------------------------------------
// <copyright file="ReviewEntries.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
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
		public ActionResult Review(int subscriptionId, DateTime? startDate = null, DateTime? endDate = null)
		{
			DateTime calculatedStartDate = startDate ?? DateTime.Now.AddMonths(-1);
			DateTime calculatedEndDate = endDate ?? DateTime.Now;
			var organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var allTimeEntries = AppService.GetTimeEntriesOverDateRange(organizationId, calculatedStartDate, calculatedEndDate).Select(x => new TimeEntryViewModel(x));
			var timeEntriesByUser = allTimeEntries.ToLookup(entry => entry.UserId);
			var dateRangeTotalsByUser = timeEntriesByUser.Select;

			var model = new ReviewViewModel
			{
				UserId = AppService.UserContext.UserId,
				SubscriptionId = subscriptionId,
				OrganizationId = organizationId,
				SubscriptionName = AppService.GetSubscription(subscriptionId).SubscriptionName,
				PayClasses = AppService.GetPayClassesByOrganizationId(organizationId).Select(x => new PayClassInfoViewModel(x)).ToList(),
				TimeEntries = allTimeEntries.ToList(),
				StartDate = calculatedStartDate,
				EndDate = calculatedEndDate
			};
			return View(model);
		}
	}
}