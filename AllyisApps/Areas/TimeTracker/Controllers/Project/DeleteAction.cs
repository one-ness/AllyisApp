//------------------------------------------------------------------------------
// <copyright file="DeleteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;

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
		/// <param name="subscriptionId">Subscription id.</param>
		/// <param name="userId">The project's Id.</param>
		/// <returns>Deletes the project from the database.</returns>
		public async Task<ActionResult> Delete(int subscriptionId, int userId)
		{
			var result = await AppService.DeleteProject(userId, subscriptionId);

			if (!string.IsNullOrEmpty(result))
			{
				// if deleted successfully
				Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", result, Resources.Strings.ProjectDeleteNotification), Variety.Success));
			}
			else if (result == null)
			{
				// Permission failure
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