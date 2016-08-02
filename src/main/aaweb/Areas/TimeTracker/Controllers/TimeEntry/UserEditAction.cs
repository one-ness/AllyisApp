//------------------------------------------------------------------------------
// <copyright file="UserEditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Areas.TimeTracker.Models;
using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services.BusinessObjects;

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
		/// <param name="organizationId">The Organization's Id.</param>
		/// <param name="userId">The User's Id.</param>
		/// <returns>The user edit page.</returns>
		public ActionResult UserEdit(int organizationId = 0, int userId = -1)
		{
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditProject))
			{
				UserInfo userInfo = AccountService.GetUserInfo(userId);
				return this.View(new UserEditViewModel
				{
					OrganizationId = organizationId,
					UserId = userId,
					UserInfo = userInfo,

					UserProjects = userInfo == null ? null : ProjectService.GetProjectsByUserId(userId),
					AllProjects = userInfo == null ? null : OrgService.GetProjectsByOrganization(organizationId),

					UserName = (userInfo == null) ? null : string.Format("{0} {1}", userInfo.FirstName, userInfo.LastName)
				});
			}

			// Permissions failure
			Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.ActionUnauthorizedMessage, Variety.Warning));
			return this.RedirectToAction(ActionConstants.Index);
		}
	}
}