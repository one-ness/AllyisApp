//------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	[Authorize]
	public partial class AccountController : BaseController
	{
	}
}
