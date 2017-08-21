//------------------------------------------------------------------------------
// <copyright file="LogOnAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;

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
		/// <param name="returnUrl">The URL the user wishes to visit.</param>
		/// <returns>The log on view.</returns>
		[AllowAnonymous]
		public ActionResult LogOn(string returnUrl)
		{
			if (Request.IsAuthenticated)
			{
				return this.RedirectToLocal(returnUrl);
			}

			ViewBag.ReturnUrl = returnUrl;
			return this.View(new LogOnViewModel());
		}

		/// <summary>
		/// POST: /Account/LogOn.
		/// </summary>
		/// <param name="model">The Log On view model.</param>
		/// <param name="returnUrl">The URL the user wishes to visit.</param>
		/// <returns>The user's home page if successful, and the log in page if not.</returns>
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
					this.SignIn(result.UserId, result.Email, model.RememberMe);
					return this.RedirectToLocal(returnUrl);
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.SignInFailureMessage, Variety.Danger));
				}
			}

			// login failed
			ViewBag.ReturnUrl = returnUrl;
			return this.View(model);
		}

		/// <summary>
		/// Sign in the given user.
		/// </summary>
		/// <param name="userId">User id.</param>
		/// <param name="userName">User name.</param>
		/// <param name="isPersisted">Is persisted.</param>
		private void SignIn(int userId, string userName, bool isPersisted = false)
		{
			this.SetAuthCookie(userId, userName, isPersisted);
		}

		/// <summary>
		/// Serialize the given CookieData object and set it to auth cookie
		/// - forms authentication module will have its own cookie, and set the given information to HttpContext.User object for each request, which will
		/// -   make the Request.IsAuthenticated to true
		/// - sample code here: https://msdn.microsoft.com/en-us/library/system.web.security.formsauthentication.encrypt(v=vs.110).aspx .
		/// </summary>
		/// <param name="userId">User id.</param>
		/// <param name="userName">User name.</param>
		/// <param name="isPersisted">Is persisted.</param>
		private void SetAuthCookie(int userId, string userName, bool isPersisted = false)
		{
			// serialize the cookie data object, then ecnrypt it using formsauthentication module
			string serialized = this.SerializeCookie(new CookieData(userId));
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
				/*AuthenticationTicketVersion*/ 1,
				userName,
				DateTime.UtcNow,
				DateTime.UtcNow.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
				isPersisted,
				serialized);
			string encryptedTicket = FormsAuthentication.Encrypt(ticket);

			// create the cookie and set encrypted ticket as its value
			HttpCookie cookie = FormsAuthentication.GetAuthCookie(FormsAuthentication.FormsCookieName, isPersisted);
			cookie.HttpOnly = true;
			cookie.Value = encryptedTicket;

			// set the cookie to response
			this.Response.Cookies.Add(cookie);
		}
	}
}
