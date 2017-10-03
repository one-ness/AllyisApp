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
		private Dictionary<int, string> organizationRoles = new Dictionary<int, string>
		{
		{ (int)OrganizationRole.Member, Strings.Member },
		{ (int)OrganizationRole.Owner, Strings.Owner }
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
		{ Strings.SetMember, (int)OrganizationRole.Member },
		{ Strings.SetOwner, (int)OrganizationRole.Owner }
	};

		private Dictionary<string, int> setTTRoles = new Dictionary<string, int>
	{
		{ Strings.RemoveFromSubscription, 0 },
		{ Strings.SetUser, (int)TimeTrackerRole.User },
		{ Strings.SetManager, (int)TimeTrackerRole.Manager }
	};

		private Dictionary<string, int> setETRoles = new Dictionary<string, int>
	{
		{ Strings.RemoveFromSubscription, 0},
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
		public ActionResult ManagePermissions(int id)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id);
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

			return this.View("Permission2", model);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="id">Organizaion Id.</param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult ManageOrgPermissions(int id)
		{
			//Get OrganizaionUser Rows
			AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id);
			var orgUsers = AppService.GetOrganizationMemberList(id);
			var orgSubs = AppService.GetSubscriptionsByOrg(id);

			PermissionsViewModel perModel = new PermissionsViewModel()
			{
				Actions = setOrganizationRoles,
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
					currentRoleName = organizationRoles[orgU.OrganizationRoleId],
					Email = orgU.Email,
					FullName = orgU.FirstName + " " + orgU.LastName,
					UserID = orgU.UserId,
					isChecked = false
				}).OrderBy(orgU => orgU.FullName).ToList()
			};

			//
			return this.View("PermissionsOrg", perModel);
		}

		/// <summary>
		/// Get page to edit SubscriptionPermissions
		/// </summary>
		/// <param name="id">Subscription ID</param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult ManageSubPermissions(int id)
		{
			var sub = AppService.GetSubscription(id);
			var orgSubs = AppService.GetSubscriptionsByOrg(sub.OrganizationId);
			var subUsers = AppService.GetSubscriptionUsers(id);

			var orgUsers = AppService.GetOrganizationMemberList(sub.OrganizationId);

			List<UserPermssionViewModel> SubUsers = subUsers.Select(subU => new UserPermssionViewModel()
			{
				currentRole = subU.UserId
			})

			List<UserPermssionViewModel> OrgUsers = orgUsers.Select(orgU => new UserPermssionViewModel()
			{
				currentRole = (int)ProductRole.NotInProduct,
				currentRoleName = Strings.Unassigned,
				FullName = orgU.FirstName + " " + orgU.LastName,
				Email = orgU.Email,
				isChecked = false,
				UserID = orgU.UserId
			}).OrderBy(orgU => orgU.FullName).ToList();

			if (sub.ProductId == ProductIdEnum.StaffingManager)
			{
				Notifications.Add(new BootstrapAlert("StaffingManager is not yet supported", Variety.Danger));
			}

			return null;
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

			if (model.SelectedAction == null)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.NoActionsSelected, Variety.Danger));
				return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
			}

			if (model.SubscriptionId == null && model.OrganizationId != 0)
			{
				// Changing organization roles
				if (!Enum.IsDefined(typeof(OrganizationRole), model.SelectedAction) && model.SelectedAction != -1)
				{
					Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
					return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
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
						return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
					}
				}

				if (model.SelectedAction == -1 && model.SubscriptionId == null)
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
			else if (model.SelectedAction != 0 && model.SubscriptionId != null && model.ProductId != null)
			{
				//Varify that roleId is correct
				switch ((ProductIdEnum)model.ProductId)
				{
					case ProductIdEnum.TimeTracker:
						// Changing time tracker roles
						if (!Enum.IsDefined(typeof(TimeTrackerRole), model.SelectedAction) && model.SelectedAction.Value != -1)
						{
							Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
							return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
						}

						break;

					case ProductIdEnum.ExpenseTracker:
						// Changing expense tracker roles
						if (!Enum.IsDefined(typeof(ExpenseTrackerRole), model.SelectedAction.Value) && model.SelectedAction != -1)
						{
							Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
							return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
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
					var updatedAndAdded = AppService.UpdateSubscriptionUserRoles(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.SelectedAction.Value, model.OrganizationId, model.ProductId);
					if (updatedAndAdded.UsersChanged > 0)
					{
						Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UsersChangedRolesInTimeTracker, updatedAndAdded.UsersChanged), Variety.Success));
					}

					if (updatedAndAdded.UsersAddedToSubscription > 0)
					{
						Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UsersAddedToTimeTracker, updatedAndAdded.UsersAddedToSubscription), Variety.Success));
					}
				}
				else
				{
					// TODO: instead of providing product id, provide subscription id of the subscription to be modified
					AppService.DeleteSubscriptionUsers(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.OrganizationId, model.ProductId.Value);
					Notifications.Add(new BootstrapAlert(Resources.Strings.UserDeletedSuccessfully, Variety.Success));
				}
			}
			return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
		}
	}
}