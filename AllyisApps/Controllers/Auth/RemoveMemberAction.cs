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
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RemoveMember(int id, int userId)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, id);
			AppService.RemoveOrganizationUser(id, userId);
			Notifications.Add(new BootstrapAlert(Resources.Strings.UserDeletedSuccessfully, Variety.Success));
			return this.RedirectToAction(ActionConstants.Manage, new { id = id });
		}
	}
}
