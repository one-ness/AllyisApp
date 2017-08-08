//------------------------------------------------------------------------------
// <copyright file="InviteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Removes the provided invitation from the invitations table.
		/// <paramref name="id"/>Organization's id.
		/// </summary>
		[HttpPost]
		public ActionResult RemoveInvitation(int id, int userId)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.DeleteInvitation, id);
			AppService.RemoveInvitation(id, userId);
			Notifications.Add(new BootstrapAlert(Resources.Strings.InvitationDeleteNotification, Variety.Success));
			return this.RedirectToAction(ActionConstants.ManageOrg, new { id = id });
		}
	}
}
