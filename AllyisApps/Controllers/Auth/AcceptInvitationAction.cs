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
		/// Action that accepts an invitation to an organization.
		/// </summary>
		[HttpPost]
		public ActionResult AcceptInvitation(int id)
		{
			string result = this.AppService.AcceptUserInvitation(id);

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