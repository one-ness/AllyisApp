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
		async public Task<ActionResult> OrgMembers(int id)
		{
			var model = new OrganizationMembersViewModel2();
			model.CanAddUser = this.AppService.CheckOrgAction(AppService.OrgAction.AddUserToOrganization, id, false);
			model.CanDeleteUser = this.AppService.CheckOrgAction(AppService.OrgAction.DeleteUserFromOrganization, id, false);
			model.CanEditUser = this.AppService.CheckOrgAction(AppService.OrgAction.EditUser, id, false);
			model.CanManagePermissions = this.AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id, false);
			model.OrganizationId = id;
			model.TabInfo.OrganizationId = id;
			var collection = await this.AppService.GetOrganizationUsersAsync(id);
			foreach (var item in collection)
			{
				var roles = await this.AppService.GetProductRolesAsync(id, Services.Billing.ProductIdEnum.AllyisApps);
				var role = roles.Where(x => x.ProductRoleId == item.OrganizationRoleId).FirstOrDefault();
				var data = new OrganizationMembersViewModel2.ViewModelItem
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

			var org = await this.AppService.GetOrganization(id);
			model.OrganizationName = org.OrganizationName;

			return View(model);
		}
	}
}