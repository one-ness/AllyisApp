using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Auth;

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
			await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Edit, AppService.AppEntity.Permission, id);
			var orgUsers = AppService.GetOrganizationMemberList(id);
			var orgSubs = await AppService.GetSubscriptionsAsync(id);
            System.Collections.Generic.List<OrganizationRoleEnum> productroles = new System.Collections.Generic.List<OrganizationRoleEnum>();
            int prodRoleCounter = 0;
            foreach (OrganizationUser orguser in orgUsers)
            {
                string role = await AppService.GetProductRoleName(id, orguser.OrganizationRoleId);
                if (role.Equals(Strings.Admin))
                {
                    productroles.Add(OrganizationRoleEnum.Admin);
                } else
                {
                    productroles.Add(OrganizationRoleEnum.Member);
                }
            }
            PermissionsViewModel perModel = new PermissionsViewModel
			{
				Actions = setOrganizationRoles,
				UserId = AppService.UserContext.UserId,
				ActionGroup = Strings.Organization,
				PossibleRoles = organizationRoles,
				RemoveUserMessage = Strings.RemoveFromOrgNoName,
				RoleHeader = Strings.OrganizationRole,
				CurrentSubscriptions = orgSubs.Select(sub => new PermissionsViewModel.OrganizaionSubscriptionsViewModel
				{
					ProductId = (int)sub.ProductId,
					ProductName = sub.ProductName,
					SubscriptionId = sub.SubscriptionId,
					SubscriptionName = sub.SubscriptionName,
					ManagePermissionsUrl = GetPermissionsUrl(sub.ProductId, sub.SubscriptionId),
				}).OrderBy(sub => sub.ProductId).ToList(),
                
				OrganizationId = id,
				ProductId = null,
				SubscriptionId = null,
				Users = orgUsers.Select(orgU => new UserPermssionViewModel
				{
					CurrentRole = orgU.OrganizationRoleId,
					CurrentRoleName = organizationRoles[(int)productroles.ElementAt(prodRoleCounter++)],
					Email = orgU.Email,
					FullName = orgU.FirstName + " " + orgU.LastName,
					UserId = orgU.UserId,
					IsChecked = false,
					IsCurrentUser = false
				}).OrderBy(orgU => orgU.FullName).ToList()
			};
			foreach (var user in perModel.Users) if (user.UserId == perModel.UserId) user.IsCurrentUser = true;
			return View("PermissionsOrg", perModel);
		}
	}
}