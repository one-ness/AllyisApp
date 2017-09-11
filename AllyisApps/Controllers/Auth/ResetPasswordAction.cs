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
		/// <param name="id">The guid for the password reset.</param>
		/// <returns>The reset password view.</returns>
		[HttpGet]
		[AllowAnonymous]
		public ActionResult ResetPassword(Guid id)
		{
			var model = new ResetPasswordViewModel();
			model.Code = id;
			return this.View(model);
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
				if (await AppService.ResetPassword(model.Code, model.Password) > 0)
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