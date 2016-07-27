//------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using AllyisApps.Core.Alert;
using AllyisApps.DBModel;
using AllyisApps.Services.Account;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Org;
using AllyisApps.Utilities;

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
			// init the services
			// TODO: See note near end of OnActionExecuting()
			this.AccountService = new AccountService(GlobalSettings.SqlConnectionString);
			this.CrmService = new CrmService(GlobalSettings.SqlConnectionString);
			this.OrgService = new OrgService(GlobalSettings.SqlConnectionString);
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
				const string tempDataKey = "Alerts";
				this.TempData[tempDataKey] = this.TempData[tempDataKey] ?? new List<BootstrapAlert>();
				return (ICollection<BootstrapAlert>)this.TempData[tempDataKey];
			}
		}

		/// <summary>
		/// Gets or sets the Account service.
		/// </summary>
		protected AccountService AccountService { get; set; }

		/// <summary>
		/// Gets or sets the Org service.
		/// </summary>
		protected OrgService OrgService { get; set; }

		/// <summary>
		/// Gets or sets the CRM service.
		/// </summary>
		protected CrmService CrmService { get; set; }

		/// <summary>
		/// Gets or sets the Authorization service.
		/// </summary>
		protected AuthorizationService AuthorizationService { get; set; }

		/// <summary>
		/// Helper method for redirecting to an action in a subdomain.
		/// </summary>
		/// <param name="organizationId">The id of the organization.</param>
		/// <param name="area">The target area.</param>
		/// <param name="action">The target action.</param>
		/// <param name="controller">The target controller.</param>
		/// <returns>Redirects to the Url defined above with new subdomain.</returns>
		public ActionResult RedirectToSubDomainAction(int organizationId, string area = null, string action = null, string controller = null)
		{
			string requestUrl = Request.Url.ToString();
			string withOutControllerAction = requestUrl.Substring(0, requestUrl.IndexOf(Request.RequestContext.RouteData.Values["controller"].ToString()));
			string rootAndMiddle = withOutControllerAction.Substring(withOutControllerAction.IndexOf(GlobalSettings.WebRoot));
			//// rootAndMiddle contains just the webroot, set in WebConfig, and whatever segments were there before the controller name (e.g. language)
			string route = controller == null ? string.Empty : action == null ? controller : string.Format("{0}/{1}", controller, action);

			if (area != null)
			{
				route = string.Format("{0}/{1}", area, route);
			}

			// if no org is set for a user the default is "default" this catchs that
			// case until the default usercontext org is looked at
			string chosenOrg = /*OrgService.GetSubdomainById(organizationId)*/ "default"; // Handicapped for now to test on server TODO: remove once we have dns entry for subdomains
			string url;
			if (chosenOrg == "default")
			{
				url = string.Format("http://{0}/{1}", rootAndMiddle, route);
			}
			else
			{
				url = string.Format("http://{0}.{1}/{2}", chosenOrg, rootAndMiddle, route);
			}

			return this.Redirect(url);
		}

		/// <summary>
		/// Redirects home.
		/// </summary>
		/// <returns>The proper redirect for the product.</returns>
		public ActionResult RouteHome()
		{
			if (Request.IsAuthenticated)
			{
				return this.RedirectToSubDomainAction(UserContext.ChosenOrganizationId, null, null, ControllerConstants.Home);
			}

			return this.RedirectToAction(ActionConstants.Index);
		}

		/// <summary>
		/// On action executing - executed before every action.
		/// </summary>
		/// <param name="filterContext">The ActionExecutingContext.</param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			int languageID = 0;
			if (Request.IsAuthenticated)
			{
				// an authenticated request MUST have user context in the cookie.
				this.UserContext = AccountService.GetCookieData(Request);
				if (this.UserContext == null || DBHelper.Instance.GetUserInfo(this.UserContext.UserId) == null)
				{
					// user context not found. can't proceed, redirect to login page.
					AccountService.SignOut(Response);
					Response.Redirect(FormsAuthentication.LoginUrl);
					return;
				}

				// Update the ChosenOrg in the database if necessary, so that the UserContext can grab the right one
				int chosenOrgID = 0, parsed_int;
				string orgId = this.HttpContext.Request.Params["OrganizationId"];

				// Not all actions deal with organizations; we don't want to update on unnecessary actions
				if (orgId != null)
				{
					bool parsed = int.TryParse(orgId, out parsed_int);
					if (parsed)
					{
						chosenOrgID = parsed_int;
					}

					if (chosenOrgID != 0 && UserContext.ChosenOrganizationId != chosenOrgID)
					{
						OrgService.UpdateActiveOrganization(UserContext.UserId, chosenOrgID);
					}
				}

				// Populate the User Context with database info
				this.UserContext = this.AccountService.PopulateUserContext(this.UserContext.UserId);

				languageID = this.UserContext.ChosenLanguageID;

				// init services which requires user context
				this.AuthorizationService = new AuthorizationService(GlobalSettings.SqlConnectionString, this.UserContext);
				this.CrmService.SetUserContext(this.UserContext);
				this.OrgService.SetUserContext(this.UserContext);
			}
			else
			{
				const string tempDataKey = "language";
				if (TempData[tempDataKey] != null)
				{
					languageID = (int)TempData[tempDataKey];
					TempData[tempDataKey] = languageID; // Store it again for next request.
				}
			}

			LanguageInfo language = AccountService.GetLanguageInfo(languageID);
			if (language != null)
			{
				CultureInfo cInfo = CultureInfo.CreateSpecificCulture(language.CultureName);
				Thread.CurrentThread.CurrentCulture = cInfo;
				Thread.CurrentThread.CurrentUICulture = cInfo;
				ViewBag.languageName = language.LanguageName;
			}
		}
	}
}