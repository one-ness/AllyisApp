//------------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using AllyisApps.DBModel.TimeTracker;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all account related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
		public Setting GetSettingsByOrganizationId(int organizationId)
		{
			if (organizationId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(organizationId), $"{nameof(organizationId)} must be greater than 0.");
			}

			return DBEntityToServiceObject(DBHelper.GetSettingsByOrganizationId(organizationId));
		}

		/// <summary>
		/// Locks all time entries with date that is less than or equal to lockDate
		/// </summary>
		/// <param name="subscriptionId">Subscription that the lock operation will be performed on.</param>
		/// <param name="lockDate">The date from which to all all time entries before.  Also the end date of the review page's date range.</param>
		/// <returns>Redirect to same page.</returns>
		public LockEntriesResult LockTimeEntries(int subscriptionId, DateTime lockDate)
		{
			if (subscriptionId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(subscriptionId), $"{nameof(subscriptionId)} must not be negative.");
			}

			int organizationId = UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			Setting timeTrackerSettings = GetSettingsByOrganizationId(organizationId);
			DateTime startDate = timeTrackerSettings.LockDate ?? System.Data.SqlTypes.SqlDateTime.MinValue.Value;
			var timeEntries = GetTimeEntriesOverDateRange(organizationId, startDate, lockDate);

			if (timeEntries.Any(entry =>
				(TimeEntryStatus)entry.TimeEntryStatusId != TimeEntryStatus.Approved ||
				(TimeEntryStatus)entry.TimeEntryStatusId != TimeEntryStatus.Rejected))
			{
				return LockEntriesResult.InvalidStatuses;
			}

			return UpdateLockDate(organizationId, lockDate) == 1 ? LockEntriesResult.Success : LockEntriesResult.DBError;
		}

		public UnlockEntriesResult UnlockTimeEntries(int subscriptionId)
		{
			if (subscriptionId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(subscriptionId), $"{nameof(subscriptionId)} must not be negative.");
			}

			int organizationId = UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			Setting timeTrackerSettings = GetSettingsByOrganizationId(organizationId);

			if (timeTrackerSettings.LockDate == null)
			{
				return UnlockEntriesResult.NoLockDate;
			}

			DateTime? lockDate = timeTrackerSettings.PayrollProcessedDate == null ? null : timeTrackerSettings.LockDate;

			return UpdateLockDate(organizationId, lockDate) == 1 ? UnlockEntriesResult.Success : UnlockEntriesResult.DBError;
		}

		public PayrollProcessEntriesResult PayrollProcessTimeEntries(int subscriptionId)
		{
			// Validate params
			if (subscriptionId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(subscriptionId), $"{nameof(subscriptionId)} must not be negative.");
			}

			int organizationId = UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			if (organizationId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(organizationId), $"{nameof(organizationId)} must not be negative.");
			}

			// Validate that there are locked entries to process
			Setting timeTrackerSettings = GetSettingsByOrganizationId(organizationId);
			if (timeTrackerSettings.LockDate == null)
			{
				return PayrollProcessEntriesResult.NoLockDate;
			}

			// Validate that all statuses in the affected date range are correct (!Pending)
			DateTime startDate = timeTrackerSettings.PayrollProcessedDate ?? System.Data.SqlTypes.SqlDateTime.MinValue.Value;
			var timeEntries = DBHelper.GetTimeEntriesOverDateRange(organizationId, startDate, timeTrackerSettings.LockDate.Value).ToList();
			if (timeEntries.Any(entry => entry.TimeEntryStatusId == (int)TimeEntryStatus.Pending))
			{
				return PayrollProcessEntriesResult.InvalidStatuses;
			}

			// Perform payroll process and lock date update,
			// Validate no unexpected db behavior (should be only one row updated)
			if (DBHelper.UpdatePayrollProcessedDate(organizationId, timeTrackerSettings.LockDate.Value) != 1
				|| DBHelper.UpdateLockDate(organizationId, null) != 1)
			{
				return PayrollProcessEntriesResult.DBError;
			}

			//Update approved entries to payroll processed
			timeEntries
				.Where(entry => entry.TimeEntryStatusId == (int)TimeEntryStatus.Approved)
				.ToList()
				.ForEach(entry => DBHelper.UpdateTimeEntryStatusById(entry.TimeEntryId, (int)TimeEntryStatus.PayrollProcessed));

			return PayrollProcessEntriesResult.Success;
		}

		#region public static

		public static DateTime SetStartingDate(DateTime? date, int startOfWeek)
		{
			if (date != null) return date.Value.Date;

			DateTime today = DateTime.Now;
			int daysIntoTheWeek = (int)today.DayOfWeek < startOfWeek
				? (int)today.DayOfWeek + (7 - startOfWeek)
				: (int)today.DayOfWeek - startOfWeek;

			return today.AddDays(-daysIntoTheWeek).Date;
		}

		public static Setting DBEntityToServiceObject(SettingDBEntity settings)
		{
			return new Setting
			{
				OrganizationId = settings.OrganizationId,
				StartOfWeek = settings.StartOfWeek,
				OvertimeHours = settings.OvertimeHours,
				OvertimePeriod = settings.OvertimePeriod,
				OvertimeMultiplier = settings.OvertimeMultiplier,
				IsLockDateUsed = settings.IsLockDateUsed,
				LockDatePeriod = settings.LockDatePeriod,
				LockDateQuantity = settings.LockDateQuantity,
				PayrollProcessedDate = settings.PayrollProcessedDate,
				LockDate = settings.LockDate,
				PayPeriod = settings.PayPeriod
			};
		}

		#endregion public static
	}
}