//------------------------------------------------------------------------------
// <copyright file="UserEditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// GET /TimeTracker/TimeEntry/UserEdit.
		/// </summary>
		/// <param name="subscriptionId">The subscription's id.</param>
		/// <param name="userId">The Id of the user to edit.</param>
		/// <returns>The user edit page.</returns>
		public ActionResult UserEdit(int subscriptionId, int userId)
		{
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			var infos = AppService.GetProjectsForOrgAndUser(userId, subscriptionId);
			string subscriptionNameToDisplay = AppService.UserContext.UserSubscriptions[subscriptionId].SubscriptionName;
			return this.View(new UserEditViewModel
			{
				UserId = this.AppService.UserContext.UserId,
				SubscriptionId = subscriptionId,
				UserProjects = infos.Item1,
				AllProjects = infos.Item2,
				UserName = infos.Item3,
				SubscriptionName = subscriptionNameToDisplay
			});
		}

		/// <summary>
		/// AJAX callback to update the projects for a user.
		/// </summary>
		/// <param name="userId">The Id of the user to edit.</param>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="offUser">The list of projects not associated with the user.</param>
		/// <param name="onUser">The list of projects associated with the user.</param>
		/// <returns>Json object representing the results of the action.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult UserEditAJAX(int userId, int subscriptionId, List<int> offUser, List<int> onUser)
		{
			int organizationId = AppService.UserContext.UserSubscriptions[subscriptionId].OrganizationId;
			if (this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId, false))
			{
				if (offUser != null)
				{
					foreach (int proj_id in offUser)
					{
						if (AppService.UpdateProjectUser(proj_id, userId, false).Equals(0))
						{
							AppService.DeleteProjectUser(proj_id, userId);
						}
					}
				}

				if (onUser != null)
				{
					foreach (int proj_id in onUser)
					{
						if (AppService.UpdateProjectUser(proj_id, userId, true).Equals(0))
						{
							AppService.CreateProjectUser(proj_id, userId);
						}
					}
				}

				return this.Json(new { status = "success" });
			}
			else
			{
				return this.Json(new { status = "failure" });
			}
		}
	}
}
