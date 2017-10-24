//------------------------------------------------------------------------------
// <copyright file="ReactivateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Crm;
using System.Threading.Tasks;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseController
	{
		/// <summary>
		/// Reactivates a project.
		/// </summary>
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <param name="userId">Project Id.</param>
		/// <returns>A redirect to the customer index page controller action.</returns>
		async public Task<ActionResult> Reactivate(int subscriptionId, int userId)
		{
			CompleteProject project = AppService.GetProject(userId);
			if (project != null)
			{
				if (!AppService.GetCustomer(project.owningCustomer.CustomerId).IsActive.Value)
				{
					await AppService.ReactivateCustomer(project.owningCustomer.CustomerId, subscriptionId, project.OrganizationId);
				}

				if (AppService.ReactivateProject(userId, project.OrganizationId, subscriptionId))
				{
					Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", Resources.Strings.ProjectReactivateNotification, project.ProjectName), Variety.Success));
					return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId = subscriptionId });
				}

				// Permission Failed
				Notifications.Add(new BootstrapAlert(Resources.Strings.DeleteUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId = subscriptionId });
		}
	}
}