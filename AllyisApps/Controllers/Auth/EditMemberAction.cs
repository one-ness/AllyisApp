//------------------------------------------------------------------------------
// <copyright file="EditMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using System.Linq;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.Auth;
using System.Threading.Tasks;
using AllyisApps.Services;
using AllyisApps.ViewModels;

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
		public async Task<ActionResult> EditMember(int id, int userId)
		{
			User user = await AppService.GetUserAsync(userId); // this call makes sure that both logged in user and userId have at least one common org
			var org = user.Organizations.Where(x => x.OrganizationId == id).FirstOrDefault();
			var model = new EditMemberViewModel();
			model.CanEditMember = this.AppService.CheckOrgAction(AppService.OrgAction.EditUser, id, false);
			model.Address = user.Address?.Address1;
			model.City = user.Address?.City;
			model.CountryName = user.Address?.CountryName;
			model.DateOfBirth = user.DateOfBirth == null ? string.Empty : user.DateOfBirth.Value.ToString("d");
			model.Email = user.Email;
			model.EmployeeId = org.EmployeeId;
			model.FirstName = user.FirstName;
			model.LastName = user.LastName;
			model.OrganizationId = id;
			model.OrganizationName = org.OrganizationName;
			model.OrgRolesList = ModelHelper.GetOrgRolesList();
			model.PhoneNumber = user.PhoneNumber;
			model.PostalCode = user.Address?.PostalCode;
			//model.Roles = 
			model.SelectedOrganizationRoleId = (int)org.OrganizationRole;
			model.StateName = user.Address?.StateName;
			return View("editmember", model);
		}
	}
}