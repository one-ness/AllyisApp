//------------------------------------------------------------------------------
// <copyright file="DeleteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseController
	{
		/// <summary>
		/// Deletes a customer.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="userIds">Comma seperated value of project ids.  Called userIds because routing.</param>
		/// <returns>Action result to the Index page.</returns>
		[HttpGet]
		public async Task<ActionResult> Delete(int subscriptionId, string userIds)
		{
			int[] projectIds = userIds.Split(',').Select(id => Convert.ToInt32(id)).ToArray();
			var orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

			foreach (var id in projectIds)
			{
				string projectName = AppService.GetProject(id).ProjectName;
				try
				{
					var result = await AppService.DeleteProject(id, subscriptionId);
					if (result == -1)
					{
						Notifications.Add(new BootstrapAlert(
							$"Project \"{projectName}\" cannot be deleted because time entries have already been made for this project",
							Variety.Danger));
					}
					else
					{
						Notifications.Add(new BootstrapAlert($"Project {projectName} successfully deleted", Variety.Success));
					}
				}
				catch
				{
					Notifications.Add(new BootstrapAlert($"Cannot Delete Project {projectName}, there are dependent time entries or users.", Variety.Warning));
				}
			}

			return RedirectToAction(ActionConstants.Index, new { subscriptionId });
		}

		/*
		/// <summary>
		/// Deletes the project.
		/// </summary>
		/// <param name="id">The project's Id.</param>
		/// <returns>Deletes the project from the database.</returns>
		public ActionResult Delete(int id)
		{
			CompleteProject project = AppService.GetProject(id);

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