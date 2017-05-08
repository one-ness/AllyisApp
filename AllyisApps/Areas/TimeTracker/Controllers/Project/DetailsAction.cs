//------------------------------------------------------------------------------
// <copyright file="DetailsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseController
	{
		/// <summary>
		/// GET: Project/Details.
		/// Get's details of the specified project.
		/// </summary>
		/// <param name="projectId">The project's Id.</param>
		/// <returns>The details view of the project.</returns>
		public ActionResult Details(int projectId)
		{
			var model = Service.GetProjectAsUser(projectId);

			if (Service.Can(Actions.CoreAction.EditProject) || model.IsProjectUser == true)
			{
				model.CanEditProject = Service.Can(Actions.CoreAction.EditProject);
				return this.View(model);
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
			}
		}
	}
}
