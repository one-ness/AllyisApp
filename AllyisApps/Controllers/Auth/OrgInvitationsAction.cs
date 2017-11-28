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
					EmployeeId = item.EmployeeId,
					InvitationId = item.InvitationId,
					InvitedOn = item.InvitationCreatedUtc,
					Status = item.InvitationStatus.GetEnumName(),
					OrgRoleName = item.OrganizaionRole.GetEnumName(),
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

			var org = await AppService.GetOrganization(id);
			model.OrganizationName = org.OrganizationName;
			model.OrganizationId = id;
			model.TabInfo.OrganizationId = id;
			model.TabInfo.MemberCount = await this.AppService.GetOrganizationUserCountAsync(id);
			model.TabInfo.PendingInvitationCount = model.PendingInvitationCount;
			model.CanDeleteInvitations = AppService.CheckOrgAction(Services.AppService.OrgAction.DeleteInvitation, id, false);
			model.CanResendInvitations = AppService.CheckOrgAction(Services.AppService.OrgAction.AddUserToOrganization, id, false);
			return View(model);
		}
	}
}