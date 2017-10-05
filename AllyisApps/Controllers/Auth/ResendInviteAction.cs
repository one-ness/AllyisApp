using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services.Auth;

namespace AllyisApps.Controllers.Auth
{
	public partial class AccountController
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="id">Invitation ID</param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult ResendInvite(int id)
		{
			var invite = AppService.GetInvitationByID(id);
			AppService.CheckOrgAction(Services.AppService.OrgAction.AddUserToOrganization, invite.OrganizationId);
			User usr = AppService.GetUserByEmail(invite.Email);
			string url = usr != null ?
						Url.Action(ActionConstants.Index, ControllerConstants.Account, null, protocol: Request.Url.Scheme) :
						Url.Action(ActionConstants.Register, ControllerConstants.Account, null, protocol: Request.Url.Scheme);
			Notifications.Add(new BootstrapAlert(String.Format(Strings.InviteResentTo, invite.Email), Variety.Success));
			AppService.SendInviteEmail(url, invite.Email.Trim());
			return RedirectToAction(ActionConstants.ManageOrg, ControllerConstants.Account, new { id = invite.OrganizationId });
		}
	}
}