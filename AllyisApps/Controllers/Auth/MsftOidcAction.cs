//------------------------------------------------------------------------------
// <copyright file="MsftOidcAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using AllyisApps.Core.Alert;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.Auth;
using AllyisApps.Core;
using System.Net;
using System.IO;
using System.Web.Helpers;

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
		/// sample login url is given above
		/// </summary>
		[AllowAnonymous]
		public async Task<ActionResult> MsftOidc()
		{

			
		}
	}
}