//------------------------------------------------------------------------------
// <copyright file="DeleteOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using System;
using System.Web.Mvc;

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
		/// <param name="orgId">Organization Id.</param>
		/// <returns>Redirection to account index, or an error page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteOrg(string orgId)
		{
			// Method includes permissions check
			if (OrgService.DeleteOrganization())
			{
				string notification = string.Format("{0} {1}", Resources.Controllers.Auth.Strings.YourOrg, Resources.Controllers.Auth.Strings.OrganizationDeleteNotification);
				Notifications.Add(new BootstrapAlert(notification, Variety.Success));

				// TODO: we shouldnt be hard coding http...
				string url = string.Format("http://{0}/{1}/{2}", GlobalSettings.WebRoot, ControllerConstants.Account, ActionConstants.Index);
				return this.Redirect(url);
			}

			// Permissions failed
			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Controllers.Auth.Strings.ActionUnauthorizedMessage), ControllerConstants.Account, ActionConstants.DeleteOrg));
		}
	}
}