//------------------------------------------------------------------------------
// <copyright file="RegisterAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.ViewModels.Auth;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;

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
		[AllowAnonymous]
		public ActionResult Register(string returnUrl)
		{
			if (Request.IsAuthenticated)
			{
				return this.RedirectToLocal(returnUrl);
			}

			ViewBag.ReturnUrl = returnUrl;
			return this.View(new RegisterViewModel
			{
				ValidCountries = AppService.ValidCountries(),
				DateOfBirth = AppService.GetDayFromDateTime(DateTime.UtcNow.AddYears(-18))
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
				// generate confirm email url template
				string confirmUrl = Url.Action(ActionConstants.ConfirmEmail, ControllerConstants.Account, new { userId = "{userId}", code = "{code}" }, protocol: Request.Url.Scheme);

				// create new user in the db and get back the userId and count of invitations
				var birthdate = AppService.GetDateTimeFromDays(model.DateOfBirth);
				// TODO: we dont need invite count, we need only user id
				System.Tuple<int, int> userIDandInviteCount = await AppService.SetupNewUser(model.Email, model.FirstName, model.LastName, birthdate, model.Address, model.City, model.State, model.Country, model.PostalCode, model.PhoneNumber, model.Password, 1, confirmUrl); // TODO: Change language preference from 1 to a value grabbed from session/URL

				if (userIDandInviteCount != null)
				{
					// sign in (and set cookie)
					this.SignIn(userIDandInviteCount.Item1, model.Email);
					return this.RedirectToLocal(returnUrl);
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.UserAccountAlreadyExists, Variety.Danger));
					return this.View(model);
				}
			}

			Notifications.Add(new BootstrapAlert(Resources.Strings.WarnProblemSigningIn, Variety.Warning));
			return this.View("Error", new HandleErrorInfo(new System.Exception(Resources.Strings.StatusErrorMessage), "Account", "Register"));
		}
	}
}
