//------------------------------------------------------------------------------
// <copyright file="MsftOidcAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Core;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		const string tenantName = "common"; // we are allowing all office 365 / azure ad users to login to our app
		// Authority is the URL for authority, composed by Azure Active Directory v2 endpoint and the tenant name
		string authority = "https://login.microsoftonline.com/common/v2.0";

		/// <summary>
		/// login to microsoft work or school account using open id connect protocol
		/// described here: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-protocols-openid-connect-code
		/// https://docs.microsoft.com/en-us/azure/active-directory/develop/guidedsetups/active-directory-aspnetwebapp
		/// </summary>
		[AllowAnonymous]
		public ActionResult MsftOidc()
		{
			string returnUrl = this.Url.Action(ActionConstants.MsftOidcReceiver, ControllerConstants.Account, null, this.Request.Url.Scheme);
			return Redirect(AllyisApps.MsftOidc.GetMsftOidcLoginUrl(returnUrl));
		}
	}
}
