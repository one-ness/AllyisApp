//------------------------------------------------------------------------------
// <copyright file="EditMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using System.Linq;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.Auth;
using System.Threading.Tasks;
using AllyisApps.Services;
using AllyisApps.ViewModels;
using AllyisApps.Services.Billing;

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
			User user = await AppService.GetUserAsync(userId); // this call makes sure that both logged in user and userId have at least one common org
			var org = user.Organizations.Where(x => x.OrganizationId == id).FirstOrDefault();
			var model = new EditMemberViewModel();
			model.CanEditMember = this.AppService.CheckOrgAction(AppService.OrgAction.EditUser, id, false);
			model.Address = user.Address?.Address1;
			model.City = user.Address?.City;
			model.CountryName = user.Address?.CountryName;
			model.DateOfBirth = user.DateOfBirth == null ? string.Empty : user.DateOfBirth.Value.ToString("d");
			model.Email = user.Email;
			model.EmployeeId = org.EmployeeId;
			model.FirstName = user.FirstName;
			model.LastName = user.LastName;
			model.OrganizationId = id;
			model.OrganizationName = org.OrganizationName;
			model.OrgRolesList = ModelHelper.GetOrgRolesList();
			model.PhoneNumber = user.PhoneNumber;
			model.PostalCode = user.Address?.PostalCode;
			await this.PopulateRoles(model, user);
			model.SelectedOrganizationRoleId = (int)org.OrganizationRole;
			model.StateName = user.Address?.StateName;
			model.UserId = userId;
			return View("editmember", model);
		}

		private async Task PopulateRoles(EditMemberViewModel model, User user)
		{
			// get all subscriptions of this organization, get a list of roles for each subscription and user's role in each subscription
			var subs = await this.AppService.GetSubscriptionsAsync(model.OrganizationId);
			foreach (var item in subs)
			{
				if (item.ProductId == ProductIdEnum.TimeTracker)
				{
					model.SubscriptionRoles.Add(new EditMemberViewModel.RoleItem()
					{
						RoleList = ModelHelper.GetTimeTrackerRolesList(),
						SelectedRoleId = user.Subscriptions.Where(x => x.SubscriptionId == item.SubscriptionId).FirstOrDefault().ProductRoleId,
						SubscriptionId = item.SubscriptionId,
						SubscriptionName = item.SubscriptionName
					});
				}
				else if (item.ProductId == ProductIdEnum.ExpenseTracker)
				{
					model.SubscriptionRoles.Add(new EditMemberViewModel.RoleItem()
					{
						RoleList = ModelHelper.GetExpenseTrackerRolesList(),
						SelectedRoleId = user.Subscriptions.Where(x => x.SubscriptionId == item.SubscriptionId).FirstOrDefault().ProductRoleId,
						SubscriptionId = item.SubscriptionId,
						SubscriptionName = item.SubscriptionName
					});
				}
				else if (item.ProductId == ProductIdEnum.StaffingManager)
				{
					model.SubscriptionRoles.Add(new EditMemberViewModel.RoleItem()
					{
						RoleList = ModelHelper.GetStaffingManagerRolesList(),
						SelectedRoleId = user.Subscriptions.Where(x => x.SubscriptionId == item.SubscriptionId).FirstOrDefault().ProductRoleId,
						SubscriptionId = item.SubscriptionId,
						SubscriptionName = item.SubscriptionName
					});
				}
			}
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
				foreach (var item in model.SubscriptionRoles)
				{
					if (item.SelectedRoleId > 0)
					{

					}
				}
			}

			// error
			User user = await AppService.GetUserAsync(model.UserId); // this call makes sure that both logged in user and userId have at least one common org
			await this.PopulateRoles(model, user);
			return View(model);
		}
	}
}