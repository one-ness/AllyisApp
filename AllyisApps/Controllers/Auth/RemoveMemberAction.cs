//------------------------------------------------------------------------------
// <copyright file="RemoveMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using System.Threading.Tasks;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// POST: Organization/RemoveUser.
		/// </summary>
		/// <param name="organizationId">Organization Id.</param>
		/// <param name="userId">User id.</param>
		/// <returns>Redirects to the manage org action.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		async public Task<ActionResult> RemoveMember(int organizationId, int userId)
		{
			await this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, organizationId);
			await AppService.RemoveOrganizationUser(organizationId, userId);
			Notifications.Add(new BootstrapAlert(Resources.Strings.UserDeletedSuccessfully, Variety.Success));
			return this.RedirectToAction(ActionConstants.OrganizationMembers, new { id = organizationId });
		}
	}
}