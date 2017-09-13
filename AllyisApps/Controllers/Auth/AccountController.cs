//------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	[Authorize]
	public partial class AccountController : BaseController
	{
	}
}