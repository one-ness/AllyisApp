//------------------------------------------------------------------------------
// <copyright file="DeleteOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;

using AllyisApps.Core;
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
		/// <param name="orgId">Organization Id.</param>        
		/// <returns>Redirection to account index, or an error page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteOrg(string orgId)
		{
			// Method includes permissions check
			if (OrgService.DeleteOrganization()) 
			{
				string notification = string.Format("Your organization {0}", Resources.Controllers.Auth.Strings.OrganizationDeleteNotification);
				Notifications.Add(new BootstrapAlert(notification, Variety.Success));

				string url = string.Format("http://{0}/Account/Index", GlobalSettings.WebRoot);
				return this.Redirect(url); 
			}

			// Permissions failed
			return this.View("Error", new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Controllers.Auth.Strings.ActionUnauthorizedMessage), "Account", "DeleteOrg"));
		}
	}
}
