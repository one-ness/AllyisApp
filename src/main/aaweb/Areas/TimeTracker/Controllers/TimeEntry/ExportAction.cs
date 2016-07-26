//------------------------------------------------------------------------------
// <copyright file="ExportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Areas.TimeTracker.Models;
using AllyisApps.Core;

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
		/// <param name="organizationId">The Organization's Id.</param>
		/// <param name="userId">The User's Id.</param>
		/// <param name="startingDate">The Starting date of the range (nullable).</param>
		/// <param name="endingDate">The Ending date of the range (nullable).</param>
		/// <returns>CSV export of time entries.</returns>
		public FileStreamResult Export(int organizationId, int userId, DateTime? startingDate = null, DateTime? endingDate = null)
		{
			if (userId == Convert.ToInt32(UserContext.UserId))
			{
				if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditSelf))
				{
					throw new UnauthorizedAccessException("You are not authorized to create a report.");
				}
			}
			else
			{
				if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers))
				{
					throw new UnauthorizedAccessException("You are not authorized to create reports for another user!");
				}
			}

			DataExportViewModel model = this.ConstructDataExportViewModel(organizationId, new List<int> { userId }, startingDate, endingDate);
			model.Output = PrepareCSVExport(model.Data, model.Projects);

			return this.File(model.Output.BaseStream, "text/csv", "export.csv");
		}

		/// <summary>
		/// Uses services to initialize a new instance of the <see cref="DataExportViewModel" /> class and returns it.
		/// </summary>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="userId">An array of user ids.</param>
		/// <param name="startingDate">The starting of the date range (nullable).</param>
		/// <param name="endingDate">The ending of the date range (nullable).</param>
		/// <param name="projectId">The project's Id (optional).</param>
		/// <param name="customerId">The Customer's Id (optional).</param>
		/// <returns>The DataExportViewModel.</returns>
		public DataExportViewModel ConstructDataExportViewModel(int organizationId, List<int> userId = null, DateTime? startingDate = null, DateTime? endingDate = null, int projectId = 0, int customerId = 0)
		{
			DataExportViewModel result = new DataExportViewModel();
			if ((userId == null) || (userId[0] == -1))
			{
				result.Data = TimeTrackerService.GetTimeEntriesOverDateRange(organizationId, startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddDays(-1));
			}
			else
			{
				result.Data = TimeTrackerService.GetTimeEntriesByUserOverDateRange(userId, organizationId, startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddDays(-1));
			}

			if (projectId != 0)
			{
				result.Data = from i in result.Data
							  where i.ProjectId == projectId
							  select i;
			}
			else if (customerId != 0)
			{
				IEnumerable<int> projects = ProjectService.GetProjectsByCustomer(customerId).Select(proj => proj.ProjectId).ToList();
				result.Data = from c in result.Data
							  where projects.Contains(c.ProjectId)
							  select c;
			}

			if (userId != null && userId.Count > 0)
			{
				if ((userId.Count > 1) || (userId[0] == -1))
				{
					result.Projects = OrgService.GetProjectsByOrganization(organizationId);
				}
				else
				{
					// single user selected
					result.Projects = ProjectService.GetProjectsByUserAndOrganization(userId[0], organizationId, false);
				}
			}

			return result;
		}
	}
}