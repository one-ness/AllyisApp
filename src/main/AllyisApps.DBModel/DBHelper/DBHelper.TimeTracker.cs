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
		/// Get a list of all setting items.
		/// </summary>
		/// <returns>A list of all setting items.</returns>
		public List<SettingDBEntity> GetSettingList()
		{
			using (var conn = new SqlConnection(this.SqlConnectionString))
			{
				return conn.Query<SettingDBEntity>("TimeTracker.GetSettingList").ToList();
			}
		}

		/// <summary>
		/// Returns the organizationid of the org associated with a project.
		/// </summary>
		/// <param name="projectID">The project.</param>
		/// <returns>The organization id.</returns>
		public int GetOrganizationFromProject(int projectID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@projectID", projectID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>("[TimeTracker].[GetOrganizationFromProject]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Returns the lock date for a specific organization.
		/// </summary>
		/// <param name="organizationID">The Organization Id.</param>
		/// <returns>The lock date.</returns>
		public DateTime? GetLockDate(int organizationID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", organizationID);
			//if (organizationID < 1)
			//{
			//	return DateTime.Now.AddDays(-7 - (int)DateTime.Now.DayOfWeek);
			//}

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				LockDateDBEntity lockDate = connection.Query<LockDateDBEntity>("[TimeTracker].[GetLockDate]", parameters, commandType: CommandType.StoredProcedure).Single();
				//if (!lockDate.HasValue)
				//{
				//	return DateTime.Now.AddDays(-7 - (int)DateTime.Now.DayOfWeek);
				//}

                if (lockDate != null && lockDate.LockDateUsed)
                {
                    DateTime date = lockDate.LockDatePeriod.Equals("Months") ? DateTime.Now.AddMonths(-1 * lockDate.LockDateQuantity) :
                        DateTime.Now.AddDays(-1 * lockDate.LockDateQuantity * (lockDate.LockDatePeriod.Equals("Weeks") ? 7 : 1));
                    return date;
                }
				return null;
			}
		}

		/// <summary>
		/// Sets the lock date for a specific organization/user.
		/// </summary>
		/// <param name="organizationID">The Organization Id.</param>
		/// <param name="userID">The User Id.</param>
		/// <param name="date">The date.</param>
		public void SetLockDate(int organizationID, int userID, DateTime date)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", organizationID);
			parameters.Add("@userID", userID);
			parameters.Add("@date", date);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[SetLockDate]", parameters, commandType: CommandType.StoredProcedure);
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
			parameters.Add("@organizationID", newHoliday.OrganizationId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				newHoliday.HolidayId = connection.Query<int>("[TimeTracker].[CreateHoliday]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Removes a holiday and related TimeEntries for an organization.
		/// </summary>
		/// <param name="holidayName">The HolidayDBEntity to delete.</param>
		/// <param name="date">The HolidayDBEntity to delete.</param>
		/// <param name="organizationID">The HolidayDBEntity to delete.</param>
		public void DeleteHoliday(string holidayName, DateTime date, int organizationID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@holidayName", holidayName);
			parameters.Add("@date", date);
			parameters.Add("@organizationID", organizationID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[DeleteHoliday]", parameters, commandType: CommandType.StoredProcedure);
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
			parameters.Add("@organizationID", organizationId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<HolidayDBEntity>("[TimeTracker].[GetHolidays]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds a new pay class to the specified organization.
		/// </summary>
		/// <param name="payClassName">The name of the pay class to add.</param>
		/// <param name="organizationID">The organization to add the pay class to.</param>
		public void CreatePayClass(string payClassName, int organizationID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", organizationID);
			parameters.Add("@Name", payClassName);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[CreatePayClass]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a pay class from the specified organization.
		/// </summary>
		/// <param name="payClassID">The name of the pay class to remove.</param>
		public void DeletePayClass(int payClassID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ID", payClassID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[DeletePayClass]", parameters, commandType: CommandType.StoredProcedure);
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
			parameters.Add("@organizationID", organizationId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<PayClassDBEntity>("[TimeTracker].[GetPayClasses]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Get play class by id.
		/// </summary>
		/// <param name="id">Id of pay class.</param>
		/// <returns>The PayClassDBEntity.</returns>
		public PayClassDBEntity GetPayClassById(int id)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ID", id);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<PayClassDBEntity>("[TimeTracker].[GetPayClassById]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Get a pay class by name and org.
		/// </summary>
		/// <param name="name">Name of class.</param>
		/// <param name="orgId">Id of org.</param>
		/// <returns>The PayClassDBEntity.</returns>
		public PayClassDBEntity GetPayClassByNameAndOrg(string name, int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", orgId);
			parameters.Add("@Name", name);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<PayClassDBEntity>("[TimeTracker].[GetPayClassByNameAndOrg]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
			users.Columns.Add("userID", typeof(string));
			for (int i = 0; i < userId.Count; i++)
			{
				users.Rows.Add(userId[i]);
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userID", users.AsTableValuedParameter("[Auth].[UserTable]"));
			parameters.Add("@organizationID", orgId);
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
			parameters.Add("@organizationID", orgId);
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
			parameters.Add("@userID", entry.UserId);
			parameters.Add("@projectID", entry.ProjectId);
			parameters.Add("@payClassID", entry.PayClassId);
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
			parameters.Add("@projectID", entry.ProjectId);
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
		/// <param name="orgId">The organization ID.</param>
		/// <param name="startOfWeek">The value for which day should be the start of the week. 1-6 M-Sat, 0 Sun.</param>
		public void UpdateTimeTrackerStartOfWeek(int orgId, int startOfWeek)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", orgId);
			parameters.Add("@StartOfWeek", startOfWeek);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[UpdateStartOfWeek]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Update settings for an Organization.
		/// </summary>
		/// <param name="orgId">The Id of the time entry to be updated.</param>
		/// <param name="startOfWeek">The new approval state.</param>
		/// <param name="overtimeHours">Hours until overtime.</param>
		/// <param name="overtimePeriod">Time period for hours until overtime.</param>
		/// <param name="overtimeMultiplier">Overtime pay multiplier.</param>
		public void UpdateSettings(int orgId, int startOfWeek, int overtimeHours, string overtimePeriod, float overtimeMultiplier)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", orgId);
			parameters.Add("@StartOfWeek", startOfWeek);
			parameters.Add("@OvertimeHours", overtimeHours);
			parameters.Add("@OvertimePeriod", overtimePeriod);
			parameters.Add("@OvertimeMultiplier", overtimeMultiplier);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[TimeTracker].[UpdateSettings]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Get start of week for organization.
		/// </summary>
		/// <param name="orgId">The organization Id for which the StartOfWeek is to be retrieved.</param>
		/// <returns>The start of week int.</returns>
		public int GetStartOfWeek(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationID", orgId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>(
					"[TimeTracker].[GetStartOfWeek]",
					parameters,
					commandType: CommandType.StoredProcedure).Single();
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
			parameters.Add("@organizationID", orgId);

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
		/// <param name="orgId">The organization ID.</param>
		public void InitializeTimeTrackerSettings(int orgId)
		{
			try
			{
				DBHelper.Instance.GetSettings(orgId);
			}
			catch
			{
				DynamicParameters parameters = new DynamicParameters();

				// Init the actaul settings
				parameters.Add("@organizationID", orgId);
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
					parameters.Add("@organizationID", 0);
					holidays = connection.Query<HolidayDBEntity>("[TimeTracker].[GetHolidays]", parameters, commandType: CommandType.StoredProcedure);
					if (holidays != null && holidays.Count() > 0)
					{
						foreach (HolidayDBEntity currentHoliday in holidays)
						{
							parameters = new DynamicParameters();
							parameters.Add("@organizationID", orgId);
							parameters.Add("@holidayName", currentHoliday.HolidayName);
							parameters.Add("@date", currentHoliday.Date);
							connection.Execute("[TimeTracker].[CreateHoliday]", parameters, commandType: CommandType.StoredProcedure);
						}
					}
				}

				// init new set of pay classes
				IEnumerable<PayClassDBEntity> payClasses;

				using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
				{
					parameters = new DynamicParameters();
					parameters.Add("@organizationID", 0);
					payClasses = connection.Query<PayClassDBEntity>("[TimeTracker].[GetPayClasses]", parameters, commandType: CommandType.StoredProcedure);
					if (payClasses != null && payClasses.Count() > 0)
					{
						foreach (PayClassDBEntity currentPayClass in payClasses)
						{
							parameters = new DynamicParameters();
							parameters.Add("@organizationID", orgId);
							parameters.Add("@Name", currentPayClass.Name);
							connection.Execute("[TimeTracker].[CreatePayClass]", parameters, commandType: CommandType.StoredProcedure);
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
        public bool UpdateLockDate (int organizationId, bool lockDateUsed, string lockDatePeriod, int lockDateQuantity)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@organizationID", organizationId);
            parameters.Add("@LockDateUsed", lockDateUsed);
            parameters.Add("@LockDatePeriod", lockDatePeriod);
            parameters.Add("@LockDateQuantity", lockDateQuantity);

            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                connection.Execute("[TimeTracker].[UpdateLockDate]", parameters, commandType: CommandType.StoredProcedure);
            }

            return true;
        }
	}
}