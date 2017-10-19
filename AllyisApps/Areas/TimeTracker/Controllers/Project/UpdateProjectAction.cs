//------------------------------------------------------------------------------
// <copyright file="UpdateProjectAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using AllyisApps.Controllers;
using AllyisApps.Lib;
using AllyisApps.ViewModels.TimeTracker.Project;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseController
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

			if (string.IsNullOrWhiteSpace(project.ProjectOrgId))
			{
				throw new InvalidOperationException("Project requires a project id. Cannot update.");
			}

			if (project.OrganizationId == 0)
			{
				throw new InvalidOperationException("Project requires an organization. Cannot update.");
			}

			AppService.UpdateProjectAndUsers(
				project.ProjectId,
				project.ProjectName,
				project.ProjectOrgId,
				project.StartDate != -1 ? Utility.GetNullableDateTimeFromDays(project.StartDate) : null,
				project.EndDate != -1 ? Utility.GetNullableDateTimeFromDays(project.EndDate) : null,
				project.SelectedProjectUserIds.Select(userIdString => int.Parse(userIdString)),
				project.SubscriptionId);

			// project.IsHourly; // TODO: add an isHourly parameter to update the project's isHourly column.  Currently disabled feature
		}
	}
}