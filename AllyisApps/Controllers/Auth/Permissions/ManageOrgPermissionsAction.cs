using AllyisApps.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.Resources;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// 
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Manage Organizaion 
		/// </summary>
		/// <param name="id">Organizaion Id.</param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> ManageOrgPermissions(int id)
		{
			//Get OrganizaionUser Rows
			AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id);
			var orgUsers = AppService.GetOrganizationMemberList(id);
			var orgSubs = await AppService.GetSubscriptionsAsync(id);

			PermissionsViewModel perModel = new PermissionsViewModel
			{
				Actions = setOrganizationRoles,
				ActionGroup = Strings.Organization,
				PossibleRoles = organizationRoles,
				RemoveUserMessage = Strings.RemoveFromOrgNoName,
				RoleHeader = Strings.OrganizationRole,
				CurrentSubscriptions = orgSubs.Select(sub => new PermissionsViewModel.OrganizaionSubscriptionsViewModel
				{
					ProductId = (int)sub.ProductId,
					ProductName = sub.ProductName,
					SubscriptionId = sub.SubscriptionId,
					SubscriptionName = sub.SubscriptionName
				}).OrderBy(sub => sub.ProductId).ToList(),

				OrganizationId = id,
				ProductId = null,
				SubscriptionId = null,
				Users = orgUsers.Select(orgU => new UserPermssionViewModel
				{
					CurrentRole = orgU.OrganizationRoleId,
					CurrentRoleName = organizationRoles[orgU.OrganizationRoleId],
					Email = orgU.Email,
					FullName = orgU.FirstName + " " + orgU.LastName,
					UserId = orgU.UserId,
					IsChecked = false
				}).OrderBy(orgU => orgU.FullName).ToList()
			};

			await Task.Delay(1);
			return View("PermissionsOrg", perModel);
		}

	}
}