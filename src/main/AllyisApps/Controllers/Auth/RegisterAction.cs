//------------------------------------------------------------------------------
// <copyright file="RegisterAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.ViewModels;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/Register.
		/// </summary>
		/// <param name="returnUrl">Return Url.</param>
		/// <returns>The result of this action.</returns>
		[AllowAnonymous]
		public ActionResult Register(string returnUrl)
		{
			if (Request.IsAuthenticated)
			{
				// already authenticated, take user to return url
				return this.RedirectToLocal(returnUrl);
			}

			ViewBag.ReturnUrl = returnUrl;
			return this.View(new RegisterViewModel
			{
				ValidCountries = Service.ValidCountries()
			});
		}

		/// <summary>
		/// POST: /Account/Register.
		/// </summary>
		/// <param name="model">The view model for registration.</param>
		/// <param name="returnUrl">Return Url.</param>
		/// <returns>The async task responsible for this action.</returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				// create new user in the db.
				int userID = Service.SetupNewUser(model.Email, model.FirstName, model.LastName, model.DateOfBirth, model.Address, model.City, model.State, model.Country, model.PostalCode, model.PhoneNumber, model.Password, 1); // TODO: Change language preference from 1 to a value grabbed from session/URL

				if (userID > 0)
				{
					// sign in (and set cookie)
					this.SignIn(userID, model.FirstName, model.Email, Response);

					// send confirmation email
					string confirmCode = await Service.GetConfirmEmailCode(userID);
					string url = Url.Action(ActionConstants.ConfirmEmail, ControllerConstants.Account, new { userId = userID, code = confirmCode }, protocol: Request.Url.Scheme);
					await Service.SendConfirmationEmail("support@allyisapps.com", model.Email, url);

					if (Service.GetInvitationsByUser(model.Email).Count > 0)
					{
						// If the user was invited, redirect to the index page to display invitations
						return this.RedirectToAction(ActionConstants.Index);
					}
					else
					{
						// Else redirect the user to create an organization
						return this.RedirectToAction(ActionConstants.CreateOrg);
					}
				}
				else
				{
					ModelState.AddModelError("Email", "User account already exists. Click on 'Forgot Password' link to reset your password.");
					return this.View(model);
				}
			}

			Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.WarnProblemSigningIn, Variety.Warning));
			return this.View("Error", new HandleErrorInfo(new System.Exception(Resources.Controllers.Auth.Strings.StatusErrorMessage), "Account", "Register"));
		}
	}
}