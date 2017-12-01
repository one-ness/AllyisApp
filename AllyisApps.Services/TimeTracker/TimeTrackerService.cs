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
using Newtonsoft.Json.Linq;

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
			Setting settings = await GetSettingsByOrganizationId(organizationId);

			if (lockDate <= settings?.PayrollProcessedDate)
			{
				return LockEntriesResult.InvalidLockDate;
			}

			if (lockDate < settings?.LockDate)
			{
				return UpdateLockDate(organizationId, lockDate) == 1 ? LockEntriesResult.Success : LockEntriesResult.DBError;
			}

			if (lockDate == settings?.LockDate)
			{
				return LockEntriesResult.NoChange;
			}

			DateTime startDate = settings.LockDate ?? settings.PayrollProcessedDate ?? SqlDateTime.MinValue.Value;
			startDate = startDate.AddDays(1);
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
			DateTime startDate = timeTrackerSettings.PayrollProcessedDate ?? SqlDateTime.MinValue.Value;
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
				{"type", PayPeriodType.Duration.GetEnumName()},
				{"duration", duration.ToString()},
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
				{"type", PayPeriodType.Dates.ToString()},
				{"dates", dates.ToArray()}
			};

			string payPeriodJson = JsonConvert.SerializeObject(jsonDic);

			return await DBHelper.UpdatePayPeriod(payPeriodJson, organizationId);
		}

		public async Task<int> CreateEmployeeType(int orgId, string employeeName)
		{
			if (!(orgId >= 0)) throw new ArgumentNullException("OrgId");
			if (string.IsNullOrEmpty(employeeName)) throw new ArgumentNullException("employeeName");

			var result = await DBHelper.CreateEmployeeType(orgId, employeeName);

			return result;
		}

		public async Task DeleteEmployeeType(int employeeTypeId)
		{
			if (employeeTypeId <= 0) throw new ArgumentNullException("EmployeeTypeId");

			await DBHelper.DeleteEmployeeType(employeeTypeId);

			return;
		}

		public async Task<EmployeeType> GetEmployeeType(int employeeTypeId)
		{
			if (employeeTypeId <= 0) throw new ArgumentNullException("EmployeeTypeId");
			var results = await DBHelper.GetEmployeeType(employeeTypeId);
			EmployeeType employee = new EmployeeType() { EmployeeTypeId = results.EmployeeTypeId, EmployeeTypeName = results.EmployeeTypeName, OrganizationId = results.OrganizationId };
			return employee;
		}

		public async Task UpdateUserOrgEmployeeType(int userId, int employeeType)
		{
			await DBHelper.UpdateUserOrgEmployeeType(userId, employeeType);
		}

		public async Task<List<int>> GetAssignedPayClasses(int employeeTypeId)
		{
			if (employeeTypeId <= 0) throw new ArgumentNullException("EmployeeTypeId");

			return await DBHelper.GetAssignedPayClasses(employeeTypeId);
		}

		public async Task AddPayClassToEmployeeType(int employeeTypeId, int payClassId)
		{
			if (employeeTypeId <= 0) throw new ArgumentNullException("EmployeeTypeId");
			if (payClassId <= 0) throw new ArgumentNullException("payClassId");

			await DBHelper.AddPayClassToEmployeeType(employeeTypeId, payClassId);
		}

		public async Task RemovePayClassFromEmployeeType(int employeeTypeId, int payClassId)
		{
			if (employeeTypeId <= 0) throw new ArgumentNullException("EmployeeTypeId");
			if (payClassId <= 0) throw new ArgumentNullException("payClassId");

			await DBHelper.RemovePayClassFromEmployeeType(employeeTypeId, payClassId);
		}

		public async Task<List<EmployeeType>> GetEmployeeTypeByOrganization(int organizationId)
		{
			if (organizationId <= 0) throw new ArgumentNullException("OrgId");

			var results = (await DBHelper.GetEmployeeTypesByOrganization(organizationId)).Select(x => new EmployeeType { EmployeeTypeId = x.EmployeeTypeId, EmployeeTypeName = x.EmployeeTypeName, OrganizationId = x.OrganizationId });

			return results.ToList();
		}

		public async Task<PayPeriodRanges> GetPayPeriodRanges(int organizationId)
		{
			if (organizationId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(organizationId), $"{nameof(organizationId)} must be greater than 0.");
			}

			var settings = await GetSettingsByOrganizationId(organizationId);
			dynamic payPeriodObj = JsonConvert.DeserializeObject(settings.PayPeriod);
			var payPeriodEnum = (PayPeriodType)Enum.Parse(typeof(PayPeriodType), (string)payPeriodObj.type);

			switch (payPeriodEnum)
			{
				case PayPeriodType.Duration:
					var startDate = (DateTime)payPeriodObj.startDate;
					var duration = (int)payPeriodObj.duration;
					var currentPayPeriod = GetCurrentPayPeriodByDuration(duration, startDate);

					return new PayPeriodRanges
					{
						Previous = GetNthPayPeriodByDuration(currentPayPeriod, -1),
						Current = currentPayPeriod,
						Next = GetNthPayPeriodByDuration(currentPayPeriod, 1)
					};

				case PayPeriodType.Dates:
					var dates = ((JArray)payPeriodObj.dates).Select(date => (int)date).ToList();

					return new PayPeriodRanges
					{
						Previous = GetNthPayPeriodByDates(dates, -1),
						Current = GetNthPayPeriodByDates(dates, 0),
						Next = GetNthPayPeriodByDates(dates, 1)
					};

				default:
					throw new ArgumentOutOfRangeException(nameof(payPeriodObj), "The pay period object just be of type 'duration' or 'dates'.");
			}
		}

		#region public static

		public async Task<bool> CheckUpdateProjectStartEndDate(int projectId, DateTime? newStartDate, DateTime? newEndDate)
		{
			// Get all time entries for selected project
			var project = DBHelper.GetProjectById(projectId);
			var oldStartDate = project.StartDate ?? (DateTime)SqlDateTime.MinValue;
			var oldEndDate = project.EndDate ?? (DateTime)SqlDateTime.MaxValue;
			var entries = await DBHelper.GetTimeEntriesOverDateRange(project.OrganizationId, oldStartDate, oldEndDate);

			// Check new End Date, see if time entries are outside of new project date range
			bool outsideNewDateRange = false;
			foreach (var entry in entries)
			{
				if (entry.ProjectId == projectId) //only check for the specific project
				{
					if (newEndDate != null && entry.Date > newEndDate)
					{
						outsideNewDateRange = true;
						throw new ArgumentOutOfRangeException(nameof(newEndDate), String.Format("{0}{1}{2}", "Cannot change Project End Date to the specified new date, there are currently time entries outside of the project's new date range: (", entry.Date.ToShortDateString(), "). "));
					}
					if (newStartDate != null && entry.Date < newStartDate)
					{
						outsideNewDateRange = true;
						throw new ArgumentOutOfRangeException(nameof(newStartDate), String.Format("{0}{1}{2}", "Cannot change Project Start Date to the specified new date, there are currently time entries outside of the project's new date range: (", entry.Date.ToShortDateString(), "). "));
					}
				}
			}

			return outsideNewDateRange;
		}

		public static DateTime SetStartingDate(DateTime? date, int startOfWeek)
		{
			if (date != null) return date.Value.Date;

			DateTime today = DateTime.Now;
			int daysIntoTheWeek = (int)today.DayOfWeek < startOfWeek
				? (int)today.DayOfWeek + (7 - startOfWeek)
				: (int)today.DayOfWeek - startOfWeek;

			return today.AddDays(-daysIntoTheWeek).Date;
		}

		/// <summary>
		/// Gets the pay period that is n pay periods away from the current pay period.
		/// </summary>
		/// <param name="dates">List of month days. Each represent the startDate of a pay period.</param>
		/// <param name="n">The number of pay periods away from the current pay period, e.g.
		/// 1 is 1 pay period after the current
		/// 0 is the current pay period
		/// -1 is 1 pay period before the current.
		/// </param>
		/// <returns></returns>
		public static PayPeriodRanges.PayPeriodRange GetNthPayPeriodByDates(List<int> dates, int n)
		{
			dates.Sort();
			int startIndex = 0;

			// find the start index of the current pay period (where today is inbetween dates[startIndex] and dates[startIndex+1]
			for (int i = 0; i < dates.Count; i++)
			{
				if (DateTime.Now.Day < dates[i])
				{
					startIndex = Mod(i - 1, dates.Count);
					break;
				}

				if (i == dates.Count - 1)
				{
					startIndex = i;
				}
			}

			// navigate to correct startIndex of nth pay period, and calculate any overflow
			int monthOverflow = 0;
			if (n >= 0)
			{
				for (int i = 0; i < n; i++)
				{
					int nextIndex = Mod(startIndex + 1, dates.Count);

					if (nextIndex <= startIndex)
					{
						monthOverflow++;
					}

					startIndex = nextIndex;
				}
			}
			else
			{
				for (int i = n; i < 0; i++)
				{
					int nextIndex = Mod(startIndex - 1, dates.Count);

					if (nextIndex >= startIndex)
					{
						monthOverflow--;
					}

					startIndex = nextIndex;
				}
			}

			//calculate year, month including overflows for startDate
			int month = monthOverflow + DateTime.Now.Month - 1; // subtract 1 to be compatible with 0 based index modulus
			int year = DateTime.Now.Year + month / 11;
			month = Mod(month, 12) + 1; //add 1 to convert to 1 base index

			// calculate endDate with conditionals to handle month and year overflow
			DateTime endDate = startIndex == dates.Count - 1
				? (month == 12
					? new DateTime(year + 1, 1, dates[0])
					: new DateTime(year, month + 1, dates[0]))
				: new DateTime(year, month, dates[Mod(startIndex + 1, dates.Count)]);

			var range = new PayPeriodRanges.PayPeriodRange();
			range.StartDate = new DateTime(year, month, dates[startIndex]);
			range.EndDate = endDate.AddDays(-1);
			return range;
		}

		/// <summary>
		/// Calculates the pay period that contains today.  Calculated with start date and duration.
		/// </summary>
		/// <param name="duration">The duration of the pay period.</param>
		/// <param name="startDate">The start date from which to base the pay period dates on.</param>
		/// <returns>The current pay period range.</returns>
		public static PayPeriodRanges.PayPeriodRange GetCurrentPayPeriodByDuration(int duration, DateTime startDate)
		{
			DateTime endDate = startDate;

			if (startDate < DateTime.Now)
			{
				while (endDate < DateTime.Now)
				{
					startDate = endDate;
					endDate = endDate.AddDays(duration);
				}
			}
			else if (startDate > DateTime.Now)
			{
				while (startDate > DateTime.Now)
				{
					endDate = startDate;
					startDate = startDate.AddDays(-duration);
				}
			}

			return new PayPeriodRanges.PayPeriodRange
			{
				StartDate = startDate,
				EndDate = startDate == DateTime.Now ? startDate.AddDays(duration - 1) : endDate.AddDays(-1) //end date is one day before the next start date
			};
		}

		/// <summary>
		/// Calculates the pay period that is n number of pay periods away from the current pay period, by duration.
		/// </summary>
		/// <param name="currentPayPeriod">The current pay period range (that contains today).</param>
		/// <param name="numberOfPayPeriodsAway">
		/// The number of pay periods displaced from the current pay period. e.g.:
		/// 1 is 1 pay period after the current
		/// 0 is the current pay period
		/// -1 is 1 pay period before the current.
		/// </param>
		/// <returns>The nth pay period away from the current one.</returns>
		public static PayPeriodRanges.PayPeriodRange GetNthPayPeriodByDuration(PayPeriodRanges.PayPeriodRange currentPayPeriod, int numberOfPayPeriodsAway)
		{
			return new PayPeriodRanges.PayPeriodRange
			{
				StartDate = currentPayPeriod.StartDate.AddDays(numberOfPayPeriodsAway * ((currentPayPeriod.EndDate - currentPayPeriod.StartDate).TotalDays + 1)),
				EndDate = currentPayPeriod.EndDate.AddDays(numberOfPayPeriodsAway * ((currentPayPeriod.EndDate - currentPayPeriod.StartDate).TotalDays + 1))
			};
		}

		public static Setting DBEntityToServiceObject(SettingDBEntity settings)
		{
			return new Setting
			{
				OrganizationId = settings.OrganizationId,
				StartOfWeek = settings.StartOfWeek,
				OvertimeHours = settings.OvertimeHours,
				OvertimePeriod = settings.OvertimePeriod,
				PayrollProcessedDate = settings.PayrollProcessedDate,
				LockDate = settings.LockDate,
				PayPeriod = settings.PayPeriod
			};
		}

		/// <summary>
		/// Does a real modulo operation.  Unlike c#'s % operator.
		/// </summary>
		/// <param name="x">Left side.</param>
		/// <param name="m">Right side.</param>
		/// <returns></returns>
		public static int Mod(int x, int m)
		{
			return (x % m + m) % m;
		}

		#endregion public static
	}
}