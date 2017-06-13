//------------------------------------------------------------------------------
// <copyright file="ReactivateAction.cs" company="Allyis, Inc.">
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
		/// Reactivates a project
		/// </summary>
		/// <param name="id">Project ID</param>
		/// <returns></returns>
		public ActionResult Reactivate(int id)
		{
			CompleteProjectInfo project = AppService.GetProject(id);
			if (project != null)
			{

                if (!AppService.GetCustomer(project.CustomerId).IsActive)
                {
                    AppService.ReactivateCustomer(project.CustomerId);
                }

				if (AppService.ReactivateProject(id))
				{
					Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", Resources.Strings.ProjectReactivateNotification, project.ProjectName), Variety.Success));
					return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
				}

				//Permission Failed
				Notifications.Add(new BootstrapAlert(Resources.Strings.DeleteUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
		}
	}
}
