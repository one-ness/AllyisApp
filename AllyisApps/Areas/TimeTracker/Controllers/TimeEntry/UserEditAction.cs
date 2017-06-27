//------------------------------------------------------------------------------
// <copyright file="UserEditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
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
        /// <param name="subscriptionId">The subscription's id</param>
		/// <param name="userId">The User's Id.</param>
		/// <returns>The user edit page.</returns>
		public ActionResult UserEdit(int subscriptionId, int userId = -1)
		{
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			var infos = AppService.GetProjectsForOrgAndUser(userId, subscriptionId);
			return this.View(new UserEditViewModel {
				UserId = this.UserContext.UserId,
				SubscriptionId = subscriptionId,
				UserProjects = infos.Item1,
				AllProjects = infos.Item2,
				UserName = infos.Item3
			});
		}
	}
}
