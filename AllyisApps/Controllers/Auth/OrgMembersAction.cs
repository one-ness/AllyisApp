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
using System.Collections.Generic;

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
				CanEditUser = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Edit, AppService.AppEntity.OrganizationUser, id, false),
				CanManagePermissions = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Edit, AppService.AppEntity.Permission, id, false),
				OrganizationId = id,
				CurrentUserId = AppService.UserContext.UserId
			};

			var roles = await this.AppService.GetProductRolesAsync(id, ProductIdEnum.AllyisApps);
			var dicy = new Dictionary<int, string>();
			foreach (var item in roles)
			{
				dicy.Add(item.ProductRoleId, item.ProductRoleShortName);
			}

			model.PossibleRoles = dicy;
			model.TabInfo.OrganizationId = id;
			
			var collection = await AppService.GetOrganizationUsersAsync(id, false);
			foreach (var item in collection)
			{
				var data = new OrganizationMembersViewModel.ViewModelItem
				{
					Email = item.Email,
					EmployeeId = item.EmployeeId,
					JoinedDate = item.OrganizationUserCreatedUtc,
					RoleName = dicy[item.OrganizationRoleId],
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