//------------------------------------------------------------------------------
// <copyright file="EditMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.Auth;
using System.Threading.Tasks;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/EditMember.
		/// </summary>
		public async Task<ActionResult> EditMember2(int id)
		{
			var model = await Task.Run(() => new EditMemberViewModel2());
			User user = await this.AppService.GetUser(id);
			return View(model);
			model.Address = user.Address?.Address1;
			model.City = user.Address?.City;
			model.Country = user.Address?.CountryName;
			model.DateOfBirth = user.DateOfBirth == null ? string.Empty : user.DateOfBirth.Value.ToString("d");
			model.Email = user.Email;
			return View("editmember", model);
		}
	}
}