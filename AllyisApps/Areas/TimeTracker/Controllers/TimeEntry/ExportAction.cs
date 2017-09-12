//------------------------------------------------------------------------------
// <copyright file="ExportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
using AllyisApps.Utilities;
using AllyisApps.ViewModels.TimeTracker.Project;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Export used on TimeEntry/Index. (May be removed soon).
		/// </summary>
		/// <param name="userId">The User's Id.</param>
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <param name="startingDate">The Starting date of the range (nullable).</param>
		/// <param name="endingDate">The Ending date of the range (nullable).</param>
		/// <returns>CSV export of time entries.</returns>
		public FileStreamResult Export(int userId, int subscriptionId, int? startingDate = null, int? endingDate = null)
		{
			int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			if (userId != this.AppService.UserContext.UserId)
			{
				this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);
			}

			DateTime? start = startingDate.HasValue ? (DateTime?)AppService.GetDateTimeFromDays(startingDate.Value) : null;
			DateTime? end = endingDate.HasValue ? (DateTime?)AppService.GetDateTimeFromDays(endingDate.Value) : null;

			return this.File(AppService.PrepareCSVExport(orgId, new List<int> { userId }, start, end).BaseStream, "text/csv", "export.csv");
		}

		/// <summary>
		/// Uses services to initialize a new instance of the <see cref="DataExportViewModel" /> class and returns it.
		/// </summary>
		/// <param name="subscriptionId">The subscriptionId.</param>
		/// <param name="orgId">The organization's Id.</param>
		/// <param name="userIds">An array of user ids.</param>
		/// <param name="startingDate">The starting of the date range (nullable).</param>
		/// <param name="endingDate">The ending of the date range (nullable).</param>
		/// <param name="projectId">The project's Id (optional).</param>
		/// <param name="customerId">The Customer's Id (optional).</param>
		/// <returns>The DataExportViewModel.</returns>
		public DataExportViewModel ConstructDataExportViewModel(int subscriptionId, int orgId = -1, List<int> userIds = null, DateTime? startingDate = null, DateTime? endingDate = null, int projectId = 0, int customerId = 0)
		{
			if (-1 <= orgId)
			{
				orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			}

			DataExportViewModel result = new DataExportViewModel();
			if ((userIds == null) || (userIds[0] == -1))
			{
				result.Data = AppService.GetTimeEntriesOverDateRange(orgId, startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddDays(-1))
				.AsParallel().Select(timeEntry => ConstuctTimeEntryViewModel(timeEntry)).AsEnumerable();
			}
			else
			{
				result.Data = AppService.GetTimeEntriesByUserOverDateRange(userIds, startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddDays(-1), orgId)
				.AsParallel().Select(timeEntry => ConstuctTimeEntryViewModel(timeEntry));
			}

			if (projectId != 0)
			{
				result.Data = from i in result.Data
							  where i.ProjectId == projectId
							  select i;
			}
			else if (customerId != 0)
			{
				IEnumerable<int> projects = AppService.GetProjectsByCustomer(customerId).Select(proj => proj.ProjectId).ToList();
				result.Data = from c in result.Data
							  where projects.Contains(c.ProjectId)
							  select c;
			}

			if (userIds != null && userIds.Count > 0)
			{
				if ((userIds.Count > 1) || (userIds[0] == -1))
				{
					result.Projects = AppService.GetProjectsByOrganization(orgId).AsParallel().Select(proj => ViewModelHelper.ConstuctCompleteProjectViewModel(proj));
				}
				else
				{
					// single user selected
					result.Projects = AppService.GetProjectsByUserAndOrganization(userIds[0], orgId, false).AsParallel().Select(proj => ViewModelHelper.ConstuctCompleteProjectViewModel(proj));
				}

				// Add default project in case there are holiday entries
				List<CompleteProjectViewModel> defaultProject = new List<CompleteProjectViewModel>();
				defaultProject.Add(ViewModelHelper.ConstuctCompleteProjectViewModel(AppService.GetProject(0)));
				result.Projects = result.Projects.Concat(defaultProject);
			}

			return result;
		}

		/// <summary>
		/// Constucts Time entry View model for time entry info.
		/// </summary>
		/// <param name="timeEntry">Time entry</param>
		/// <returns>Time EntryView</returns>
		public static TimeEntryViewModel ConstuctTimeEntryViewModel(TimeEntryInfo timeEntry)
		{
			return new TimeEntryViewModel()
			{
				ApprovalState = timeEntry.ApprovalState,
				Date = timeEntry.Date,
				Description = timeEntry.Description,
				Duration = timeEntry.Duration,
				Email = timeEntry.Email,
				EmployeeId = timeEntry.EmployeeId,
				FirstName = timeEntry.FirstName,
				IsLockSaved = timeEntry.IsLockSaved,
				LastName = timeEntry.LastName,
				ModSinceApproval = timeEntry.ModSinceApproval,
				PayClassId = timeEntry.PayClassId,
				PayClassName = timeEntry.PayClassName,
				ProjectId = timeEntry.ProjectId,
				TimeEntryId = timeEntry.TimeEntryId,
				UserId = timeEntry.UserId
			};
		}
	}
}