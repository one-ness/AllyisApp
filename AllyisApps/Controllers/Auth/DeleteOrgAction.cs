//------------------------------------------------------------------------------
// <copyright file="DeleteOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// POST: Deletes current organization.
		/// </summary>
		/// <param name="id">Organization Id.</param>
		/// <returns>Redirection to account index, or an error page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteOrg(int id)
		{
			// Method includes permissions check
			await AppService.DeleteOrganization(id);
			string notification = string.Format("{0} {1}", Resources.Strings.YourOrg, Resources.Strings.OrganizationDeleteNotification);
			Notifications.Add(new BootstrapAlert(notification, Variety.Success));
			await Task.Delay(1);
			return this.RouteUserHome();
		}
	}
}