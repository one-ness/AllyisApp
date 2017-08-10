//------------------------------------------------------------------------------
// <copyright file="RemoveInvitationAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
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
		/// Removes the provided invitation from the invitations table.
		/// </summary>
		/// <param name="id">Organization's id.</param>
		/// <param name="userId">User id.</param>
		/// <returns>Redirects to the manage org action.</returns>
		[HttpPost]
		public ActionResult RemoveInvitation(int id, int userId)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditInvitation, id);
			AppService.RemoveInvitation(id, userId);
			Notifications.Add(new BootstrapAlert(Resources.Strings.InvitationDeleteNotification, Variety.Success));
			return this.RedirectToAction(ActionConstants.ManageOrg, new { id = id });
		}
	}
}
