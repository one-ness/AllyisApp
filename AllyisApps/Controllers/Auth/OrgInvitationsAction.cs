//------------------------------------------------------------------------------
// <copyright file="OrgInvitationsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.Auth;
using Newtonsoft.Json;
using AllyisApps.Services.Billing;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Get: Account/OrgInvitations
		/// </summary>
		public async Task<ActionResult> OrgInvitations(int id)
		{
			var model = new OrganizationInvitationsViewModel();
			model.PossibleRoles = organizationRoles;
			var collection = await AppService.GetInvitationsAsync(id);
			foreach (var item in collection)
			{
				var data = new OrganizationInvitationsViewModel.ViewModelItem
				{
					DecisionDate = item.DecisionDateUtc,
					Email = item.Email,
					EmployeeTypeId = item.EmployeeTypeId,
					EmployeeId = item.EmployeeId,
					InvitationId = item.InvitationId,
					InvitedOn = item.InvitationCreatedUtc,
					Status = item.InvitationStatus.GetEnumName(),
					OrgRoleName = item.OrganizationRole.GetEnumName(),
					Username = $"{item.FirstName} {item.LastName}",
					
					ProductAndRoleNames = new List<Tuple<string, string>>()
				};
				
				var productRoleNames = JsonConvert.DeserializeObject<List<InvitationPermissionsJson>>(item.ProductRolesJson) ?? new List<InvitationPermissionsJson>();
				foreach (var invitation in productRoleNames)
				{
					var productInfo = await AppService.GetSubscription(invitation.SubscriptionId);

					data.ProductAndRoleNames.Add(new Tuple<string, string>(
						(await AppService.GetSubscription(invitation.SubscriptionId)).ProductName,
						(await AppService.GetProductRoles(id, productInfo.ProductId)).FirstOrDefault(role => invitation.ProductRoleId == role.ProductRoleId)?.ProductRoleName ?? "Unassigned"
					));
				}

				if (item.InvitationStatus == InvitationStatusEnum.Pending)
				{
					model.PendingInvitationCount++;
				}

				model.Invitations.Add(data);
			}

			var org = await AppService.GetOrganizationAsync(id);
			model.OrganizationName = org.OrganizationName;
			model.OrganizationId = id;
			model.TabInfo.OrganizationId = id;
			model.TabInfo.MemberCount = org.UserCount;
			model.TabInfo.PendingInvitationCount = model.PendingInvitationCount;
			model.CanDeleteInvitations = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, Services.AppService.UserAction.Delete, Services.AppService.AppEntity.OrganizationUser, id, false);
			model.CanResendInvitations = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, Services.AppService.UserAction.Create, Services.AppService.AppEntity.OrganizationUser, id, false);
			return View(model);
		}
	}
}