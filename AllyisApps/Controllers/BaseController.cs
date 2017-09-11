//------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Filters;
using AllyisApps.Lib;
using AllyisApps.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Common controller base class.
	/// </summary>
	[NotificationFilter]
	public partial class BaseController : Controller
	{
		/// <summary>
		/// Language Key.
		/// </summary>
		protected const string LanguageKey = "language";

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseController" /> class.
		/// </summary>
		public BaseController()
		{
			// init the service
			ServiceSettings settings = new ServiceSettings(GlobalSettings.SqlConnectionString, GlobalSettings.SupportEmail, GlobalSettings.SendGridApiKey);
			this.AppService = new AppService(settings);
		}

		/// <summary>
		/// Gets a list of notifications to display in the next view render.
		/// </summary>
		public ICollection<BootstrapAlert> Notifications
		{
			get
			{
				const string TempDataKey = "Alerts";
				this.TempData[TempDataKey] = this.TempData[TempDataKey] ?? new List<BootstrapAlert>();
				return (ICollection<BootstrapAlert>)this.TempData[TempDataKey];
			}
		}

		/// <summary>
		/// Gets the application root url.
		/// </summary>
		public string ApplicationRootUrl
		{
			get
			{
				var request = HttpContext.Request;
				StringBuilder sb = new StringBuilder();
				if (request.Url.IsDefaultPort)
				{
					sb.AppendFormat("{0}://{1}", request.Url.Scheme, request.Url.Host);
				}
				else
				{
					sb.AppendFormat("{0}://{1}:{2}", request.Url.Scheme, request.Url.Host, request.Url.Port);
				}

				return sb.ToString();
			}
		}

		/// <summary>
		/// Gets or sets the service.
		/// </summary>
		protected AppService AppService { get; set; }

		/// <summary>
		/// Serializes a CookieData.
		/// </summary>
		/// <param name="cookie">The CookieData.</param>
		/// <returns>The serialized string.</returns>
		public string SerializeCookie(CookieData cookie)
		{
			return Serializer.SerilalizeToJson(cookie);
		}

		/// <summary>
		/// Deserializes a CookieData.
		/// </summary>
		/// <param name="serializedCookie">The serialized CookieData string.</param>
		/// <returns>The CookieData.</returns>
		public CookieData DeserializeCookie(string serializedCookie)
		{
			return Serializer.DeserializeFromJson<CookieData>(serializedCookie);
		}

		/// <summary>
		/// Redirect to user home page or the return url.
		/// </summary>
		/// <returns>Redirection to the user home page.</returns>
		public ActionResult RouteUserHome()
		{
			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Account);
		}

		/// <summary>
		/// Get user context from cookie.
		/// </summary>
		/// <returns>The user cookie data.</returns>
		public CookieData GetCookieData()
		{
			CookieData result = null;
			HttpCookie cookie = this.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (cookie != null)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(cookie.Value))
					{
						//// decrypt and deserialize the UserContext from the cookie data
						FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
						result = this.DeserializeCookie(ticket.UserData);
					}
				}
				catch
				{
				}
			}

			return result;
		}

		/// <summary>
		/// Sign out.
		/// </summary>
		public void SignOut()
		{
			FormsAuthentication.SignOut();

			// TODO: check if this action is really required. Sometimes the SignOut call above is not deleting the cookie
			//// get the cookie, set expire time in the past, and set it in response to delete it
			HttpCookie cookie = FormsAuthentication.GetAuthCookie(FormsAuthentication.FormsCookieName, false);
			cookie.Expires = DateTime.UtcNow.AddDays(-5);
			this.Response.Cookies.Add(cookie);
		}

		/// <summary>
		/// Helper for ensuring a returnUrl is local and hasn't been tampered with.
		/// </summary>
		/// <param name="returnUrl">The returnUrl.</param>
		/// <returns>The redirection action, or a redirection to home if the url is bad.</returns>
		protected ActionResult RedirectToLocal(string returnUrl = "")
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return this.Redirect(returnUrl);
			}
			else
			{
				return this.RouteUserHome();
			}
		}

		/// <summary>
		/// On action executing - executed before every action.
		/// </summary>
		/// <param name="filterContext">The ActionExecutingContext.</param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			// get the language id from TempData dictionary, which was set in previous request
			string cultureName = Language.DefaultLanguageCultureName;
			if (TempData[LanguageKey] != null)
			{
				cultureName = ((string)TempData[LanguageKey]).Trim();
			}

			if (Request.IsAuthenticated)
			{
				// an authenticated request MUST have user id in the cookie.
				CookieData cookie = this.GetCookieData();
				if (cookie != null && cookie.UserId > 0)
				{
					this.AppService.PopulateUserContext(cookie.UserId);
				}

				if (this.AppService.UserContext != null)
				{
					// user context obtained. set user's language on the thread.
					if (string.Compare(cultureName, this.AppService.UserContext.PreferedLanguageId, true) != 0)
					{
						// change it
						cultureName = ChangeLanguage(this.AppService.UserContext.PreferedLanguageId);
					}
				}
				else
				{
					// User context not found
					this.SignOut();
					Response.Redirect(FormsAuthentication.LoginUrl);
					return;
				}
			}
			else if (string.Compare(Language.DefaultLanguageCultureName, cultureName, true) != 0)
			{
				// non logged-in user changing language
				cultureName = ChangeLanguage(cultureName);
			}

			// store language for next request
			TempData[LanguageKey] = cultureName;
			TempData.Keep(LanguageKey);
		}

		/// <summary>
		/// Change the language displayed in the App.
		/// </summary>
		/// <param name="cultureName">Name of the culture.</param>
		/// <returns>The culture name.</returns>
		private string ChangeLanguage(string cultureName)
		{
			if (string.IsNullOrEmpty(cultureName))
			{
				return string.Empty;
			}

			Language language = this.AppService.GetLanguage(cultureName);
			if (language != null)
			{
				CultureInfo cInfo = CultureInfo.CreateSpecificCulture(language.CultureName);
				Thread.CurrentThread.CurrentCulture = cInfo;
				Thread.CurrentThread.CurrentUICulture = cInfo;
				ViewBag.languageName = language.LanguageName;
			}

			return cultureName;
		}
	}
}