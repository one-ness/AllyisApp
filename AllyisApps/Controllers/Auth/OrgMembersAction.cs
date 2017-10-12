//------------------------------------------------------------------------------
// <copyright file="OrgSubscriptionsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services.Auth;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.Auth;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Text;
using AllyisApps.Services;

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
			model.IsAddUserAllowed = this.AppService.CheckOrgAction(AppService.OrgAction.AddUserToOrganization, id, false);
			model.IsDeleteUserAllowed = this.AppService.CheckOrgAction(AppService.OrgAction.DeleteUserFromOrganization, id, false);
			model.IsEditUserAllowed = this.AppService.CheckOrgAction(AppService.OrgAction.EditUser, id, false);
			model.IsManagePermissionsAllowed = this.AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id, false);
			model.OrganizationId = id;
			var collection = await this.AppService.GetOrganizationUsersAsync(id);
			foreach (var item in collection)
			{
				var data = new OrganizationMembersViewModel2.ViewModelItem();
				data.Email = item.Email;
				data.EmployeeId = item.EmployeeId;
				data.JoinedDate = item.OrganizationUserCreatedUtc;
				data.RoleName = ModelHelper.GetOrganizationRoleName((OrganizationRole)item.OrganizationRoleId);
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