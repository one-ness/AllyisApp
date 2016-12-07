//------------------------------------------------------------------------------
// <copyright file="LogOnAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using AllyisApps.Core;
using AllyisApps.Lib;
using AllyisApps.Services.Account;
using AllyisApps.ViewModels;

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
				return this.HandleRedirects(returnURL);
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
		public async Task<ActionResult> LogOn(LogOnViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				UserContext result = null;
				if ((result = await AccountService.ValidateLogin(model.Email, model.Password)) != null)
				{
					// sign in
					this.SignIn(result.UserId, result.UserName, result.Email, Response, model.RememberMe);

					this.UserContext = this.AccountService.PopulateUserContext(result.UserId);

					return this.HandleRedirects(returnUrl);
				}
				else
				{
					ModelState.AddModelError(string.Empty, @Resources.Errors.SignInFailureMessage);
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
            else if (!AccountService.IsEmailAddressValid(email))
            {
                throw new FormatException("Email address must be in a valid format.");
            }
            #endregion Validation

            UserContext context = new UserContext(userId, userName, email);
            this.SetAuthCookie(context, response, isPersisted);
        }

        private ActionResult HandleRedirects(string returnURL)
		{
			// take user to return url, if supplied
			if ((returnURL != null) && (string.Empty != returnURL))
			{
				return this.RedirectToLocal(returnURL);
			}
			else if (this.UserContext != null)
			{
				// if chosen subscription is in the current organization, go to the subscription, else go to the organization page.
				int subscriptionID = this.UserContext.ChosenSubscriptionId;
				int organizationID = this.UserContext.ChosenOrganizationId;

				bool subIsInOrg = this.UserContext.UserOrganizationInfoList.Find(
					x => x.UserSubscriptionInfoList.Any(
					y => y.SubscriptionId == subscriptionID)) != null;

				if (subIsInOrg)
				{
					string area = CrmService.GetProductNameBySubscriptionID(subscriptionID);
					return this.RedirectToSubDomainAction(organizationID, area);
				}

				// sub is not in the org or is not set, checking if the user is a member of only one sub to send them to instead
				if (this.UserContext.UserOrganizationInfoList.Count > 0)
				{
					if (this.UserContext.UserOrganizationInfoList.Count == 1)
					{
						// have only one organization, check for only 1 sub
						if (this.UserContext.UserOrganizationInfoList.First().UserSubscriptionInfoList.Count == 0 ||
							this.UserContext.UserOrganizationInfoList.First().UserSubscriptionInfoList.Count > 1)
						{
							// no subs or multiple subs, redirect to subscription management
							this.UserContext.ChosenOrganizationId = this.UserContext.UserOrganizationInfoList.First().OrganizationId;
							return this.RedirectToSubDomainAction(this.UserContext.UserOrganizationInfoList.First().OrganizationId, null, ActionConstants.Manage, ControllerConstants.Account);
						}
						else
						{
							string area = CrmService.GetProductNameBySubscriptionID(this.UserContext.UserOrganizationInfoList.First().UserSubscriptionInfoList.First().SubscriptionId);
							return this.RedirectToSubDomainAction(this.UserContext.UserOrganizationInfoList.First().OrganizationId, area);
						}
					}
					else
					{
						// They have multiple Organizations, check if any of them match the chosenID
						var org = this.UserContext.UserOrganizationInfoList.Find(x => x.OrganizationId == this.UserContext.ChosenOrganizationId);
						if (org != null)
						{
							return this.RedirectToSubDomainAction(org.OrganizationId, null, ActionConstants.Manage, ControllerConstants.Account);
						}
						else
						{
							// otherwise send them to select an org
							return this.RedirectToAction(ActionConstants.Organizations, ControllerConstants.Account);
						}
					}
				}
				else
				{
					// They must not have an org.  Just redirect to account page so they can create one.
					return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Account);
				}
			}
			else
			{
				// well, that's awkward. just take the user back to the home page.
				return this.RedirectToLocal();
			}
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
            string serialized = Serializer.SerilalizeToJson(context);
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