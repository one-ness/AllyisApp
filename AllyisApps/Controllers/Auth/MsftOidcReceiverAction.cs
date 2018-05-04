﻿//------------------------------------------------------------------------------
// <copyright file="MsftOidcReceiverAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using System;
using AllyisApps.Services.Auth;
using AllyisApps.Resources;
using AllyisApps.Services;

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
			try
			{
				// get id token
				var idtoken = this.Request.Form[AllyisApps.MsftOidc.IdTokenKey];
				if (!string.IsNullOrWhiteSpace(idtoken))
				{
					// decode the id_token
					dynamic tokenJson = System.Web.Helpers.Json.Decode(AllyisApps.MsftOidc.DecodeIdToken(idtoken));
					if (tokenJson != null && !string.IsNullOrWhiteSpace(tokenJson.upn))
					{
						// unique name is available. check our database
						var user = this.AppService.GetUser2ByEmailAsync(tokenJson.upn);
						if (user == null)
						{
							// user doesn't exist, create the user and return to profile page
							Guid code = Guid.NewGuid();
							string confirmUrl = Url.Action(ActionConstants.ConfirmEmail, ControllerConstants.Account, new { id = code }, protocol: Request.Url.Scheme);
							string confirmEmailSubject = string.Format(Strings.ConfirmEmailSubject, Strings.ApplicationTitle);
							string confirmEmailBody = string.Format(Strings.ConfirmEmailMessage, Strings.ApplicationTitle, confirmUrl);
							string firstName = 
							// create new user in the db and get back the userId and count of invitations
							int userId = this.await AppService.SetupNewUser(tokenJson.upn, null, model.FirstName, model.LastName, code, model.DateOfBirth, model.PhoneNumber, model.Address, null, model.City, model.SelectedStateId, model.PostalCode, model.SelectedCountryCode, confirmEmailSubject, confirmEmailBody);
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
								return RedirectToAction(ActionConstants.LogOn);
							}
							else
							{
								// user exists, with a different employer as login provider
								// show error message and take to login page
								Notifications.Add(new Core.Alert.BootstrapAlert("Your login information already exists, but you used a different employer account. Please login using that account.", Core.Alert.Variety.Danger));
								return RedirectToAction(ActionConstants.LogOn);
							}
						}
					}

					return null;
				}
				else
				{
					// add notifications, redirect to login url
					Notifications.Add(new Core.Alert.BootstrapAlert("Microsoft server did not return your identification information. Please close your browser, then re-launch and try again.", Core.Alert.Variety.Danger));
					return RedirectToAction(ActionConstants.LogOn);
				}
			}
			catch
			{
				// add notifications, redirect to login url
				Notifications.Add(new Core.Alert.BootstrapAlert("Unexpected error while decoding your Microsoft identification information. Please close your browser, then re-launch and try again.", Core.Alert.Variety.Danger));
				return RedirectToAction(ActionConstants.LogOn);
			}
		}
	}
}
