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
		/// Action that accepts an invitation to an organization.
		/// </summary>
		/// <param name="invitationId">The id of the accepted invitation.</param>
		/// <returns>The action result.</returns>
		[HttpPost]
		public ActionResult AcceptInvitation(int invitationId)
		{
			string result = AppService.AcceptUserInvitation(invitationId);

			if (result == null)
			{
				Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			}
			else
			{
				Notifications.Add(new Core.Alert.BootstrapAlert(result, Core.Alert.Variety.Success));
			}

			return this.RouteUserHome();
		}
	}
}
