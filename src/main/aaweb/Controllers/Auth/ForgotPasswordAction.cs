//------------------------------------------------------------------------------
// <copyright file="ForgotPasswordAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.ViewModels;
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
		/// GET: /Account/ForgotPassword.
		/// </summary>
		/// <returns>The ActionResult.</returns>
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return this.View();
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
				PasswordResetInfo info = await AccountService.GetPasswordResetInfo(model.Email);
				if (info != null)
				{
					// user exists, reset code is generated.
					string callbackUrl = Url.Action(ActionConstants.ResetPassword, ControllerConstants.Account, new { userId = info.UserId, code = info.Code.ToString() }, protocol: Request.Url.Scheme);

					EmailService mail = new EmailService();
					string msgbody = new System.Web.HtmlString(string.Format("Please reset your password by clicking <a href=\"{0}\">here</a>", callbackUrl)).ToString();
					await mail.CreateMessage(msgbody, model.Email, "Reset password");
				}

				// irrespective of failure/success, show the confirmation
				return this.View(ViewConstants.ForgotPasswordConfirmation);
			}

			return this.View(model);
		}

		/// <summary>
		/// GET: /Account/ForgotPasswordConfirmation.
		/// </summary>
		/// <returns>The ActionResult.</returns>
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation()
		{
			return this.View();
		}
	}
}