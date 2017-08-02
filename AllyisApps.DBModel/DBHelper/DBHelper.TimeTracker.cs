//------------------------------------------------------------------------------
// <copyright file="DBHelper.TimeTracker.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Crm;
using AllyisApps.DBModel.TimeTracker;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// DBHelper Partial.
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// Returns the lock date for a specific organization.
		/// </summary>
		/// <param name="organizationId">The Organization Id.</param>
		/// <returns>The lock date.</returns>
		public LockDateDBEntity GetLockDate(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<LockDateDBEntity>("[TimeTracker].[GetLockDate]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Creates a holiday and related TimeEntries for an organization.
		/// </summary>
		/// <param name="newHoliday">The HolidayDBEntity to create.</param>
		public void CreateHoliday(HolidayDBEntity newHoliday)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@holidayName", newHoliday.HolidayName);
			parameters.Add("@date", newHoliday.Date);
			parameters.Add("@organizationId", newHoliday.OrganizationId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				newHoliday.HolidayId = connection.Query<int>("[Hrm].[CreateHoliday]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Removes a holiday and related TimeEntries for an organization.
		/// </summary>
		/// <param name="holidayName">The HolidayDBEntity to delete.</param>
		/// <param name="date">The HolidayDBEntity to delete.</param>
		/// <param name="organizationId">The HolidayDBEntity to delete.</param>
		public void DeleteHoliday(string holidayName, DateTime date, int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@holidayName", holidayName);
			parameters.Add("@date", date);
			parameters.Add("@organizationId", organizationId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Hrm].[DeleteHoliday]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets a list of the Holidays for an organization.
		/// </summary>
		/// <param name="organizationId">The organization id to retrieve holidays for or nothing for defaults.</param>
		/// <returns>A list of HolidayDBEntity objects.</returns>
		public IEnumerable<HolidayDBEntity> GetHolidays(int organizationId = 0)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<HolidayDBEntity>("[Hrm].[GetHolidays]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds a new pay class to the specified organization.
		/// </summary>
		/// <param name="payClassName">The name of the pay class to add.</param>
		/// <param name="organizationId">The organization to add the pay class to.</param>
		public void CreatePayClass(string payClassName, int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@Name", payClassName);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Hrm].[CreatePayClass]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a pay class from the specified organization.
		/// </summary>
		/// <param name="payClassId">The id of the pay class to remove.</param>
		/// <param name="destPayClass">The id of the payclass to move all old entries to (nullable)</param>
		public void DeletePayClass(int payClassId, int? destPayClass)
		{
			//TODO: move this part in the DeletePayClass stored procedure
			if (destPayClass != null)
			{
				IEnumerable<TimeEntryDBEntity> allEntries = GetTimeEntriesThatUseAPayClass(payClassId);
				//update the payClassId for all time entries that used the old pay class
				foreach (TimeEntryDBEntity entry in allEntries)
				{
					entry.PayClassId = destPayClass.Value;
					UpdateTimeEntry(entry);
				}
			}
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", payClassId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Hrm].[DeletePayClass]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Return a list of TimeEntryIds that use the specified payClassId
		/// </summary>
		/// <param name="payClassId">The id of the pay class.</param>
		public IEnumerable<TimeEntryDBEntity> GetTimeEntriesThatUseAPayClass(int payClassId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@payClassId", payClassId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				IEnumerable<TimeEntryDBEntity> result = connection.Query<TimeEntryDBEntity>(
					"[TimeTracker].[GetTimeEntriesThatUseAPayClass]",
					parameters,
					commandType: CommandType.StoredProcedure);

				return result == null ? new List<TimeEntryDBEntity>() : result;
			}
		}

		/// <summary>
		/// Returns a list of the payclasses for a given organization or a default list if there aren't any.
		/// </summary>
		/// <param name="organizationId">The organizationid to look for or null for defaults.</param>
		/// <returns>The PayClassDBEntity.</returns>
		public IEnumerable<PayClassDBEntity> GetPayClasses(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<PayClassDBEntity>("[Hrm].[GetPayClasses]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets all of the time entries with the defined params.
		/// </summary>
		/// <param name="userId">An array of user Id's.</param>
		/// <param name="orgId">The organization's Id.</param>
		/// <param name="startingDate">The beginning date of the date range.</param>
		/// <param name="endingDate">The ending date of the date range.</param>
		/// <returns>A collection of time entries.</returns>
		public IEnumerable<TimeEntryDBEntity> GetTimeEntriesByUserOverDateRange(
			List<int> userId,
			int orgId,
			DateTime startingDate,
			DateTime endingDate)
		{
			DataTable users = new DataTable();
			users.Columns.Add("userId", typeof(string));
			for (int i = 0; i < userId.Count; i++)
			{
				users.Rows.Add(userId[i]);
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", users.AsTableValuedParameter("[Auth].[UserTable]"));
			parameters.Add("@organizationId", orgId);
			parameters.Add("@StartingDate", startingDate);
			parameters.Add("@EndingDate", endingDate);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				IEnumerable<TimeEntryDBEntity> result = connection.Query<TimeEntryDBEntity>(
					"[TimeTracker].[GetTimeEntriesByUserOverDateRange]",
					parameters,
					commandType: CommandType.StoredProcedure);

				return result == null ? new List<TimeEntryDBEntity>() : result;
			}
		}

		/// <summary>
		/// Gets all of the time entries with the defined params.
		/// </summary>
		/// <param name="orgId">The organization's Id.</param>
		/// <param name="startingDate">The beginning date of the date range.</param>
		/// <param name="endingDate">The ending date of the date range.</param>
		/// <returns>A collection of time entries.</returns>
		public IEnumerable<TimeEntryDBEntity> GetTimeEntriesOverDateRange(int orgId, DateTime startingDate, DateTime endingDate)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);
			parameters.Add("@StartingDate", startingDate);
			parameters.Add("@EndingDate", endingDate);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				IEnumerable<TimeEntryDBEntity> result = connection.Query<TimeEntryDBEntity>(
																	"[TimeTracker].[GetTimeEntriesOverDateRange]",
																	parameters,
																	commandType: CommandType.StoredProcedure);

				return result == null ? new List<TimeEntryDBEntity>() : result;
			}
		}

		/// <summary>
		/// Gets a specific time entry.
		/// </summary>
		/// <param name="timeEntryId">The id of the time entry.</param>
		/// <returns>The time entry.</returns>
		public TimeEntryDBEntity GetTimeEntryById(int timeEntryId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@TimeEntryId", timeEntryId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<TimeEntryDBEntity>(
					"[TimeTracker].[GetTimeEntryById]",
					parameters,
					commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Creates a new time entry in the database.
		/// </summary>
		/// <param name="entry">Object to be saved to the database.</param>
		/// <returns>The id of the created timeEntry. If unsuccessful, returns -1.</returns>
		public int CreateTimeEntry(TimeEntryDBEntity entry)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", entry.UserId);
			parameters.Add("@projectId", entry.ProjectId);
			parameters.Add("@payClassId", entry.PayClassId);
			parameters.Add("@date", entry.Date);
			parameters.Add("@Duration", entry.Duration);
			parameters.Add("@Description", entry.Description);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				entry.TimeEntryId = connection.Query<int>("[TimeTracker].[CreateTimeEntry]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}

			return entry.TimeEntryId;
		}

		/// <summary>
		/// Updates the database entry at the entries Id with all updatable information.
		/// </summary>
		/// <param name="entry">Object representing the database entry.</param>
		public void UpdateTimeEntry(TimeEntryDBEntity entry)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@TimeEntryId", entry.TimeEntryId);
			parameters.Add("@projectId", entry.ProjectId);
			parameters.Add("@PayClassId", entry.PayClassId);
			parameters.Add("@Duration", entry.Duration);
			parameters.Add("@Description", entry.Description);
			parameters.Add("@LockSaved", entry.LockSaved);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[UpdateTimeEntry]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a time entry.
		/// </summary>
		/// <param name="timeEntryId">The time entry to be deleted.</param>
		public void DeleteTimeEntry(int timeEntryId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@TimeEntryId", timeEntryId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[DeleteTimeEntry]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Sets the approval state of a time entry in the database.
		/// </summary>
		/// <param name="timeEntryId">The Id of the time entry to be updated.</param>
		/// <param name="approvalState">The new approval state.</param>
		public void SetTimeEntryApprovalStateById(int timeEntryId, int approvalState)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@TimeEntryId", timeEntryId);
			parameters.Add("@ApprovalState", approvalState);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[SetTimeEntryApprovalStateById]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updatese only the start of week for an org.
		/// </summary>
		/// <param name="orgId">The organization Id.</param>
		/// <param name="startOfWeek">The value for which day should be the start of the week. 1-6 M-Sat, 0 Sun.</param>
		public void UpdateTimeTrackerStartOfWeek(int orgId, int startOfWeek)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);
			parameters.Add("@StartOfWeek", startOfWeek);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[UpdateStartOfWeek]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Update overtime settings for an Organization.
		/// </summary>
		/// <param name="orgId">The Id of the time entry to be updated.</param>
		/// <param name="overtimeHours">Hours until overtime.</param>
		/// <param name="overtimePeriod">Time period for hours until overtime.</param>
		/// <param name="overtimeMultiplier">Overtime pay multiplier.</param>
		public void UpdateOvertime(int orgId, int overtimeHours, string overtimePeriod, float overtimeMultiplier)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);
			parameters.Add("@OvertimeHours", overtimeHours);
			parameters.Add("@OvertimePeriod", overtimePeriod);
			parameters.Add("@OvertimeMultiplier", overtimeMultiplier);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[UpdateOvertime]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Get settings for organization.
		/// </summary>
		/// <param name="orgId">The organization Id for which the settings are to be retrieved.</param>
		/// <returns>Settings for an organization.</returns>
		public SettingDBEntity GetSettings(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<SettingDBEntity>(
					"[TimeTracker].[GetSettings]",
					parameters,
					commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Initializes time tracker settings.
		/// </summary>
		/// <param name="orgId">The organization Id.</param>
		public void InitializeTimeTrackerSettings(int orgId)
		{
			try
			{
				this.GetSettings(orgId);
			}
			catch
			{
				DynamicParameters parameters = new DynamicParameters();

				// Init the actaul settings
				parameters.Add("@organizationId", orgId);
				parameters.Add("@StartOfWeek", 1);
				parameters.Add("@OverTimeHours", 40);
				parameters.Add("@OverTimePeriod", "Week");
				parameters.Add("@OverTimeMultiplier", 1.5);
				using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
				{
					connection.Execute("[TimeTracker].[UpdateSettings]", parameters, commandType: CommandType.StoredProcedure);
				}
                
                // Init new set of default holidays for time tracker
                IEnumerable<HolidayDBEntity> holidays;

				using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
				{
					parameters = new DynamicParameters();
					parameters.Add("@organizationId", 0);
					holidays = connection.Query<HolidayDBEntity>("[Hrm].[GetHolidays]", parameters, commandType: CommandType.StoredProcedure);
					if (holidays != null && holidays.Count() > 0)
					{
						foreach (HolidayDBEntity currentHoliday in holidays)
						{
							parameters = new DynamicParameters();
							parameters.Add("@organizationId", orgId);
							parameters.Add("@holidayName", currentHoliday.HolidayName);
							parameters.Add("@date", currentHoliday.Date);
							connection.Execute("[Hrm].[CreateHoliday]", parameters, commandType: CommandType.StoredProcedure);
						}
					}
				}

				// init new set of pay classes
				IEnumerable<PayClassDBEntity> payClasses;

				using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
				{
					parameters = new DynamicParameters();
					parameters.Add("@organizationId", 0);
					payClasses = connection.Query<PayClassDBEntity>("[Hrm].[GetPayClasses]", parameters, commandType: CommandType.StoredProcedure);
					if (payClasses != null && payClasses.Count() > 0)
					{
						foreach (PayClassDBEntity currentPayClass in payClasses)
						{
							parameters = new DynamicParameters();
							parameters.Add("@organizationId", orgId);
							parameters.Add("@Name", currentPayClass.Name);
							connection.Execute("[Hrm].[CreatePayClass]", parameters, commandType: CommandType.StoredProcedure);
						}
					}
				}
			}
		}

		/// <summary>
		/// Updates lock date settings.
		/// </summary>
		/// <param name="organizationId"></param>
		/// <param name="lockDateUsed"></param>
		/// <param name="lockDatePeriod"></param>
		/// <param name="lockDateQuantity"></param>
		/// <returns></returns>
		public bool UpdateLockDate(int organizationId, bool lockDateUsed, int lockDatePeriod, int lockDateQuantity)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@LockDateUsed", lockDateUsed);
			parameters.Add("@LockDatePeriod", lockDatePeriod);
			parameters.Add("@LockDateQuantity", lockDateQuantity);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[UpdateLockDate]", parameters, commandType: CommandType.StoredProcedure);
			}

			return true;
		}

		/// <summary>
		/// Returns a SettingsDBEntity with start of week, overtime, and lock date settings, a list of PayClassDBEntities,
		/// and a list of HolidayDBEntites for the given organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns></returns>
		public Tuple<SettingDBEntity, List<PayClassDBEntity>, List<HolidayDBEntity>> GetAllSettings(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[TimeTracker].[GetAllSettings]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<SettingDBEntity>().SingleOrDefault(),
					results.Read<PayClassDBEntity>().ToList(),
					results.Read<HolidayDBEntity>().ToList());
			}
		}

		/// <summary>
		/// Gets a list of CutomerDBEntities for all customers in the given organization, a list of CompleteProjectDBEntities
		/// for all projects in the given organization, and a list of SubscriptionUserDBEntities for all users in the given
		/// subscription.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns></returns>
		public Tuple<List<CustomerDBEntity>, List<ProjectDBEntity>, List<SubscriptionUserDBEntity>> GetReportInfo(int orgId, int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgId);
			parameters.Add("@subscriptionId", subscriptionId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[TimeTracker].[GetReportInfo]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<CustomerDBEntity>().ToList(),
					results.Read<ProjectDBEntity>().ToList(),
					results.Read<SubscriptionUserDBEntity>().ToList());
			}
		}

		/// <summary>
		/// Returns a SettingsDBEntity for the given organization's TimeTracker settings (with only start of week and
		/// lock date fields populated), a list of PayClassDBEntities for all the organization's pay classes, a list of
		/// HolidayDBEntities for all the organization's holidays, a list of CompleteProjectDBEntities for all projects
		/// in the given org that the given user is or has been assigned to (active or not), a list of UserDBEntities
		/// for all the users in the org who are users of the time tracker subscription, and a list of TimeEntryDBEntities
		/// for all time entries for the given user in the given time range.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="timeTrackerProductId">Product Id for TimeTracker.</param>
		/// <param name="userId">User Id.</param>
		/// <param name="startingDate">Start of date range.</param>
		/// <param name="endingDate">End of date range.</param>
		/// <returns></returns>
		public Tuple<SettingDBEntity, List<PayClassDBEntity>, List<HolidayDBEntity>, List<ProjectDBEntity>, List<UserDBEntity>, List<TimeEntryDBEntity>>
			GetTimeEntryIndexPageInfo(int orgId, int timeTrackerProductId, int userId, DateTime? startingDate, DateTime? endingDate)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", orgId);
			parameters.Add("@ProductId", timeTrackerProductId);
			parameters.Add("@UserId", userId);
			parameters.Add("@StartingDate", startingDate);
			parameters.Add("@EndingDate", endingDate);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[TimeTracker].[GetTimeEntryIndexInfo]",
					parameters,
					commandType: CommandType.StoredProcedure);

				return Tuple.Create(results.Read<SettingDBEntity>().SingleOrDefault(),
					results.Read<PayClassDBEntity>().ToList(),
					results.Read<HolidayDBEntity>().ToList(),
					results.Read<ProjectDBEntity>().ToList(),
					results.Read<UserDBEntity>().ToList(),
					results.Read<TimeEntryDBEntity>().ToList());
			}
		}
	}
}
