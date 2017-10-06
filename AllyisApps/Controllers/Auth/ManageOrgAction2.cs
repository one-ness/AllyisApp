//------------------------------------------------------------------------------
// <copyright file="ManageOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Get: Account/Manage/id
		/// The management page for an organization, displays billing, subscriptions, etc.
		/// </summary>
		/// <param name="id">The organization Id.</param>
		/// <returns>The organization's management page.</returns>
		public ActionResult ManageOrg2(int id)
		{
			return View();
		}
	}
}
