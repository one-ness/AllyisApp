//------------------------------------------------------------------------------
// <copyright file="UserEditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Areas.TimeTracker.Models;
using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// GET /TimeTracker/TimeEntry/UserEdit.
		/// </summary>
		/// <param name="userId">The User's Id.</param>
		/// <returns>The user edit page.</returns>
		public ActionResult UserEdit(int userId = -1)
		{
			if (Service.Can(Actions.CoreAction.EditProject))
			{
				UserInfo userInfo = Service.GetUserInfo(userId);
				return this.View(new UserEditViewModel
				{
					UserId = userId,
					UserInfo = userInfo,

					UserProjects = userInfo == null ? null : Service.GetProjectsByUserId(userId),
					AllProjects = userInfo == null ? null : Service.GetProjectsByOrganization(UserContext.ChosenOrganizationId),

					UserName = (userInfo == null) ? null : string.Format("{0} {1}", userInfo.FirstName, userInfo.LastName)
				});
			}

			// Permissions failure
			Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.ActionUnauthorizedMessage, Variety.Warning));
			return this.RedirectToAction(ActionConstants.Index);
		}
	}
}