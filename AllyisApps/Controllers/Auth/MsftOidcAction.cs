//------------------------------------------------------------------------------
// <copyright file="MsftOidcAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// login to microsoft work or school account using open id connect protocol
		/// described here: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-protocols-openid-connect-code
		/// https://docs.microsoft.com/en-us/azure/active-directory/develop/guidedsetups/active-directory-aspnetwebapp
		/// </summary>
		[AllowAnonymous]
		public ActionResult MsftOidc()
		{
			string returnUrl = this.Url.Action(ActionConstants.MsftOidcReceiver, ControllerConstants.Account, null, this.Request.Url.Scheme);
			return new RedirectResult(AllyisApps.MsftOidc.GetMsftOidcLoginUrl(returnUrl));
		}
	}
}
