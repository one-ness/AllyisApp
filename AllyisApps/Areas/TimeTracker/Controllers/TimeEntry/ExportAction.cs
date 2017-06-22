//------------------------------------------------------------------------------
// <copyright file="ExportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
        /// <param name="subscriptionId">The subscription Id</param>
		/// <param name="startingDate">The Starting date of the range (nullable).</param>
		/// <param name="endingDate">The Ending date of the range (nullable).</param>
		/// <returns>CSV export of time entries.</returns>
		public FileStreamResult Export(int userId, int subscriptionId, int? startingDate = null, int? endingDate = null)
		{
            int orgId = AppService.GetSubscription(subscriptionId).OrganizationId;
			if (userId == Convert.ToInt32(UserContext.UserId))
			{
				if (!AppService.Can(Actions.CoreAction.TimeTrackerEditSelf, false, orgId, subscriptionId))
				{
					throw new UnauthorizedAccessException(Resources.Strings.UnauthorizedReports);
				}
			}
			else
			{
				if (!AppService.Can(Actions.CoreAction.TimeTrackerEditOthers, false, orgId, subscriptionId))
				{
					throw new UnauthorizedAccessException(Resources.Strings.UnauthorizedReportsOtherUser);
				}
			}

			DateTime? start = startingDate.HasValue ? (DateTime?)AppService.GetDateTimeFromDays(startingDate.Value) : null;
			DateTime? end = endingDate.HasValue ? (DateTime?)AppService.GetDateTimeFromDays(endingDate.Value) : null;

			return this.File(AppService.PrepareCSVExport(new List<int> { userId }, start, end).BaseStream, "text/csv", "export.csv");
		}

		/// <summary>
		/// Uses services to initialize a new instance of the <see cref="DataExportViewModel" /> class and returns it.
		/// </summary>
        /// <param name="subscriptionId">The subscriptionId</param>
        /// <param name="orgId">The organization's Id</param>
		/// <param name="userIds">An array of user ids.</param>
		/// <param name="startingDate">The starting of the date range (nullable).</param>
		/// <param name="endingDate">The ending of the date range (nullable).</param>
		/// <param name="projectId">The project's Id (optional).</param>
		/// <param name="customerId">The Customer's Id (optional).</param>
		/// <returns>The DataExportViewModel.</returns>
		public DataExportViewModel ConstructDataExportViewModel(int subscriptionId, int orgId = -1, List<int> userIds = null, DateTime? startingDate = null, DateTime? endingDate = null, int projectId = 0, int customerId = 0)
		{
            if (-1 <= orgId) orgId = AppService.GetSubscription(subscriptionId).OrganizationId;

			DataExportViewModel result = new DataExportViewModel();
			if ((userIds == null) || (userIds[0] == -1))
			{
				result.Data = AppService.GetTimeEntriesOverDateRange(startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddDays(-1));
			}
			else
			{
				result.Data = AppService.GetTimeEntriesByUserOverDateRange(userIds, startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddDays(-1));
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
					result.Projects = AppService.GetProjectsByOrganization(orgId);
				}
				else
				{
					// single user selected
					result.Projects = AppService.GetProjectsByUserAndOrganization(userIds[0], orgId, false);
				}

				// Add default project in case there are holiday entries
				List<CompleteProjectInfo> defaultProject = new List<CompleteProjectInfo>();
				defaultProject.Add(AppService.GetProject(0));
				result.Projects = result.Projects.Concat(defaultProject);
			}

			return result;
		}
	}
}
