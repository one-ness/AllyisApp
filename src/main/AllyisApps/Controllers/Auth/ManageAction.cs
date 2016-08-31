﻿//------------------------------------------------------------------------------
// <copyright file="ManageAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Core;
using AllyisApps.Services.Account;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Org;
using AllyisApps.ViewModels;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Get: Account/Manage.
		/// The management page for an organization, displays billing, subscriptions, etc.
		/// </summary>
		/// <returns>The organization's management page.</returns>
		public ActionResult Manage()
		{
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization))
			{
				OrganizationManageViewModel model = this.ConstructOrganizationManageViewModel();
				return this.View(model);
			}

			Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			return this.RedirectToAction(ActionConstants.OrgIndex);
		}

		/// <summary>
		/// Uses services to populate the lists of an <see cref="OrganizationManageViewModel"/> and returns it.
		/// </summary>
		/// <returns>The OrganizationManageViewModel.</returns>
		[CLSCompliant(false)]
		public OrganizationManageViewModel ConstructOrganizationManageViewModel()
		{
			OrganizationInfo organization = OrgService.GetOrganization(UserContext.ChosenOrganizationId);
			bool canEditOrganization = AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization);
			IEnumerable<OrganizationUserViewModel> displayUsers = OrgService.GetOrganizationMemberList(UserContext.ChosenOrganizationId).Select(x => new OrganizationUserViewModel()
			{
				EmployeeId = x.EmployeeId,
				FullName = (new[] { AccountService.GetUserInfo(x.UserId) }).Select(u => string.Format("{0} {1}", u.FirstName, u.LastName)).Single(),
				OrganizationId = x.OrganizationId,
				PermissionLevel = ((OrganizationRole)x.OrgRoleId).ToString(),
				UserId = x.UserId
			});

			IEnumerable<SubscriptionDisplayInfo> subs = CrmService.GetSubscriptionsDisplay();
			IEnumerable<SubscriptionDisplayViewModel> subscriptions = CrmService.GetProductInfoList().Select(p =>
			{
				return new SubscriptionDisplayViewModel
				{
					CanEditSubscriptions = canEditOrganization,
					Info = subs.Where(x => x.ProductId == p.ProductId).SingleOrDefault(),
					ProductId = p.ProductId,
					ProductName = p.ProductName
				};
			});

			BillingServicesCustomerId customerId = CrmService.GetOrgBillingServicesCustomerId();
			BillingServicesCustomer customer = (customerId == null) ? null : CrmService.RetrieveCustomer(customerId);

			return new OrganizationManageViewModel
			{
				Add = this.ConstructOrganizationAddMembersViewModel(),
				CanEditOrganization = canEditOrganization,
				Details = organization,
				Edit = this.ConstructEditOrganizationViewModel(organization, canEditOrganization, AccountService.ValidCountries()),
				LastFour = customer == null ? string.Empty : customer.Last4,
				Members = new OrganizationMembersViewModel
				{
					AccessCode = string.Empty,
					CurrentUserId = UserContext.UserId,
					DisplayUsers = displayUsers,
					OrganizationId = UserContext.ChosenOrganizationId,
					OrganizationName = OrgService.GetOrganization(UserContext.ChosenOrganizationId).Name,
					PendingInvitation = OrgService.GetUserInvitations(),
					TotalUsers = displayUsers.Count()
				},
				OrganizationId = UserContext.ChosenOrganizationId,
				BillingCustomer = customer,
				SubscriptionCount = subscriptions.Count(),
				Subscriptions = subscriptions
			};
		}
	}
}