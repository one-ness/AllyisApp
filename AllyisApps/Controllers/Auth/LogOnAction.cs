﻿//------------------------------------------------------------------------------
// <copyright file="LogOnAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/LogOn.
		/// </summary>
		/// <param name="returnURL">The URL the user wishes to visit.</param>
		/// <returns>The action's result.</returns>
		[AllowAnonymous]
		public ActionResult LogOn(string returnURL)
		{
			if (Request.IsAuthenticated)
			{
				return this.RouteHome();
			}

			ViewBag.ReturnURL = returnURL;
			return this.View();
		}

		/// <summary>
		/// POST: /Account/LogOn.
		/// </summary>
		/// <param name="model">The Log On view model.</param>
		/// <param name="returnUrl">The URL the user wishes to visit.</param>
		/// <returns>The action's result.</returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult LogOn(LogOnViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				UserContext result = null;
				if ((result = AppService.ValidateLogin(model.Email, model.Password)) != null)
				{
					// sign in
					this.SignIn(result.UserId, result.UserName, result.Email, Response, model.RememberMe);

					this.UserContext = this.AppService.PopulateUserContext(result.UserId);

					// todo: redirect to the last chosen subscription
					return this.RouteHome();
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.SignInFailureMessage, Variety.Danger));
				}
			}

			// login failed
			return this.View(model);
		}

		/// <summary>
		/// Sign in the given user.
		/// </summary>
		/// <param name="userId">The user ID.</param>
		/// <param name="userName">The user name.</param>
		/// <param name="email">The user's email.</param>
		/// <param name="response">The Response object passed in from a controller.</param>
		/// <param name="isPersisted">Whether to set a persistent cookie or not (i.e. whether "Remember Me" is checked).</param>
		public void SignIn(
			int userId,
			string userName,
			string email,
			HttpResponseBase response,
			bool isPersisted = false)
		{
			#region Validation

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User ID cannot be 0 or negative.");
			}

			if (string.IsNullOrEmpty(userName))
			{
				throw new ArgumentException("User name must have a value.");
			}

			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException("email", "Email address must have a value.");
			}
			else if (!AppService.IsEmailAddressValid(email))
			{
				throw new FormatException("Email address must be in a valid format.");
			}

			#endregion Validation

			UserContext context = new UserContext(userId, userName, email);
			this.SetAuthCookie(context, response, isPersisted);
		}

		/// <summary>
		/// Serialize the given CookieData object and set it to auth cookie
		/// - forms authentication module will have its own cookie, and set the given information to HttpContext.User object for each request, which will
		/// -   make the Request.IsAuthenticated to true
		/// - sample code here: https://msdn.microsoft.com/en-us/library/system.web.security.formsauthentication.encrypt(v=vs.110).aspx .
		/// </summary>
		/// <param name="context">The UserContext.</param>
		/// <param name="response">The Response object passed in from a controller.</param>
		/// <param name="isPersisted">Whether to set a persistent cookie or not.</param>
		private void SetAuthCookie(UserContext context, HttpResponseBase response, bool isPersisted = false)
		{
			//// serialize the cookie data object, then ecnrypt it using formsauthentication module
			string serialized = this.SerializeCookie(this.GetCookieDataFromUserContext(context));
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
				/*AuthenticationTicketVersion*/1,
				context.UserName,
				DateTime.UtcNow,
				DateTime.UtcNow.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
				isPersisted,
				serialized);
			string encryptedTicket = FormsAuthentication.Encrypt(ticket);

			//// create the cookie (not set in response yet) and set its value
			HttpCookie cookie = FormsAuthentication.GetAuthCookie(FormsAuthentication.FormsCookieName, isPersisted);
			cookie.HttpOnly = true;
			cookie.Value = encryptedTicket;

			//// set the cookie to response
			response.Cookies.Add(cookie);
		}
	}
}
