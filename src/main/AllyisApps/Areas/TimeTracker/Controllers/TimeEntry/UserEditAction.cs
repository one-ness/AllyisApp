//------------------------------------------------------------------------------
// <copyright file="UserEditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
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
		/// <param name="userId">The User's Id.</param>
		/// <returns>The user edit page.</returns>
		public ActionResult UserEdit(int userId = -1)
		{
			if (userId <= 0)
			{
				userId = UserContext.UserId;
			}

			var infos = Service.GetProjectsForOrgAndUser(userId);

			if (Service.Can(Actions.CoreAction.EditProject))
			{
				return this.View(new UserEditViewModel
				{
					UserId = userId,

					UserProjects = infos.Item1,
					AllProjects = infos.Item2,
					UserName = infos.Item3
					//UserName = (userInfo == null) ? null : string.Format("{0} {1}", userInfo.FirstName, userInfo.LastName)
				});
			}

			// Permissions failure
			Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.ActionUnauthorizedMessage, Variety.Warning));
			return this.RedirectToAction(ActionConstants.Index);
		}
	}
}
