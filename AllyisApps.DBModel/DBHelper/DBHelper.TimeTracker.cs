//------------------------------------------------------------------------------
// <copyright file="DBHelper.TimeTracker.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Crm;
using AllyisApps.DBModel.Hrm;
using AllyisApps.DBModel.TimeTracker;
using Dapper;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// DBHelper Partial.
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// Creates a holiday and related TimeEntries for an organization.
		/// </summary>
		/// <param name="newHoliday">The HolidayDBEntity to create.</param>
		public async Task<int> CreateHoliday(HolidayDBEntity newHoliday)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@holidayName", newHoliday.HolidayName);
			parameters.Add("@date", newHoliday.Date);
			parameters.Add("@organizationId", newHoliday.OrganizationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return (await connection.QueryAsync<int>("[Hrm].[CreateHoliday]", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
			}
		}

		/// <summary>
		/// Removes a holiday and related TimeEntries for an organization.
		/// </summary>
		/// <param name="holidayName">The HolidayDBEntity to delete.</param>
		/// <param name="date">The HolidayDBEntity to delete.</param>
		/// <param name="organizationId">The HolidayDBEntity to delete.</param>
		public async void DeleteHoliday(string holidayName, DateTime date, int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@holidayName", holidayName);
			parameters.Add("@date", date);
			parameters.Add("@organizationId", organizationId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync("[Hrm].[DeleteHoliday]", parameters, commandType: CommandType.StoredProcedure);
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
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<HolidayDBEntity>("[Hrm].[GetHolidays]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds a new pay class to the specified organization.
		/// </summary>
		/// <param name="payClassName">The name of the pay class to add.</param>
		/// <param name="organizationId">The organization to add the pay class to.</param>
		public int CreatePayClass(string payClassName, int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@payClassName", payClassName);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.QueryFirst<int>("[Hrm].[CreatePayClass]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a pay class from the specified organization.
		/// </summary>
		/// <param name="payClassId">The id of the pay class to remove.</param>
		/// <param name="destPayClass">The id of the payclass to move all old entries to (nullable).</param>
		public async void DeletePayClass(int payClassId, int? destPayClass)
		{
			// TODO: move this part in the DeletePayClass stored procedure
			if (destPayClass != null)
			{
				IEnumerable<TimeEntryDBEntity> allEntries = GetTimeEntriesThatUseAPayClass(payClassId);
				// update the payClassId for all time entries that used the old pay class
				foreach (TimeEntryDBEntity entry in allEntries)
				{
					entry.PayClassId = destPayClass.Value;
					UpdateTimeEntry(entry);
				}
			}
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@id", payClassId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync("[Hrm].[DeletePayClass]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Return a list of TimeEntryIds that use the specified payClassId.
		/// </summary>
		/// <param name="payClassId">The id of the pay class.</param>
		public IEnumerable<TimeEntryDBEntity> GetTimeEntriesThatUseAPayClass(int payClassId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@payClassId", payClassId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				IEnumerable<TimeEntryDBEntity> result = connection.Query<TimeEntryDBEntity>(
					"[TimeTracker].[GetTimeEntriesThatUseAPayClass]",
					parameters,
					commandType: CommandType.StoredProcedure);

				return result ?? new List<TimeEntryDBEntity>();
			}
		}

		/// <summary>
		/// Returns a list of the payclasses for a given organization or a default list if there aren't any.
		/// </summary>
		/// <param name="organizationId">The organizationid to look for or null for defaults.</param>
		/// <returns>The PayClassDBEntity.</returns>
		public async Task<IEnumerable<PayClassDBEntity>> GetPayClasses(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.QueryAsync<PayClassDBEntity>("[Hrm].[GetPayClasses]", parameters, commandType: CommandType.StoredProcedure);
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
		public async Task<IEnumerable<TimeEntryDBEntity>> GetTimeEntriesByUsersOverDateRange(List<int> userId, int orgId, DateTime startingDate, DateTime endingDate)
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
			parameters.Add("@startingDate", startingDate);
			parameters.Add("@endingDate", endingDate);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				IEnumerable<TimeEntryDBEntity> result = await connection.QueryAsync<TimeEntryDBEntity>(
					"[TimeTracker].[GetTimeEntriesByUserOverDateRange]",
					parameters,
					commandType: CommandType.StoredProcedure);

				return result ?? new List<TimeEntryDBEntity>();
			}
		}

		/// <summary>
		/// Gets all of the time entries with the defined params.
		/// </summary>
		/// <param name="orgId">The organization's Id.</param>
		/// <param name="startingDate">The beginning date of the date range.</param>
		/// <param name="endingDate">The ending date of the date range.</param>
		/// <returns>A collection of time entries.</returns>
		public async Task<List<TimeEntryDBEntity>> GetTimeEntriesOverDateRange(int orgId, DateTime startingDate, DateTime endingDate)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);
			parameters.Add("@startingDate", startingDate);
			parameters.Add("@endingDate", endingDate);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				IEnumerable<TimeEntryDBEntity> result = await connection.QueryAsync<TimeEntryDBEntity>(
																	"[TimeTracker].[GetTimeEntriesOverDateRange]",
																	parameters,
																	commandType: CommandType.StoredProcedure);

				return result?.ToList() ?? new List<TimeEntryDBEntity>();
			}
		}

		/// <summary>
		/// Gets a specific time entry.
		/// </summary>
		/// <param name="timeEntryId">The id of the time entry.</param>
		/// <returns>The time entry.</returns>
		public async Task<TimeEntryDBEntity> GetTimeEntryById(int timeEntryId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@timeEntryId", timeEntryId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var result = await connection.QueryAsync<TimeEntryDBEntity>(
					"[TimeTracker].[GetTimeEntryById]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return result.Single();
			}
		}

		/// <summary>
		/// Creates a new time entry in the database.
		/// </summary>
		/// <param name="entry">Object to be saved to the database.</param>
		/// <returns>The id of the created timeEntry. If unsuccessful, returns -1.</returns>
		public async Task<int> CreateTimeEntry(TimeEntryDBEntity entry)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", entry.UserId);
			parameters.Add("@projectId", entry.ProjectId);
			parameters.Add("@payClassId", entry.PayClassId);
			parameters.Add("@date", entry.Date);
			parameters.Add("@duration", entry.Duration);
			parameters.Add("@description", entry.Description);
			parameters.Add("@timeEntryStatusId", entry.TimeEntryStatusId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var result = await connection.QueryAsync<int>("[TimeTracker].[CreateTimeEntry]", parameters, commandType: CommandType.StoredProcedure);
				return result.Single();
			}
		}

		/// <summary>
		/// Updates the database entry at the entries Id with all updatable information.
		/// </summary>
		/// <param name="entry">Object representing the database entry.</param>
		public void UpdateTimeEntry(TimeEntryDBEntity entry)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@timeEntryId", entry.TimeEntryId);
			parameters.Add("@projectId", entry.ProjectId);
			parameters.Add("@payClassId", entry.PayClassId);
			parameters.Add("@duration", entry.Duration);
			parameters.Add("@description", entry.Description);
			parameters.Add("@timeEntryStatusId", entry.TimeEntryStatusId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
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
			parameters.Add("@timeEntryId", timeEntryId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[DeleteTimeEntry]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates the status of a time entry.
		/// </summary>
		/// <param name="timeEntryId">The Id of the time entry to be updated.</param>
		/// <param name="timeEntryStatusId">The new status.</param>
		public async Task<int> UpdateTimeEntryStatusById(int timeEntryId, int timeEntryStatusId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@timeEntryId", timeEntryId);
			parameters.Add("@timeEntryStatusId", timeEntryStatusId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.ExecuteAsync("[TimeTracker].[UpdateTimeEntryStatusById]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updatese only the start of week for an org.
		/// </summary>
		/// <param name="organizationId">The organization Id.</param>
		/// <param name="startOfWeek">The value for which day should be the start of the week. 1-6 M-Sat, 0 Sun.</param>
		public async Task<int> UpdateTimeTrackerStartOfWeek(int organizationId, int startOfWeek)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@startOfWeek", startOfWeek);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.ExecuteAsync("[TimeTracker].[UpdateStartOfWeek]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Update overtime settings for an Organization.
		/// </summary>
		/// <param name="organizationId">The Id of the time entry to be updated.</param>
		/// <param name="overtimeHours">Hours until overtime.</param>
		/// <param name="overtimePeriod">Time period for hours until overtime.</param>
		public async Task<int> UpdateOvertime(int organizationId, int? overtimeHours, string overtimePeriod)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@overtimeHours", overtimeHours);
			parameters.Add("@overtimePeriod", overtimePeriod);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.ExecuteAsync("[TimeTracker].[UpdateOvertime]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates the time tracker settings [OtSettingRecentlyChanged] column.
		/// </summary>
		/// <param name="organizationId">The organization that the settings belong to.</param>
		/// <param name="flag">The new value to change the column to.</param>
		/// <returns>Number of rows updated -- should be 1.</returns>
		public async Task<int> UpdateOvertimeChangedFlag(int organizationId, bool flag)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.ExecuteAsync("[TimeTracker].[UpdateOvertimeChangedFlag]", new { organizationId, flag }, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Get settings for organization.
		/// </summary>
		/// <param name="organizationId">The organization Id for which the settings are to be retrieved.</param>
		/// <returns>Settings for an organization.</returns>
		public async Task<SettingDBEntity> GetSettingsByOrganizationId(int organizationId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.QuerySingleAsync<SettingDBEntity>("[TimeTracker].[GetSettings]", new { organizationId }, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Initializes time tracker settings.
		/// </summary>
		/// <param name="organizationId">The organization that the time tracker settings belong to.</param>
		public async Task MergeDefaultTimeTrackerSettings(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@startOfWeek", 1);
			parameters.Add("@overTimeHours", 40);
			parameters.Add("@overTimePeriod", "Week");

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync("[TimeTracker].[MergeSettings]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates lock date in time tracker settings.
		/// </summary>
		/// <param name="organizationId">The organization that the settings belong to.</param>
		/// <param name="lockDate">The new lock date.  Can be null, to set the lock date to null.</param>
		/// <returns>.</returns>
		public async Task<int> UpdateLockDate(int organizationId, DateTime? lockDate)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@lockDate", lockDate);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.ExecuteAsync("[TimeTracker].[UpdateLockDate]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates payroll processed date in time tracker settings.
		/// </summary>
		/// <param name="organizationId">The organization that the settings belong to.</param>
		/// <param name="payrollProcessedDate">The new lock date.</param>
		/// <returns>.</returns>
		public async Task<int> UpdatePayrollProcessedDate(int organizationId, DateTime payrollProcessedDate)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@payrollProcessedDate", payrollProcessedDate);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.ExecuteAsync("[TimeTracker].[UpdatePayrollProcessedDate]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Returns a SettingsDBEntity with start of week, overtime, and lock date settings, a list of PayClassDBEntities,
		/// and a list of HolidayDBEntites for the given organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>.</returns>
		public Tuple<SettingDBEntity, List<PayClassDBEntity>, List<HolidayDBEntity>/*, List<EmployeeTypeDBEntity>*/> GetAllSettings(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[TimeTracker].[GetAllSettings]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<SettingDBEntity>().SingleOrDefault(),
					results.Read<PayClassDBEntity>().ToList(),
					results.Read<HolidayDBEntity>().ToList());
				//,
				//results.Read<EmployeeTypeDBEntity>().ToList());
			}
		}

		/// <summary>
		/// Gets a list of CutomerDBEntities for all customers in the given organization, a list of CompleteProjectDBEntities
		/// for all projects in the given organization, and a list of SubscriptionUserDBEntities for all users in the given
		/// subscription.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns>.</returns>
		public async Task<Tuple<List<dynamic>, List<ProjectDBEntity>, List<SubscriptionUserDBEntity>>> GetReportInfo(int orgId, int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgId);
			parameters.Add("@subscriptionId", subscriptionId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryMultipleAsync(
					"[TimeTracker].[GetReportInfo]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<dynamic>().ToList(),
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
		/// <returns>.</returns>
		public async Task<Tuple<SettingDBEntity, List<PayClassDBEntity>, List<HolidayDBEntity>, List<ProjectDBEntity>, List<UserDBEntity>, List<TimeEntryDBEntity>>>
			GetTimeEntryIndexPageInfo(int orgId, int timeTrackerProductId, int userId, DateTime? startingDate, DateTime? endingDate)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);
			parameters.Add("@productId", timeTrackerProductId);
			parameters.Add("@userId", userId);
			parameters.Add("@startingDate", startingDate);
			parameters.Add("@endingDate", endingDate);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryMultipleAsync(
					"[TimeTracker].[GetTimeEntryIndexInfo]",
					parameters,
					commandType: CommandType.StoredProcedure);

				return Tuple.Create(
					results.Read<SettingDBEntity>().Single(),
					results.Read<PayClassDBEntity>().ToList(),
					results.Read<HolidayDBEntity>().ToList(),
					results.Read<ProjectDBEntity>().ToList(),
					results.Read<UserDBEntity>().ToList(),
					results.Read<TimeEntryDBEntity>().ToList());
			}
		}

		/// <summary>
		/// Updates the pay period of the time tracker settings to store a json
		/// object containing duration type info.  Pay period can either be duration or
		/// dates type.
		/// </summary>
		/// <param name="payPeriodJson">Json object conatining:
		///  > the duration in days of each pay period.
		///  > the start date from which to base the pay period off of.</param>
		/// <param name="organizationId">The organization that this time tracker settings belongs to.</param>
		/// <returns>The number of updated rows.</returns>
		public async Task<int> UpdatePayPeriod(string payPeriodJson, int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@payPeriodJson", payPeriodJson);
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.ExecuteAsync(
					"[TimeTracker].[UpdatePayPeriod]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets all time entries that belong to the given project.
		/// </summary>
		/// <param name="projectId">The id of the project that the time entries belong to.</param>
		/// <returns>Gets all time entries that belong to the given project.</returns>
		public async Task<IEnumerable<TimeEntryDBEntity>> GetTimeEntriesByProjectId(int projectId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectId", projectId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.QueryAsync<TimeEntryDBEntity>(
					"[TimeTracker].[GetTimeEntriesByProjectId]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public async Task<int> CreateEmployeeType(int orgId, string employeeName)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgId);
			parameters.Add("@employeeName", employeeName);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<int>(
					"[Hrm].[CreateEmployeeType]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return results.FirstOrDefault();
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="employeeTypeId"></param>
		/// <returns></returns>
		public async Task DeleteEmployeeType(int employeeTypeId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@employeeTypeId", employeeTypeId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync(
				   "[Hrm].[DeleteEmployeeType]",
				   parameters,
				   commandType: CommandType.StoredProcedure);

				return;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="employeeTypeId"></param>
		/// <returns></returns>
		public async Task<List<int>> GetAssignedPayClasses(int employeeTypeId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@employeeTypeId", employeeTypeId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<int>(
					"[Hrm].[GetAssingedPayClasses]",
					parameters,
					commandType: CommandType.StoredProcedure);

				return results.ToList();
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="employeeTypeId"></param>
		/// <param name="payClassId"></param>
		/// <returns></returns>
		public async Task AddPayClassToEmployeeType(int employeeTypeId, int payClassId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@employeeTypeId", employeeTypeId);
			parameters.Add("@payclassId", payClassId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.ExecuteAsync(
					"[Hrm].[AddPayClassToEmployeeType]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="employeeTypeId"></param>
		/// <param name="payClassId"></param>
		/// <returns></returns>
		public async Task RemovePayClassFromEmployeeType(int employeeTypeId, int payClassId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@employeeTypeId", employeeTypeId);
			parameters.Add("@payclassId", payClassId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.ExecuteAsync(
					"[Hrm].[RemovePayClassFromEmployeeType]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="employeeTypeId"></param>
		/// <returns></returns>
		public async Task<EmployeeTypeDBEntity> GetEmployeeType(int employeeTypeId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@employeeTypeId", employeeTypeId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<EmployeeTypeDBEntity>(
					"[Hrm].[GetEmployeeType]",
					parameters,
					commandType: CommandType.StoredProcedure);

				return results.FirstOrDefault();
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="organizationId"></param>
		/// <returns></returns>
		public async Task<List<EmployeeTypeDBEntity>> GetEmployeeTypesByOrganization(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<EmployeeTypeDBEntity>(
					"[Hrm].[GetEmployeeTypesByOrganization]",
					parameters,
					commandType: CommandType.StoredProcedure);

				return results.ToList();
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="employeeType"></param>
		/// <returns></returns>
		public async Task UpdateUserOrgEmployeeType(int userId, int employeeType)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userId", userId);
			parameters.Add("@employeeType", employeeType);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync(
					"[TimeTracker].[UpdateUserOrgEmployeeType]",
					parameters,
					commandType: CommandType.StoredProcedure);
			}
		}
	}
}