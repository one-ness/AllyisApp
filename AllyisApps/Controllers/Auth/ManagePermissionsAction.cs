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
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;
using Newtonsoft.Json;
using AllyisApps.Core.Alert;

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
		private readonly Dictionary<int, string> organizationRoles = new Dictionary<int, string>
		{
		{ (int)OrganizationRoleEnum.Member, Strings.Member },
		{ (int)OrganizationRoleEnum.Owner, Strings.Owner }
		};


		private Dictionary<string, int> setOrganizationRoles = new Dictionary<string, int>
	{
		{ Strings.RemoveOrg, -1 },
		{ Strings.SetMember, (int)OrganizationRoleEnum.Member },
		{ Strings.SetOwner, (int)OrganizationRoleEnum.Owner }
	};



		
		/*
		/// <summary>
		/// Get page to edit SubscriptionPermissions
		/// </summary>
		/// <param name="id">Subscription ID</param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> ManageSubPermissions(int id)
		{
			var sub = await AppService.GetSubscription(id);
			var orgSubs = await AppService.GetSubscriptionsAsync(sub.OrganizationId);

			var subUsers = AppService.GetSubscriptionUsers(id);
			var organizationMembers = AppService.GetOrganizationMemberList(sub.OrganizationId);

			//Get Strings speffic to Product for page
			Dictionary<int, string> roles = null;
			Dictionary<string, int> actions = null;
			string roleHeader = null;
			string actionGroup = null;
			switch (sub.ProductId)
			{
				case ProductIdEnum.TimeTracker:
					roles = ttRoles;
					actions = setTTRoles;
					roleHeader = Strings.TimeTrackerRole;
					actionGroup = Strings.TimeTracker;
					break;

				case ProductIdEnum.ExpenseTracker:
					roles = etRoles;
					actions = setETRoles;
					roleHeader = Strings.ExpenseTrackerRole;
					actionGroup = Strings.ExpenseTracker;
					break;

				case ProductIdEnum.StaffingManager:
					throw new NotImplementedException("StaffingManager permissions not implmented");
			}

			List<UserPermssionViewModel> orgUsers = organizationMembers.Select(orgU => new UserPermssionViewModel
			{
				CurrentRole = ProductRole.NotInProduct,
				CurrentRoleName = Strings.Unassigned,
				FullName = $"{orgU.FirstName} {orgU.LastName}",
				Email = orgU.Email,
				IsChecked = false,
				UserId = orgU.UserId
			}).OrderBy(orgU => orgU.FullName).ToList();

			foreach (var subU in subUsers)
			{
				var orgUserWithSub = orgUsers.First(orgU => orgU.UserId == subU.UserId);
				orgUserWithSub.CurrentRole = subU.ProductRoleId;
				orgUserWithSub.CurrentRoleName = roles[subU.ProductRoleId];
			}

			PermissionsViewModel model = new PermissionsViewModel
			{
				Actions = actions,
				OrganizationId = sub.OrganizationId,
				ActionGroup = actionGroup,
				PossibleRoles = roles,
				ProductId = (int)sub.ProductId,
				CurrentSubscriptions = orgSubs.Select(cursub => new PermissionsViewModel.OrganizaionSubscriptionsViewModel
				{
					ProductId = (int)cursub.ProductId,
					ProductName = cursub.ProductName,
					SubscriptionId = cursub.SubscriptionId,
					SubscriptionName = cursub.SubscriptionName,
					ManagePermissionsUrl = getPermissionsUrl(cursub.ProductId,cursub.SubscriptionId)
				}).OrderBy(cursub => cursub.ProductId).ToList(),
				SubscriptionId = id,
				RoleHeader = roleHeader,
				RemoveUserMessage = "Are you sure you want to remove selcted Users from Subscription",
				Users = orgUsers
			};

			await Task.Delay(1);
			return View("PermissionsOrg", model);
		}
		*/


		internal string getPermissionsUrl(ProductIdEnum productId, int subscripitonId)
		{
			switch (productId)
			{
				case ProductIdEnum.TimeTracker:
					return Url.Action("ManageTimeTrackerPermissions", new { id = subscripitonId });
				case ProductIdEnum.ExpenseTracker:
					return Url.Action("ManageExpensetrackerPermissions", new { id = subscripitonId });
					
				case ProductIdEnum.StaffingManager:
					return Url.Action("ManageStaffingManagerPermissions", new { id = subscripitonId });
				default:
					throw new ArgumentOutOfRangeException("Product id is not in system");
			}
		}



		/// <summary>
		/// Makes changes to users' permissions in the organization.
		/// Called from Account/Permission do_it_submit().
		/// </summary>
		/// <param name="data">The JSON string of the model of actions and users.</param>
		/// <returns>A Json object representing the results of the actions.</returns>
		[HttpPost]
		async public Task<ActionResult> ManagePermissions(string data)
		{
			UserPermissionsAction model = JsonConvert.DeserializeObject<UserPermissionsAction>(data);

			this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, model.OrganizationId);

			if (model.SelectedUsers == null || model.SelectedUsers.Count() == 0)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.NoUsersSelected, Variety.Danger));
				return Redirect(model.FromUrl);
			}

			if (model.SelectedAction == null)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.NoActionsSelected, Variety.Danger));
				return Redirect(model.FromUrl);
			}

			//If is from ManageOrganizationPage
			if (model.SubscriptionId == null && model.OrganizationId != 0)
			{
				// Changing organization roles
				if (!Enum.IsDefined(typeof(OrganizationRoleEnum), model.SelectedAction) && model.SelectedAction != -1)
				{
					Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
					return Redirect(model.FromUrl);
				}

				if (model.SelectedUsers.Where(tu => tu.UserId == this.AppService.UserContext.UserId).Any())
				{
					if (model.SelectedAction == -1)
					{
						Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouAreUnableToRemoveYourself, Variety.Danger));
					}
					else
					{
						Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouAreUnableToChangeYourOwnRole, Variety.Danger));
					}

					model.SelectedUsers = model.SelectedUsers.Where(tu => tu.UserId != this.AppService.UserContext.UserId);
					if (model.SelectedUsers.Count() == 0)
					{
						return Redirect(model.FromUrl);
					}
				}

				if (model.SelectedAction == -1 && model.SubscriptionId == null)
				{
					int numberChanged = AppService.DeleteOrganizationUsers(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.OrganizationId);
					Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UsersRemovedFromOrg, numberChanged), Variety.Success));
				}
				else
				{
					int numberChanged = AppService.UpdateOrganizationUsersRole(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.SelectedAction.Value, model.OrganizationId);
					Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UsersChangedRolesInOrg, numberChanged), Variety.Success));
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
							Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
							return Redirect(model.FromUrl);
						}

						break;

					case ProductIdEnum.ExpenseTracker:

						// Changing expense tracker roles
						UsersModifiedMessage = Strings.UserChangedRolesInExpenseTracker;
						UsersAddedMessage = Strings.UserAddedToExpenseTracker;
						if (!Enum.IsDefined(typeof(ExpenseTrackerRole), model.SelectedAction.Value) && model.SelectedAction != -1)
						{
							Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
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
					var updatedAndAdded = await AppService.UpdateSubscriptionUsersRoles(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.OrganizationId, model.SelectedAction.Value , model.ProductId.Value);
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
					Notifications.Add(new BootstrapAlert(Resources.Strings.UserDeletedSuccessfully, Variety.Success));
				}
			}

			return Redirect(model.FromUrl);
		}
	}
}