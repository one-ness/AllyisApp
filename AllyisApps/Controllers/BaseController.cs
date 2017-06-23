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
using System.Linq;
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
		/// product id
		/// </summary>
		public ProductIdEnum ProductId { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseController" /> class.
		/// </summary>
		public BaseController()
		{
			// init the service
			this.AppService = new AppService(GlobalSettings.SqlConnectionString);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseController"/> class with a product id.
		/// </summary>
		/// <param name="productId">Product id.</param>
		public BaseController(ProductIdEnum productId) : this()
		{
			this.ProductId = productId;
		}

		/// <summary>
		/// Gets or sets the user context of the logged in user.
		/// </summary>
		public UserContext UserContext { get; set; }

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
		/// Gets or sets the service.
		/// </summary>
		protected AppService AppService { get; set; }

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
				return this.RouteHome();
			}
		}

		/// <summary>
		/// Redirects home.
		/// </summary>
		/// <returns>The proper redirect for the product.</returns>
		public ActionResult RouteHome(int id = -1)
		{
            if (this.ProductId == ProductIdEnum.TimeTracker)
            {
                return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Home, new { subscriptionId = id, id = 1, area = "TimeTracker" });
            }
			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Home);
		}

		/// <summary>
		/// Get user context from cookie.
		/// </summary>
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

		private const string languageKey = "language";
		/// <summary>
		/// On action executing - executed before every action.
		/// </summary>
		/// <param name="filterContext">The ActionExecutingContext.</param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			// get the language id from TempData dictionary, which was set in previous request
			int languageId = 0;
			if (TempData[languageKey] != null)
			{
				languageId = (int)TempData[languageKey];
			}

			if (Request.IsAuthenticated)
			{
				// an authenticated request MUST have user context in the cookie.
				CookieData cookie = this.GetCookieData();
				if (cookie != null && cookie.UserId > 0)
				{
					this.UserContext = this.AppService.PopulateUserContext(cookie.UserId);
				}

				if (this.UserContext != null)
				{
					// user context obtained. set user's language on the thread.
					if (languageId == 0 || languageId != this.UserContext.ChosenLanguageId)
					{
						// user's language is either not set, or user has changed the language to a different one
						if (this.UserContext.ChosenLanguageId > 0)
						{
							Language language = this.AppService.GetLanguage(this.UserContext.ChosenLanguageId);
							if (language != null)
							{
								CultureInfo cInfo = CultureInfo.CreateSpecificCulture(language.CultureName);
								Thread.CurrentThread.CurrentCulture = cInfo;
								Thread.CurrentThread.CurrentUICulture = cInfo;
								ViewBag.languageName = language.LanguageName;
								languageId = language.LanguageId;
							}
						}
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

			// store language for next request
			TempData[languageKey] = languageId;
		}

		/// <summary>
		/// Serializes a CookieData.
		/// </summary>
		/// <param name="cookie">The CookieData</param>
		/// <returns>The serialized string</returns>
		public string SerializeCookie(CookieData cookie)
		{
			return Serializer.SerilalizeToJson(cookie);
		}

		/// <summary>
		/// Deserializes a CookieData
		/// </summary>
		/// <param name="serializedCookie">The serialized CookieData string</param>
		/// <returns>The CookieData</returns>
		public CookieData DeserializeCookie(string serializedCookie)
		{
			return Serializer.DeserializeFromJson<CookieData>(serializedCookie);
		}

		/// <summary>
		/// Gets the application root url
		/// </summary>
		public string ApplicationRootUrl
		{
			get
			{
				var request = HttpContext.Request;
				StringBuilder sb = new StringBuilder();
				sb.Append(request.Url.Scheme);
				sb.Append("://");
				sb.Append(request.Url.Host);
				return sb.ToString();
			}
		}
	}
}
