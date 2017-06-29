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
		/// <param name="id">Organization Id.</param>
		/// <returns>Redirection to account index, or an error page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteOrg(int id)
		{
			// Method includes permissions check
			if (AppService.DeleteOrganization(id))
			{
				string notification = string.Format("{0} {1}", Resources.Strings.YourOrg, Resources.Strings.OrganizationDeleteNotification);
				Notifications.Add(new BootstrapAlert(notification, Variety.Success));
				return this.RouteUserHome();
			}

			// Permissions failed
			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Strings.ActionUnauthorizedMessage), ControllerConstants.Account, ActionConstants.DeleteOrg));
		}
	}
}
