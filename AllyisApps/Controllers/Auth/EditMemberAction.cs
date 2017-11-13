//------------------------------------------------------------------------------
// <copyright file="EditMemberAction.cs" company="Allyis, Inc.">
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
using AllyisApps.Services.Auth;
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
			return View(ActionConstants.EditMember, model);
		}

		private async Task<EditMemberViewModel> ConstructViewModel(int orgId, int userId)
		{
			// get all subscriptions of this organization, get a list of roles for each subscription and user's role in each subscription
			var subs = await AppService.GetSubscriptionsAsync(orgId, true);
			User user = await AppService.GetUserAsync(userId, orgId, OrgAction.ReadOrganization); // this call makes sure that both logged in user and userId have at least one common org
			UserOrganization org = user.Organizations.FirstOrDefault(x => x.OrganizationId == orgId);

			var model = new EditMemberViewModel
			{
				CanEditMember = AppService.CheckOrgAction(OrgAction.EditUser, orgId, false),
				Address = user.Address?.Address1,
				City = user.Address?.City,
				CountryName = user.Address?.CountryName,
				DateOfBirth = user.DateOfBirth.ToString("d"),
				Email = user.Email,
				EmployeeId = org.EmployeeId,
				FirstName = user.FirstName,
				LastName = user.LastName,
				OrganizationId = orgId,
				OrganizationName = org.OrganizationName,
				OrgRolesList = ModelHelper.GetOrgRolesList(),
				PhoneNumber = user.PhoneNumber,
				PostalCode = user.Address?.PostalCode,
				SubscriptionRoles = subs.Select(sub => new EditMemberViewModel.RoleItem
				{
					RoleList = ModelHelper.GetRolesList(sub.ProductId),
					SelectedRoleId =
						user.Subscriptions.FirstOrDefault(x => x.SubscriptionId == sub.SubscriptionId)?.ProductRoleId ?? 0,
					SubscriptionId = sub.SubscriptionId,
					SubscriptionName = sub.SubscriptionName
				}).ToList(),
				SelectedOrganizationRoleId = (int)org.OrganizationRole,
				StateName = user.Address?.StateName,
				UserId = userId
			};

			return model;
		}

		/// <summary>
		/// edit member
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditMember(EditMemberViewModel model)
		{
			if (!ModelState.IsValid) return RedirectToAction(ActionConstants.EditMember, new { id = model.OrganizationId, userid = model.UserId });
			// TODO: do this in a transaction

			// update employee id 
			UpdateEmployeeIdAndOrgRoleResult result = await AppService.UpdateEmployeeIdAndOrgRole(model.OrganizationId, model.UserId, model.EmployeeId, (OrganizationRoleEnum)model.SelectedOrganizationRoleId);
			switch (result)
			{
				case UpdateEmployeeIdAndOrgRoleResult.CannotSelfUpdateOrgRole:
					Notifications.Add(new BootstrapAlert(Strings.CannotSelfUpdateOrgRole, Variety.Danger));
					break;
				case UpdateEmployeeIdAndOrgRoleResult.EmployeeIdNotUnique:
					Notifications.Add(new BootstrapAlert(Strings.EmployeeIdNotUniqueError, Variety.Danger));
					break;
				case UpdateEmployeeIdAndOrgRoleResult.Success:
					// get the subscription roles in to a dictionary
					var changedsubRoles = new Dictionary<int, int>();
					var removedSubRole = new List<int>();
					foreach (EditMemberViewModel.RoleItem item in model.SubscriptionRoles)
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
						await AppService.UpdateSubscriptionUserRoles(model.UserId, changedsubRoles);
					}

					foreach (int subtoRemove in removedSubRole)
					{
						await AppService.DeleteSubscriptionUser(subtoRemove, model.UserId);
					}

					return RedirectToAction(ActionConstants.OrganizationMembers, ControllerConstants.Account, new { id = model.OrganizationId });
				default:
					throw new ArgumentOutOfRangeException(nameof(result));
			}

			return RedirectToAction("EditMember", new { id = model.OrganizationId, userid = model.UserId });

		}
	}
}
