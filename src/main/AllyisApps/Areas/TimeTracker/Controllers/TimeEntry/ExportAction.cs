//------------------------------------------------------------------------------
// <copyright file="ExportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// Export used on TimeEntry/Index. (May be removed soon).
		/// </summary>
		/// <param name="userId">The User's Id.</param>
		/// <param name="startingDate">The Starting date of the range (nullable).</param>
		/// <param name="endingDate">The Ending date of the range (nullable).</param>
		/// <returns>CSV export of time entries.</returns>
		public FileStreamResult Export(int userId, int? startingDate = null, int? endingDate = null)
		{
            if (userId == Convert.ToInt32(UserContext.UserId))
			{
				if (!Service.Can(Actions.CoreAction.TimeTrackerEditSelf))
				{
					throw new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.UnauthorizedReports);
				}
			}
			else
			{
				if (!Service.Can(Actions.CoreAction.TimeTrackerEditOthers))
				{
					throw new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.UnauthorizedReportsOtherUser);
				}
			}

            DateTime? start = startingDate.HasValue ? (DateTime?)TimeTrackerService.GetDateTimeFromDays(startingDate.Value) : null;
            DateTime? end = endingDate.HasValue ? (DateTime?)TimeTrackerService.GetDateTimeFromDays(endingDate.Value) : null;

            return this.File(TimeTrackerService.PrepareCSVExport(new List<int> { userId }, start, end).BaseStream, "text/csv", "export.csv");
		}

		/// <summary>
		/// Uses services to initialize a new instance of the <see cref="DataExportViewModel" /> class and returns it.
		/// </summary>
		/// <param name="userIds">An array of user ids.</param>
		/// <param name="startingDate">The starting of the date range (nullable).</param>
		/// <param name="endingDate">The ending of the date range (nullable).</param>
		/// <param name="projectId">The project's Id (optional).</param>
		/// <param name="customerId">The Customer's Id (optional).</param>
		/// <returns>The DataExportViewModel.</returns>
		public DataExportViewModel ConstructDataExportViewModel(List<int> userIds = null, DateTime? startingDate = null, DateTime? endingDate = null, int projectId = 0, int customerId = 0)
		{
			DataExportViewModel result = new DataExportViewModel();
			if ((userIds == null) || (userIds[0] == -1))
			{
				result.Data = TimeTrackerService.GetTimeEntriesOverDateRange(startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddDays(-1));
			}
			else
			{
				result.Data = TimeTrackerService.GetTimeEntriesByUserOverDateRange(userIds, startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddDays(-1));
			}

			if (projectId != 0)
			{
				result.Data = from i in result.Data
							  where i.ProjectId == projectId
							  select i;
			}
			else if (customerId != 0)
			{
				IEnumerable<int> projects = Service.GetProjectsByCustomer(customerId).Select(proj => proj.ProjectId).ToList();
				result.Data = from c in result.Data
							  where projects.Contains(c.ProjectId)
							  select c;
			}

			if (userIds != null && userIds.Count > 0)
			{
				if ((userIds.Count > 1) || (userIds[0] == -1))
				{
					result.Projects = Service.GetProjectsByOrganization(UserContext.ChosenOrganizationId);
				}
				else
				{
					// single user selected
					result.Projects = Service.GetProjectsByUserAndOrganization(userIds[0], false);
				}

				// Add default project in case there are holiday entries
				List<CompleteProjectInfo> defaultProject = new List<CompleteProjectInfo>();
				defaultProject.Add(Service.GetProject(0));
				result.Projects = result.Projects.Concat(defaultProject);
			}

			return result;
		}
	}
}