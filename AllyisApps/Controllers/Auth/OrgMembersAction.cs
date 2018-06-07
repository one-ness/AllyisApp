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
using AllyisApps.Services.Billing;

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
				CanAddUser = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Create, AppService.AppEntity.OrganizationUser, id, false),
				CanDeleteUser = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Delete, AppService.AppEntity.OrganizationUser, id, false),
				CanEditUser = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Update, AppService.AppEntity.OrganizationUser, id, false),
				CanManagePermissions = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Update, AppService.AppEntity.Permission, id, false),
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

			var org = await AppService.GetOrganizationAsync(id);
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