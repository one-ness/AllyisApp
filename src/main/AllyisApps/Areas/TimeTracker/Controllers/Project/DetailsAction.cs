//------------------------------------------------------------------------------
// <copyright file="DetailsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseProductController
	{
		/// <summary>
		/// GET: Project/Details.
		/// Get's details of the specified project.
		/// </summary>
		/// <param name="projectId">The project's Id.</param>
		/// <returns>The details view of the project.</returns>
		public ActionResult Details(int projectId)
		{   // Unless the user is a manager, the user shouldn't view a project he is not assigned to
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditProject) ||
				(ProjectService.GetUsersByProjectId(projectId).Where(x => x.UserId == UserContext.UserId).FirstOrDefault() != null))
			{
				var model = ProjectService.GetProject(projectId);
				model.CanEditProject = AuthorizationService.Can(Services.Account.Actions.CoreAction.EditProject);
				return this.View(model);
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
			}
		}
	}
}