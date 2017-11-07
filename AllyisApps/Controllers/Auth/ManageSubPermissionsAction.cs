using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers.Auth
{
	public partial class AccountController : BaseController
	{
		private Dictionary<int, string> ttRoles = new Dictionary<int, string>
		{
			{ (int)TimeTrackerRole.User, Strings.User },
			{ (int)TimeTrackerRole.Manager, Strings.Manager },
			{ (int)TimeTrackerRole.NotInProduct, Strings.Unassigned }
		};
		private Dictionary<string, int> setTTRoles = new Dictionary<string, int>
		{
			{ Strings.RemoveFromSubscription, -1 },
			{ Strings.SetUser, (int)TimeTrackerRole.User },
			{ Strings.SetManager, (int)TimeTrackerRole.Manager }
		};


		private Dictionary<int, string> etRoles = new Dictionary<int, string>
		{
			{ (int)ExpenseTrackerRole.User, Strings.User },
			{ (int)ExpenseTrackerRole.Manager, Strings.Manager },
			{ (int)ExpenseTrackerRole.SuperUser, "Super User" },
			{ (int)ExpenseTrackerRole.NotInProduct, Strings.Unassigned }
		};

		private Dictionary<string, int> setETRoles = new Dictionary<string, int>
		{
			{ Strings.RemoveFromSubscription, -1},
			{ Strings.SetUser, (int)ExpenseTrackerRole.User },
			{ Strings.SetManager, (int)ExpenseTrackerRole.Manager },
			{ "Set Super User", (int)ExpenseTrackerRole.SuperUser }
		};

		private Dictionary<int, string> smRoles = new Dictionary<int, string>
		{
			{ (int)StaffingManagerRole.User, Strings.User  },
			{ (int)StaffingManagerRole.Manager, Strings.Manager },
			{(int)StaffingManagerRole.NotInProduct,Strings.Unassigned }
		};

		private Dictionary<string, int> setSMRoles = new Dictionary<string, int>
		{
			{ Strings.RemoveFromSubscription, -1},
			{ Strings.SetUser, (int)StaffingManagerRole.User },
			{ Strings.SetManager, (int)StaffingManagerRole.Manager },
		};
		/// <summary>
		/// ManageTimeTrackerPermissions
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> ManageTimeTrackerPermissions(int id)
		{
			var sub = await AppService.GetSubscription(id);
			var orgSubs = await AppService.GetSubscriptionsAsync(sub.OrganizationId);


			var subUsers = AppService.GetSubscriptionUsers(id);
			var organizationMembers = AppService.GetOrganizationMemberList(sub.OrganizationId);

			//Get Strings speffic to Product for page
			Dictionary<int, string> roles = ttRoles;
			Dictionary<string, int> actions = setTTRoles;
			String roleHeader = Strings.TimeTrackerRole; ;
			String ActionGroup = Strings.TimeTracker;

			List<UserPermssionViewModel> OrgUsers = organizationMembers.Select(orgU => new UserPermssionViewModel()
			{
				CurrentRole = (int)ProductRole.NotInProduct,
				CurrentRoleName = Strings.Unassigned,
				FullName = orgU.FirstName + " " + orgU.LastName,
				Email = orgU.Email,
				IsChecked = false,
				UserId = orgU.UserId
			}).OrderBy(orgU => orgU.FullName).ToList();

			foreach (var subU in subUsers)
			{
				var OrgUserWithSub = OrgUsers.First(orgU => orgU.UserId == subU.UserId);
				OrgUserWithSub.CurrentRole = subU.ProductRoleId;
				OrgUserWithSub.CurrentRoleName = roles[subU.ProductRoleId];
			}

			PermissionsViewModel model = new PermissionsViewModel()
			{
				Actions = actions,
				OrganizationId = sub.OrganizationId,
				ActionGroup = ActionGroup,
				PossibleRoles = roles,
				ProductId = (int)sub.ProductId,
				CurrentSubscriptions = orgSubs.Select(cursub => new PermissionsViewModel.OrganizaionSubscriptionsViewModel()
				{
					ProductId = (int)cursub.ProductId,
					ProductName = cursub.ProductName,
					SubscriptionId = cursub.SubscriptionId,
					ManagePermissionsUrl = GetPermissionsUrl(cursub.ProductId, cursub.SubscriptionId),
					SubscriptionName = cursub.SubscriptionName
				}).OrderBy(cursub => cursub.ProductId).ToList(),
				SubscriptionId = id,
				RoleHeader = roleHeader,
				RemoveUserMessage = "Are you sure you want to remove selcted Users from Subscription",
				Users = OrgUsers
			};
			return View("PermissionsOrg", model);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> ManageExpensetrackerPermissions(int id)
		{
			var sub = await AppService.GetSubscription(id);
			var orgSubs = await AppService.GetSubscriptionsAsync(sub.OrganizationId);

			var subUsers = AppService.GetSubscriptionUsers(id);
			var organizationMembers = AppService.GetOrganizationMemberList(sub.OrganizationId);

			//Get Strings speffic to Product for page
			Dictionary<int, string> roles = etRoles;
			Dictionary<string, int> actions = setETRoles;
			String roleHeader = Strings.ExpenseTrackerRole;
			String ActionGroup = Strings.ExpenseTracker;

			List<UserPermssionViewModel> OrgUsers = organizationMembers.Select(orgU => new UserPermssionViewModel()
			{
				CurrentRole = (int)ProductRole.NotInProduct,
				CurrentRoleName = Strings.Unassigned,
				FullName = orgU.FirstName + " " + orgU.LastName,
				Email = orgU.Email,
				IsChecked = false,
				UserId = orgU.UserId
			}).OrderBy(orgU => orgU.FullName).ToList();

			foreach (var subU in subUsers)
			{
				var OrgUserWithSub = OrgUsers.First(orgU => orgU.UserId == subU.UserId);
				OrgUserWithSub.CurrentRole = subU.ProductRoleId;
				OrgUserWithSub.CurrentRoleName = roles[subU.ProductRoleId];
			}

			PermissionsViewModel model = new PermissionsViewModel()
			{
				Actions = actions,
				OrganizationId = sub.OrganizationId,
				ActionGroup = ActionGroup,
				PossibleRoles = roles,
				ProductId = (int)sub.ProductId,
				CurrentSubscriptions = orgSubs.Select(cursub => new PermissionsViewModel.OrganizaionSubscriptionsViewModel()
				{
					ProductId = (int)cursub.ProductId,
					ProductName = cursub.ProductName,
					SubscriptionId = cursub.SubscriptionId,
					SubscriptionName = cursub.SubscriptionName,
					ManagePermissionsUrl = GetPermissionsUrl(cursub.ProductId, cursub.SubscriptionId)

				}).OrderBy(cursub => cursub.ProductId).ToList(),
				SubscriptionId = id,
				RoleHeader = roleHeader,
				RemoveUserMessage = "Are you sure you want to remove selcted Users from Subscription",
				Users = OrgUsers
			};
			return View("PermissionsOrg", model);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> ManageStaffingManagerPermissions(int id)
		{
			var sub = await AppService.GetSubscription(id);
			var orgSubs = await AppService.GetSubscriptionsAsync(sub.OrganizationId);

			var subUsers = AppService.GetSubscriptionUsers(id);
			var organizationMembers = AppService.GetOrganizationMemberList(sub.OrganizationId);

			//Get Strings speffic to Product for page
			Dictionary<int, string> roles = smRoles;
			Dictionary<string, int> actions = setSMRoles;
			String roleHeader = Strings.StaffingManagerRole;
			String ActionGroup = Strings.StaffingManager;

			List<UserPermssionViewModel> OrgUsers = organizationMembers.Select(orgU => new UserPermssionViewModel()
			{
				CurrentRole = (int)ProductRole.NotInProduct,
				CurrentRoleName = Strings.Unassigned,
				FullName = orgU.FirstName + " " + orgU.LastName,
				Email = orgU.Email,
				IsChecked = false,
				UserId = orgU.UserId
			}).OrderBy(orgU => orgU.FullName).ToList();

			foreach (var subU in subUsers)
			{
				var OrgUserWithSub = OrgUsers.First(orgU => orgU.UserId == subU.UserId);
				OrgUserWithSub.CurrentRole = subU.ProductRoleId;
				OrgUserWithSub.CurrentRoleName = roles[subU.ProductRoleId];
			}

			PermissionsViewModel model = new PermissionsViewModel()
			{
				Actions = actions,
				OrganizationId = sub.OrganizationId,
				ActionGroup = ActionGroup,
				PossibleRoles = roles,
				ProductId = (int)sub.ProductId,
				CurrentSubscriptions = orgSubs.Select(cursub => new PermissionsViewModel.OrganizaionSubscriptionsViewModel()
				{
					ProductId = (int)cursub.ProductId,
					ProductName = cursub.ProductName,
					SubscriptionId = cursub.SubscriptionId,
					SubscriptionName = cursub.SubscriptionName,
					ManagePermissionsUrl= GetPermissionsUrl(cursub.ProductId, cursub.SubscriptionId),
				}).OrderBy(cursub => cursub.ProductId).ToList(),
				SubscriptionId = id,
				RoleHeader = roleHeader,
				RemoveUserMessage = "Are you sure you want to remove selcted Users from Subscription",
				Users = OrgUsers
			};
			return View("PermissionsOrg", model);
		}

		private async Task UpdateSubRoles(UserPermissionsAction model, string UsersModifiedMessage, string UsersAddedMessage)
		{
			var updatedAndAdded = await AppService.UpdateSubscriptionUsersRoles(model.SelectedUsers.Select(tu => tu.UserId).ToList(),
								model.SelectedAction.Value, model.OrganizationId, model.ProductId.Value);
			if (updatedAndAdded.UsersChanged > 0)
			{
				Notifications.Add(new BootstrapAlert(string.Format(UsersModifiedMessage, updatedAndAdded.UsersChanged), Variety.Success));
			}

			if (updatedAndAdded.UsersAddedToSubscription > 0)
			{
				Notifications.Add(new BootstrapAlert(string.Format(UsersAddedMessage, updatedAndAdded.UsersAddedToSubscription), Variety.Success));
			}
		}
		bool Validate(UserPermissionsAction model, ProductIdEnum product)
		{

			if (model.SelectedUsers == null || model.SelectedUsers.Count() == 0)
			{
				Notifications.Add(new BootstrapAlert(Strings.NoUsersSelected, Variety.Danger));
				return false;
			}

			if (model.SelectedAction == null)
			{
				Notifications.Add(new BootstrapAlert(Strings.NoActionsSelected, Variety.Danger));
				return false;
			}

			if (model.SubscriptionId == null && model.OrganizationId != 0 || (model.ProductId.HasValue && model.ProductId.Value != (int)product))
			{
				return false;
			}
			return true;
		}
	}


}