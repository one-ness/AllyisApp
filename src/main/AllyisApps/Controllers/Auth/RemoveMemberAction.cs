//------------------------------------------------------------------------------
// <copyright file="RemoveMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;

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
		/// <returns>The result of this action.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RemoveMember(int userId)
		{
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				Service.RemoveOrganizationUser(UserContext.ChosenOrganizationId, userId);

				Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.UserDeletedSuccessfully, Variety.Success));

				return this.RedirectToAction(ActionConstants.Manage);
			}
			else
			{
				return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Controllers.Auth.Strings.CannotEditMembersMessage), ControllerConstants.Account, ActionConstants.RemoveUser));
			}
		}
	}
}