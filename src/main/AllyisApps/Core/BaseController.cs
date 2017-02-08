//------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using AllyisApps.Core.Alert;
using AllyisApps.Filters;
using AllyisApps.Lib;
using AllyisApps.Services;

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
			string rootAndMiddle = withOutControllerAction.Substring(withOutControllerAction.IndexOf(GlobalSettings.WebRoot));
            
            //// rootAndMiddle contains just the webroot, set in WebConfig, and whatever segments were there before the controller name (e.g. language)
            string route = pController == null ? string.Empty : pAction == null ? pController : string.Format("{0}/{1}", pController, pAction);

			if (pArea != null)
			{
				route = string.Format("{0}/{1}", pArea, route);
			}

            // if no org is set for a user the default is "default" this catchs that
            // case until the default usercontext org is looked at
            string url, chosenOrg = Service.GetSubdomainById(pOrganizationId);
			//if (chosenOrg == "default")       SUBDOMAINS DISABLED - to reenable, uncomment this if/else block and the "url =..." line at the end of the else
			//{
				url = string.Format("{0}/{1}", rootAndMiddle, route);
			//}
			//else
			//{
				// Update the ChosenOrg in the database if necessary, so that the UserContext can grab the right one
				if (this.UserContext.ChosenOrganizationId != pOrganizationId)
				{
					this.Service.UpdateActiveOrganization(UserContext.UserId, pOrganizationId);
				}

				//url = string.Format("{0}.{1}/{2}", chosenOrg, rootAndMiddle, route);
			//}

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
		public UserContext GetCookieData(HttpRequestBase request)
        {
            if (request == null)
            {
                throw new NullReferenceException("Http request must not be null");
            }

            UserContext result = null;
            HttpCookie cookie = request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(cookie.Value))
                    {
                        //// decrypt and deserialize the UserContext from the cookie data
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                        result = Serializer.DeserializeFromJson<UserContext>(ticket.UserData);
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
				this.UserContext = this.GetCookieData(Request);
				if(this.UserContext != null)
				{
					this.UserContext = this.Service.PopulateUserContext(this.UserContext.UserId);
				}

				if(this.UserContext != null)
				{
					// User context successfully populated
					languageID = this.UserContext.ChosenLanguageID;
					this.Service.SetUserContext(this.UserContext);
					ViewBag.ShowOrganizationPartial = true;
				}
				else
				{
					// User context not found
					this.SignOut(Response);
					Response.Redirect(FormsAuthentication.LoginUrl);
					return;
				}

				//if (this.UserContext == null || this.Service.GetUserInfo(this.UserContext.UserId) == null)
				//{
				//	// user context not found. can't proceed, redirect to login page.
				//	this.SignOut(Response);
				//	Response.Redirect(FormsAuthentication.LoginUrl);
				//	return;
				//}

				//// Populate the User Context with database info
				//this.UserContext = this.Service.PopulateUserContext(this.UserContext.UserId);

				//languageID = this.UserContext.ChosenLanguageID;

				//// Update service user context
				//this.Service.SetUserContext(this.UserContext);

    //            ViewBag.ShowOrganizationPartial = true;
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
    }
}