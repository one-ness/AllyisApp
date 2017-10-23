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
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Project;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System.Threading.Tasks;

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
		async public Task<FileStreamResult> Export(int userId, int subscriptionId, int? startingDate = null, int? endingDate = null)
		{
			int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			if (userId != this.AppService.UserContext.UserId)
			{
				this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);
			}

			DateTime? start = startingDate.HasValue ? (DateTime?)Utility.GetDateTimeFromDays(startingDate.Value) : null;
			DateTime? end = endingDate.HasValue ? (DateTime?)Utility.GetDateTimeFromDays(endingDate.Value) : null;

			var file = await AppService.PrepareCSVExport(orgId, new List<int> { userId }, start, end);
			return this.File(file.BaseStream, "text/csv", "export.csv");
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
		async public Task<DataExportViewModel> ConstructDataExportViewModel(int subscriptionId, int orgId = -1, List<int> userIds = null, DateTime? startingDate = null, DateTime? endingDate = null, int projectId = 0, int customerId = 0)
		{
			if (-1 <= orgId)
			{
				orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			}

			DataExportViewModel result = new DataExportViewModel();
			if ((userIds == null) || (userIds[0] == -1))
			{
				result.Data = AppService.GetTimeEntriesOverDateRange(orgId, startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddDays(-1))
				.AsParallel().Select(timeEntry => new TimeEntryViewModel(timeEntry)).AsEnumerable();
			}
			else
			{
				var Get = await AppService.GetTimeEntriesByUserOverDateRange(userIds, startingDate ?? DateTime.MinValue.AddYears(1754), endingDate ?? DateTime.MaxValue.AddDays(-1), orgId);
				result.Data = Get.AsParallel().Select(timeEntry => new TimeEntryViewModel(timeEntry));
			}

			if (projectId != 0)
			{
				result.Data = from i in result.Data
							  where i.ProjectId == projectId
							  select i;
			}
			else if (customerId != 0)
			{
				var projGet = await AppService.GetProjectsByCustomer(customerId);
				IEnumerable<int> projects = projGet.Select(proj => proj.ProjectId).ToList();
				result.Data = from c in result.Data
							  where projects.Contains(c.ProjectId)
							  select c;
			}

			if (userIds != null && userIds.Count > 0)
			{
				if ((userIds.Count > 1) || (userIds[0] == -1))
				{
					result.Projects = AppService.GetProjectsByOrganization(orgId).AsParallel().Select(proj =>
					new CompleteProjectViewModel(proj));
				}
				else
				{
					// single user selected
					var getProj = await AppService.GetProjectsByUserAndOrganization(userIds[0], orgId, false);
					result.Projects = getProj.AsParallel().Select(proj =>
					new CompleteProjectViewModel(proj));
				}

				// Add default project in case there are holiday entries
				//List<CompleteProjectViewModel> defaultProject = new List<CompleteProjectViewModel>();
				////defaultProject.Add(new CompleteProjectViewModel(AppService.GetProject(0)));
				//result.Projects = result.Projects.Concat(defaultProject);
			}

			return result;
		}
	}
}