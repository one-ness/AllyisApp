//------------------------------------------------------------------------------
// <copyright file="ReviewAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
using AllyisApps.Services.TimeTracker;
using AllyisApps.Utilities;
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
		public async Task<ActionResult> Review(int subscriptionId, DateTime? startDate = null, DateTime? endDate = null)
		{
			int organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			DateTime calculatedStartDate = startDate ?? DateTime.Now.AddMonths(-1);
			DateTime calculatedEndDate = endDate ?? DateTime.Now;

			var payClasses = (await AppService.GetPayClassesByOrganizationId(organizationId)).Select(x => new PayClassInfoViewModel(x)).ToList();
			var subscription = AppService.UserContext.SubscriptionsAndRoles[subscriptionId];
			var allTimeEntries = (await AppService.GetTimeEntriesOverDateRange(organizationId, calculatedStartDate, calculatedEndDate))
				.Select(entry =>
				{
					CompleteProject project = AppService.GetProject(entry.ProjectId);
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

			PayPeriodRanges payperiodRanges = await AppService.GetPayPeriodRanges(organizationId);

			var model = new ReviewViewModel
			{
				UserId = AppService.UserContext.UserId,
				SubscriptionId = subscriptionId,
				OrganizationId = organizationId,
				SubscriptionName = subscription.SubscriptionName,
				PayClasses = payClasses.ToList(),
				TimeEntriesByUser = timeEntriesByUser,
				TimeEntryTotalsByUserByPayClass = timeEntryTotalsByUserByPayClass,
				TimeEntryIdsJSON = JsonConvert.SerializeObject(allTimeEntries.Select(entry => entry.TimeEntryId).ToArray()),
				StartDate = calculatedStartDate,
				EndDate = calculatedEndDate,
				TimeEntryStatusOptions = ModelHelper.GetLocalizedTimeEntryStatuses(AppService),
				PayPeriodRanges = payperiodRanges
			};

			return View(model);
		}

		/// <summary>
		/// Changes the status of all the timeEntries provided.
		/// </summary>
		/// <param name="subscriptionId">Subscription that the time entries belong to.</param>
		/// <param name="timeEntryIdsJson">Int array of time entry ids to change, in JSON string format.</param>
		/// <param name="timeEntryStatusId">Status to change to.</param>
		/// <param name="endDate">Date range for reloading the page.</param>
		/// <param name="startDate">Date range for reloading the page.</param>
		/// <returns>Redirect to same page.</returns>
		public async Task<ActionResult> UpdateTimeEntryStatus(int subscriptionId, string timeEntryIdsJson, int timeEntryStatusId, DateTime startDate, DateTime endDate)
		{
			var timeEntryIds = JsonConvert.DeserializeObject<List<int>>(timeEntryIdsJson);
			if (timeEntryIds.Count == 0)
			{
				Notifications.Add(new BootstrapAlert(Strings.UpdateTimeEntryStatusNoStatusSelected, Variety.Warning));
				return RedirectToAction(ActionConstants.Review, new { subscriptionId, startDate, endDate });
			}

			await timeEntryIds.ForEachAsync(async id => await AppService.UpdateTimeEntryStatusById(id, timeEntryStatusId));

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
		public async Task<ActionResult> LockTimeEntries(int subscriptionId, DateTime startDate, DateTime lockDate)
		{
			LockEntriesResult result = await AppService.LockTimeEntries(subscriptionId, lockDate.Date);
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
				case LockEntriesResult.InvalidLockDate:
					Notifications.Add(new BootstrapAlert(string.Format(Strings.LockTimeEntriesInvalidLockDate, lockDate.ToShortDateString()), Variety.Danger));
					break;
				default:
					throw new ArgumentOutOfRangeException(string.Format(Strings.InvalidEnum, nameof(result), nameof(LockEntriesResult)));
			}

			return RedirectToAction(ActionConstants.Review, new { subscriptionId, startDate, endDate = lockDate });
		}

		/// <summary>
		/// Unlocks all time entries with date that is less than or equal to lockDate
		/// </summary>
		/// <param name="subscriptionId">Subscription that the unlock operation will be performed on.</param>
		/// <param name="startDate">Used for the redirect back to the review page -- need to preserve the date range they came in with.</param>
		/// <param name="endDate">Used for the redirect back to the review page -- need to preserve the date range they came in with.</param>
		/// <exception cref="ArgumentOutOfRangeException">Expection for if an invalid enum is returned from the service layer.</exception>
		/// <returns>Redirect to same page.</returns>
		public async Task<ActionResult> UnlockTimeEntries(int subscriptionId, DateTime startDate, DateTime endDate)
		{
			UnlockEntriesResult result = await AppService.UnlockTimeEntries(subscriptionId);
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
					throw new ArgumentOutOfRangeException(string.Format(Strings.InvalidEnum, nameof(result), nameof(UnlockEntriesResult)));
			}

			return RedirectToAction(ActionConstants.Review, new { subscriptionId, startDate, endDate });
		}

		/// <summary>
		/// Attempts to change the status of all entries between the lock date and the previous payroll process date to PayrollProcessed
		/// </summary>
		/// <param name="subscriptionId">Subscription that the unlock operation will be performed on.</param>
		/// <param name="startDate">Used for the redirect back to the review page -- need to preserve the date range they came in with.</param>
		/// <param name="endDate">Used for the redirect back to the review page -- need to preserve the date range they came in with.</param>
		/// <exception cref="ArgumentOutOfRangeException">Expection for if an invalid enum is returned from the service layer.</exception>
		/// <returns>Redirect to same page.</returns>
		public async Task<ActionResult> PayrollProcessTimeEntries(int subscriptionId, DateTime startDate, DateTime endDate)
		{
			PayrollProcessEntriesResult result = await AppService.PayrollProcessTimeEntriesAsync(subscriptionId);
			switch (result)
			{
				case PayrollProcessEntriesResult.NoLockDate:
					Notifications.Add(new BootstrapAlert(Strings.PayrollProcessNoLockDate, Variety.Danger));
					break;
				case PayrollProcessEntriesResult.DBError:
					Notifications.Add(new BootstrapAlert(Strings.PayrollProcessDBError, Variety.Danger));
					break;
				case PayrollProcessEntriesResult.InvalidStatuses:
					Notifications.Add(new BootstrapAlert(Strings.PayrollProcessInvalidStatuses, Variety.Danger));
					break;
				case PayrollProcessEntriesResult.Success:
					Notifications.Add(new BootstrapAlert(Strings.PayrollProcessSuccess, Variety.Success));
					break;
				default:
					throw new ArgumentOutOfRangeException(string.Format(Strings.InvalidEnum, nameof(result), nameof(PayrollProcessEntriesResult)));
			}

			return RedirectToAction(ActionConstants.Review, new { subscriptionId, startDate, endDate });
		}
	}
}