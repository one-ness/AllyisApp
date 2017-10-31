//------------------------------------------------------------------------------
// <copyright file="EditMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.Auth;

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
			User user = await AppService.GetUserAsync(userId); // this call makes sure that both logged in user and userId have at least one common org
			var org = user.Organizations.Single(x => x.OrganizationId == orgId);
			var subs = await AppService.GetSubscriptionsAsync(orgId);

			return new EditMemberViewModel
			{
				CanEditMember = AppService.CheckOrgAction(AppService.OrgAction.EditUser, orgId, false),
				Address = user.Address?.Address1,
				City = user.Address?.City,
				CountryName = user.Address?.CountryName,
				DateOfBirth = user.DateOfBirth?.ToString("d") ?? string.Empty,
				Email = user.Email,
				EmployeeId = org.EmployeeId,
				FirstName = user.FirstName,
				LastName = user.LastName,
				OrganizationId = orgId,
				OrganizationName = org.OrganizationName,
				OrgRolesList = ModelHelper.GetOrgRolesList(),
				PhoneNumber = user.PhoneNumber,
				PostalCode = user.Address?.PostalCode,
				SubscriptionRoles = subs
					.Select(sub => new EditMemberViewModel.RoleItem
					{
						RoleList = ModelHelper.GetRolesList(sub.ProductId),
						SelectedRoleId = user.Subscriptions.FirstOrDefault(x => x.SubscriptionId == sub.SubscriptionId)?.ProductRoleId ?? 0,
						SubscriptionId = sub.SubscriptionId,
						SubscriptionName = sub.SubscriptionName
					})
					.ToList(),
				SelectedOrganizationRoleId = (int)org.OrganizationRole,
				StateName = user.Address?.StateName,
				UserId = userId
			};
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
						var subRoles = model.SubscriptionRoles.Where(item => item.SelectedRoleId > 0).ToDictionary(item => item.SubscriptionId, item => item.SelectedRoleId);

						// update the subscription roles
						if (subRoles.Count <= 0) await AppService.UpdateSubscriptionUserRoles(model.UserId, subRoles);
						return RedirectToAction(ActionConstants.OrganizationMembers, ControllerConstants.Account, new { id = model.OrganizationId });
					default:
						throw new ArgumentOutOfRangeException(nameof(result));
				}
			}

			// error, copy values from existing model
			var newModel = await ConstructViewModel(model.OrganizationId, model.UserId);
			newModel.SelectedOrganizationRoleId = model.SelectedOrganizationRoleId;
			foreach (EditMemberViewModel.RoleItem item in model.SubscriptionRoles)
			{
				EditMemberViewModel.RoleItem sub = newModel.SubscriptionRoles.FirstOrDefault(x => x.SubscriptionId == item.SubscriptionId);
				if (sub != null)
				{
					sub.SelectedRoleId = item.SelectedRoleId;
				}
			}

			return View(model);
		}
	}
}
