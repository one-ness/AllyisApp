//------------------------------------------------------------------------------
// <copyright file="TimeTrackerService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services.Account;
using AllyisApps.Services.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.Services.TimeTracker
{
	/// <summary>
	/// The TimeTracker Service.
	/// </summary>
	public class TimeTrackerService : BaseService
	{
		/// <summary>
		/// Authorization in use for select methods.
		/// </summary>
		private AuthorizationService authorizationService;

		/// <summary>
		/// Initializes a new instance of the <see cref="TimeTrackerService"/> class.
		/// </summary>
		/// <param name="connectionString">The Connection string.</param>
		public TimeTrackerService(string connectionString) : base(connectionString)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TimeTrackerService"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="userContext">The user context.</param>
		public TimeTrackerService(string connectionString, UserContext userContext) : base(connectionString, userContext)
		{
			this.authorizationService = new AuthorizationService(connectionString, userContext);
		}

		/// <summary>
		/// Converts an int representing days since the DateTime min value (Jan 1st, 0001) into a DateTime date.
		/// </summary>
		/// <param name="days">An int of the date as days since Jan 1st, 0001.</param>
		/// <returns>The DateTime date.</returns>
		public static DateTime GetDateFromDays(int days)
		{
			return DateTime.MinValue.AddDays(days);
		}

		/// <summary>
		/// Converts a DateTime date into an int representing days since the DateTime min value (Jan 1st, 0001).
		/// </summary>
		/// <param name="date">The DateTime date.</param>
		/// <returns>An int of the date as days since Jan 1st, 0001.</returns>
		public int GetDayFromDateTime(DateTime date)
		{
			return (int)date.Subtract(DateTime.MinValue).TotalDays;
		}

		/// <summary>
		/// Converts an int representing days since the DateTime min value (Jan 1st, 0001) into a DateTime date.
		/// </summary>
		/// <param name="days">An int of the date as days since Jan 1st, 0001.</param>
		/// <returns>The DateTime date.</returns>
		public DateTime GetDateTimeFromDays(int days)
		{
			return GetDateFromDays(days);
		}

		/// <summary>
		/// Gets a <see cref="TimeEntryInfo"/>.
		/// </summary>
		/// <param name="timeEntryId">Time entry Id.</param>
		/// <returns>A TimeEntryDBEntity.</returns>
		public TimeEntryInfo GetTimeEntry(int timeEntryId)
		{
			#region Validation

			if (timeEntryId <= 0)
			{
				throw new ArgumentOutOfRangeException("timeEntryId", "Time entry id cannot be 0 or negative.");
			}

			#endregion Validation

			return BusinessObjectsHelper.InitializeTimeEntryInfo(DBHelper.GetTimeEntryById(timeEntryId));
		}

		/// <summary>
		/// Creates a time entry.
		/// </summary>
		/// <param name="entry">Time entry info.</param>
		/// <returns>Time entry id.</returns>
		public int CreateTimeEntry(TimeEntryInfo entry)
		{
			#region Validation

			if (entry == null)
			{
				throw new ArgumentNullException("entry", "Time entry must not be null.");
			}

			#endregion Validation

			return DBHelper.CreateTimeEntry(BusinessObjectsHelper.GetDBEntityFromTimeEntryInfo(entry));
		}

		/// <summary>
		/// Updates a time entry.
		/// </summary>
		/// <param name="entry">Updated info.</param>
		public void UpdateTimeEntry(TimeEntryInfo entry)
		{
			#region Validation

			if (entry == null)
			{
				throw new ArgumentNullException("entry", "Time entry must not be null.");
			}

			#endregion Validation

			DBHelper.UpdateTimeEntry(BusinessObjectsHelper.GetDBEntityFromTimeEntryInfo(entry));
		}

		/// <summary>
		/// Deletes a time entry.
		/// </summary>
		/// <param name="timeEntryId">Time entry Id.</param>
		public void DeleteTimeEntry(int timeEntryId)
		{
			#region Validation

			if (timeEntryId <= 0)
			{
				throw new ArgumentOutOfRangeException("timeEntryId", "Time entry id cannot be 0 or negative.");
			}

			#endregion Validation

			DBHelper.DeleteTimeEntry(timeEntryId);
		}

		/// <summary>
		/// Sets the approval state of a time entry.
		/// </summary>
		/// <param name="timeEntryId">Time entry Id.</param>
		/// <param name="approvalState">Approval state.</param>
		public void SetTimeEntryApprovalStateById(int timeEntryId, int approvalState)
		{
			#region Validation

			if (timeEntryId <= 0)
			{
				throw new ArgumentOutOfRangeException("timeEntryId", "Time entry id cannot be 0 or negative.");
			}

			if (approvalState < 0)
			{ // TODO: Figure out what values of approval state are actually allowed and constrain this further.
				throw new ArgumentOutOfRangeException("approvalState", "Approval state cannot be negative.");
			}

			#endregion Validation

			DBHelper.SetTimeEntryApprovalStateById(timeEntryId, approvalState);
		}

		/// <summary>
		/// Gets a list of <see cref="TimeEntryInfo"/>'s for a given organization and start/end times.
		/// </summary>
		/// <param name="start">Starting. <see cref="DateTime"/></param>
		/// <param name="end">Ending. <see cref="DateTime"/></param>
		/// <returns>A list of TimeEntryInfo's for a given organization and start/end times.</returns>
		public IEnumerable<TimeEntryInfo> GetTimeEntriesOverDateRange(DateTime start, DateTime end)
		{
			#region Validation

			if (start == null)
			{
				throw new ArgumentNullException("start", "Project must have a start time");
			}

			if (end == null)
			{
				throw new ArgumentNullException("end", "Project must have an end time");
			}

			if (DateTime.Compare(start, end) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			return DBHelper.GetTimeEntriesOverDateRange(UserContext.ChosenOrganizationId, start, end).Select(te => BusinessObjectsHelper.InitializeTimeEntryInfo(te));
		}

		/// <summary>
		/// Gets a list of <see cref="TimeEntryInfo"/>'s for a given set of users, organization, and start/end times.
		/// </summary>
		/// <param name="userIds">List of user Id's.</param>
		/// <param name="start">Starting. <see cref="DateTime"/></param>
		/// <param name="end">Ending. <see cref="DateTime"/></param>
		/// <returns><see cref="IEnumerable{TimeEntryInfo}"/></returns>
		public IEnumerable<TimeEntryInfo> GetTimeEntriesByUserOverDateRange(List<int> userIds, DateTime start, DateTime end)
		{
			#region Validation

			if (userIds == null || userIds.Count == 0)
			{
				throw new ArgumentNullException("userIds", "There must be at least one provided user id.");
			}

			if (userIds.Where(u => u <= 0).Count() > 0)
			{
				throw new ArgumentOutOfRangeException("userIds", "User ids cannot be 0 or negative.");
			}

			if (start == null)
			{
				throw new ArgumentNullException("start", "Project must have a start time");
			}

			if (end == null)
			{
				throw new ArgumentNullException("end", "Project must have an end time");
			}

			if (DateTime.Compare(start, end) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			return DBHelper.GetTimeEntriesByUserOverDateRange(userIds, UserContext.ChosenOrganizationId, start, end).Select(te => BusinessObjectsHelper.InitializeTimeEntryInfo(te));
		}

		/// <summary>
		/// Gets the lock date for an organization/user.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <returns>Lock date.</returns>
		public DateTime GetLockDate(int userId)
		{
			#region Validation

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			#endregion Validation

			return DBHelper.GetLockDate(UserContext.ChosenOrganizationId, userId);
		}

		/// <summary>
		/// Sets the lock date for an organization/user.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="date">Lock date.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool SetLockDate(int userId, DateTime date)
		{
			#region Validation

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			if (date == null)
			{
				throw new ArgumentNullException("date", "Lock date must not be null.");
			}

			#endregion Validation

			if (this.authorizationService.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				DBHelper.SetLockDate(UserContext.ChosenOrganizationId, userId, date);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates a holiday and related time entries for an organization.
		/// </summary>
		/// <param name="holiday">Holiday info.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool CreateHoliday(HolidayInfo holiday)
		{
			#region Validation

			if (holiday == null)
			{
				throw new ArgumentNullException("holiday", "Holiday must not be null.");
			}

			#endregion Validation

			if (this.authorizationService.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				DBHelper.CreateHoliday(BusinessObjectsHelper.GetDBEntityFromHolidayInfo(holiday));
				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets a list of <see cref="HolidayInfo"/>'s for the holidays in the current organization.
		/// </summary>
		/// <returns>A list of HolidayInfo's for the holidays in the organization.</returns>
		public IEnumerable<HolidayInfo> GetHolidays()
		{
			return DBHelper.GetHolidays(UserContext.ChosenOrganizationId).Select(hol => BusinessObjectsHelper.InitializeHolidayInfo(hol));
		}

		/// <summary>
		/// Deletes a holiday and related time entries for the current organization.
		/// </summary>
		/// <param name="holidayName">Name of holiday to delete.</param>
		/// <param name="date">Date of holiday.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool DeleteHoliday(string holidayName, DateTime date)
		{
			#region Validation

			if (string.IsNullOrEmpty(holidayName))
			{
				throw new ArgumentNullException("holidayName", "Holiday name must have a value.");
			}

			if (date == null)
			{
				throw new ArgumentNullException("date", "Date must not be null.");
			}

			#endregion Validation

			if (this.authorizationService.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				DBHelper.DeleteHoliday(holidayName, date, UserContext.ChosenOrganizationId);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates a new pay class for an organization.
		/// </summary>
		/// <param name="payClassName">Name of pay class.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool CreatePayClass(string payClassName)
		{
			#region Validation

			if (string.IsNullOrEmpty(payClassName))
			{
				throw new ArgumentNullException("payClassName", "Pay class name must have a value.");
			}

			#endregion Validation

			if (this.authorizationService.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				DBHelper.CreatePayClass(payClassName, UserContext.ChosenOrganizationId);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Deletes a pay class.
		/// </summary>
		/// <param name="payClassId">Pay class Id.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool DeletePayClass(int payClassId)
		{
			#region Validation

			if (payClassId <= 0)
			{
				throw new ArgumentOutOfRangeException("payClassId", "Pay class id cannot be 0 or negative.");
			}

			#endregion Validation

			if (this.authorizationService.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				DBHelper.DeletePayClass(payClassId);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets a list of <see cref="PayClassInfo"/>'s for an organization.
		/// </summary>
		/// <returns>List of PayClassInfo's.</returns>
		public IEnumerable<PayClassInfo> GetPayClasses()
		{
			return DBHelper.GetPayClasses(UserContext.ChosenOrganizationId).Select(pc => BusinessObjectsHelper.InitializePayClassInfo(pc));
		}

		/// <summary>
		/// Gets a <see cref="PayClassInfo"/>.
		/// </summary>
		/// <param name="name">Name of pay class.</param>
		/// <returns>A PayClassInfo instance.</returns>
		public PayClassInfo GetPayClassByName(string name)
		{
			#region Validation

			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "Name of pay class must have a value.");
			}

			#endregion Validation

			return BusinessObjectsHelper.InitializePayClassInfo(DBHelper.GetPayClassByNameAndOrg(name, UserContext.ChosenOrganizationId));
		}

		/// <summary>
		/// Gets the Start of Week for an organiztion.
		/// </summary>
		/// <returns>Start of Week.</returns>
		public StartOfWeekEnum GetStartOfWeek()
		{
			var queried = DBHelper.GetStartOfWeek(UserContext.ChosenOrganizationId);

			if (Enum.IsDefined(typeof(StartOfWeekEnum), queried))
			{
				return (StartOfWeekEnum)queried;
			}
			else
			{
				throw new InvalidCastException(string.Format("The value \"{0}\" retrieved from the database is invalid and cannot be converted to a day of the week.", queried));
			}
		}

		/// <summary>
		/// Updates the Start of Week for an organization.
		/// </summary>
		/// <param name="startOfWeek">Start of Week.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool UpdateStartOfWeek(int startOfWeek)
		{
			#region Validation

			if (!Enum.IsDefined(typeof(StartOfWeekEnum), startOfWeek))
			{
				throw new ArgumentException("Start of week must correspond to a value of StartOfWeekEnum.");
			}

			#endregion Validation

			if (this.authorizationService.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				DBHelper.UpdateTimeTrackerStartOfWeek(UserContext.ChosenOrganizationId, startOfWeek);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets settings for the chosen organization.
		/// </summary>
		/// <returns>Organization settings.</returns>
		public SettingsInfo GetSettings()
		{
			return BusinessObjectsHelper.InitializeSettingsInfo(DBHelper.GetSettings(UserContext.ChosenOrganizationId));
		}

		/// <summary>
		/// Updates settings for an organization.
		/// </summary>
		/// <param name="startOfWeek">Start of Week.</param>
		/// <param name="overtimeHours">Hours until overtime.</param>
		/// <param name="overtimePeriod">Time period for hours until overtime.</param>
		/// <param name="overtimeMultiplier">Overtime pay multiplier.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool UpdateSettings(StartOfWeekEnum startOfWeek, int overtimeHours, string overtimePeriod, float overtimeMultiplier)
		{
			#region Validation

			if (Enum.IsDefined(typeof(StartOfWeekEnum), startOfWeek))
			{
				throw new ArgumentException("Start of week must correspond to a value of StartOfWeekEnum.");
			}

			if (overtimeHours < -1)
			{
				throw new ArgumentOutOfRangeException("overtimeHours", "Overtime hours cannot be negative, unless it is -1 to indicate overtime unavailable.");
			}

			if (overtimePeriod == null)
			{ // TODO: Figure out what this even is and what the allowable values are.
				throw new ArgumentNullException("overtimePeriod", "Overtime period must not be null.");
			}

			if (overtimeMultiplier < 1.0)
			{
				throw new ArgumentOutOfRangeException("overtimeMultiplier", "Overtime rate cannot be less than regular rate (i.e. overtimeMultiplier less than one).");
			}

			#endregion Validation

			if (this.authorizationService.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				DBHelper.UpdateSettings(UserContext.ChosenOrganizationId, (int)startOfWeek, overtimeHours, overtimePeriod, overtimeMultiplier);

				return true;
			}

			return false;
		}
	}
}