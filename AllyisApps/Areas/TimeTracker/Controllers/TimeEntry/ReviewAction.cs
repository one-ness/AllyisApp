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
using AllyisApps.Resources;
using AllyisApps.Services.TimeTracker;
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
			int organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var payClasses = AppService.GetPayClassesByOrganizationId(organizationId).Select(x => new PayClassInfoViewModel(x)).ToList();
			var allTimeEntries = AppService.GetTimeEntriesOverDateRange(organizationId, calculatedStartDate, calculatedEndDate)
				.Select(entry =>
				{
					var project = AppService.GetProject(entry.ProjectId);
					return new TimeEntryViewModel(entry)
					{
						ProjectName = project.ProjectName,
						CustomerName = project.owningCustomer.CustomerName
					};
				})
				.ToList();
			var timeEntriesByUser = allTimeEntries.ToLookup(entry => entry.UserId);

			var timeEntryTotalsByUserByPayClass = new Dictionary<int, Dictionary<int, float>>();
			foreach (var group in timeEntriesByUser)
			{
				timeEntryTotalsByUserByPayClass.Add(group.Key, new Dictionary<int, float>());
				foreach (int payClassId in payClasses.Select(p => p.PayClassId))
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
		/// <returns>Redirect to same page.</returns>
		[HttpPost]
		public ActionResult UpdateTimeEntryStatus(int subscriptionId, string timeEntryIdsJSON, int timeEntryStatusId, DateTime startDate, DateTime endDate)
		{
			var timeEntryIds = JsonConvert.DeserializeObject<List<int>>(timeEntryIdsJSON);
			timeEntryIds.ForEach(entryId => AppService.UpdateTimeEntryStatusById(entryId, timeEntryStatusId));
			Notifications.Add(new BootstrapAlert(Strings.UpdateTimeEntryStatusSuccess, Variety.Success));
			return RedirectToAction(ActionConstants.Review, new { subscriptionId, startDate, endDate });
		}

		/// <summary>
		/// Locks all time entries with date that is less than or equal to lockDate
		/// </summary>
		/// <param name="subscriptionId">Subscription that the lock operation will be performed on.</param>
		/// <param name="startDate">Used for the redirect back to the review page -- need to preserve the date range they came in with.</param>
		/// <param name="lockDate">The date from which to all all time entries before.  Also the end date of the review page's date range.</param>
		/// <exception cref="ArgumentOutOfRangeException">Expection for if an invalid enum is returned from the service layer.</exception>
		/// <returns>Redirect to same page.</returns>
		[HttpPost]
		public ActionResult LockTimeEntries(int subscriptionId, DateTime startDate, DateTime lockDate)
		{
			LockEntriesResult result = AppService.LockTimeEntries(subscriptionId, lockDate);
			switch (result)
			{
				case LockEntriesResult.InvalidStatuses:
					Notifications.Add(new BootstrapAlert(string.Format(Strings.LockTimeEntriesInvalidStatuses, lockDate.ToShortDateString()), Variety.Danger));
					break;
				case LockEntriesResult.DBError:
					Notifications.Add(new BootstrapAlert(string.Format(Strings.LockTimeEntriesDBError, lockDate.ToShortDateString()), Variety.Danger));
					break;
				case LockEntriesResult.Success:
					Notifications.Add(new BootstrapAlert(string.Format(Strings.LockTimeEntriesSuccess, lockDate.ToShortDateString()), Variety.Success));
					break;
				default:
					throw new ArgumentOutOfRangeException(string.Format(Strings.InvalidEnum, nameof(result), nameof(LockTimeEntries)));
			}

			return RedirectToAction(ActionConstants.Review, new { subscriptionId, startDate, lockDate });
		}

		/// <summary>
		/// Locks all time entries with date that is less than or equal to lockDate
		/// </summary>
		/// <param name="subscriptionId">Subscription that the lock operation will be performed on.</param>
		/// <param name="startDate">Used for the redirect back to the review page -- need to preserve the date range they came in with.</param>
		/// <param name="endDate">The date from which to all all time entries before.  Also the end date of the review page's date range.</param>
		/// <exception cref="ArgumentOutOfRangeException">Expection for if an invalid enum is returned from the service layer.</exception>
		/// <returns>Redirect to same page.</returns>
		[HttpPost]
		public ActionResult UnlockTimeEntries(int subscriptionId, DateTime startDate, DateTime endDate)
		{
			UnlockEntriesResult result = AppService.UnlockTimeEntries(subscriptionId);
			switch (result)
			{
				case UnlockEntriesResult.NoLockDate:
					Notifications.Add(new BootstrapAlert(Strings.UnlockTimeEntriesNoLockDate, Variety.Danger));
					break;
				case UnlockEntriesResult.DBError:
					Notifications.Add(new BootstrapAlert(Strings.UnlockTimeEntriesDbError, Variety.Danger));
					break;
				case UnlockEntriesResult.Success:
					Notifications.Add(new BootstrapAlert(Strings.UnlockTimeEntriesSuccess, Variety.Success));
					break;
				default:
					throw new ArgumentOutOfRangeException(string.Format(Strings.InvalidEnum, nameof(result), nameof(UnlockTimeEntries)));
			}

			return RedirectToAction(ActionConstants.Review, new { subscriptionId, startDate, endDate });
		}
	}
}