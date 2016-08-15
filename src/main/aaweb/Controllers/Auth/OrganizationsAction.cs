//------------------------------------------------------------------------------
// <copyright file="OrganizationsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Services.Account;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Get /Account/Organizations
		/// View a list of organizations.
		/// </summary>
		/// <returns>What to do.</returns>
		public ActionResult Organizations()
		{
			var model = new AccountOrgsViewModel
			{
				Organizations = AccountService.GetOrganizationsByUserId()
			};

			return this.View(model);
		}
	}
}
