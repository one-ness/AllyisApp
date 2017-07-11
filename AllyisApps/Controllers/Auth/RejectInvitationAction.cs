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
		/// Action that rejects an invitation to an organization.
		/// </summary>
		/// <param name="invitationId">The id of the accepted invitation.</param>
		/// <returns>The Action result.</returns>
		[HttpPost]
		public ActionResult RejectInvitation(int invitationId)
		{
			string result = AppService.RejectUserInvitation(invitationId);
			if (result != null)
			{
				// Validate that the user does have the requested pending invitation
				Notifications.Add(new Core.Alert.BootstrapAlert(result, Core.Alert.Variety.Success));
			}
			else
			{
				// Not a part of the invitation
				Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			}

			return RedirectToAction(ActionConstants.Index);
		}
	}
}
