//------------------------------------------------------------------------------
// <copyright file="LogOnAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using AllyisApps.Core;
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
					AccountService.SignIn(result.UserId, result.UserName, result.Email, Response, model.RememberMe, result.ChosenOrganizationId, result.ChosenSubscriptionId, result.UserOrganizationInfoList);

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
	}
}
