//------------------------------------------------------------------------------
// <copyright file="ManageAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;

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
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				OrganizationManageViewModel model = this.ConstructOrganizationManageViewModel();
				return this.View(model);
			}

			Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			return this.RedirectToAction(ActionConstants.Organizations);
		}

		/// <summary>
		/// Uses services to populate the lists of an <see cref="OrganizationManageViewModel"/> and returns it.
		/// </summary>
		/// <returns>The OrganizationManageViewModel.</returns>
		[CLSCompliant(false)]
		public OrganizationManageViewModel ConstructOrganizationManageViewModel()
		{
			OrganizationInfo organization = Service.GetOrganization(UserContext.ChosenOrganizationId);
			bool canEditOrganization = Service.Can(Actions.CoreAction.EditOrganization);
			IEnumerable<OrganizationUserViewModel> displayUsers = Service.GetOrganizationMemberList(UserContext.ChosenOrganizationId).Select(x => new OrganizationUserViewModel()
			{
				EmployeeId = x.EmployeeId,
				FullName = (new[] { Service.GetUserInfo(x.UserId) }).Select(u => string.Format("{0} {1}", u.FirstName, u.LastName)).Single(),
				OrganizationId = x.OrganizationId,
				PermissionLevel = ((OrganizationRole)x.OrgRoleId).ToString(),
				UserId = x.UserId
			});

			IEnumerable<SubscriptionDisplayInfo> subs = Service.GetSubscriptionsDisplay();
			IEnumerable<SubscriptionDisplayViewModel> subscriptions = Service.GetProductInfoList().Select(p =>
			{
				return new SubscriptionDisplayViewModel
				{
					CanEditSubscriptions = canEditOrganization,
					Info = subs.Where(x => x.ProductId == p.ProductId).SingleOrDefault(),
					ProductId = p.ProductId,
					ProductName = p.ProductName
				};
			});

			BillingServicesCustomerId customerId = Service.GetOrgBillingServicesCustomerId();
			BillingServicesCustomer customer = (customerId == null) ? null : Service.RetrieveCustomer(customerId);

			return new OrganizationManageViewModel
			{
				Add = this.ConstructOrganizationAddMembersViewModel(),
				CanEditOrganization = canEditOrganization,
				Details = organization,
				Edit = this.ConstructEditOrganizationViewModel(organization, canEditOrganization, Service.ValidCountries()),
				LastFour = customer == null ? string.Empty : customer.Last4,
				Members = new OrganizationMembersViewModel
				{
					AccessCode = string.Empty,
					CurrentUserId = UserContext.UserId,
					DisplayUsers = displayUsers,
					OrganizationId = UserContext.ChosenOrganizationId,
					OrganizationName = Service.GetOrganization(UserContext.ChosenOrganizationId).Name,
					PendingInvitation = Service.GetUserInvitations(),
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