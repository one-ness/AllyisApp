using System.Threading.Tasks;
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
        /// <param name="checkedResendIds">Invitation ID</param>
        /// <param name="orgId">Invitation ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ResendInvite(string checkedResendIds, int orgId)
        {
            int[] concat = StringToIntList(checkedResendIds);
            foreach (int inviteId in concat)
            {
                var invite = await AppService.GetInvitation(inviteId);
                AppService.CheckOrgAction(Services.AppService.OrgAction.AddUserToOrganization, invite.OrganizationId);
                UserOld usr = await AppService.GetUserOldByEmailAsync(invite.Email);
                string url = usr != null ?
                            Url.Action(ActionConstants.Index, ControllerConstants.Account, null, protocol: Request.Url.Scheme) :
                            Url.Action(ActionConstants.Register, ControllerConstants.Account, null, protocol: Request.Url.Scheme);
                Notifications.Add(new BootstrapAlert(string.Format(Strings.InviteResentTo, invite.Email), Variety.Success));
                AppService.SendInviteEmail(url, invite.Email.Trim());
            }
            return RedirectToAction(ActionConstants.OrganizationInvitations, ControllerConstants.Account, new { id = orgId });
        }
    }
}