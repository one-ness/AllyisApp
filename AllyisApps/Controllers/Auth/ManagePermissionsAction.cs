//------------------------------------------------------------------------------
// <copyright file="ManagePermissionsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;
using Newtonsoft.Json;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET Account/ManagePermissions.
		/// </summary>
		/// <param name="id">The Organization Id.</param>
		/// <returns>Action result.</returns>
		[HttpGet]
		public ActionResult ManagePermissions(int id)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id);
			var infos = AppService.GetOrgAndSubRoles(id);
			ManagePermissionsViewModel model = new ManagePermissionsViewModel
			{
				Users = new List<UserPermissionsViewModel>(),
				Subscriptions = infos.Item2.Select(sub => new
					SubscriptionDisplayViewModel(sub)).ToList(),
				SubIds = infos.Item2.Select(s => s.SubscriptionId).ToList(),
				OrganizationId = id,

				// TODO: Get rid of this once product panes in Permissions page are genericized.
				TimeTrackerId = (int)ProductIdEnum.TimeTracker,
				ExpenseTrackerId = (int)ProductIdEnum.ExpenseTracker
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

			foreach (UserRolesInfo role in infos.Item1)
			{
				UserPermissionsViewModel modelUser = model.Users.Where(u => u.UserId == int.Parse(role.UserId)).SingleOrDefault();
				if (modelUser == null)
				{
					modelUser = new UserPermissionsViewModel
					{
						FirstName = role.FirstName,
						LastName = role.LastName,
						UserId = int.Parse(role.UserId),
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

			return this.View("Permission2", model);
		}

		/// <summary>
		/// Makes changes to users' permissions in the organization.
		/// Called from Account/Permission do_it_submit().
		/// </summary>
		/// <param name="data">The JSON string of the model of actions and users.</param>
		/// <returns>A Json object representing the results of the actions.</returns>
		[HttpPost]
		public ActionResult ManagePermissions(string data)
		{
			UserPermissionsAction model = JsonConvert.DeserializeObject<UserPermissionsAction>(data);
			this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, model.OrganizationId);
			if (model.SelectedUsers == null || model.SelectedUsers.Count() == 0)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.NoUsersSelected, Variety.Danger));
				return RedirectToAction(ActionConstants.ManageOrg);
			}

			if (model.SelectedActions == null)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.NoActionsSelected, Variety.Danger));
				return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
			}

			if (model.SelectedActions.OrganizationRoleTarget != 0)
			{
				// Changing organization roles
				if (!Enum.IsDefined(typeof(OrganizationRole), model.SelectedActions.OrganizationRoleTarget) && model.SelectedActions.OrganizationRoleTarget != -1)
				{
					Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
					return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
				}

				if (model.SelectedUsers.Where(tu => tu.UserId == this.AppService.UserContext.UserId).Any())
				{
					if (model.SelectedActions.OrganizationRoleTarget == -1)
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
						return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
					}
				}

				if (model.SelectedActions.OrganizationRoleTarget == -1)
				{
					int numberChanged = AppService.DeleteOrganizationUsers(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.OrganizationId);
					Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UsersRemovedFromOrg, numberChanged), Variety.Success));
				}
				else
				{
					int numberChanged = AppService.UpdateOrganizationUsersRole(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.SelectedActions.OrganizationRoleTarget.Value, model.OrganizationId);
					Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UsersChangedRolesInOrg, numberChanged), Variety.Success));
				}
			}
			else if (model.SelectedActions.TimeTrackerRoleTarget != 0)
			{
				// Changing time tracker roles
				if (!Enum.IsDefined(typeof(TimeTrackerRole), model.SelectedActions.TimeTrackerRoleTarget) && model.SelectedActions.TimeTrackerRoleTarget != -1)
				{
					Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
					return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
				}
				else if (!Enum.IsDefined(typeof(ExpenseTrackerRole), model.SelectedActions.ExpenseTrackerRoleTarget) && model.SelectedActions.ExpenseTrackerRoleTarget != -1)
				{
					Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
					return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
				}

				if (model.SelectedActions.TimeTrackerRoleTarget.Value != -1)
				{
					// TODO: instead of providing product id, provide subscription id of the subscription to be modified
					// TODO: split updating user roles and creating new sub users
					Tuple<int, int> updatedAndAdded = AppService.UpdateSubscriptionUserRoles(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.SelectedActions.TimeTrackerRoleTarget.Value, model.OrganizationId, (int)ProductIdEnum.TimeTracker);
					if (updatedAndAdded.Item1 > 0)
					{
						Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UsersChangedRolesInTimeTracker, updatedAndAdded.Item1), Variety.Success));
					}

					if (updatedAndAdded.Item2 > 0)
					{
						Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UsersAddedToTimeTracker, updatedAndAdded.Item2), Variety.Success));
					}
				}
				else
				{
					// TODO: instead of providing product id, provide subscription id of the subscription to be modified
					AppService.DeleteSubscriptionUsers(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.OrganizationId, (int)ProductIdEnum.TimeTracker);
					Notifications.Add(new BootstrapAlert(Resources.Strings.UserDeletedSuccessfully, Variety.Success));
				}
			}
			else
			{
				// Changing expense tracker roles
				if (!Enum.IsDefined(typeof(ExpenseTrackerRole), model.SelectedActions.TimeTrackerRoleTarget) && model.SelectedActions.ExpenseTrackerRoleTarget != -1)
				{
					Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
					return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
				}
				else if (!Enum.IsDefined(typeof(ExpenseTrackerRole), model.SelectedActions.ExpenseTrackerRoleTarget) && model.SelectedActions.ExpenseTrackerRoleTarget != -1)
				{
					Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
					return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
				}

				if (model.SelectedActions.ExpenseTrackerRoleTarget.Value != -1)
				{
					// TODO: instead of providing product id, provide subscription id of the subscription to be modified
					// TODO: split updating user roles and creating new sub users
					Tuple<int, int> updatedAndAdded = AppService.UpdateSubscriptionUserRoles(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.SelectedActions.ExpenseTrackerRoleTarget.Value, model.OrganizationId, (int)ProductIdEnum.ExpenseTracker);
					if (updatedAndAdded.Item1 > 0)
					{
						Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UserChangedRolesInExpenseTracker, updatedAndAdded.Item1), Variety.Success));
					}

					if (updatedAndAdded.Item2 > 0)
					{
						Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UserAddedToExpenseTracker, updatedAndAdded.Item2), Variety.Success));
					}
				}
				else
				{
					// TODO: instead of providing product id, provide subscription id of the subscription to be modified
					AppService.DeleteSubscriptionUsers(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.OrganizationId, (int)ProductIdEnum.ExpenseTracker);
					Notifications.Add(new BootstrapAlert(Resources.Strings.UserDeletedSuccessfully, Variety.Success));
				}
			}

			/*
			if (!model.isPermissions2) // TODO: Delete once there's only one manage permissions page (also delete the action constant)
			{
				return RedirectToAction(ActionConstants.ManagePermissions2);
			}*/

			return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
		}
	}
}