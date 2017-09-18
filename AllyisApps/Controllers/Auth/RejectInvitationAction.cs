using System.Web.Mvc;
using AllyisApps.Services;

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
		/// <param name="id">The id of the accepted invitation.</param>
		/// <returns>The Action result.</returns>
		[HttpPost]
		public ActionResult RejectInvitation(int id)
		{
			bool result = AppService.RejectInvitation(id);
			if (result)
			{
				// Validate that the user does have the requested pending invitation
				Notifications.Add(new Core.Alert.BootstrapAlert("The invitation has been rejected.", Core.Alert.Variety.Success));
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