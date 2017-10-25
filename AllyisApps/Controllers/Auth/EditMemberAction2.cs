//------------------------------------------------------------------------------
// <copyright file="EditMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.Auth;

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
			EditMemberViewModel2 model = await Task.Run(() => new EditMemberViewModel2());
			User user = await AppService.GetUser(id);
			model.Address = user.Address?.Address1;
			model.City = user.Address?.City;
			model.Country = user.Address?.CountryName;
			model.DateOfBirth = user.DateOfBirth?.ToString("d") ?? string.Empty;
			model.Email = user.Email;
			return View("editmember", model);
		}
	}
}