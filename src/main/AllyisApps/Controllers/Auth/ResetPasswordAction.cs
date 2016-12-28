//------------------------------------------------------------------------------
// <copyright file="ResetPasswordAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.ViewModels.Auth;

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
		public ActionResult ResetPassword(string userId, string code)
		{
			return code == null || userId == null ? this.View(ViewConstants.Error, new HandleErrorInfo(new ArgumentException(@Resources.Errors.ParameterErrorMessage), ControllerConstants.Subscription, ActionConstants.Subscribe)) : this.View();
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
				if (await Service.ResetPassword(int.Parse(model.UserId), model.Code, model.Password))
				{
					return this.RedirectToAction(ActionConstants.ResetPasswordConfirmation, ControllerConstants.Account);
				}
			}

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