//------------------------------------------------------------------------------
// <copyright file="Service.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.DBModel.Crm;
using AllyisApps.DBModel.Hrm;
using AllyisApps.DBModel.StaffingManager;
using AllyisApps.DBModel.TimeTracker;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Expense;
using AllyisApps.Services.Lookup;
using AllyisApps.Services.StaffingManager;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Services
{
	public partial class AppService : BaseService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AppService"/> class.
		/// </summary>
		public AppService(ServiceSettings settings) : base(settings) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="AppService"/> class.
		/// </summary>
		public AppService(ServiceSettings settings, UserContext userContext) : base(settings, userContext) { }

		/// <summary>
		/// Gets a <see cref="TimeEntry"/>.
		/// </summary>
		/// <param name="timeEntryId">Time entry Id.</param>
		/// <returns>A TimeEntryDBEntity.</returns>
		public async Task<TimeEntry> GetTimeEntry(int timeEntryId)
		{
			#region Validation

			if (timeEntryId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(timeEntryId), "Time entry id cannot be 0 or negative.");
			}

			#endregion Validation

			return InitializeTimeEntryInfo(await DBHelper.GetTimeEntryById(timeEntryId));
		}

		/// <summary>
		/// Creates a time entry.
		/// </summary>
		/// <param name="entry">Time entry info.</param>
		/// <returns>Time entry id.</returns>
		public async Task<int> CreateTimeEntry(TimeEntry entry)
		{
			#region Validation

			if (entry == null)
			{
				throw new ArgumentNullException(nameof(entry), "Time entry must not be null.");
			}

			#endregion Validation

			return await DBHelper.CreateTimeEntry(GetDBEntityFromTimeEntryInfo(entry));
		}

		/// <summary>
		/// Updates a time entry.
		/// </summary>
		/// <param name="entry">Updated info.</param>
		public void UpdateTimeEntry(TimeEntry entry)
		{
			#region Validation

			if (entry == null)
			{
				throw new ArgumentNullException(nameof(entry), "Time entry must not be null.");
			}

			#endregion Validation

			DBHelper.UpdateTimeEntry(GetDBEntityFromTimeEntryInfo(entry));
		}

		/// <summary>
		/// Sets the approval state of a time entry in the database.
		/// </summary>
		/// <param name="timeEntryId">The Id of the time entry to be updated.</param>
		/// <param name="timeEntryStatusId">The new status.</param>
		public async Task<int> UpdateTimeEntryStatusById(int timeEntryId, int timeEntryStatusId)
		{
			if (timeEntryId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(timeEntryId), $"{nameof(timeEntryId)} must be greater than 0.");
			}

			if (timeEntryStatusId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(timeEntryStatusId), $"{nameof(timeEntryStatusId)} must not be negative.");
			}

			return await DBHelper.UpdateTimeEntryStatusById(timeEntryId, timeEntryStatusId);
		}

		/// <summary>
		/// Deletes a time entry.
		/// </summary>
		/// <param name="timeEntryId">Time entry Id.</param>
		public async void DeleteTimeEntry(int timeEntryId)
		{
			#region Validation

			if (timeEntryId <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(timeEntryId), "Time entry id cannot be 0 or negative.");
			}

			#endregion Validation

			DBHelper.DeleteTimeEntry(timeEntryId);
			await Task.Yield();
		}

		/// <summary>
		/// Gets a list of <see cref="TimeEntry"/>'s for a given organization and start/end times.
		/// </summary>
		/// <param name="orgId">.</param>
		/// <param name="start">Starting. <see cref="DateTime"/>.</param>
		/// <param name="end">Ending. <see cref="DateTime"/>.</param>
		/// <returns>A list of TimeEntry's for a given organization and start/end times.</returns>
		public async Task<IEnumerable<TimeEntry>> GetTimeEntriesOverDateRange(int orgId, DateTime start, DateTime end)
		{
			#region Validation

			if (start == null)
			{
				throw new ArgumentNullException(nameof(start), "Project must have a start time");
			}
			if (end == null)
			{
				throw new ArgumentNullException(nameof(end), "Project must have an end time");
			}
			if (DateTime.Compare(start, end) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			return (await DBHelper.GetTimeEntriesOverDateRange(orgId, start, end)).Select(InitializeTimeEntryInfo);
		}

		/// <summary>
		/// Gets a list of <see cref="TimeEntry"/>'s for a given set of users, organization, and start/end times.
		/// </summary>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="userIds">List of user Id's.</param>
		/// <param name="start">Starting. <see cref="DateTime"/>.</param>
		/// <param name="end">Ending. <see cref="DateTime"/>.</param>
		/// <returns><see cref="IEnumerable{TimeEntryInfo}"/>.</returns>
		public async Task<IEnumerable<TimeEntry>> GetTimeEntriesByUserOverDateRange(List<int> userIds, DateTime? start, DateTime? end, int organizationId = -1)
		{
			#region Validation

			if (userIds == null || userIds.Count == 0)
			{
				throw new ArgumentNullException(nameof(userIds), "There must be at least one provided user id.");
			}
			if (userIds.Any(u => u <= 0))
			{
				throw new ArgumentOutOfRangeException(nameof(userIds), "User ids cannot be 0 or negative.");
			}
			if (start == null)
			{
				throw new ArgumentNullException(nameof(start), "Date range must have a start date.");
			}
			if (end == null)
			{
				throw new ArgumentNullException(nameof(end), "Date range must have an end date.");
			}
			if (DateTime.Compare(start.Value, end.Value) > 0)
			{
				throw new ArgumentException("Date range cannot end before it starts.");
			}

			#endregion Validation

			return (await DBHelper.GetTimeEntriesByUserOverDateRange(userIds, organizationId, start.Value, end.Value)).Select(InitializeTimeEntryInfo);
		}

		/// <summary>
		/// Gets a list of Customers for all customers in the organization, a list of CompleteProjectInfos for all
		/// projects in the organization, and a list of SubscriptionUserInfos for all users in the current subscription.
		/// </summary>
		public async Task<ReportInfo> GetReportInfo(int subscriptionId)
		{
			UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out UserContext.SubscriptionAndRole subInfo);
			var spResults = await DBHelper.GetReportInfo(subInfo.OrganizationId, subscriptionId);
			return new ReportInfo(
				spResults.Item1.Select(cdb => (Customer)InitializeCustomer(cdb)).ToList(),
				spResults.Item2.Select(InitializeCompleteProjectInfo).ToList(),
				spResults.Item3.Select(InitializeSubscriptionUser).ToList());
		}

		/// <summary>
		/// Creates a holiday and related time entries for an organization.
		/// </summary>
		/// <param name="holiday">Holiday.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public async Task<bool> CreateHoliday(Holiday holiday, int subscriptionId)
		{
			if (holiday == null) throw new ArgumentException("holiday");
			CheckTimeTrackerAction(TimeTrackerAction.EditOthers, subscriptionId);
			DBHelper.CreateHoliday(GetDBEntityFromHoliday(holiday));
			await Task.Yield();
			return true;
		}

		/// <summary>
		/// Deletes a holiday and related time entries for the current organization.
		/// </summary>
		/// <param name="holidayId">Id of holiday to delete.</param>
		/// <param name="orgId">The organization's Id.</param>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public async Task<bool> DeleteHoliday(int holidayId, int orgId, int subscriptionId)
		{
			if (holidayId <= 0) throw new ArgumentException("holidayId");
			if (orgId <= 0) throw new ArgumentException("orgId");
			if (subscriptionId <= 0) throw new ArgumentException("subscriptionId");
			CheckTimeTrackerAction(TimeTrackerAction.EditOthers, subscriptionId);

			HolidayDBEntity deletedHoliday = DBHelper.GetHolidays(orgId).Where(h => h.HolidayId == holidayId).SingleOrDefault();
			if (deletedHoliday != null)
			{
				DBHelper.DeleteHoliday(deletedHoliday.HolidayName, deletedHoliday.Date, orgId);
				await Task.Yield();
			}

			return true;
		}

		/// <summary>
		/// Creates a new pay class for an organization.
		/// </summary>
		/// <param name="payClassName">Name of pay class.</param>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public async Task<bool> CreatePayClass(string payClassName, int orgId, int subscriptionId)
		{
			CheckTimeTrackerAction(TimeTrackerAction.EditOthers, subscriptionId);
			DBHelper.CreatePayClass(payClassName, orgId);
			await Task.Yield();
			return true;
		}

		/// <summary>
		/// Deletes a pay class.
		/// </summary>
		/// <param name="payClassId">Pay class Id.</param>
		/// <param name="orgId">The organization's Id.</param>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="destPayClass">The id of the destination payclass.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public async Task<bool> DeletePayClass(int payClassId, int orgId, int subscriptionId, int? destPayClass)
		{
			CheckTimeTrackerAction(TimeTrackerAction.EditOthers, subscriptionId);
			DBHelper.DeletePayClass(payClassId, destPayClass);
			await Task.Yield();
			return true;
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
				throw new ArgumentOutOfRangeException(nameof(payClassId), "Pay class id cannot be 0 or negative.");
			}

			#endregion Validation

			return DBHelper.GetTimeEntriesThatUseAPayClass(payClassId);
		}

		/// <summary>
		/// Gets a list of <see cref="PayClass"/>'s for an organization.
		/// </summary>
		public async Task<IEnumerable<PayClass>> GetPayClassesBySubscriptionId(int subscriptionId)
		{
			var getClass = await DBHelper.GetPayClasses(UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId);
			return getClass.Select(pc => InitializePayClassInfo(pc));
		}

		/// <summary>
		/// Gets a list of <see cref="PayClass"/>'s for an organization.
		/// </summary>
		public async Task<IEnumerable<PayClass>> GetPayClassesByOrganizationId(int organizationId)
		{
			var getClass = await DBHelper.GetPayClasses(organizationId);
			return getClass.Select(pc => InitializePayClassInfo(pc));
		}

		/// <summary>
		/// Updates the Start of Week for an organization.
		/// </summary>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="startOfWeek">Start of Week.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public async Task<bool> UpdateStartOfWeek(int organizationId, int subscriptionId, int startOfWeek)
		{
			#region Validation

			if (!Enum.IsDefined(typeof(StartOfWeekEnum), startOfWeek))
			{
				throw new ArgumentException("Start of week must correspond to a value of StartOfWeekEnum.");
			}

			#endregion Validation

			CheckTimeTrackerAction(TimeTrackerAction.EditOthers, subscriptionId);
			DBHelper.UpdateTimeTrackerStartOfWeek(organizationId, startOfWeek);
			await Task.Yield();
			return true;
		}

		/// <summary>
		/// Updates overtime settings for an organization.
		/// </summary>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="overtimeHours">Hours until overtime.</param>
		/// <param name="overtimePeriod">Time period for hours until overtime.</param>
		/// <param name="overtimeMultiplier">Overtime pay multiplier.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public async Task<bool> UpdateOvertime(int subscriptionId, int organizationId, int overtimeHours, string overtimePeriod, float overtimeMultiplier)
		{
			#region Validation

			if (overtimeHours < -1)
			{
				throw new ArgumentOutOfRangeException(nameof(overtimeHours), "Overtime hours cannot be negative, unless it is -1 to indicate overtime unavailable.");
			}

			if (!new[] { "Day", "Week", "Month" }.Contains(overtimePeriod))
			{
				throw new ArgumentException(string.Format("{0} is not a valid value for lock date period.", overtimePeriod));
			}

			if (overtimeMultiplier < 1.0)
			{
				throw new ArgumentOutOfRangeException(nameof(overtimeMultiplier), "Overtime rate cannot be less than regular rate (i.e. overtimeMultiplier less than one).");
			}

			#endregion Validation

			CheckTimeTrackerAction(TimeTrackerAction.EditOthers, subscriptionId);
			DBHelper.UpdateOvertime(organizationId, overtimeHours, overtimePeriod, overtimeMultiplier);
			await Task.Yield();
			return true;
		}

		/// <summary>
		/// Prepares the Excel file for output of time entry information.
		/// </summary>
		/// <param name="orgId">The current Organization Id.</param>
		/// <param name="userIds">List of user ids to filter by.</param>
		/// <param name="startingDate">Start of date range.</param>
		/// <param name="endingDate">End of date range.</param>
		/// <param name="projectId">Project id to filter by.</param>
		/// <param name="customerId">Customer id to filter by.</param>
		/// <returns>The stream writer.</returns>
		public async Task<StreamWriter> PrepareCSVExport(int orgId, List<int> userIds = null, DateTime? startingDate = null,
			DateTime? endingDate = null, int projectId = 0, int customerId = 0)
		{
			// Preparing data
			IEnumerable<TimeEntry> data = new List<TimeEntry>();
			List<CompleteProject> projects = new List<CompleteProject>();

			if (userIds == null || userIds.Count == 0 || userIds[0] == -1)
			{
				data = await GetTimeEntriesOverDateRange(orgId, startingDate ?? DateTime.MinValue.AddYears(1754),
					endingDate ?? DateTime.MaxValue.AddYears(-1));
			}
			else
			{
				data = await GetTimeEntriesByUserOverDateRange(userIds, startingDate ?? DateTime.MinValue.AddYears(1754),
					endingDate ?? DateTime.MaxValue.AddYears(-1), orgId);
			}

			if (projectId > 0)
			{
				data = data.Where(t => t.ProjectId == projectId);
			}
			else
			{
				if (customerId > 0)
				{
					var proj = await GetProjectsByCustomer(customerId);
					data = data.Where(t => proj.Select(p => p.ProjectId).Contains(t.ProjectId));
				}
			}

			if (userIds != null && userIds.Count == 1 && userIds[0] > 0)
			{
				projects = (await GetProjectsByUserAndOrganization(userIds[0], orgId, false)).ToList();
			}
			else
			{
				projects = GetProjectsByOrganization(orgId, false).ToList();
			}

			// Add default project in case there are holiday entries
			projects.Add(GetProject(0));

			var output = new StreamWriter(new MemoryStream());
			var columns = new[] {
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
				ColumnHeaders.Description,
				ColumnHeaders.Status
			};
			output.WriteLine("\"{0}\"", string.Join("\",\"", columns));

			foreach (TimeEntry entry in data)
			{
				try
				{
					var project = projects.FirstOrDefault(x => x.ProjectId == entry.ProjectId);
					if (project.ProjectId == 0) project = null;
					var rowData = new[]
					{
						entry.LastName,
						entry.FirstName,
						entry.EmployeeId,
						entry.Email,
						entry.Date.ToShortDateString(),
						entry.Duration.ToString(CultureInfo.CurrentCulture),
						entry.PayClassName,
						project?.ProjectName ?? string.Empty,
						project?.ProjectOrgId ?? string.Empty,
						project?.owningCustomer?.CustomerName ?? string.Empty,
						project?.owningCustomer?.CustomerOrgId ?? string.Empty,
						entry.Description,
						((TimeEntryStatus)entry.TimeEntryStatusId).ToString()
					};
					output.WriteLine("\"{0}\"", string.Join("\",\"", rowData));
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

		public StreamWriter PrepareExpenseCSVExport(int orgId, IEnumerable<ExpenseReport> reports, DateTime startDate, DateTime endDate)
		{
			StreamWriter output = new StreamWriter(new MemoryStream());
			var columns = new[]
			{
				"Expense Report Id",
				"Report Title",
				"Organization Id",
				"Submitted By",
				"Report Status",
				"Created On",
				"Modified On",
				"Submitted On"
			};
			output.WriteLine("\"{0}\"", string.Join("\",\"", columns));

			foreach (ExpenseReport report in reports)
			{
				var rowData = new[]
				{
					report.ExpenseReportId.ToString(),
					report.ReportTitle,
					report.OrganizationId.ToString(),
					report.SubmittedById.ToString(),
					report.ReportStatus.ToString(),
					report.CreatedUtc.ToString(CultureInfo.CurrentCulture),
					report.ModifiedUtc.ToString(CultureInfo.CurrentCulture),
					report.SubmittedUtc?.ToString() ?? string.Empty
				};
				output.WriteLine("\"{0}\"", string.Join("\",\"", rowData));
			}

			output.Flush();
			output.BaseStream.Seek(0, SeekOrigin.Begin);

			return output;
		}

		public int UpdateLockDate(int organizationId, DateTime? lockDate)
		{
			if (organizationId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(organizationId), $"{nameof(organizationId)} must be greater than 0.");
			}

			return DBHelper.UpdateLockDate(organizationId, lockDate);
		}

		/// <summary>
		/// Returns a SettingsInfo with start of week, overtime, and lock date settings, a list of PayClassInfos,
		/// and a list of Holidays for the current organization.
		/// </summary>
		/// <param name="organizaionId"></param>
		/// <returns>.</returns>
		public Tuple<Setting, List<PayClass>, List<Holiday>> GetAllSettings(int organizaionId)
		{
			UserContext.OrganizationsAndRoles.TryGetValue(organizaionId, out UserContext.OrganizationAndRole orgInfo);
			var spResults = DBHelper.GetAllSettings(orgInfo.OrganizationId);
			return Tuple.Create(
				DBEntityToServiceObject(spResults.Item1),
				spResults.Item2.Select(InitializePayClassInfo).ToList(),
				spResults.Item3.Select(InitializeHoliday).ToList());
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
		/// <param name="orgId">Organization Id.</param>
		/// <param name="startingDate">Start of date range.</param>
		/// <param name="endingDate">End of date range.</param>
		/// <returns>.</returns>
		public async Task<Tuple<Setting, List<PayClass>, List<Holiday>, List<CompleteProject>, List<User>, List<TimeEntry>>>
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

			var spResults = await DBHelper.GetTimeEntryIndexPageInfo(orgId, (int)ProductIdEnum.TimeTracker, userId.Value, startingDate, endingDate);

			return Tuple.Create(DBEntityToServiceObject(spResults.Item1),
				spResults.Item2.Select(InitializePayClassInfo).ToList(),
				spResults.Item3.Select(InitializeHoliday).ToList(),
				spResults.Item4.Select(InitializeCompleteProjectInfo).ToList(),
				spResults.Item5.Select(udb => InitializeUser(udb, false)).ToList(),
				spResults.Item6.Select(InitializeTimeEntryInfo).ToList());
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
				PayClassName = pc.PayClassName,
				OrganizationId = pc.OrganizationId,
				PayClassId = pc.PayClassId,
				CreatedUtc = pc.CreatedUtc
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
				CreatedUtc = hol.CreatedUtc,
				Date = hol.Date,
				HolidayId = hol.HolidayId,
				HolidayName = hol.HolidayName,
				OrganizationId = hol.OrganizationId
			};
		}

		/// <summary>
		/// Initializes a TimeEntry object based on a given TimeEntryDBEntity.
		/// </summary>
		/// <param name="entity">The TimeEntryDBEntity to use.</param>
		/// <returns>The initialized TimeEntry object.</returns>
		public static TimeEntry InitializeTimeEntryInfo(TimeEntryDBEntity entity)
		{
			return new TimeEntry
			{
				ApprovalState = entity.ApprovalState,
				Date = entity.Date,
				Description = entity.Description,
				Duration = entity.Duration,
				FirstName = entity.FirstName,
				LastName = entity.LastName,
				IsLockSaved = entity.IsLockSaved,
				ModSinceApproval = entity.ModSinceApproval,
				PayClassId = entity.PayClassId,
				PayClassName = entity.PayClassName,
				ProjectId = entity.ProjectId,
				TimeEntryId = entity.TimeEntryId,
				UserId = entity.UserId,
				EmployeeId = entity.EmployeeId,
				Email = entity.Email,
				TimeEntryStatusId = entity.TimeEntryStatusId,
				IsLocked = entity.IsLocked
			};
		}

		/// <summary>
		/// Builds a TimeEntryDBEntity based on a given TimeEntry object.
		/// </summary>
		/// <param name="info">The TimeEntry to use.</param>
		/// <returns>The built TimeEntryDBEntity based on the TimeEntry object.</returns>
		public static TimeEntryDBEntity GetDBEntityFromTimeEntryInfo(TimeEntry info)
		{
			return new TimeEntryDBEntity
			{
				ApprovalState = info.ApprovalState,
				Date = info.Date,
				Description = info.Description,
				Duration = info.Duration,
				FirstName = info.FirstName,
				LastName = info.LastName,
				IsLockSaved = info.IsLockSaved,
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
			return new HolidayDBEntity
			{
				CreatedUtc = holiday.CreatedUtc,
				Date = holiday.Date,
				HolidayId = holiday.HolidayId,
				HolidayName = holiday.HolidayName,
				OrganizationId = holiday.OrganizationId
			};
		}

		#endregion Info-DBEntity Conversions

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>.</returns>
		public async Task<Tuple<List<PositionThumbnailInfo>, List<Tag>, List<EmploymentType>, List<PositionLevel>, List<PositionStatus>, List<ApplicationStatus>, List<Customer>>>
			GetStaffingIndexInfo(int orgId, int? userId = null)
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

			#endregion Validation

			var results = await DBHelper.GetStaffingIndexPageInfo(orgId);

			return Tuple.Create(
				results.Item1.Select(posdb => InitializePositionThumbnailInfo(posdb, results.Item2, results.Item5)).ToList(),
				results.Item2.Select(InitializeTags).ToList(),
				results.Item3.Select(InitializeEmploymentTypes).ToList(),
				results.Item4.Select(InitializePositionLevel).ToList(),
				results.Item5.Select(InitializePositionStatus).ToList(),
				results.Item6.Select(InitializeApplicationStatus).ToList(),
				results.Item7.Select(InitializeBaseCustomer).ToList()
				);
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="statusName"></param>
		/// <param name="typeName"></param>
		/// <param name="tags">tags.</param>
		/// <param name="userId">User Id.</param>
		/// <returns>.</returns>
		public async Task<Tuple<List<PositionThumbnailInfo>, List<Tag>, List<EmploymentType>, List<PositionLevel>, List<PositionStatus>, List<ApplicationStatus>, List<Customer>>>
			GetStaffingIndexInfoFiltered(int orgId, List<string> statusName, List<string> typeName, List<string> tags = null, int? userId = 0)
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

			#endregion Validation

			var results = await DBHelper.GetStaffingIndexPageInfoFiltered(orgId, statusName, typeName, tags);

			return Tuple.Create(
				results.Item1.Select(posdb => InitializePositionThumbnailInfo(posdb, results.Item2, results.Item5)).ToList(),
				results.Item2.Select(InitializeTags).ToList(),
				results.Item3.Select(InitializeEmploymentTypes).ToList(),
				results.Item4.Select(InitializePositionLevel).ToList(),
				results.Item5.Select(InitializePositionStatus).ToList(),
				results.Item6.Select(InitializeApplicationStatus).ToList(),
				results.Item7.Select(InitializeBaseCustomer).ToList()
				);
		}

		/// <summary>
		/// Initializes a PositionThumbnailInfo from a PositionDBEntity.
		/// </summary>
		/// <param name="pos">the PositionDBEntity to be converted.</param>
		/// <param name="tags">the list of PositionTagDBEntity from initial results.</param>
		/// <param name="statuses">the list of PositionStatusDBEntity from initial results.</param>
		/// <returns>PositionThumbnailInfo.</returns>
		public static PositionThumbnailInfo InitializePositionThumbnailInfo(PositionDBEntity pos, List<PositionTagDBEntity> tags, List<PositionStatusDBEntity> statuses)
		{
			return new PositionThumbnailInfo
			{
				PositionId = pos.PositionId,
				OrganizationId = pos.OrganizationId,
				CustomerId = pos.CustomerId,
				PositionModifiedUtc = pos.PositionModifiedUtc,
				PositionStatusName = statuses.FirstOrDefault(status => status.PositionStatusId == pos.PositionStatusId)?.PositionStatusName ?? string.Empty,
				StartDate = pos.StartDate,
				PositionTitle = pos.PositionTitle,
				PositionCount = pos.PositionCount,
				TeamName = pos.TeamName,
				HiringManager = pos.HiringManager,
				Tags = tags
					.Where(tag => tag.PositionId == pos.PositionId)
					.Select(tag =>
						new Tag
						{
							TagId = tag.TagId,
							TagName = tag.TagName,
							PositionId = tag.PositionId
						})
					.ToList()
			};
		}

		/// <summary>
		/// Converts PositioNTagDBEntity to Tag services object
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public static Tag InitializeTags(PositionTagDBEntity tag)
		{
			return new Tag
			{
				TagId = tag.TagId,
				TagName = tag.TagName,
				PositionId = tag.PositionId
			};
		}

		/// <summary>
		/// initialize a base customer object
		/// </summary>
		/// <param name="customer"></param>
		/// <returns></returns>
		public static Customer InitializeBaseCustomer(CustomerDBEntity customer)
		{
			return new Customer
			{
				ContactEmail = customer.ContactEmail,
				ContactPhoneNumber = customer.ContactPhoneNumber,
				CreatedUtc = customer.CreatedUtc,
				CustomerId = customer.CustomerId,
				CustomerOrgId = customer.CustomerOrgId,
				EIN = customer.EIN,
				FaxNumber = customer.FaxNumber,
				CustomerName = customer.CustomerName,
				OrganizationId = customer.OrganizationId,
				Website = customer.Website,
				IsActive = customer.IsActive
			};
		}

		/// <summary>
		/// Converts employmentTypeDBEntity to employment type service object
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static EmploymentType InitializeEmploymentTypes(EmploymentTypeDBEntity type)
		{
			return new EmploymentType
			{
				EmploymentTypeId = type.EmploymentTypeId,
				EmploymentTypeName = type.EmploymentTypeName
			};
		}

		/// <summary>
		/// Converts positionLevelDBEntity to employment level service object
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public static PositionLevel InitializePositionLevel(PositionLevelDBEntity level)
		{
			return new PositionLevel
			{
				PositionLevelId = level.PositionLevelId,
				PositionLevelName = level.PositionLevelName
			};
		}

		/// <summary>
		/// converts PositionStatusDBEntity to position status service obejct
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public static PositionStatus InitializePositionStatus(PositionStatusDBEntity status)
		{
			return new PositionStatus
			{
				PositionStatusId = status.PositionStatusId,
				PositionStatusName = status.PositionStatusName
			};
		}

		/// <summary>
		/// converts PositionStatusDBEntity to position status service obejct
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public static ApplicationStatus InitializeApplicationStatus(ApplicationStatusDBEntity status)
		{
			return new ApplicationStatus
			{
				ApplicationStatusId = status.ApplicationStatusId,
				ApplicationStatusName = status.ApplicationStatusName
			};
		}
	}
}