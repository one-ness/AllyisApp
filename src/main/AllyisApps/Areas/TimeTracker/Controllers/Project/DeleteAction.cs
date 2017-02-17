//------------------------------------------------------------------------------
// <copyright file="DeleteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController: BaseController
	{
		/// <summary>
		/// Deletes the project.
		/// </summary>
		/// <param name="id">The project's Id.</param>
		/// <returns>Deletes the project from the database.</returns>
		public ActionResult Delete(int id)
		{
            CompleteProjectInfo project = Service.GetProject(id);

            if (project != null)
            {
                if (Service.DeleteProject(id))
                {
                    Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", project.ProjectName, Resources.TimeTracker.Controllers.Project.Strings.ProjectDeleteNotification), Variety.Success));
                    return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
                }

                // Permissions failure
                Notifications.Add(new BootstrapAlert("You do not have permission to delete projects", Variety.Warning));
            }

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
		}
	}
}