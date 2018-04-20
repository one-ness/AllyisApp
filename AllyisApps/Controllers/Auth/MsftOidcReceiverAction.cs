//------------------------------------------------------------------------------
// <copyright file="MsftOidcReceiverAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// user returns to here after logging in to MSFT
		/// described here: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-protocols-openid-connect-code
		/// </summary>
		[AllowAnonymous]
		[HttpPost]
		public ActionResult MsftOidcReceiver()
		{
			// get id token
			var idtoken = this.Request.Form[AllyisApps.MsftOidc.IdTokenKey];
			if (!string.IsNullOrWhiteSpace(idtoken))
			{
				// decode the id_token
				dynamic tokenJson = System.Web.Helpers.Json.Decode(AllyisApps.MsftOidc.DecodeIdToken(idtoken));
				if (tokenJson != null && tokenJson.upn != null && !string.IsNullOrWhiteSpace(tokenJson.upn))
				{
					// unique name is available. check our database

				}
			}
			else
			{
				// add notifications, redirect to login url
				Notifications.Add(new Core.Alert.BootstrapAlert("Microsoft server did not return your identification information. Please close and re-launch your browser and try again.", Core.Alert.Variety.Danger));
				return RedirectToAction(ActionConstants.LogOn);
			}

			

			return null;
		}
	}
}