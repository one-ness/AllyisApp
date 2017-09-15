//------------------------------------------------------------------------------
// <copyright file="DeleteOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Core.Alert;

namespace AllyisApps.Controllers
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
		public ActionResult DeleteOrg(int id)
		{
			// Method includes permissions check
			AppService.DeleteOrganization(id);
			string notification = string.Format("{0} {1}", Resources.Strings.YourOrg, Resources.Strings.OrganizationDeleteNotification);
			Notifications.Add(new BootstrapAlert(notification, Variety.Success));
			return this.RouteUserHome();
		}
	}
}