//------------------------------------------------------------------------------
// <copyright file="ConvertToGoogleAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using System.Threading.Tasks;
using AllyisApps.Services.Auth;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// convert the user's allyis apps account to google account
		/// </summary>
		public async Task<ActionResult> ConvertToGoogle()
		{
			// change login provider to microsoft and set password hash to null
			await this.AppService.ChangeLoginProviderAsync(LoginProviderEnum.Google);
			return RedirectToAction(ActionConstants.LogOff);
		}
	}
}
