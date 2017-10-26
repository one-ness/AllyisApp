using System.Threading.Tasks;
using System.Web.Mvc;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Action that accepts an invitation to an organization.
		/// </summary>
		[HttpPost]
		public async Task<ActionResult> AcceptInvitation(int id)
		{
			bool result = await AppService.AcceptUserInvitation(id);

			if (!result)
			{
				Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			}
			else
			{
				var invitation = AppService.GetInvitationById(id);

				string res = string.Format("You have successfully joined {0} in the role of {1}.");
				Notifications.Add(new Core.Alert.BootstrapAlert(res, Core.Alert.Variety.Success));
			}

			return RouteUserHome();
		}
	}
}