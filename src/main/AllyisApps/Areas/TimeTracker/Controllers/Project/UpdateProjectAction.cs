//------------------------------------------------------------------------------
// <copyright file="UpdateProjectAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;

using AllyisApps.Core;
using AllyisApps.ViewModels.TimeTracker.Project;

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

			Service.UpdateProjectAndUsers(
				project.ProjectId,
				project.ProjectName,
				project.PriceType,
				TimeTrackerService.GetDateTimeFromDays(project.StartDate),
				TimeTrackerService.GetDateTimeFromDays(project.EndDate),
				project.SelectedProjectUserIds.Select(userIdString => int.Parse(userIdString)));
		}
	}
}