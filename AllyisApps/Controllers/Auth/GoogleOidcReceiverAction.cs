﻿//------------------------------------------------------------------------------
// <copyright file="GoogleOidcReceiverAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using System;
using AllyisApps.Services.Auth;
using AllyisApps.Resources;
using AllyisApps.Services;
using System.Threading.Tasks;
using AllyisApps.Utilities;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// user returns to here after logging in to google
		/// </summary>
		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult> GoogleOidcReceiver()
		{
			string returnStr = ActionConstants.Index;

			try
			{
				// get id token
				var idtoken = this.Request.Form[OidcUtility.IdTokenKey];
				if (!string.IsNullOrWhiteSpace(idtoken))
				{
					// decode the id_token
					dynamic tokenJson = System.Web.Helpers.Json.Decode(OidcUtility.DecodeIdToken(idtoken));
					string email = null;
					if (tokenJson != null && !string.IsNullOrWhiteSpace(email = tokenJson.upn))
					{
						// unique name is available. check our database
						var user = await this.AppService.GetUser2ByEmailAsync(email);
						if (user == null)
						{
							// user doesn't exist, create the user and return to profile page
							Guid code = Guid.NewGuid();
							string confirmUrl = Url.Action(ActionConstants.ConfirmEmail, ControllerConstants.Account, new { id = code }, protocol: Request.Url.Scheme);
							string confirmEmailSubject = string.Format(Strings.ConfirmEmailSubject, Strings.ApplicationTitle);
							string confirmEmailBody = string.Format(Strings.ConfirmEmailMessage, Strings.ApplicationTitle, confirmUrl);
							string firstName = tokenJson.given_name;
							string lastName = tokenJson.family_name;
							// create new user in the db and get back the userId
							int userId = await this.AppService.SetupNewUser(email, null, firstName, lastName, code, null, null, null, null, null, null, null, null, confirmEmailSubject, confirmEmailBody, LoginProviderEnum.Microsoft);
							// set cookie and take to profile page
							SignIn(userId, email, false);
						}
						else
						{
							if (user.LoginProvider == LoginProviderEnum.Microsoft)
							{
								// user exists, with microsoft as login provider
								// set cookie and take to profile page
								SignIn(user.UserId, user.Email, false);
							}
							else if (user.LoginProvider == LoginProviderEnum.AllyisApps)
							{
								// user exists, with allyis apps as login provider
								// show error message and take to login page
								Notifications.Add(new Core.Alert.BootstrapAlert("Your login information already exists, but you used an Allyis Apps account. Please login using that account. (You can convert to an employer account in your Profile page.)", Core.Alert.Variety.Danger));
								returnStr = ActionConstants.LogOn;
							}
							else
							{
								// user exists, with a different employer as login provider
								// show error message and take to login page
								Notifications.Add(new Core.Alert.BootstrapAlert("Your login information already exists, but you used a different employer account. Please login using that account.", Core.Alert.Variety.Danger));
								returnStr = ActionConstants.LogOn;
							}
						}
					}
				}
				else
				{
					// add notifications, redirect to login url
					Notifications.Add(new Core.Alert.BootstrapAlert("Microsoft server did not return your identification information. Please close your browser, then re-launch and try again.", Core.Alert.Variety.Danger));
					returnStr = ActionConstants.LogOn;
				}
			}
			catch
			{
				// add notifications, redirect to login url
				Notifications.Add(new Core.Alert.BootstrapAlert("Unexpected error while decoding your Microsoft identification information. Please close your browser, then re-launch and try again.", Core.Alert.Variety.Danger));
				returnStr = ActionConstants.LogOn;
			}

			return RedirectToAction(returnStr);
		}
	}
}
