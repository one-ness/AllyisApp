//------------------------------------------------------------------------------
// <copyright file="OrgSubscriptionsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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
			var model = new OrganizationMembersViewModel2();
			model.CanAddUser = this.AppService.CheckOrgAction(AppService.OrgAction.AddUserToOrganization, id, false);
			model.CanDeleteUser = this.AppService.CheckOrgAction(AppService.OrgAction.DeleteUserFromOrganization, id, false);
			model.CanEditUser = this.AppService.CheckOrgAction(AppService.OrgAction.EditUser, id, false);
			model.CanManagePermissions = this.AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id, false);
			model.OrganizationId = id;
			var collection = await this.AppService.GetOrganizationUsersAsync(id);
			foreach (var item in collection)
			{
				var data = new OrganizationMembersViewModel2.ViewModelItem();
				data.Email = item.Email;
				data.EmployeeId = item.EmployeeId;
				data.JoinedDate = item.OrganizationUserCreatedUtc;
				var roles = await this.AppService.GetProductRolesAsync(id, Services.Billing.ProductIdEnum.AllyisApps);
				var role = roles.Where(x => x.ProductRoleId == item.OrganizationRoleId).FirstOrDefault();
				data.RoleName = role != null ? role.ProductRoleName : string.Empty;
				data.UserId = item.UserId;
				StringBuilder sb = new StringBuilder();
				sb.Append(item.FirstName);
				sb.Append(" ");
				sb.Append(item.LastName);
				data.Username = sb.ToString();
				model.Users.Add(data);
			}

			var org = this.AppService.GetOrganization(id);
			model.OrganizationName = org.OrganizationName;

			return View(model);
		}
	}
}