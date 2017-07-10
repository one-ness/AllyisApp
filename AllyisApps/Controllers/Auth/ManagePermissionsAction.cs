﻿//------------------------------------------------------------------------------
// <copyright file="ManagePermissionsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		//      /// <summary>
		//      /// GET Account/ManagePermissions2.
		//      /// </summary>
		//      /// <param name="id">The Organization Id</param>
		//      /// <returns>Action result.</returns>
		//      [HttpGet]
		//public ActionResult ManagePermissions2(int id)
		//{
		//	this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, id);
		//	PermissionsManagementViewModel model = this.ConstructPermissionsManagementViewModel(id);
		//	return this.View("Permission", model);
		//}

		/// <summary>
		/// GET Account/ManagePermissions.
		/// </summary>
		/// <param name="id">The Organization Id</param>
		/// <returns>Action result.</returns>
		[HttpGet]
		public ActionResult ManagePermissions(int id)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, id);
			var infos = AppService.GetOrgAndSubRoles(id);
			ManagePermissionsViewModel model = new ManagePermissionsViewModel
			{
				Users = new List<UserPermissionsViewModel>(),
				Subscriptions = infos.Item2,
				SubIds = infos.Item2.Select(s => s.SubscriptionId).ToList(),
				OrganizationId = id,
				// TODO: Get rid of this once product panes in Permissions page are genericized.
				TimeTrackerId = (int)ProductIdEnum.TimeTracker
			};

			// This can also be axed after finding a good way to genericize products in the Permissions page.
			var ttsub = model.Subscriptions.Where(s => s.ProductId == model.TimeTrackerId).SingleOrDefault();
			if (ttsub != null)
			{
				model.TimeTrackerSubIndex = model.Subscriptions.IndexOf(ttsub);
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
						OrgRoleId = role.OrgRoleId,
						ProductRoleIds = new List<int>()
					};

					// Start out with default TT NotInProduct role if org is subscribed to TT.
					foreach (SubscriptionDisplayInfo sub in model.Subscriptions)
					{
						modelUser.ProductRoleIds.Add((int)TimeTrackerRole.NotInProduct);
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

		/*
		/// <summary>
		/// Uses services to populate a <see cref="PermissionsManagementViewModel"/> and returns it.
		/// </summary>
        /// <param name="id">The Organization Id</param>
		/// <returns>The PermissionsManagementViewModel.</returns>
		public PermissionsManagementViewModel ConstructPermissionsManagementViewModel(int id)
		{
			PermissionsManagementViewModel result = new PermissionsManagementViewModel()
			{
				TimeTrackerId = (int)ProductIdEnum.TimeTracker
			};
			result.Subscriptions = AppService.GetSubscriptionsDisplay(id);

			List<UserPermissionsManagement> permissions = new List<UserPermissionsManagement>();
			IEnumerable<UserRolesInfo> users = AppService.GetUserRoles(id).OrderBy(u => u.UserId); // In case of multiple subscriptions, there can be multiple items per user, one for each sub role
			string currentUser = string.Empty;
			UserPermissionsManagement currentUserPerm = null;
			foreach (UserRolesInfo user in users)
			{
				if (!user.UserId.Equals(currentUser))
				{
					currentUser = user.UserId;
					currentUserPerm = new UserPermissionsManagement()
					{
						UserId = user.UserId,
						UserName = string.Format("{0} {1}", user.FirstName, user.LastName),
						OrganizationRoleId = user.OrgRoleId,
						SubscriptionRoles = new List<ProductRole>()
					};
					permissions.Add(currentUserPerm);
				}

				if (user.ProductRoleId > 0)
				{
					try
					{
						currentUserPerm.SubscriptionRoles.Add(new ProductRole
						{
							ProductRoleId = user.ProductRoleId,
							ProductId = result.Subscriptions.Where(s => s.SubscriptionId == user.SubscriptionId).Single().ProductId
						});
					}
					catch (InvalidOperationException) { } // Deleted subscription
				}
			}

			// Add in "Not in Product" roles for subscriptions each user is not assigned to
			foreach (UserPermissionsManagement permission in permissions)
			{
				foreach (SubscriptionDisplayInfo subscription in result.Subscriptions)
				{
					if (permission.SubscriptionRoles.Where(s => s.ProductId == subscription.ProductId).Count() == 0)
					{
						permission.SubscriptionRoles.Add(new ProductRole
						{
							ProductId = subscription.ProductId,
							ProductRoleId = (int)TimeTrackerRole.NotInProduct
						});
					}
				}
			}

			result.UserPermissions = permissions.DistinctBy(u => u.UserId).OrderBy(u => u.UserName.Split(' ').Last()).ToList();   // UserRoles are unique via SubscriptionId and UserId, but UserPermissionsManagement does not track SubscriptionId, causing duplicate users to be stored

			result.Filters = new FilterDataModel();
			result.Filters.UnassignedUsers = new ViewModels.Auth.Filter("Unassigned", users, x => x.ProductRoleId == 0);
			result.Filters.AllUsers = new ViewModels.Auth.Filter("All Users", users);
			FilterGroup orgFilters = result.Filters.AddNewFilterGroup("Organization");
			orgFilters.Filters.Add(new ViewModels.Auth.Filter("Owner", users, x => x.OrgRoleId == (int)OrganizationRole.Owner));
			orgFilters.Filters.Add(new ViewModels.Auth.Filter("Member", users, x => x.OrgRoleId == (int)OrganizationRole.Member));

			FilterGroup timeTrackerFilters = result.Filters.AddNewFilterGroup("TimeTracker");
			timeTrackerFilters.Filters.Add(new ViewModels.Auth.Filter("Any", users, x => x.ProductRoleId != 0));
			timeTrackerFilters.Filters.Add(new ViewModels.Auth.Filter("Manager", users, u => u.ProductRoleId == (int)TimeTrackerRole.Manager));
			timeTrackerFilters.Filters.Add(new ViewModels.Auth.Filter("User", users, u => u.ProductRoleId == (int)TimeTrackerRole.User));
			timeTrackerFilters.Filters.Add(new ViewModels.Auth.Filter("Unassigned", users, x => x.ProductRoleId == 0));

			return result;
		} */

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

			if (model.SelectedActions.OrgRoleTarget != 0)
			{
				// Changing organization roles
				if (!Enum.IsDefined(typeof(OrganizationRole), model.SelectedActions.OrgRoleTarget) && model.SelectedActions.OrgRoleTarget != -1)
				{
					Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
					return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
				}

				if (model.SelectedUsers.Where(tu => tu.UserId == this.AppService.UserContext.UserId).Any())
				{
					if (model.SelectedActions.OrgRoleTarget == -1)
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

				int numberChanged = AppService.ChangeUserRoles(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.SelectedActions.OrgRoleTarget.Value, model.OrganizationId);
				if (model.SelectedActions.OrgRoleTarget == -1)
				{
					Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UsersRemovedFromOrg, numberChanged), Variety.Success));
				}
				else
				{
					Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UsersChangedRolesInOrg, numberChanged), Variety.Success));
				}
			}
			else
			{
				// Changing time tracker roles
				if (!Enum.IsDefined(typeof(TimeTrackerRole), model.SelectedActions.TimeTrackerRoleTarget) && model.SelectedActions.TimeTrackerRoleTarget != -1)
				{
					Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDidNotDefineATargetRole, Variety.Danger));
					return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
				}

				Tuple<int, int> updatedAndAdded = AppService.ChangeSubscriptionUserRoles(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.SelectedActions.TimeTrackerRoleTarget.Value, model.OrganizationId);
				if (updatedAndAdded.Item1 == -1)
				{
					Notifications.Add(new BootstrapAlert(AllyisApps.Resources.Strings.YouDontHaveASubscriptionToTimeTracker, Variety.Danger));
					return RedirectToAction(ActionConstants.ManagePermissions, new { id = model.OrganizationId });
				}

				if (updatedAndAdded.Item1 != 0)
				{
					Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", updatedAndAdded.Item1, model.SelectedActions.TimeTrackerRoleTarget == -1 ?
						Resources.Strings.UsersRemovedFromTimeTracker : Resources.Strings.UsersChangedRolesInTimeTracker), Variety.Success));
				}

				if (updatedAndAdded.Item2 == -1)
				{
					Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.TooManyUsersInSubToAdd, model.SelectedUsers.Count()), Variety.Danger));
				}
				else
				{
					if (updatedAndAdded.Item2 != 0)
					{
						Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UsersAddedToTimeTracker, updatedAndAdded.Item2), Variety.Success));
					}
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
