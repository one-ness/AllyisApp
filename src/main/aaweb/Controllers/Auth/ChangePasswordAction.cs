//------------------------------------------------------------------------------
// <copyright file="ChangePasswordAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.ViewModels;

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
			return this.View();
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
			if (!ModelState.IsValid)
			{
				return this.View(model);
			}

			if (model.NewPassword.CompareTo(model.ConfirmPassword) == 0)
			{
				if (await AccountService.ChangePassword(model.OldPassword, model.NewPassword))
				{
					Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Controllers.Auth.Strings.ChangePasswordSuccessMessage, Variety.Success));

					return this.RedirectToAction("Index");
				}
			}

			ModelState.AddModelError(string.Empty, "Incorrect Password");

			return this.View(model);
		}
	}
}
