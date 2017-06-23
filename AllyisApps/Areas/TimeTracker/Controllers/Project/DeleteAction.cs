//------------------------------------------------------------------------------
// <copyright file="DeleteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using AllyisApps.Controllers;
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
        /// Deletes the project.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="id">The project's Id.</param>
        /// <returns>Deletes the project from the database.</returns>
        public ActionResult Delete(int subscriptionId, int id = 0)
        {
            var result = AppService.DeleteProject(id, subscriptionId);
            // if deleted successfully
            if (result != null && result != "")
            {
                Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", result, Resources.Strings.ProjectDeleteNotification), Variety.Success));
            }
            // Permission failure
            else if (result == null)
            {
                Notifications.Add(new BootstrapAlert(Resources.Strings.DeleteUnauthorizedMessage, Variety.Warning));
            }
            return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId = subscriptionId });
        }

        /*
        /// <summary>
        /// Deletes the project.
        /// </summary>
        /// <param name="id">The project's Id.</param>
        /// <returns>Deletes the project from the database.</returns>
        public ActionResult Delete(int id)
		{
			CompleteProjectInfo project = AppService.GetProject(id);

			if (project != null)
			{
				if (AppService.DeleteProject(id))
				{
					Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", project.ProjectName, Resources.Strings.ProjectDeleteNotification), Variety.Success));
					return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
				}

				// Permissions failure
				Notifications.Add(new BootstrapAlert(Resources.Strings.DeleteUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
		}
        */
	}
}
