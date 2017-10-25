//------------------------------------------------------------------------------
// <copyright file="ManagePermissionsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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
using Newtonsoft.Json;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Dictionaries of Strings resource used for view Maybe: Move to view Model
		/// </summary>
		private Dictionary<int, string> organizationRoles = new Dictionary<int, string>
		{
		{ (int)OrganizationRoleEnum.Member, Strings.Member },
		{ (int)OrganizationRoleEnum.Owner, Strings.Owner }
		};

		private Dictionary<int, string> ttRoles = new Dictionary<int, string>
		{
			{ (int)TimeTrackerRole.User, Strings.User },
			{ (int)TimeTrackerRole.Manager, Strings.Manager },
			{ (int)TimeTrackerRole.NotInProduct, Strings.Unassigned }
		};

		private Dictionary<int, string> etRoles = new Dictionary<int, string>
	{
		{ (int)ExpenseTrackerRole.User, Strings.User },
		{ (int)ExpenseTrackerRole.Manager, Strings.Manager },
		{ (int)ExpenseTrackerRole.SuperUser, "Super User" },
		{ (int)ExpenseTrackerRole.NotInProduct, Strings.Unassigned }
	};

		private Dictionary<string, int> setOrganizationRoles = new Dictionary<string, int>
	{
		{ Strings.RemoveOrg, -1 },
		{ Strings.SetMember, (int)OrganizationRoleEnum.Member },
		{ Strings.SetOwner, (int)OrganizationRoleEnum.Owner }
	};

		private Dictionary<string, int> setTTRoles = new Dictionary<string, int>
	{
		{ Strings.RemoveFromSubscription, -1 },
		{ Strings.SetUser, (int)TimeTrackerRole.User },
		{ Strings.SetManager, (int)TimeTrackerRole.Manager }
	};

		private Dictionary<string, int> setETRoles = new Dictionary<string, int>
	{
		{ Strings.RemoveFromSubscription, -1},
		{ Strings.SetUser, (int)ExpenseTrackerRole.User },
		{ Strings.SetManager, (int)ExpenseTrackerRole.Manager },
		{ "Set Super User", (int)ExpenseTrackerRole.SuperUser }
	};

		/// <summary>
		/// GET Account/ManagePermissions.
		/// </summary>
		/// <param name="id">The Organization Id.</param>
		/// <returns>Action result.</returns>
		[HttpGet]
		public async Task<ActionResult> ManagePermissions(int id)
		{
			AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id);
			var infos = AppService.GetOrgAndSubRoles(id);
			ManagePermissionsViewModel model = new ManagePermissionsViewModel
			{
				Users = new List<UserPermissionsViewModel>(),
				Subscriptions = infos.Subscriptions.Select(sub => new
					SubscriptionDisplayViewModel(sub)).ToList(),
				SubIds = infos.Subscriptions.Select(s => s.SubscriptionId).ToList(),
				OrganizationId = id,

				// TODO: Get rid of this once product panes in Permissions page are genericized.
				TimeTrackerId = ProductIdEnum.TimeTracker,
				ExpenseTrackerId = ProductIdEnum.ExpenseTracker
			};

			// This can also be axed after finding a good way to genericize products in the Permissions page.
			var ttsub = model.Subscriptions.Where(s => s.ProductId == model.TimeTrackerId).SingleOrDefault();
			var etsub = model.Subscriptions.Where(s => s.ProductId == model.ExpenseTrackerId).SingleOrDefault();

			if (ttsub != null)
			{
				model.TimeTrackerSubIndex = model.Subscriptions.IndexOf(ttsub);
			}

			if (etsub != null)
			{
				model.ExpenseTrackerSubIndex = model.Subscriptions.IndexOf(etsub);
			}

			foreach (UserRole role in infos.UserRoles)
			{
				UserPermissionsViewModel modelUser = model.Users.Where(u => u.UserId == role.UserId).SingleOrDefault();
				if (modelUser == null)
				{
					modelUser = new UserPermissionsViewModel
					{
						FirstName = role.FirstName,
						LastName = role.LastName,
						UserId = role.UserId,
						Email = role.Email,
						OrganizationRoleId = role.OrganizationRoleId,
						ProductRoleIds = new List<int>()
					};

					// Start out with default TT NotInProduct role if org is subscribed to TT.
					foreach (SubscriptionDisplayViewModel sub in model.Subscriptions)
					{
						modelUser.ProductRoleIds.Add(ProductRole.NotInProduct);
					}

					model.Users.Add(modelUser);
				}

				if (role.SubscriptionId != -1)
				{
					int index = model.SubIds.IndexOf(role.SubscriptionId);
					if (index >= 0)
					{
						modelUser.ProductRoleIds[model.SubIds.IndexOf(role.SubscriptionId)] = role.ProductRoleId;
					}
				}
			}

			await Task.Delay(1);
			return View("Permission2", model);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="id">Organizaion Id.</param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> ManageOrgPermissions(int id)
		{
			//Get OrganizaionUser Rows
			AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id);
			var orgUsers = AppService.GetOrganizationMemberList(id);
			var orgSubs = AppService.GetSubscriptionsByOrg(id);

			PermissionsViewModel perModel = new PermissionsViewModel()
			{
				Actions = setOrganizationRoles,
				ActionGroup = Strings.Organization,
				PossibleRoles = organizationRoles,
				RemoveUserMessage = Strings.RemoveFromOrgNoName,
				RoleHeader = Strings.OrganizationRole,
				CurrentSubscriptions = orgSubs.Select(sub => new PermissionsViewModel.OrganizaionSubscriptionsViewModel()
				{
					ProductId = (int)sub.ProductId,
					ProductName = sub.ProductName,
					SubscriptionId = sub.SubscriptionId,
					SubscriptionName = sub.SubscriptionName
				}).OrderBy(sub => sub.ProductId).ToList(),

				OrganizationId = id,
				ProductId = null,
				SubscriptionId = null,
				Users = orgUsers.Select(orgU => new UserPermssionViewModel()
				{
					currentRole = orgU.OrganizationRoleId,
					CurrentRoleName = organizationRoles[orgU.OrganizationRoleId],
					Email = orgU.Email,
					FullName = orgU.FirstName + " " + orgU.LastName,
					UserId = orgU.UserId,
					isChecked = false
				}).OrderBy(orgU => orgU.FullName).ToList()
			};

			await Task.Delay(1);
			return View("PermissionsOrg", perModel);
		}

		/// <summary>
		/// Get page to edit SubscriptionPermissions
		/// </summary>
		/// <param name="id">Subscription ID</param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> ManageSubPermissions(int id)
		{
			var sub = await AppService.GetSubscription(id);
			var orgSubs = AppService.GetSubscriptionsByOrg(sub.OrganizationId);

			var subUsers = AppService.GetSubscriptionUsers(id);
			var organizationMembers = AppService.GetOrganizationMemberList(sub.OrganizationId);

			//Get Strings speffic to Product for page
			Dictionary<int, string> roles = null;
			Dictionary<string, int> actions = null;
			String roleHeader = null;
			String ActionGroup = null;
			switch (sub.ProductId)
			{
				case ProductIdEnum.TimeTracker:
					roles = ttRoles;
					actions = setTTRoles;
					roleHeader = Strings.TimeTrackerRole;
					ActionGroup = Strings.TimeTracker;
					break;

				case ProductIdEnum.ExpenseTracker:
					roles = etRoles;
					actions = setETRoles;
					roleHeader = Strings.ExpenseTrackerRole;
					ActionGroup = Strings.ExpenseTracker;
					break;

				case ProductIdEnum.StaffingManager:
					throw new NotImplementedException("StaffingManager permissions not implmented");
			}

			List<UserPermssionViewModel> OrgUsers = organizationMembers.Select(orgU => new UserPermssionViewModel()
			{
				currentRole = (int)ProductRole.NotInProduct,
				CurrentRoleName = Strings.Unassigned,
				FullName = orgU.FirstName + " " + orgU.LastName,
				Email = orgU.Email,
				isChecked = false,
				UserId = orgU.UserId
			}).OrderBy(orgU => orgU.FullName).ToList();

			foreach (var subU in subUsers)
			{
				var OrgUserWithSub = OrgUsers.First(orgU => orgU.UserId == subU.UserId);
				OrgUserWithSub.currentRole = subU.ProductRoleId;
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
					SubscriptionName = cursub.SubscriptionName
				}).OrderBy(cursub => cursub.ProductId).ToList(),
				SubscriptionId = id,
				RoleHeader = roleHeader,
				RemoveUserMessage = "Are you sure you want to remove selcted Users from Subscription",
				Users = OrgUsers
			};

			await Task.Delay(1);
			return View("PermissionsOrg", model);
		}

		/// <summary>
		/// Makes changes to users' permissions in the organization.
		/// Called from Account/Permission do_it_submit().
		/// </summary>
		/// <param name="data">The JSON string of the model of actions and users.</param>
		/// <returns>A Json object representing the results of the actions.</returns>
		[HttpPost]
		public async Task<ActionResult> ManagePermissions(string data)
		{
			UserPermissionsAction model = JsonConvert.DeserializeObject<UserPermissionsAction>(data);

			AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, model.OrganizationId);

			if (model.SelectedUsers == null || model.SelectedUsers.Count() == 0)
			{
				Notifications.Add(new BootstrapAlert(Strings.NoUsersSelected, Variety.Danger));
				return Redirect(model.FromUrl);
			}

			if (model.SelectedAction == null)
			{
				Notifications.Add(new BootstrapAlert(Strings.NoActionsSelected, Variety.Danger));
				return Redirect(model.FromUrl);
			}

			//If is from ManageOrganizationPage
			if (model.SubscriptionId == null && model.OrganizationId != 0)
			{
				// Changing organization roles
				if (!Enum.IsDefined(typeof(OrganizationRoleEnum), model.SelectedAction) && model.SelectedAction != -1)
				{
					Notifications.Add(new BootstrapAlert(Strings.YouDidNotDefineATargetRole, Variety.Danger));
					return Redirect(model.FromUrl);
				}

				if (model.SelectedUsers.Where(tu => tu.UserId == AppService.UserContext.UserId).Any())
				{
					if (model.SelectedAction == -1)
					{
						Notifications.Add(new BootstrapAlert(Strings.YouAreUnableToRemoveYourself, Variety.Danger));
					}
					else
					{
						Notifications.Add(new BootstrapAlert(Strings.YouAreUnableToChangeYourOwnRole, Variety.Danger));
					}

					model.SelectedUsers = model.SelectedUsers.Where(tu => tu.UserId != AppService.UserContext.UserId);
					if (model.SelectedUsers.Count() == 0)
					{
						return Redirect(model.FromUrl);
					}
				}

				if (model.SelectedAction == -1 && model.SubscriptionId == null)
				{
					int numberChanged = AppService.DeleteOrganizationUsers(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.OrganizationId);
					Notifications.Add(new BootstrapAlert(string.Format(Strings.UsersRemovedFromOrg, numberChanged), Variety.Success));
				}
				else
				{
					int numberChanged = AppService.UpdateOrganizationUsersRole(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.SelectedAction.Value, model.OrganizationId);
					Notifications.Add(new BootstrapAlert(string.Format(Strings.UsersChangedRolesInOrg, numberChanged), Variety.Success));
				}
			}
			else if (model.SelectedAction != 0 && model.SubscriptionId != null && model.ProductId != null)
			{
				string UsersModifiedMessage = null;
				string UsersAddedMessage = null;
				//Varify that roleId is correct
				switch ((ProductIdEnum)model.ProductId)
				{
					case ProductIdEnum.TimeTracker:
						// Changing time tracker roles
						UsersModifiedMessage = Strings.UsersChangedRolesInTimeTracker;
						UsersAddedMessage = Strings.UsersAddedToTimeTracker;

						if (!Enum.IsDefined(typeof(TimeTrackerRole), model.SelectedAction) && model.SelectedAction.Value != -1)
						{
							Notifications.Add(new BootstrapAlert(Strings.YouDidNotDefineATargetRole, Variety.Danger));
							return Redirect(model.FromUrl);
						}

						break;

					case ProductIdEnum.ExpenseTracker:

						// Changing expense tracker roles
						UsersModifiedMessage = Strings.UserChangedRolesInExpenseTracker;
						UsersAddedMessage = Strings.UserAddedToExpenseTracker;
						if (!Enum.IsDefined(typeof(ExpenseTrackerRole), model.SelectedAction.Value) && model.SelectedAction != -1)
						{
							Notifications.Add(new BootstrapAlert(Strings.YouDidNotDefineATargetRole, Variety.Danger));
							return Redirect(model.FromUrl);
						}

						break;
					/*
					case ProductIdEnum.StaffingManager:
						/*Staffing Manager needs Roles
						if (!Enum.IsDefined(typeof(StaffingMan), model.SelectedAction.Value) && model.SelectedAction != -1)
						{
							Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
							return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
						}

						break;
					*/
					default:
						//Should not happen
						throw new ArgumentOutOfRangeException("Failed to Find product for produtID: " + model.ProductId.Value);
				}

				if (model.SelectedAction.Value != -1)
				{
					// TODO: instead of providing product id, provide subscription id of the subscription to be modified
					// TODO: split updating user roles and creating new sub users
					var updatedAndAdded = await AppService.UpdateSubscriptionUserRoles(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.SelectedAction.Value, model.OrganizationId, model.ProductId.Value);
					if (updatedAndAdded.UsersChanged > 0)
					{
						Notifications.Add(new BootstrapAlert(string.Format(UsersModifiedMessage, updatedAndAdded.UsersChanged), Variety.Success));
					}

					if (updatedAndAdded.UsersAddedToSubscription > 0)
					{
						Notifications.Add(new BootstrapAlert(string.Format(UsersAddedMessage, updatedAndAdded.UsersAddedToSubscription), Variety.Success));
					}
				}
				else
				{
					// TODO: instead of providing product id, provide subscription id of the subscription to be modified
					AppService.DeleteSubscriptionUsers(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.OrganizationId, model.ProductId.Value);
					Notifications.Add(new BootstrapAlert(Strings.UserDeletedSuccessfully, Variety.Success));
				}
			}

			await Task.Delay(1);
			return Redirect(model.FromUrl);
		}
	}
}