//------------------------------------------------------------------------------
// <copyright file="RemoveInvitationAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;

namespace AllyisApps.Controllers.Auth
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
		/// <returns>Redirects to the manage org action.</returns>
		[HttpPost]
		public ActionResult RemoveInvitation(int id)
		{
			var orgId = AppService.GetInvitationByID(id).OrganizationId;
			this.AppService.CheckOrgAction(AppService.OrgAction.DeleteInvitation, orgId);
			var results = AppService.RemoveInvitation(id);

			if (results)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.InvitationDeleteNotification, Variety.Success));
				return this.RedirectToAction(ActionConstants.OrganizationInvitations, new { id = orgId });
			}
			else
			{
				Notifications.Add(new BootstrapAlert("Deleting Invitation Failed.", Variety.Warning));
				return this.RedirectToAction(ActionConstants.OrganizationInvitations, new { id = orgId });
			}
		}
	}
}