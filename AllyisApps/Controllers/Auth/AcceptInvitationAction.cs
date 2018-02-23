using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services.Auth;

namespace AllyisApps.Controllers.Auth
{
	/// <inheritdoc />
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Action that accepts an invitation to an organization.
		/// </summary>
		/// <param name="id">The invitation id.</param>
		[HttpPost]
		public async Task<ActionResult> AcceptInvitation(int id)
		{
			bool result = await AppService.AcceptUserInvitation(id);

			if (result)
			{
				Invitation invitation = await AppService.GetInvitation(id);
				//Notifications.Add(new BootstrapAlert(string.Format(Strings.AcceptInvitationSuccess, invitation.OrganizationName), Variety.Success));
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Strings.ActionUnauthorizedMessage, Variety.Warning));
			}

			return RouteUserHome();
		}
	}
}