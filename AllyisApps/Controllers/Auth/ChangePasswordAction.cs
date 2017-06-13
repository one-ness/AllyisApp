//------------------------------------------------------------------------------
// <copyright file="ChangePasswordAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.ViewModels.Auth;
using System.Web.Mvc;

namespace AllyisApps.Controllers
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
			ViewBag.ShowOrganizationPartial = false;
			return this.View();
		}

		/// <summary>
		/// POST: /Manage/ChangePassword.
		/// </summary>
		/// <param name="model">The change password view model.</param>
		/// <returns>The async task responsible for this action.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return this.View(model);
			}
			else if (model.NewPassword.CompareTo(model.ConfirmPassword) == 0 && AppService.ChangePassword(model.OldPassword, model.NewPassword))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.ChangePasswordSuccessMessage, Variety.Success));

				return this.RedirectToAction(ActionConstants.Index);
			}
			Notifications.Add(new BootstrapAlert(Resources.Strings.IncorrectPassword, Variety.Danger));
			return this.View(model);
		}
	}
}
