﻿//------------------------------------------------------------------------------
// <copyright file="ChangePasswordAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Manage/ChangePassword.
		/// </summary>
		/// <returns>The result of this action.</returns>
		public ActionResult ChangePassword()
		{
			return View(new ChangePasswordViewModel());
		}

		/// <summary>
		/// POST: /Manage/ChangePassword.
		/// </summary>
		/// <param name="model">The change password view model.</param>
		/// <returns>The async task responsible for this action.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			ActionResult result = View(model);
			if (ModelState.IsValid)
			{
				// model state is valid
				if (string.Compare(model.NewPassword, model.ConfirmPassword, true) == 0)
				{
					// passwords match
					if (await AppService.ChangePasswordAsync(model.OldPassword, model.NewPassword))
					{
						// successfully changed
						Notifications.Add(new BootstrapAlert(Resources.Strings.ChangePasswordSuccessMessage, Variety.Success));
						result = RouteUserHome();
					}
					else
					{
						// old password incorrect
						Notifications.Add(new BootstrapAlert(Resources.Strings.IncorrectPassword, Variety.Danger));
					}
				}
			}

			return result;
		}
	}
}