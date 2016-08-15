//------------------------------------------------------------------------------
// <copyright file="UpdateProjectAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;

using AllyisApps.Core;
using AllyisApps.ViewModels;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseProductController
	{
		/// <summary>
		/// Updates a project.
		/// </summary>
		/// <param name="project">The model containing the project information.</param>
		public void UpdateProject(EditProjectViewModel project)
		{
			if (string.IsNullOrWhiteSpace(project.ProjectName))
			{
				throw new InvalidOperationException("Project requires a name. Cannot update.");
			}

			if (project.OrganizationId == null)
			{
				if (UserContext.ChosenOrganizationId == 0)
				{
					throw new InvalidOperationException("Project requires an organization. Cannot update.");
				}

				project.OrganizationId = UserContext.ChosenOrganizationId;
			}

			// Update ProjectUser table
			var oldUserList = ProjectService.GetUsersByProjectId(project.ProjectId);
			foreach (var id in project.SelectedProjectUserIds.Where(user => !oldUserList.Any(oldUser => (int.Parse(user) == oldUser.UserId))))
			{
				// For each new user that is not included in the old user list
				ProjectService.CreateProjectUser(project.ProjectId, int.Parse(id));
			}

			foreach (var user in oldUserList.Where(oldUser => !project.SelectedProjectUserIds.Any(user => (int.Parse(user) == oldUser.UserId))))
			{
				// For each old user that is not included in the new user list
				ProjectService.DeleteProjectUser(project.ProjectId, user.UserId);
			}

			// Update project table
			ProjectService.UpdateProject(
				project.ProjectId, 
				project.ProjectName, 
				project.PriceType, 
				TimeTrackerService.GetDateTimeFromDays(project.StartDate), 
				TimeTrackerService.GetDateTimeFromDays(project.EndDate));
		}
	}
}