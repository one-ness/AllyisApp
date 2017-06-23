﻿//------------------------------------------------------------------------------
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
        /// <param name="subscriptionId">The subscription Id</param>
		/// <param name="id">Project ID</param>
		/// <returns></returns>
		public ActionResult Reactivate(int subscriptionId, string id)
		{
            int projId;
            bool parsed = System.Int32.TryParse(id, out projId);

            if (!parsed)
            {
                return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId = subscriptionId });
            }
            CompleteProjectInfo project = AppService.GetProject(projId);
			if (project != null)
			{
                if (!AppService.GetCustomer(project.CustomerId).IsActive)
                {
                    AppService.ReactivateCustomer(project.CustomerId, subscriptionId, project.OrganizationId);
                }

				if (AppService.ReactivateProject(projId, project.OrganizationId, subscriptionId))
				{
					Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", Resources.Strings.ProjectReactivateNotification, project.ProjectName), Variety.Success));
					return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId = subscriptionId });
				}

				//Permission Failed
				Notifications.Add(new BootstrapAlert(Resources.Strings.DeleteUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId = subscriptionId });
		}
	}
}
