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

		public bool LockTimeEntries(int subscriptionId, DateTime lockDate)
		{
			var organizationId = UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var timeTrackerSettings = GetSettingsByOrganizationId(organizationId);
			var startDate = timeTrackerSettings.LockDate == null ? DateTime.MinValue : timeTrackerSettings.LockDate;
			var timeEntries = GetTimeEntriesOverDateRange(organizationId, startDate, lockDate);

			if (timeEntries.Any(entry =>
				(TimeEntryStatus)entry.TimeEntryStatusId != TimeEntryStatus.Approved ||
				(TimeEntryStatus)entry.TimeEntryStatusId != TimeEntryStatus.Rejected))
			{
				return false;
			}

			return UpdateLockDate(organizationId, lockDate) == 1;
		}

		#region public static

		public static DateTime SetStartingDate(DateTime? date, int startOfWeek)
		{
			if (date == null && !date.HasValue)
			{
				DateTime today = DateTime.Now;
				int daysIntoTheWeek = (int)today.DayOfWeek < startOfWeek
					? (int)today.DayOfWeek + (7 - startOfWeek)
					: (int)today.DayOfWeek - startOfWeek;

				date = today.AddDays(-daysIntoTheWeek);
			}

			return date.Value.Date;
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