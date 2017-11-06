//------------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.DBModel.TimeTracker;
using AllyisApps.Services.TimeTracker;
using Newtonsoft.Json;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all account related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
		public async Task<Setting> GetSettingsByOrganizationId(int organizationId)
		{
			if (organizationId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(organizationId), $"{nameof(organizationId)} must be greater than 0.");
			}

			return DBEntityToServiceObject(await DBHelper.GetSettingsByOrganizationId(organizationId));
		}

		/// <summary>
		/// Locks all time entries with date that is less than or equal to lockDate
		/// </summary>
		/// <param name="subscriptionId">Subscription that the lock operation will be performed on.</param>
		/// <param name="lockDate">The date from which to all all time entries before.  Also the end date of the review page's date range.</param>
		/// <returns>Redirect to same page.</returns>
		public async Task<LockEntriesResult> LockTimeEntries(int subscriptionId, DateTime lockDate)
		{
			if (subscriptionId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(subscriptionId), $"{nameof(subscriptionId)} must not be negative.");
			}

			int organizationId = UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			Setting timeTrackerSettings = await GetSettingsByOrganizationId(organizationId);
			DateTime startDate = timeTrackerSettings.LockDate ?? timeTrackerSettings.PayrollProcessedDate ?? System.Data.SqlTypes.SqlDateTime.MinValue.Value;
			startDate = startDate.AddDays(1);

			if (startDate > lockDate)
			{
				return LockEntriesResult.InvalidLockDate;
			}

			var timeEntries = await GetTimeEntriesOverDateRange(organizationId, startDate, lockDate);
			if (timeEntries.Any(entry =>
				entry.TimeEntryStatusId == (int)TimeEntryStatus.Pending ||
				entry.TimeEntryStatusId == (int)TimeEntryStatus.PayrollProcessed))
			{
				return LockEntriesResult.InvalidStatuses;
			}

			return UpdateLockDate(organizationId, lockDate) == 1 ? LockEntriesResult.Success : LockEntriesResult.DBError;
		}

		public async Task<UnlockEntriesResult> UnlockTimeEntries(int subscriptionId)
		{
			if (subscriptionId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(subscriptionId), $"{nameof(subscriptionId)} must not be negative.");
			}

			int organizationId = UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			Setting timeTrackerSettings = await GetSettingsByOrganizationId(organizationId);

			if (timeTrackerSettings.LockDate == null)
			{
				return UnlockEntriesResult.NoLockDate;
			}

			return UpdateLockDate(organizationId, null) == 1 ? UnlockEntriesResult.Success : UnlockEntriesResult.DBError;
		}

		public async Task<PayrollProcessEntriesResult> PayrollProcessTimeEntriesAsync(int subscriptionId)
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
			Setting timeTrackerSettings = await GetSettingsByOrganizationId(organizationId);
			if (timeTrackerSettings.LockDate == null)
			{
				return PayrollProcessEntriesResult.NoLockDate;
			}

			// Validate that all statuses in the affected date range are correct (!Pending)
			DateTime startDate = timeTrackerSettings.PayrollProcessedDate ?? System.Data.SqlTypes.SqlDateTime.MinValue.Value;
			startDate = startDate.AddDays(1);
			var timeEntries = await DBHelper.GetTimeEntriesOverDateRange(organizationId, startDate, timeTrackerSettings.LockDate.Value);
			if (timeEntries.Any(entry =>
				entry.TimeEntryStatusId == (int)TimeEntryStatus.Pending ||
				entry.TimeEntryStatusId == (int)TimeEntryStatus.PayrollProcessed))
			{
				return PayrollProcessEntriesResult.InvalidStatuses;
			}

			//Update approved entries to payroll processed
			timeEntries = timeEntries.Where(entry => entry.TimeEntryStatusId == (int)TimeEntryStatus.Approved).ToList();

			foreach (var entry in timeEntries)
			{
				await DBHelper.UpdateTimeEntryStatusById(entry.TimeEntryId, (int)TimeEntryStatus.PayrollProcessed);
			}

			// Perform payroll process and lock date update,
			// Validate no unexpected db behavior (should be only one row updated)
			if (DBHelper.UpdatePayrollProcessedDate(organizationId, timeTrackerSettings.LockDate.Value) != 1
				|| DBHelper.UpdateLockDate(organizationId, null) != 1)
			{
				return PayrollProcessEntriesResult.DBError;
			}

			return PayrollProcessEntriesResult.Success;
		}

		public async Task<int> UpdateDurationPayPeriod(int duration, DateTime startDate, int organizationId)
		{
			if (duration < 1 || duration > 365)
			{
				throw new ArgumentOutOfRangeException(nameof(duration), $"{nameof(duration)} must be between 1 and 365.");
			}

			if (startDate < SqlDateTime.MinValue)
			{
				throw new ArgumentOutOfRangeException(nameof(startDate), $"{nameof(startDate)} must be greater than {SqlDateTime.MinValue}.");
			}

			if (organizationId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(organizationId), $"{nameof(organizationId)} must be greater than 0.");
			}

			var jsonDic = new Dictionary<string, string>
			{
				{"type", "duration"},
				{"duration", $"{duration}"},
				{"startDate", startDate.ToString("d")}
			};

			string payPeriodJson = JsonConvert.SerializeObject(jsonDic);

			return await DBHelper.UpdatePayPeriod(payPeriodJson, organizationId);
		}

		public async Task<int> UpdateDatesPayPeriod(List<int> dates, int organizationId)
		{
			if (dates == null)
			{
				throw new ArgumentNullException(nameof(dates));
			}

			if (dates.Any(date => date <= 0 || date > 28))
			{
				throw new ArgumentOutOfRangeException(nameof(dates), $"{dates} must be between 1 and 28.");
			}

			if (organizationId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(organizationId), $"{nameof(organizationId)} must be greater than 0.");
			}

			var jsonDic = new Dictionary<string, dynamic>
			{
				{"type", "duration"},
				{"dates", dates.ToArray()}
			};

			string payPeriodJson = JsonConvert.SerializeObject(jsonDic);

			return await DBHelper.UpdatePayPeriod(payPeriodJson, organizationId);
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
				PayrollProcessedDate = settings.PayrollProcessedDate,
				LockDate = settings.LockDate,
				PayPeriod = settings.PayPeriod,

			};
		}

		#endregion public static
	}
}