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
using AllyisApps.Core.Alert;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using Newtonsoft.Json;

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
			var allTimeEntries = AppService.GetTimeEntriesOverDateRange(organizationId, calculatedStartDate, calculatedEndDate)
				.Select(entry =>
				{
					var project = AppService.GetProject(entry.ProjectId);
					return new TimeEntryViewModel(entry)
					{
						ProjectName = project.ProjectName,
						CustomerName = project.owningCustomer.CustomerName,
					};
				});
			var timeEntriesByUser = allTimeEntries.ToLookup(entry => entry.UserId);

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
				TimeEntryIdsJSON = JsonConvert.SerializeObject(allTimeEntries.Select(entry => entry.TimeEntryId).ToArray()),
				StartDate = calculatedStartDate,
				EndDate = calculatedEndDate,
				TimeEntryStatusOptions = ModelHelper.GetLocalizedTimeEntryStatuses(AppService)
			};
			return View(model);
		}

		/// <summary>
		/// Changes the status of all the timeEntries provided.
		/// </summary>
		/// <param name="subscriptionId">Subscription that the time entries belong to.</param>
		/// <param name="timeEntryIdsJSON">Int array of time entry ids to change, in JSON string format.</param>
		/// <param name="timeEntryStatusId">Status to change to.</param>
		/// <param name="endDate">Date range for reloading the page.</param>
		/// <param name="startDate">Date range for reloading the page.</param>
		/// <returns>Redirect to same page</returns>
		[HttpPost]
		public ActionResult UpdateTimeEntryStatus(int subscriptionId, string timeEntryIdsJSON, int timeEntryStatusId, DateTime startDate, DateTime endDate)
		{
			var timeEntryIds = JsonConvert.DeserializeObject<List<int>>(timeEntryIdsJSON);
			timeEntryIds.ForEach(entryId => AppService.UpdateTimeEntryStatusById(entryId, timeEntryStatusId));
			Notifications.Add(new BootstrapAlert("Successfully updated selected time entry statuses", Variety.Success));
			return RedirectToAction(ActionConstants.Review, new { subscriptionId = subscriptionId, startDate = startDate, endDate = endDate });
		}
	}
}