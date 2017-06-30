//------------------------------------------------------------------------------
// <copyright file="ResetPasswordAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.ViewModels.Auth;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/ResetPassword.
		/// </summary>
		/// <param name="userId">The provided user's Id.</param>
		/// <param name="code">The verification code.</param>
		/// <returns>The result of this action.</returns>
		[AllowAnonymous]
		public ActionResult ResetPassword(int userId, string code)
		{
			if (userId < 1 || string.IsNullOrWhiteSpace(code))
			{
				return this.View(ViewConstants.Error, new HandleErrorInfo(new ArgumentException(@Resources.Strings.ParameterErrorMessage), ControllerConstants.Account, ActionConstants.ResetPassword));
			}

			return this.View(new ResetPasswordViewModel());
		}

		/// <summary>
		/// POST: /Account/ResetPassword.
		/// </summary>
		/// <param name="model">The reset password view model.</param>
		/// <returns>The async task responsible for this action.</returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (await AppService.ResetPassword(int.Parse(model.UserId), model.Code, model.Password))
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.ResetPasswordSuccessDialogue, Variety.Success));
					return this.RedirectToAction(ActionConstants.LogOn, ControllerConstants.Account);
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.PasswordResetError, Variety.Danger));
					return this.RedirectToAction(ActionConstants.ForgotPassword, ControllerConstants.Account);
				}
			}

			Notifications.Add(new BootstrapAlert(Resources.Strings.IncorrectPassword, Variety.Danger));
			return this.View(model);
		}

		/// <summary>
		/// GET: /Account/ResetPasswordConfirmation.
		/// </summary>
		/// <returns>The result of this action.</returns>
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation()
		{
			return this.View();
		}
	}
}
