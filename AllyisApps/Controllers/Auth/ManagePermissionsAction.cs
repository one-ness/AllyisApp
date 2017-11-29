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
		private readonly Dictionary<int, string> organizationRoles = new Dictionary<int, string>
		{
			{ (int)OrganizationRoleEnum.Member, Strings.Member },
			{ (int)OrganizationRoleEnum.Owner, Strings.Owner }
		};

		private readonly Dictionary<string, int> setOrganizationRoles = new Dictionary<string, int>
		{
			{ Strings.RemoveOrg, -1 },
			{ Strings.SetMember, (int)OrganizationRoleEnum.Member },
			{ Strings.SetOwner, (int)OrganizationRoleEnum.Owner }
		};

		private string GetPermissionsUrl(ProductIdEnum productId, int subscripitonId)
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
					throw new ArgumentOutOfRangeException(nameof(productId), "Product id is not in system");
			}
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
			var model = JsonConvert.DeserializeObject<UserPermissionsAction>(data);
			var modelSelectedUsers = model.SelectedUsers as IList<TargetUser> ?? model.SelectedUsers.ToList(); //prevent multiple enumeration of IEnumerable

			AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, model.OrganizationId);

			if (model.SelectedUsers == null || !modelSelectedUsers.Any())
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

				if (modelSelectedUsers.Any(tu => tu.UserId == AppService.UserContext.UserId))
				{

					Notifications.Add(model.SelectedAction == -1
						? new BootstrapAlert(Strings.YouAreUnableToRemoveYourself, Variety.Danger)
						: new BootstrapAlert(Strings.YouAreUnableToChangeYourOwnRole, Variety.Danger));

					model.SelectedUsers = model.SelectedUsers.Where(tu => tu.UserId != AppService.UserContext.UserId);
					if (!modelSelectedUsers.Any())
					{
						return Redirect(model.FromUrl);
					}
				}

				if (model.SelectedAction == -1 && model.SubscriptionId == null)
				{
					try
					{
						int numberChanged = await AppService.DeleteOrganizationUsers(modelSelectedUsers.Select(tu => tu.UserId).ToList(), model.OrganizationId);
						Notifications.Add(new BootstrapAlert(string.Format(Strings.UsersRemovedFromOrg, numberChanged), Variety.Success));
					}
					catch (ArgumentNullException)
					{
						Notifications.Add(new BootstrapAlert("You must select users to remove from the organization.", Variety.Warning));
					}
					catch (ArgumentException)
					{
						Notifications.Add(new BootstrapAlert("Cannot delete yourself from an organization.", Variety.Danger));
					}
				}
				else
				{
					int numberChanged = AppService.UpdateOrganizationUsersRole(modelSelectedUsers.Select(tu => tu.UserId).ToList(), model.SelectedAction.Value, model.OrganizationId);
					Notifications.Add(new BootstrapAlert(string.Format(Strings.UsersChangedRolesInOrg, numberChanged), Variety.Success));
				}
			}
			else if (model.SelectedAction != 0 && model.SubscriptionId != null && model.ProductId != null)
			{
				//Manage Subscription Page 
				string usersModifiedMessage;
				string usersAddedMessage;
				//Varify that roleId is correct
				switch ((ProductIdEnum)model.ProductId)
				{
					case ProductIdEnum.TimeTracker:
						// Changing time tracker roles
						usersModifiedMessage = Strings.UsersChangedRolesInTimeTracker;
						usersAddedMessage = Strings.UsersAddedToTimeTracker;

						if (!Enum.IsDefined(typeof(TimeTrackerRole), model.SelectedAction) && model.SelectedAction.Value != -1)
						{
							Notifications.Add(new BootstrapAlert(Strings.YouDidNotDefineATargetRole, Variety.Danger));
							return Redirect(model.FromUrl);
						}

						break;

					case ProductIdEnum.ExpenseTracker:

						// Changing expense tracker roles
						usersModifiedMessage = Strings.UserChangedRolesInExpenseTracker;
						usersAddedMessage = Strings.UserAddedToExpenseTracker;
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
						throw new ArgumentOutOfRangeException(nameof(model.ProductId), $"Failed to find product for produtId: {model.ProductId.Value}");
				}

				int productRole = 0;

				if (model.SelectedAction.Value == -1)
				{
					productRole = 0;
				}
				else
				{
					productRole = model.SelectedAction.Value;
				}

				// TODO: instead of providing product id, provide subscription id of the subscription to be modified
				// TODO: split updating user roles and creating new sub users
				var usersUpdated = await AppService.UpdateSubscriptionUsersRoles(model.SelectedUsers.Select(tu => tu.UserId).ToList(), model.OrganizationId, productRole, model.ProductId.Value);
				if (usersUpdated > 0)
				{
					Notifications.Add(new BootstrapAlert(string.Format(usersModifiedMessage, usersUpdated), Variety.Success));
				}
			}
			return Redirect(model.FromUrl);
		}
	}
}