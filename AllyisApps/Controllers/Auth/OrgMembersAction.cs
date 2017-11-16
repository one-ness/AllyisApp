//------------------------------------------------------------------------------
// <copyright file="OrgMembersAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Get: Account/OrgMembers
		/// </summary>
		public async Task<ActionResult> OrgMembers(int id)
		{
			var model = new OrganizationMembersViewModel();
			model.CanAddUser = AppService.CheckOrgAction(AppService.OrgAction.AddUserToOrganization, id, false);
			model.CanDeleteUser = AppService.CheckOrgAction(AppService.OrgAction.DeleteUserFromOrganization, id, false);
			model.CanEditUser = AppService.CheckOrgAction(AppService.OrgAction.EditUser, id, false);
			model.CanManagePermissions = AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id, false);
			model.OrganizationId = id;
			model.TabInfo.OrganizationId = id;
			var collection = await AppService.GetOrganizationUsersAsync(id);
			foreach (var item in collection)
			{
				var roles = await AppService.GetProductRolesAsync(id, Services.Billing.ProductIdEnum.AllyisApps);
				var role = roles.FirstOrDefault(x => x.ProductRoleId == item.OrganizationRoleId);
				var data = new OrganizationMembersViewModel.ViewModelItem
				{
					Email = item.Email,
					EmployeeId = item.EmployeeId,
					JoinedDate = item.OrganizationUserCreatedUtc,
					RoleName = role?.ProductRoleName ?? string.Empty,
					UserId = item.UserId,
					Username = $"{item.FirstName} {item.LastName}"
				};

				model.Users.Add(data);
			}

			var org = await AppService.GetOrganization(id);
			model.OrganizationName = org.OrganizationName;

			return View(model);
		}
	}
}