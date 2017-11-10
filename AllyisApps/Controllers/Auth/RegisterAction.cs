//------------------------------------------------------------------------------
// <copyright file="RegisterAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/Register.
		/// </summary>
		/// <param name="returnUrl">The return url.</param>
		/// <returns>A registration view.</returns>
		[AllowAnonymous]
		public ActionResult Register(string returnUrl)
		{
			if (Request.IsAuthenticated)
			{
				return RedirectToLocal(returnUrl);
			}

			ViewBag.ReturnUrl = returnUrl;
			var model = new RegisterViewModel
			{
				DateOfBirth = DateTime.UtcNow.AddYears(-18).AddDays(-1),
				LocalizedCountries = ModelHelper.GetLocalizedCountries(AppService)
			};

			return View(model);
		}

		/// <summary>
		/// POST: /Account/Register.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="returnUrl">The return url.</param>
		/// <returns>An async registration view.</returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				Guid code = Guid.NewGuid();
				string confirmUrl = Url.Action(ActionConstants.ConfirmEmail, ControllerConstants.Account, new { id = code }, protocol: Request.Url.Scheme);
				string confirmEmailSubject = string.Format(Strings.ConfirmEmailSubject, Strings.ApplicationTitle);
				string confirmEmailBody = string.Format(Strings.ConfirmEmailMessage, Strings.ApplicationTitle, confirmUrl);

				// create new user in the db and get back the userId and count of invitations
				int userId = await AppService.SetupNewUser(model.Email, model.Password, model.FirstName, model.LastName, code, model.DateOfBirth, model.PhoneNumber, model.Address, null, model.City, model.SelectedStateId, model.PostalCode, model.SelectedCountryCode, confirmEmailSubject, confirmEmailBody);
				if (userId > 0)
				{
					// sign in (and set cookie) do not set cookie need to confirm email
					// this.SignIn(userId, model.Email);
					Notifications.Add(new BootstrapAlert(Strings.RegistationSucessful, Variety.Success));
					return RedirectToLocal(returnUrl);
				}

				Notifications.Add(new BootstrapAlert(Strings.UserAccountAlreadyExists, Variety.Danger));
			}

			// error
			model.LocalizedCountries = ModelHelper.GetLocalizedCountries(AppService);
			model.LocalizedStates = ModelHelper.GetLocalizedStates(AppService, model.SelectedCountryCode);
			return View(model);
		}
	}
}