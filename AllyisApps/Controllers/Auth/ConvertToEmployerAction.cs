//------------------------------------------------------------------------------
// <copyright file="ConvertToEmployerAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.ViewModels.Auth;
using AllyisApps.Services.Auth;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// convert local account to employer account
		/// described here: https://developers.google.com/identity/protocols/OpenIDConnect
		/// </summary>
		public ActionResult ConvertToEmployer()
		{
			if (this.AppService.UserContext.LoginProvider != LoginProviderEnum.AllyisApps)
			{
				// convert to employer available only for allyisapps local account
				Notifications.Add(new Core.Alert.BootstrapAlert("Your are already using an employer/school account. Convert to employer functionality is available only for Allyis Apps local accounts.", Core.Alert.Variety.Danger);
				return RedirectToAction(ActionConstants.Index);
			}

			var model = new ConvertToEmployerViewModel();
			model.Email = this.AppService.UserContext.Email;
			return View(model);
		}
	}
}
