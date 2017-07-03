//------------------------------------------------------------------------------
// <copyright file="ForgotPasswordAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;
using System.Text;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/ForgotPassword.
		/// </summary>
		/// <returns>The ActionResult.</returns>
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return this.View(new ForgotPasswordViewModel());
		}

		/// <summary>
		/// POST: /Account/ForgotPassword
		/// Sends email to the user with a link to reset password.
		/// </summary>
		/// <param name="model">The ForgotPasswordViewModel.</param>
		/// <returns>The actionResult.</returns>
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				// NOTE: do not check for failure, always display success message and redirect to login page
				string code = Guid.NewGuid().ToString();
				string callbackUrl = Url.Action(ActionConstants.ResetPassword, ControllerConstants.Account, null, protocol: Request.Url.Scheme);
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("{0}/{1}/{2}", callbackUrl, this.AppService.UserContext.UserId, code);
				await AppService.SendPasswordResetMessage(model.Email, code, sb.ToString());
				Notifications.Add(new Core.Alert.BootstrapAlert(string.Format("{0} {1}.", Resources.Strings.ResetEmailHasBeenSent, model.Email), Core.Alert.Variety.Success));
				return this.RedirectToAction(ActionConstants.LogOn);
			}

			return this.View(model);
		}
	}
}
