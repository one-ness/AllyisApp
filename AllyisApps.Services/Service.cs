﻿//------------------------------------------------------------------------------
// <copyright file="Service.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.TimeTracker;
using AllyisApps.Services.TimeTracker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AllyisApps.Services
{
	public partial class AppService : BaseService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AppService"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public AppService(string connectionString) : base(connectionString) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="AppService"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="userContext">The user context.</param>
		public AppService(string connectionString, UserContext userContext) : base(connectionString, userContext) { }

		/// <summary>
		/// Converts an int representing days since the DateTime min value (Jan 1st, 0001) into a DateTime date.
		/// </summary>
		/// <param name="days">An int of the date as days since Jan 1st, 0001. Use -1 for null date.</param>
		/// <returns>The DateTime date.</returns>
		public static DateTime? GetDateFromDays(int days)
		{
			if (days <= -1)
			{
				return null;
			}

			return DateTime.MinValue.AddDays(days);
		}

		/// <summary>
		/// Converts a DateTime? date into an int representing days since the DateTime min value (Jan 1st, 0001).
		/// </summary>
		/// <param name="date">The DateTime? date.</param>
		/// <returns>An int of the date as days since Jan 1st, 0001. Returns -1 for null.</returns>
		public int GetDayFromDateTime(DateTime? date)
		{
			if (!date.HasValue)
			{
				return -1;
			}

			return (int)date.Value.Subtract(DateTime.MinValue).TotalDays;
		}

        /// <summary>
        /// Converts an int representing days since the DateTime min value (Jan 1st, 0001) into a DateTime date.
        /// </summary>
        /// <param name="days">An int of the date as days since Jan 1st, 0001. Use -1 for null dates.</param>
        /// <returns>The DateTime date.</returns>
        public DateTime? GetDateTimeFromDays(int days)
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

			return InitializeTimeEntryInfo(DBHelper.GetTimeEntryById(timeEntryId));
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

			return DBHelper.CreateTimeEntry(GetDBEntityFromTimeEntryInfo(entry));
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

			DBHelper.UpdateTimeEntry(GetDBEntityFromTimeEntryInfo(entry));
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

		///// <summary>
		///// Sets the approval state of a time entry.
		///// </summary>
		///// <param name="timeEntryId">Time entry Id.</param>
		///// <param name="approvalState">Approval state.</param>
		//public void SetTimeEntryApprovalStateById(int timeEntryId, int approvalState)
		//{
		//	#region Validation

		//	if (timeEntryId <= 0)
		//	{
		//		throw new ArgumentOutOfRangeException("timeEntryId", "Time entry id cannot be 0 or negative.");
		//	}
		//	else if (approvalState < 0)
		//	{ // TODO: Figure out what values of approval state are actually allowed and constrain this further.
		//		throw new ArgumentOutOfRangeException("approvalState", "Approval state cannot be negative.");
		//	}

		//	#endregion Validation

		//	DBHelper.SetTimeEntryApprovalStateById(timeEntryId, approvalState);
		//}

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
			else if (end == null)
			{
				throw new ArgumentNullException("end", "Project must have an end time");
			}
			else if (DateTime.Compare(start, end) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			return DBHelper.GetTimeEntriesOverDateRange(UserContext.ChosenOrganizationId, start, end).Select(te => InitializeTimeEntryInfo(te));
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
			else if (userIds.Where(u => u <= 0).Count() > 0)
			{
				throw new ArgumentOutOfRangeException("userIds", "User ids cannot be 0 or negative.");
			}
			else if (start == null)
			{
				throw new ArgumentNullException("start", "Date range must have a start date.");
			}
			else if (end == null)
			{
				throw new ArgumentNullException("end", "Date range must have an end date.");
			}
			else if (DateTime.Compare(start, end) > 0)
			{
				throw new ArgumentException("Date range cannot end before it starts.");
			}

			#endregion Validation

			return DBHelper.GetTimeEntriesByUserOverDateRange(userIds, UserContext.ChosenOrganizationId, start, end).Select(te => InitializeTimeEntryInfo(te));
		}

		/// <summary>
		/// Gets a list of Customers for all customers in the organization, a list of CompleteProjectInfos for all
		/// projects in the organization, and a list of SubscriptionUserInfos for all users in the current subscription.
		/// </summary>
		/// <param name="orgId">Organization Id. If null, current organization will be used.</param>
		/// <returns></returns>
		public Tuple<List<Customer>, List<CompleteProjectInfo>, List<SubscriptionUserInfo>> GetReportInfo(int? orgId = null)
		{
			var spResults = DBHelper.GetReportInfo(orgId == null ? UserContext.ChosenOrganizationId : orgId.Value, UserContext.ChosenSubscriptionId);
			return Tuple.Create(
				spResults.Item1.Select(cdb => InitializeCustomer(cdb)).ToList(),
				spResults.Item2.Select(cpdb => InitializeCompleteProjectInfo(cpdb)).ToList(),
				spResults.Item3.Select(sudb => InitializeSubscriptionUserInfo(sudb)).ToList());
		}

		/// <summary>
		/// Gets the lock date for the current organization.
		/// </summary>
		/// <returns>Lock date.</returns>
		public DateTime? GetLockDate()
		{
			LockDateDBEntity lockDate = DBHelper.GetLockDate(UserContext.ChosenOrganizationId);
			return GetLockDateFromParameters(lockDate.LockDateUsed, lockDate.LockDatePeriod, lockDate.LockDateQuantity);
		}

		/// <summary>
		/// Gets a nullable DateTime for the lock date, based on supplied lock date settings.
		/// </summary>
		/// <param name="isLockDateUsed">A value indicating whether the lock date is used.</param>
		/// <param name="lockDatePeriod">The lock date period ("Monthd", "Weeks", or "Days").</param>
		/// <param name="lockDateQuantity">The quantity of the time unit defined by the period.</param>
		/// <returns>A nullable DateTime expressing the date on or before which time entries are locked.</returns>
		public DateTime? GetLockDateFromParameters(bool isLockDateUsed, string lockDatePeriod, int lockDateQuantity)
		{
			if (!isLockDateUsed)
			{
				return null;
			}

			DateTime date = lockDatePeriod.Equals("Months") ? DateTime.Now.AddMonths(-1 * lockDateQuantity) :
				DateTime.Now.AddDays(-1 * lockDateQuantity * (lockDatePeriod.Equals("Weeks") ? 7 : 1));
			return date;
		}

		/// <summary>
		/// Creates a holiday and related time entries for an organization.
		/// </summary>
		/// <param name="holiday">Holiday.</param>
        /// <param name="subscriptionId">Subscription ID</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool CreateHoliday(Holiday holiday, int subscriptionId)
		{
			#region Validation

			if (holiday == null)
			{
				throw new ArgumentNullException("holiday", "Holiday must not be null.");
			}

			#endregion Validation

			if (this.Can(Actions.CoreAction.TimeTrackerEditOthers, false, holiday.OrganizationId, subscriptionId))
			{
				DBHelper.CreateHoliday(GetDBEntityFromHoliday(holiday));
				return true;
			}

			return false;
		}

		/// <summary>
		/// Deletes a holiday and related time entries for the current organization.
		/// </summary>
		/// <param name="holidayId">Id of holiday to delete.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool DeleteHoliday(int holidayId)
		{
			#region Validation

			if (holidayId <= 0)
			{
				throw new ArgumentNullException("holidayId", "Holiday Id cannot be negative or 0.");
			}

			#endregion Validation

			if (this.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				HolidayDBEntity deletedHoliday = DBHelper.GetHolidays(UserContext.ChosenOrganizationId).Where(h => h.HolidayId == holidayId).SingleOrDefault();
				if (deletedHoliday != null)
				{
					DBHelper.DeleteHoliday(deletedHoliday.HolidayName, deletedHoliday.Date, UserContext.ChosenOrganizationId);
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates a new pay class for an organization.
		/// </summary>
		/// <param name="payClassName">Name of pay class.</param>
        /// <param name="orgId">Organization ID</param>
        /// <param name="subscriptionId">Subscription ID</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool CreatePayClass(string payClassName, int orgId, int subscriptionId)
		{
			#region Validation

			if (string.IsNullOrEmpty(payClassName))
			{
				throw new ArgumentNullException("payClassName", "Pay class name must have a value.");
			}

			if (this.GetPayClasses().Where(p => p.Name.Equals(payClassName)).Any())
			{
				throw new ArgumentException("payClassName", "Pay class name must be unique for the organization.");
			}

			#endregion Validation

			if (this.Can(Actions.CoreAction.TimeTrackerEditOthers, false, orgId, subscriptionId))
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

			if (this.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				DBHelper.DeletePayClass(payClassId);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Check whether a payclass is used by a time entry before.
		/// </summary>
		/// <param name="payClassId">Pay class Id.</param>
		/// <returns>Returns a list of timeEntryIds that used that payclass.</returns>
		public IEnumerable<TimeEntryDBEntity> GetTimeEntriesThatUseAPayClass(int payClassId)
		{
			#region Validation

			if (payClassId <= 0)
			{
				throw new ArgumentOutOfRangeException("payClassId", "Pay class id cannot be 0 or negative.");
			}

			#endregion Validation

			return DBHelper.GetTimeEntriesThatUseAPayClass(payClassId);
		}

		/// <summary>
		/// Gets a list of <see cref="PayClass"/>'s for an organization.
		/// </summary>
		/// <returns>List of PayClassInfo's.</returns>
		public IEnumerable<PayClass> GetPayClasses()
		{
			return DBHelper.GetPayClasses(UserContext.ChosenOrganizationId).Select(pc => InitializePayClassInfo(pc));
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

			if (this.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				DBHelper.UpdateTimeTrackerStartOfWeek(UserContext.ChosenOrganizationId, startOfWeek);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Updates overtime settings for an organization.
		/// </summary>
		/// <param name="overtimeHours">Hours until overtime.</param>
		/// <param name="overtimePeriod">Time period for hours until overtime.</param>
		/// <param name="overtimeMultiplier">Overtime pay multiplier.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool UpdateOvertime(int overtimeHours, string overtimePeriod, float overtimeMultiplier)
		{
			#region Validation

			if (overtimeHours < -1)
			{
				throw new ArgumentOutOfRangeException("overtimeHours", "Overtime hours cannot be negative, unless it is -1 to indicate overtime unavailable.");
			}

			if (!new string[] { "Day", "Week", "Month" }.Contains(overtimePeriod))
			{
				throw new ArgumentException(string.Format("{0} is not a valid value for lock date period.", overtimePeriod));
			}

			if (overtimeMultiplier < 1.0)
			{
				throw new ArgumentOutOfRangeException("overtimeMultiplier", "Overtime rate cannot be less than regular rate (i.e. overtimeMultiplier less than one).");
			}

			#endregion Validation

			if (this.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				DBHelper.UpdateOvertime(UserContext.ChosenOrganizationId, overtimeHours, overtimePeriod, overtimeMultiplier);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Prepares the Excel file for output of time entry information.
		/// </summary>
		/// <param name="userIds">List of user ids to filter by.</param>
		/// <param name="startingDate">Start of date range.</param>
		/// <param name="endingDate">End of date range.</param>
		/// <param name="projectId">Project id to filter by.</param>
		/// <param name="customerId">Customer id to filter by.</param>
		/// <returns>The stream writer.</returns>
		public StreamWriter PrepareCSVExport(List<int> userIds = null, DateTime? startingDate = null, DateTime? endingDate = null, int projectId = 0, int customerId = 0)
		{
			//Preparing data
			IEnumerable<TimeEntryInfo> data = new List<TimeEntryInfo>();
			IEnumerable<CompleteProjectInfo> projects = new List<CompleteProjectInfo>();

			if (userIds == null || userIds.Count == 0 || userIds[0] == -1)
			{
				data = this.GetTimeEntriesOverDateRange(startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddYears(-1));
			}
			else
			{
				data = this.GetTimeEntriesByUserOverDateRange(userIds, startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddYears(-1));
			}

			if (projectId > 0)
			{
				data = data.Where(t => t.ProjectId == projectId);
			}
			else
			{
				if (customerId > 0)
				{
					IEnumerable<int> customerProjects = GetProjectsByCustomer(customerId).Select(p => p.CustomerId);
					data = data.Where(t => customerProjects.Contains(t.ProjectId));
				}
			}

			if (userIds != null && userIds.Count == 1 && userIds[0] > 0)
			{
				projects = GetProjectsByUserAndOrganization(userIds[0], false);
			}
			else
			{
				projects = GetProjectsByOrganization(UserContext.ChosenOrganizationId, false);
			}

			// Add default project in case there are holiday entries
			List<CompleteProjectInfo> defaultProject = new List<CompleteProjectInfo>();
			defaultProject.Add(GetProject(0));
			projects = projects.Concat(defaultProject);

			StreamWriter output = new StreamWriter(new MemoryStream());
			output.WriteLine(
				string.Format(
					"\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\"",
					ColumnHeaders.UserLastName,
					ColumnHeaders.UserFirstName,
					ColumnHeaders.EmployeeId,
					ColumnHeaders.UserEmail,
					ColumnHeaders.Date,
					ColumnHeaders.Duration,
					ColumnHeaders.PayClass,
					ColumnHeaders.ProjectName,
					ColumnHeaders.ProjectId,
					ColumnHeaders.CustomerName,
					ColumnHeaders.CustomerId,
					ColumnHeaders.Description
				));

			foreach (TimeEntryInfo entry in data)
			{
				try
				{
					var project = projects.Where(x => x.ProjectId == entry.ProjectId).FirstOrDefault();
					if (project.ProjectId == 0) project = null;
					output.WriteLine(
						string.Format(
							"\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\"",
							entry.LastName,
							entry.FirstName,
							entry.EmployeeId,
							entry.Email,
							entry.Date.ToShortDateString(),
							entry.Duration,
							entry.PayClassName,
							project != null ? (project.ProjectName ?? string.Empty) : string.Empty,
							project != null ? (project.ProjectOrgId ?? string.Empty) : string.Empty,
							project != null ? (project.CustomerName ?? string.Empty) : string.Empty,
							project != null ? (project.CustomerOrgId ?? string.Empty) : string.Empty,
							entry.Description));
				}
				catch (Exception ex)
				{
					string blah = ex.Message;
				}
			}

			output.Flush();
			output.BaseStream.Seek(0, SeekOrigin.Begin);

			return output;
		}

		/// <summary>
		/// Updates the lock date setttings.
		/// </summary>
		/// <param name="lockDateUsed">Whether or not to use a lock date.</param>
		/// <param name="lockDatePeriod">The lock date period (days/weeks/months).</param>
		/// <param name="lockDateQuantity">The quantity of the selected period.</param>
		/// <returns></returns>
		public bool UpdateLockDate(bool lockDateUsed, string lockDatePeriod, int lockDateQuantity)
		{
			if (!new string[] { "Days", "Weeks", "Months" }.Contains(lockDatePeriod))
			{
				throw new ArgumentException(string.Format("{0} is not a valid value for lock date period.", lockDatePeriod));
			}

			if (lockDateQuantity < 0)
			{
				throw new ArgumentException("Lock date quantity cannot be less than zero.");
			}

			return DBHelper.UpdateLockDate(this.UserContext.ChosenOrganizationId, lockDateUsed, lockDatePeriod, lockDateQuantity);
		}

        /// <summary>
        /// Returns a SettingsInfo with start of week, overtime, and lock date settings, a list of PayClassInfos,
        /// and a list of Holidays for the current organization.
        /// </summary>
        /// <param name="orgId">Organization Id</param>
        /// <returns></returns>
        public Tuple<Setting, List<PayClass>, List<Holiday>> GetAllSettings(int orgId)
		{
			var spResults = DBHelper.GetAllSettings(orgId);
			return Tuple.Create(
				InitializeSettingsInfo(spResults.Item1),
				spResults.Item2.Select(pcdb => InitializePayClassInfo(pcdb)).ToList(),
				spResults.Item3.Select(hdb => InitializeHoliday(hdb)).ToList());
		}

		/// <summary>
		/// Returns a SettingsInfo for the current organization's TimeTracker settings (with only start of week and
		/// lock date fields populated), a list of PayClassInfos for all the organization's pay classes, a list of
		/// Holidays for all the organization's holidays, a list of CompleteProjectInfos for all projects
		/// in the current org that the given user is or has been assigned to (active or not), a list of Users
		/// for all the users in the org who are users of the time tracker subscription, and a list of TimeEntryInfos
		/// for all time entries for the given user in the given time range.
		/// </summary>
		/// <param name="userId">User Id.</param>
        /// <param name="orgId">Organization Id</param>
		/// <param name="startingDate">Start of date range.</param>
		/// <param name="endingDate">End of date range.</param>
		/// <returns></returns>
		public Tuple<Setting, List<PayClass>, List<Holiday>, List<CompleteProjectInfo>, List<User>, List<TimeEntryInfo>>
			GetTimeEntryIndexInfo(int orgId, DateTime? startingDate, DateTime? endingDate, int? userId = null)
		{
			#region Validation

			if (userId == null)
			{
				userId = UserContext.UserId;
			}
            if (userId <= 0)
			{
				throw new ArgumentException("User Id cannot be zero or negative.");
			}
            if (orgId <= 0)
            {
                throw new ArgumentException("Organization Id cannot be zero or negative.");
            }
            if (startingDate.HasValue && endingDate.HasValue && DateTime.Compare(startingDate.Value, endingDate.Value) > 0)
			{
				throw new ArgumentException("Date range cannot end before it starts.");
			}

			#endregion Validation

			var spResults = DBHelper.GetTimeEntryIndexPageInfo(orgId, (int)ProductIdEnum.TimeTracker, userId.Value, startingDate, endingDate);

			return Tuple.Create(InitializeSettingsInfo(spResults.Item1),
				spResults.Item2.Select(pcdb => InitializePayClassInfo(pcdb)).ToList(),
				spResults.Item3.Select(hdb => InitializeHoliday(hdb)).ToList(),
				spResults.Item4.Select(cpdb => InitializeCompleteProjectInfo(cpdb)).ToList(),
				spResults.Item5.Select(udb => InitializeUser(udb)).ToList(),
				spResults.Item6.Select(tedb => InitializeTimeEntryInfo(tedb)).ToList());
		}

		#region Info-DBEntity Conversions

		/// <summary>
		/// Initializes a PayClassInfo from a PayClassDBEntity.
		/// </summary>
		/// <param name="pc">PayClassDBEntity.</param>
		/// <returns>PayClassInfo.</returns>
		public static PayClass InitializePayClassInfo(PayClassDBEntity pc)
		{
			return new PayClass
			{
				Name = pc.Name,
				OrganizationId = pc.OrganizationId,
				PayClassID = pc.PayClassID,
				CreatedUTC = pc.CreatedUTC,
				ModifiedUTC = pc.ModifiedUTC
			};
		}

		/// <summary>
		/// Initialized holiday with a given HolidayDBEntity.
		/// </summary>
		/// <param name="hol">The HolidayDBEntity to use.</param>
		/// <returns>A holiday object.</returns>
		public static Holiday InitializeHoliday(HolidayDBEntity hol)
		{
			return new Holiday
			{
				CreatedUTC = hol.CreatedUTC,
				ModifiedUTC = hol.ModifiedUTC,
				Date = hol.Date,
				HolidayId = hol.HolidayId,
				HolidayName = hol.HolidayName,
				OrganizationId = hol.OrganizationId
			};
		}

		/// <summary>
		/// Initialized a SettingsInfo object based on a given SettingDBEntity.
		/// </summary>
		/// <param name="settings">The SettingsDBEntity to use.</param>
		/// <returns>The initialized SettingsInfo object.</returns>
		public static Setting InitializeSettingsInfo(SettingDBEntity settings)
		{
			if (settings == null)
			{
				return null;
			}

			return new Setting
			{
				OrganizationId = settings.OrganizationId,
				OvertimeHours = settings.OvertimeHours,
				OvertimeMultiplier = settings.OvertimeMultiplier,
				OvertimePeriod = settings.OvertimePeriod,
				StartOfWeek = settings.StartOfWeek,
				LockDatePeriod = settings.LockDatePeriod,
				LockDateQuantity = settings.LockDateQuantity,
				LockDateUsed = settings.LockDateUsed
			};
		}

		/// <summary>
		/// Initializes a TimeEntryInfo object based on a given TimeEntryDBEntity.
		/// </summary>
		/// <param name="entity">The TimeEntryDBEntity to use.</param>
		/// <returns>The initialized TimeEntryInfo object.</returns>
		public static TimeEntryInfo InitializeTimeEntryInfo(TimeEntryDBEntity entity)
		{
			return new TimeEntryInfo
			{
				ApprovalState = entity.ApprovalState,
				Date = entity.Date,
				Description = entity.Description,
				Duration = entity.Duration,
				FirstName = entity.FirstName,
				LastName = entity.LastName,
				LockSaved = entity.LockSaved,
				ModSinceApproval = entity.ModSinceApproval,
				PayClassId = entity.PayClassId,
				PayClassName = entity.PayClassName,
				ProjectId = entity.ProjectId,
				TimeEntryId = entity.TimeEntryId,
				UserId = entity.UserId,
				EmployeeId = entity.EmployeeId,
				Email = entity.Email
			};
		}

		/// <summary>
		/// Builds a TimeEntryDBEntity based on a given TimeEntryInfo object.
		/// </summary>
		/// <param name="info">The TimeEntryInfo to use.</param>
		/// <returns>The built TimeEntryDBEntity based on the TimeEntryInfo object.</returns>
		public static TimeEntryDBEntity GetDBEntityFromTimeEntryInfo(TimeEntryInfo info)
		{
			return new TimeEntryDBEntity
			{
				ApprovalState = info.ApprovalState,
				Date = info.Date,
				Description = info.Description,
				Duration = info.Duration,
				FirstName = info.FirstName,
				LastName = info.LastName,
				LockSaved = info.LockSaved,
				ModSinceApproval = info.ModSinceApproval,
				PayClassId = info.PayClassId,
				ProjectId = info.ProjectId,
				TimeEntryId = info.TimeEntryId,
				UserId = info.UserId
			};
		}

		/// <summary>
		/// Creates a HolidayDBEntity based on a Holiday object.
		/// </summary>
		/// <param name="holiday">The Holiday to use to creat the DB entity.</param>
		/// <returns>The created HolidayDBEntity object.</returns>
		public static HolidayDBEntity GetDBEntityFromHoliday(Holiday holiday)
		{
			return new HolidayDBEntity()
			{
				CreatedUTC = holiday.CreatedUTC,
				Date = holiday.Date,
				HolidayId = holiday.HolidayId,
				HolidayName = holiday.HolidayName,
				ModifiedUTC = holiday.ModifiedUTC,
				OrganizationId = holiday.OrganizationId,
			};
		}

		#endregion Info-DBEntity Conversions
	}
}
