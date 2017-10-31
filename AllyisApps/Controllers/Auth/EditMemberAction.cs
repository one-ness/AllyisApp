//------------------------------------------------------------------------------
// <copyright file="EditMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.Auth;
using static AllyisApps.Services.AppService;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/EditMember.
		/// </summary>
		public async Task<ActionResult> EditMember(int id, int userId)
		{
			var model = await ConstructViewModel(id, userId);
			return View("editmember", model);
		}

		private async Task<EditMemberViewModel> ConstructViewModel(int orgId, int userId)
		{
			User user = await AppService.GetUserAsync(userId, orgId, OrgAction.ReadOrganization); // this call makes sure that both logged in user and userId have at least one common org
			var org = user.Organizations.Where(x => x.OrganizationId == orgId).FirstOrDefault();
			var model = new EditMemberViewModel();

			model.CanEditMember = AppService.CheckOrgAction(AppService.OrgAction.EditUser, orgId, false);

			model.Address = user.Address?.Address1;
			model.City = user.Address?.City;
			model.CountryName = user.Address?.CountryName;
			model.DateOfBirth = user.DateOfBirth == null ? string.Empty : user.DateOfBirth.Value.ToString("d");
			model.Email = user.Email;
			model.EmployeeId = org.EmployeeId;
			model.FirstName = user.FirstName;
			model.LastName = user.LastName;
			model.OrganizationId = orgId;
			model.OrganizationName = org.OrganizationName;
			model.OrgRolesList = ModelHelper.GetOrgRolesList();
			model.PhoneNumber = user.PhoneNumber;
			model.PostalCode = user.Address?.PostalCode;

			// get all subscriptions of this organization, get a list of roles for each subscription and user's role in each subscription
			var subs = await AppService.GetSubscriptionsAsync(model.OrganizationId, true);
			foreach (var item in subs)
			{
				// note: selectedRoleId = 0 means Unassigned or NotInProduct
				int selectedRoleId = 0;
				var sub = user.Subscriptions.Where(x => x.SubscriptionId == item.SubscriptionId).FirstOrDefault();
				if (sub != null)
				{
					// user is part of this subscription
					selectedRoleId = sub.ProductRoleId;
				}

				if (item.ProductId == ProductIdEnum.TimeTracker)
				{
					model.SubscriptionRoles.Add(new EditMemberViewModel.RoleItem
					{
						RoleList = ModelHelper.GetTimeTrackerRolesList(),
						SelectedRoleId = selectedRoleId,
						SubscriptionId = item.SubscriptionId,
						SubscriptionName = item.SubscriptionName
					});
				}
				else if (item.ProductId == ProductIdEnum.ExpenseTracker)
				{
					model.SubscriptionRoles.Add(new EditMemberViewModel.RoleItem
					{
						RoleList = ModelHelper.GetExpenseTrackerRolesList(),
						SelectedRoleId = selectedRoleId,
						SubscriptionId = item.SubscriptionId,
						SubscriptionName = item.SubscriptionName
					});
				}
				else if (item.ProductId == ProductIdEnum.StaffingManager)
				{
					model.SubscriptionRoles.Add(new EditMemberViewModel.RoleItem
					{
						RoleList = ModelHelper.GetStaffingManagerRolesList(),
						SelectedRoleId = selectedRoleId,
						SubscriptionId = item.SubscriptionId,
						SubscriptionName = item.SubscriptionName
					});
				}
			}

			model.SelectedOrganizationRoleId = (int)org.OrganizationRole;
			model.StateName = user.Address?.StateName;
			model.UserId = userId;
			return model;
		}

		/// <summary>
		/// edit member
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditMember(EditMemberViewModel model)
		{
			if (ModelState.IsValid)
			{
				// TODO: do this in a transaction

				// update employee id 
				var result = await AppService.UpdateEmployeeIdAndOrgRole(model.OrganizationId, model.UserId, model.EmployeeId, (OrganizationRoleEnum)model.SelectedOrganizationRoleId);
				if (result == UpdateEmployeeIdAndOrgRoleResult.CannotSelfUpdateOrgRole)
				{
					Notifications.Add(new Core.Alert.BootstrapAlert(Strings.CannotSelfUpdateOrgRole, Core.Alert.Variety.Danger));
				}
				else if (result == UpdateEmployeeIdAndOrgRoleResult.EmployeeIdNotUnique)
				{
					Notifications.Add(new Core.Alert.BootstrapAlert(Strings.EmployeeIdNotUniqueError, Core.Alert.Variety.Danger));
				}
				else
				{
					// get the subscription roles in to a dictionary
					Dictionary<int, int> changedsubRoles = new Dictionary<int, int>();
					List<int> removedSubRole = new List<int>();
					foreach (var item in model.SubscriptionRoles)
					{
						// note: selectedRoleId = 0 means Unassigned or NotInProduct
						if (item.SelectedRoleId > 0)
						{
							//role changed
							changedsubRoles.Add(item.SubscriptionId, item.SelectedRoleId);
						}
						else
						{
							//removed from subscription 
							removedSubRole.Add(item.SubscriptionId);
						}
					}

					// update the subscription roles
					if (changedsubRoles.Count > 0)
					{
						await this.AppService.UpdateSubscriptionUserRoles(model.UserId, changedsubRoles);
					}

					foreach (var subtoRemove in removedSubRole)
					{
						await this.AppService.DeleteSubscriptionUser(subtoRemove, model.UserId);
					}

					return RedirectToAction(ActionConstants.OrganizationMembers, ControllerConstants.Account, new { @id = model.OrganizationId });
				}
			}

			return RedirectToAction("EditMember", new { id = model.OrganizationId, userid = model.UserId }); 
			
		}
	}
}
