//------------------------------------------------------------------------------
// <copyright file="ReviewAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services.Crm;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using Newtonsoft.Json;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <inheritdoc />
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

			// if no start or end date specified, use default start and end dates.  Redirect for proper url route.
			if (!startDate.HasValue || !endDate.HasValue)
			{
				var payPeriods = await AppService.GetPayPeriodRanges(organizationId);
				startDate = payPeriods.Previous.StartDate;
				endDate = payPeriods.Previous.EndDate;
				return RedirectToAction(ActionConstants.Review, new { subscriptionId, startDate, endDate });
			}

			var model = await ConstructReviewViewModel(organizationId, subscriptionId, startDate.Value, endDate.Value);

			return View(model);
		}

		private async Task<ReviewViewModel> ConstructReviewViewModel(int organizationId, int subscriptionId, DateTime startDate, DateTime endDate)
		{
			var payClasses = (await AppService.GetPayClassesByOrganizationId(organizationId)).Select(x => new PayClassInfoViewModel(x)).ToList();
			var projects = (await AppService.GetProjectsByOrganization(organizationId, false)).ToDictionary(pro => pro.ProjectId);

			var subscription = AppService.UserContext.SubscriptionsAndRoles[subscriptionId];
			var allTimeEntries = (await AppService.GetTimeEntriesOverDateRange(organizationId, startDate, endDate))
				.Select(entry =>
				{
					CompleteProject project = projects[entry.ProjectId];
					return new TimeEntryViewModel(entry)
					{
						ProjectName = project.ProjectName,
						CustomerName = project.OwningCustomer.CustomerName
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
			var settings = await AppService.GetSettingsByOrganizationId(organizationId);
			var payPeriodRangesViewModel = new PayPeriodRangesViewModel(payperiodRanges);

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
				StartDate = startDate,
				EndDate = endDate,
				TimeEntryStatusOptions = ModelHelper.GetLocalizedTimeEntryStatuses(),
				PayPeriodRanges = payPeriodRangesViewModel,
				LockDate = settings.LockDate,
				PayrollProcessedDate = settings.PayrollProcessedDate
			};

			return model;
		}

		/// <summary>
		/// Changes the status of all the timeEntries provided.
		/// </summary>
		/// <param name="subscriptionId">Subscription that the time entries belong to.</param>
		/// <param name="timeEntryIdsJson">Int array of time entry ids to change, in JSON string format.</param>
		/// <param name="timeEntryUserIdsJson"></param>
		/// <param name="timeEntryStatusId">Status to change to.</param>
		/// <param name="endDate">Date range for reloading the page.</param>
		/// <param name="startDate">Date range for reloading the page.</param>
		/// <returns>Redirect to same page.</returns>
		public async Task<ActionResult> UpdateTimeEntryStatus(int subscriptionId, string timeEntryIdsJson, string timeEntryUserIdsJson, int timeEntryStatusId, DateTime startDate, DateTime endDate)
		{
			int organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var timeEntryIds = JsonConvert.DeserializeObject<List<int>>(timeEntryIdsJson);
			var userIds = JsonConvert.DeserializeObject<List<int>>(timeEntryUserIdsJson);
			HashSet<int> timeEntries = new HashSet<int>(timeEntryIds);

			if (timeEntryIds.Count == 0 && userIds.Count == 0)
			{
				Notifications.Add(new BootstrapAlert(Strings.UpdateTimeEntryStatusNoStatusSelected, Variety.Warning));
				return RedirectToAction(ActionConstants.Review, new { subscriptionId, startDate, endDate });
			}

			var settings = await AppService.GetSettingsByOrganizationId(organizationId);
			var effectiveLockDate = settings.LockDate ?? settings.PayrollProcessedDate ?? SqlDateTime.MinValue.Value;
			var startDateUnlocked = effectiveLockDate >= startDate ? effectiveLockDate.AddDays(1) : startDate;

			var orgTimeEntries = (await AppService.GetTimeEntriesOverDateRange(organizationId, startDateUnlocked, endDate)).ToLookup(tt => tt.UserId);

			foreach (int userId in userIds)
			{
				var userTimeentrys = orgTimeEntries[userId];
				foreach (var userTimeEntry in userTimeentrys)
				{
					timeEntries.Add(userTimeEntry.TimeEntryId);
				}
			}

			if (timeEntries.Count == 0)
			{
				Notifications.Add(new BootstrapAlert(Strings.UpdateTimeEntryStatusNoStatusSelected, Variety.Warning));
				return RedirectToAction(ActionConstants.Review, new { subscriptionId, startDate, endDate });
			}

			var tasks = new List<Task>();
			timeEntries.ToList().ForEach(id => tasks.Add(AppService.UpdateTimeEntryStatusById(id, timeEntryStatusId)));
			await Task.WhenAll(tasks);

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
				case LockEntriesResult.NoChange:
					Notifications.Add(new BootstrapAlert(string.Format(Strings.LockTimeEntriesNoChange, lockDate.ToShortDateString()), Variety.Warning));
					break;
				case LockEntriesResult.SuccessAndRecalculatedOvertime:
					Notifications.Add(new BootstrapAlert(string.Format(Strings.LockEntriesResultSuccessAndRecalculatedOvertime, lockDate.ToShortDateString()), Variety.Success));
					break;
				case LockEntriesResult.InvalidLockDateWithOvertimeChange:
					Notifications.Add(new BootstrapAlert(string.Format(Strings.LockTimeEntriesInvalidLockDateWithOvertimeChange, lockDate.ToShortDateString()), Variety.Danger));
					break;
				default:
					throw new ArgumentOutOfRangeException(string.Format(Strings.InvalidEnum, nameof(result), nameof(LockEntriesResult)));
			}

			return RedirectToAction(ActionConstants.Review, new { subscriptionId, startDate = startDate.ToString("yyy-MM-dd"), endDate = lockDate.ToString("yyy-MM-dd") });
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
				case UnlockEntriesResult.InvalidLockDate:
					Notifications.Add(new BootstrapAlert(Strings.UnlockTimeEntriesInvalidLockDate, Variety.Danger));
					break;
				case UnlockEntriesResult.SuccessAndRecalculatedOvertime:
					Notifications.Add(new BootstrapAlert(Strings.UnlockEntriesResultSuccessAndRecalculatedOvertime, Variety.Success));
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
			PayrollProcessEntriesResult result = await AppService.PayrollProcessTimeEntries(subscriptionId);
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


		/// <summary>
		/// Gets the user time entires and returns the partial view with this data.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="subscriptionId"></param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <param name="isChecked">Bool for whether or not the row check boxes are checked.</param>
		/// <returns>Partial view; time entry rows in the review page table.</returns>
		[HttpPost]
		public async Task<ActionResult> GetUserReviewTimeEntries(int userId, int subscriptionId, DateTime startDate, DateTime endDate, bool isChecked)
		{
			int organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var timeEntries = await AppService.GetTimeEntriesByUsersOverDateRange(new List<int> { userId }, startDate, endDate, organizationId);
			var payClasses = await AppService.GetPayClassesByOrganizationId(organizationId);

			var model = new TimeEntryUserReviewViewModel
			{
				UserTimeEntries = timeEntries.Select(te => new TimeEntryViewModel(te)),
				PayClasses = payClasses.Select(b => new PayClassInfoViewModel(b)),
				UserId = userId,
				IsChecked = isChecked
			};

			return PartialView(ViewConstants.ReviewUserTimeEntries, model);

		}
	}
}