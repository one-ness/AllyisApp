//------------------------------------------------------------------------------
// <copyright file="ConvertToEmployerAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.ViewModels.Auth;

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
			var model = new ConvertToEmployerViewModel();
			model.Email = this.AppService.UserContext.Email;
			return View(model);
		}
	}
}
