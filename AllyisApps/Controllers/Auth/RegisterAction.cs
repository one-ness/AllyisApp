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
using AllyisApps.ViewModels.Auth;

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
		/// <returns>The register view.</returns>
		[AllowAnonymous]
		public ActionResult Register(string returnUrl)
		{
			if (Request.IsAuthenticated)
			{
				return RedirectToLocal(returnUrl);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(new RegisterViewModel
			{
				ValidCountries = AppService.ValidCountries(),
				DateOfBirth = AppService.GetDayFromDateTime(DateTime.UtcNow.AddYears(-18).AddDays(-1))
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
			if (!ModelState.IsValid)
			{
				return View(model); // model error
			}

			Guid code = Guid.NewGuid();
			string confirmUrl = Url.Action(ActionConstants.ConfirmEmail, ControllerConstants.Account, new { id = code }, Request.Url?.Scheme);
			string confirmEmailSubject = string.Format(Strings.ConfirmEmailSubject, Strings.ApplicationTitle);
			string confirmEmailBody = string.Format(Strings.ConfirmEmailMessage, Strings.ApplicationTitle, confirmUrl);

			// TODO: Change language preference from 1 to a value grabbed from session/URL
			string langPreference = "en-US";

			// compute birthdate			
			var birthdate = AppService.GetDateTimeFromDays(model.DateOfBirth);

			// create new user in the db and get back the userId and count of invitations
			int userId = await AppService.SetupNewUser(model.Email, model.FirstName, model.LastName, birthdate, model.Address, model.City, model.State, model.Country, model.PostalCode, model.PhoneNumber, model.Password, langPreference, confirmEmailSubject, confirmEmailBody, code);
			if (userId > 0)
			{
				// sign in (and set cookie)
				SignIn(userId, model.Email);
				return RedirectToLocal(returnUrl);
			}

			Notifications.Add(new BootstrapAlert(Strings.UserAccountAlreadyExists, Variety.Danger));
			return View(model);
		}
	}
}
