//------------------------------------------------------------------------------
// <copyright file="OrgMembersAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
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
		/// Get: Account/OrgMembers
		/// </summary>
		public async Task<ActionResult> OrgMembers(int id)
		{

			var model = new OrganizationMembersViewModel
			{
				CanAddUser = AppService.CheckOrgAction(AppService.OrgAction.AddUserToOrganization, id, false),
				CanDeleteUser = AppService.CheckOrgAction(AppService.OrgAction.DeleteUserFromOrganization, id, false),
				CanEditUser = AppService.CheckOrgAction(AppService.OrgAction.EditUser, id, false),
				CanManagePermissions = AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id, false),
				OrganizationId = id,
				PossibleRoles = organizationRoles,
				CurrentUserId = AppService.UserContext.UserId
			};
			model.TabInfo.OrganizationId = id;
			
			var collection = await AppService.GetOrganizationUsersAsync(id);
			foreach (var item in collection)
			{
				OrganizationRoleEnum orgRole = (OrganizationRoleEnum)item.OrganizationRoleId;

				var data = new OrganizationMembersViewModel.ViewModelItem
				{
					Email = item.Email,
					EmployeeId = item.EmployeeId,
					JoinedDate = item.OrganizationUserCreatedUtc,
					RoleName = Enum.GetName(typeof(OrganizationRoleEnum), orgRole) ?? string.Empty,
					UserId = item.UserId,
					Username = $"{item.FirstName} {item.LastName}"
				};

				model.Users.Add(data);
			}

			var org = await AppService.GetOrganization(id);
			model.OrganizationName = org.OrganizationName;
			model.TabInfo.MemberCount = model.Users.Count;

			if (model.CanAddUser)
			{
				model.TabInfo.PendingInvitationCount = await this.AppService.GetOrganizationInvitationCountAsync(id, Services.Auth.InvitationStatusEnum.Pending);
			}
			return View(model);
		}
	}
}