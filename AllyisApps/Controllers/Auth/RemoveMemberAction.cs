//------------------------------------------------------------------------------
// <copyright file="RemoveMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
using System;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// POST: Organization/RemoveUser.
		/// </summary>
		/// <param name="userId">The user's ID.</param>
        /// <param name="orgId">The Organization's ID the member is being removed from</param>
		/// <returns>The result of this action.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RemoveMember(int userId, int orgId)
		{
			if (AppService.Can(Actions.CoreAction.EditOrganization, false, orgId) && AppService.Can(Actions.CoreAction.EditOrganization))
			{
				AppService.RemoveOrganizationUser(UserContext.ChosenOrganizationId, userId);

				Notifications.Add(new BootstrapAlert(Resources.Strings.UserDeletedSuccessfully, Variety.Success));

                return this.RedirectToAction(ActionConstants.Manage, new { id = orgId });
			}
			else
			{
				return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Strings.CannotEditMembersMessage), ControllerConstants.Account, ActionConstants.RemoveUser));
			}
		}
	}
}
