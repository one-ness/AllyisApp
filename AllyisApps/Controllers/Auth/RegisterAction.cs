﻿//------------------------------------------------------------------------------
// <copyright file="RegisterAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
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
				return this.RedirectToLocal(returnUrl);
			}

			ViewBag.ReturnUrl = returnUrl;
			var model = new RegisterViewModel();
			model.DateOfBirth = AppService.GetDayFromDateTime(DateTime.UtcNow.AddYears(-18).AddDays(-1));
			model.LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService);

			return this.View(model);
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

				// compute birthdate
				var birthdate = AppService.GetDateTimeFromDays(model.DateOfBirth);

				// create new user in the db and get back the userId and count of invitations
				int userId = await AppService.SetupNewUser(model.Email, model.Password, model.FirstName, model.LastName, code, birthdate, model.PhoneNumber, model.Address, null, model.City, model.SelectedStateId, model.PostalCode, model.SelectedCountryCode, confirmEmailSubject, confirmEmailBody);
				if (userId > 0)
				{
					// sign in (and set cookie) do not set cookie need to confirm email
					// this.SignIn(userId, model.Email);
					Notifications.Add(new BootstrapAlert(Strings.RegistationSucessful, Variety.Success));
					return this.RedirectToLocal(returnUrl);
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Strings.UserAccountAlreadyExists, Variety.Danger));
				}
			}

			// model error
			model.LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService);
			model.LocalizedStates = ModelHelper.GetLocalizedStates(this.AppService, model.SelectedCountryCode);
			return View(model);
		}
	}
}