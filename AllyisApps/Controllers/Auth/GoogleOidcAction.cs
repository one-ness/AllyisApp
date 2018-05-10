//------------------------------------------------------------------------------
// <copyright file="GoogleOidcAction.cs" company="Allyis, Inc.">
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
		/// login to google user, work or school account using open id connect protocol
		/// described here: https://developers.google.com/identity/protocols/OpenIDConnect
		/// </summary>
		[AllowAnonymous]
		public ActionResult GoogleOidc()
		{
			string returnUrl = this.Url.Action(ActionConstants.GoogleOidcReceiver, ControllerConstants.Account, null, this.Request.Url.Scheme);
			return new RedirectResult(AllyisApps.GoogleOAuth.GetGoogleOAuthLoginUrl(returnUrl));
		}
	}
}
