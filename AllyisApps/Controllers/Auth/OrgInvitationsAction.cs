//------------------------------------------------------------------------------
// <copyright file="OrgInvitationsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.ViewModels.Auth;

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
			var model = await Task.Run(() => new OrganizationInvitationsViewModel());

			var collection = await this.AppService.GetInvitationsAsync(id);
			foreach (var item in collection)
			{
				var data = new OrganizationInvitationsViewModel.ViewModelItem();
				data.DecisionDate = item.DecisionDateUtc;
				data.Email = item.Email;
				data.EmployeeId = item.EmployeeId;
				data.InvitationId = item.InvitationId;
				data.InvitedOn = item.InvitationCreatedUtc;
				//data.ProductAndRoleNames = item.ProductRolesJson; TODO: parse the json to get the product names and roles
				data.Status = item.InvitationStatus.ToString();
				StringBuilder sb = new StringBuilder();
				sb.Append(item.FirstName);
				sb.Append(" ");
				sb.Append(item.LastName);
				data.Username = sb.ToString();
				model.Invitations.Add(data);
			}

			var org = await this.AppService.GetOrganization(id);
			model.OrganizationName = org.OrganizationName;
			model.TabInfo.OrganizationId = id;
			model.CanDeleteInvitations = this.AppService.CheckOrgAction(Services.AppService.OrgAction.DeleteInvitation, id, false);
			model.CanResendInvitations = this.AppService.CheckOrgAction(Services.AppService.OrgAction.AddUserToOrganization, id, false);
			return View(model);
		}
	}
}