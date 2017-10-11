//------------------------------------------------------------------------------
// <copyright file="ReviewEntries.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
			var payClasses = AppService.GetPayClassesByOrganizationId(organizationId).Select(x => new PayClassInfoViewModel(x));
			var timeEntriesByUser = AppService.GetTimeEntriesOverDateRange(organizationId, calculatedStartDate, calculatedEndDate)
				.Select(entry =>
				{
					var project = AppService.GetProject(entry.ProjectId);
					return new TimeEntryViewModel(entry)
					{
						ProjectName = project.ProjectName,
						CustomerName = project.owningCustomer.CustomerName
					};
				})
				.ToLookup(entry => entry.UserId);

			var timeEntryTotalsByUserByPayClass = new Dictionary<int, Dictionary<int, float>>();
			foreach (var group in timeEntriesByUser)
			{
				timeEntryTotalsByUserByPayClass.Add(group.Key, new Dictionary<int, float>());
				foreach (var payClassId in payClasses.Select(p => p.PayClassId))
				{
					timeEntryTotalsByUserByPayClass[group.Key].Add(payClassId, group.Where(e => e.PayClassId == payClassId).Sum(e => e.Duration));
				}
			}

			var model = new ReviewViewModel
			{
				UserId = AppService.UserContext.UserId,
				SubscriptionId = subscriptionId,
				OrganizationId = organizationId,
				SubscriptionName = AppService.GetSubscription(subscriptionId).SubscriptionName,
				PayClasses = payClasses.ToList(),
				TimeEntriesByUser = timeEntriesByUser,
				TimeEntryTotalsByUserByPayClass = timeEntryTotalsByUserByPayClass,
				StartDate = calculatedStartDate,
				EndDate = calculatedEndDate
			};
			return View(model);
		}
	}
}