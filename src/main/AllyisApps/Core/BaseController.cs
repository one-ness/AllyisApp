﻿//------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Filters;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AllyisApps.Core
{
	/// <summary>
	/// Common controller base class.
	/// </summary>
	[NotificationFilter]
	public partial class BaseController : Controller
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseController" /> class.
		/// </summary>
		public BaseController()
		{
			// init the service
			this.Service = new Service(GlobalSettings.SqlConnectionString);
			this.TimeTrackerService = new TimeTrackerService(GlobalSettings.SqlConnectionString);
			this.TimeTrackerService.SetService(this.Service);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseController"/> class with a product id.
		/// </summary>
		/// <param name="productId">Product id.</param>
		public BaseController(int productId) : this()
		{
			this.cProductId = productId;
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
		protected Service Service { get; set; }

		/// <summary>
		///
		/// </summary>
		protected TimeTrackerService TimeTrackerService { get; set; }

		// Product id used by controllers in product areas. 0 by default, for no product.
		private readonly int cProductId = 0;

		/// <summary>
		/// Helper method for redirecting to an action in a subdomain.
		/// </summary>
		/// <param name="pOrganizationId">The id of the organization.</param>
		/// <param name="pArea">The target area.</param>
		/// <param name="pAction">The target action.</param>
		/// <param name="pController">The target controller.</param>
		/// <returns>Redirects to the Url defined above with new subdomain.</returns>
		public ActionResult RedirectToSubDomainAction(int pOrganizationId, string pArea = null, string pAction = null, string pController = null)
		{
			string requestUrl = Request.Url.ToString();
			var routeData = Request.RequestContext.RouteData;

			// Check for presence of area - this is a little messy, but areas are suprisingly hard to detect
			int indexOfArea = -1;
			if (routeData.Route.GetType() == typeof(SubdomainRoute))
			{
				string area = ((SubdomainRoute)routeData.Route).Area;
				indexOfArea = area == null ? -1 : requestUrl.IndexOf(area);
			}

			int indexOfController = requestUrl.IndexOf(routeData.Values["controller"].ToString());
			string withOutControllerAction = indexOfArea > -1 ? requestUrl.Substring(0, indexOfArea) : indexOfController > -1 ? requestUrl.Substring(0, indexOfController) : requestUrl;
			//string rootAndMiddle = withOutControllerAction.Substring(withOutControllerAction.IndexOf(GlobalSettings.WebRoot));
			string rootAndMiddle = withOutControllerAction.Substring(withOutControllerAction.IndexOf("//") + 2);

			//// rootAndMiddle contains just the webroot, set in WebConfig, and whatever segments were there before the controller name (e.g. language)
			string route = pController == null ? string.Empty : pAction == null ? pController : string.Format("{0}/{1}", pController, pAction);

			if (pArea != null)
			{
				route = string.Format("{0}/{1}", pArea, route);
			}

			if (this.UserContext != null && this.UserContext.ChosenOrganizationId != pOrganizationId)
			{
				this.Service.UpdateActiveOrganization(UserContext.UserId, pOrganizationId);
			}

			string url = string.Format("{0}/{1}", rootAndMiddle, route);
			if (GlobalSettings.useSubdomains)
			{
				string chosenOrg = Service.GetSubdomainById(pOrganizationId);
				if (!chosenOrg.Equals("default"))
				{
					url = string.Format("{0}.{1}", chosenOrg, url);
				}
			}

			// Any other miscellaneous route parameters need to remain in the query string
			string remainingQueryParameters = string.Empty;
			foreach (string key in Request.QueryString.AllKeys)
			{
				if (!key.Equals("pOrganizationId") && !key.Equals("pArea") && !key.Equals("pAction") && !key.Equals("pController"))
				{
					remainingQueryParameters = string.Format("{0}&{1}={2}", remainingQueryParameters, key, Request.QueryString[key]);
				}
			}

			if (!remainingQueryParameters.Equals(string.Empty))
			{
				remainingQueryParameters = string.Format("?{0}", remainingQueryParameters.Substring(1));
			}

			string finalUrl = (url + remainingQueryParameters).Replace("//", "/");

			return this.Redirect("http://" + finalUrl);
		}

		/// <summary>
		/// Redirects home.
		/// </summary>
		/// <returns>The proper redirect for the product.</returns>
		public ActionResult RouteHome()
		{
			if (Request.IsAuthenticated)
			{
				return this.RedirectToSubDomainAction(UserContext.ChosenOrganizationId, null, ActionConstants.Index, ControllerConstants.Account);
			}

			return this.RedirectToAction(ActionConstants.LogOn);
		}

		/// <summary>
		/// Get user context from cookie.
		/// </summary>
		/// <param name="request">The HttpResponseBase.</param>
		/// <returns>The UserContext, or null on error.</returns>
		public CookieData GetCookieData(HttpRequestBase request)
		{
			if (request == null)
			{
				throw new NullReferenceException("Http request must not be null");
			}

			CookieData result = null;
			HttpCookie cookie = request.Cookies[FormsAuthentication.FormsCookieName];
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
		/// <param name="response">The Response object passed in from a controller.</param>
		public void SignOut(HttpResponseBase response)
		{
			FormsAuthentication.SignOut();

			// TODO: check if this action is really required. Sometimes the SignOut call above is not deleting the cookie
			//// get the cookie, set expire time in the past, and set it in response to delete it
			HttpCookie cookie = FormsAuthentication.GetAuthCookie(FormsAuthentication.FormsCookieName, false);
			cookie.Expires = DateTime.UtcNow.AddDays(-5);
			response.Cookies.Add(cookie);
		}

		/// <summary>
		/// On action executing - executed before every action.
		/// </summary>
		/// <param name="filterContext">The ActionExecutingContext.</param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			int languageID = 0;
			const string TempDataKey = "language";

			if (Request.IsAuthenticated)
			{
				// an authenticated request MUST have user context in the cookie.
				CookieData cookie = this.GetCookieData(Request);
				if (cookie != null && cookie.userId > 0)
				{
					this.UserContext = this.Service.PopulateUserContext(cookie.userId);
				}

				if (this.UserContext != null)
				{
					// User context successfully populated
					this.TimeTrackerService.SetUserContext(this.UserContext);
					languageID = this.UserContext.ChosenLanguageID;
					ViewBag.ShowOrganizationPartial = true;

					// Update Chosen Subscription if we are in a product area
					if (cProductId > 0)
					{
						UserOrganizationInfo org = this.UserContext.UserOrganizationInfoList.Where(o => o.OrganizationId == this.UserContext.ChosenOrganizationId).SingleOrDefault();
						if (org != null)
						{
							UserSubscriptionInfo sub = org.UserSubscriptionInfoList.Where(s => (int)s.ProductId == cProductId).SingleOrDefault();
							if (sub != null && this.UserContext.ChosenSubscriptionId != sub.SubscriptionId)
							{
								Service.UpdateActiveSubscription(sub.SubscriptionId == 0 ? null : (int?)sub.SubscriptionId);
								this.UserContext.ChosenSubscriptionId = sub.SubscriptionId;
							}
						}
					}
				}
				else
				{
					// User context not found
					this.SignOut(Response);
					Response.Redirect(FormsAuthentication.LoginUrl);
					return;
				}
			}
			else
			{
				if (TempData[TempDataKey] != null)
				{
					languageID = (int)TempData[TempDataKey];
					TempData[TempDataKey] = languageID; // Store it again for next request.
				}
			}

			LanguageInfo language = this.Service.GetLanguageInfo(languageID);
			if (language != null)
			{
				CultureInfo cInfo = CultureInfo.CreateSpecificCulture(language.CultureName);
				Thread.CurrentThread.CurrentCulture = cInfo;
				Thread.CurrentThread.CurrentUICulture = cInfo;
				ViewBag.languageName = language.LanguageName;
				TempData[TempDataKey] = language.LanguageID; // Store it for next request.
			}
		}

		/// <summary>
		/// Gets a CookieData object from a UserContext.
		/// </summary>
		/// <param name="context">The UserContext to use. (If null, the current context is used.)</param>
		/// <returns>A CookieData for that UserContext.</returns>
		public CookieData GetCookieDataFromUserContext(UserContext context = null)
		{
			UserContext contextToUse;
			if (context == null)
			{
				contextToUse = this.UserContext;
			}
			else
			{
				contextToUse = context;
			}

			return new CookieData
			{
				userId = contextToUse.UserId
			};
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
	}
}
