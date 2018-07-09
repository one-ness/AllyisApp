//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Lookup;
using AllyisApps.ViewModels.Auth;
using System.Collections.Generic;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/Index.
		/// Displays the account index page.
		/// </summary>
		/// <returns>The async task responsible for this action.</returns>
		public async Task<ActionResult> Index()
		{
			AccountIndexViewModel viewModel = await this.ConstructIndexViewModel();
			return View(viewModel);
		}

		/// <summary>
		/// Constuct index view Model for Accounts.
		/// </summary>
		/// <returns>The Accound Index view model.</returns>
		public async Task<AccountIndexViewModel> ConstructIndexViewModel()
		{
			// get current user
			User user = await AppService.GetCurrentUserAsync();

			// add to view model
			var model = new AccountIndexViewModel();
			AccountIndexViewModel.UserViewModel userViewModel = new AccountIndexViewModel.UserViewModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				PhoneExtension = user.PhoneExtension,
				Address1 = user.Address?.Address1,
				Address2 = user.Address?.Address2,
				City = user.Address?.City,
				State = user.Address?.StateName,
				PostalCode = user.Address?.PostalCode,
				Country = user.Address?.CountryName,
				ShowConvertToEmployerAccount = user.LoginProvider == LoginProviderEnum.AllyisApps,
			};

			model.UserInfo = userViewModel;

			// get list of invitations pending for user
			var invites = await this.AppService.GetCurrentUserPendingInvitationsAsync();
			// add the invitations to the view model
			foreach (var item in invites)
			{
				model.Invitations.Add(new AccountIndexViewModel.InvitationViewModel
				{
					InvitationId = item.Value.InvitationId,
					OrganizationName = item.Value.OrganizationName
				});
			}

			// get a list of organizations the user is member of, from usercontext
			List<int> ids2 = new List<int>();
			foreach (var item in this.AppService.UserContext.OrganizationsAndRoles)
			{
				ids2.Add(item.Key);
			}

			var orgs2 = await this.AppService.GetActiveOrganizationsByIdsAsync(ids2);
			foreach (var item in orgs2.Values)
			{
				AccountIndexViewModel.OrganizationViewModel orgViewModel =
				new AccountIndexViewModel.OrganizationViewModel
				{
					OrganizationId = item.OrganizationId,
					OrganizationName = item.OrganizationName,
					PhoneNumber = item.PhoneNumber,
					Address1 = item.Address?.Address1,
					City = item.Address?.City,
					State = item.Address?.StateName,
					PostalCode = item.Address?.PostalCode,
					Country = item.Address?.CountryName,
					SiteUrl = item.SiteUrl,
					Subdomain = item.Subdomain,
					FaxNumber = item.FaxNumber,
					IsCreateSubscriptionAllowed = await this.AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Create, AppService.AppEntity.Subscription, item.OrganizationId, false),
					IsReadBillingDetailsAllowed = await this.AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Read, AppService.AppEntity.Billing, item.OrganizationId, false),
					IsReadMembersListAllowed = await this.AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Read, AppService.AppEntity.OrganizationUser, item.OrganizationId, false),
					IsReadOrgDetailsAllowed = await this.AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Read, AppService.AppEntity.Organization, item.OrganizationId, false),
					IsReadPermissionsListAllowed = await this.AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Read, AppService.AppEntity.Permission, item.OrganizationId, false),
					IsReadSubscriptionsListAllowed = await this.AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Read, AppService.AppEntity.Subscription, item.OrganizationId, false)
				};

				// get a list of org's subscriptions that this user is member of
				List<int> ids3 = new List<int>();
				var subAndRoles = this.AppService.UserContext.SubscriptionsAndRoles.Values.Where(x => x.OrganizationId == item.OrganizationId && x.ProductRoleId != ProductRole.NotInProduct).ToList();
				foreach (var subItem in subAndRoles)
				{
					ids3.Add(subItem.SubscriptionId);
				}

				var subs = await this.AppService.GetActiveSubscriptionsByIdsAsync(ids3);
				foreach (var subItem in subAndRoles)
				{
					string description = string.Empty;
					var subViewModel = new AccountIndexViewModel.OrganizationViewModel.SubscriptionViewModel();
					switch (subItem.ProductId)
					{
						case ProductIdEnum.TimeTracker:
							description = Resources.Strings.TimeTrackerDescription;
							subViewModel.ProductGoToUrl = Url.RouteUrl(
								RouteNameConstants.TimeTracker,
								new
								{
									subscriptionId = subItem.SubscriptionId,
									controller = ControllerConstants.TimeEntry,
									action = ActionConstants.Index,
								});
							break;

						case ProductIdEnum.ExpenseTracker:
							description = Resources.Strings.ExpenseTrackerDescription;
							subViewModel.ProductGoToUrl = Url.RouteUrl(
								RouteNameConstants.ExpenseTracker,
								new
								{
									subscriptionId = subItem.SubscriptionId,
									controller = ControllerConstants.Expense
								});
							break;

						case ProductIdEnum.StaffingManager:
							subViewModel.ProductGoToUrl = Url.RouteUrl(
								RouteNameConstants.StaffingManager,
								new
								{
									subscriptionId = subItem.SubscriptionId,
									controller = ControllerConstants.Staffing
								});
							break;
					}

					var sub = subs[subItem.SubscriptionId];
					subViewModel.ProductName = sub.ProductName;
					subViewModel.SubscriptionId = subItem.SubscriptionId;
					subViewModel.SubscriptionName = subItem.SubscriptionName;
					subViewModel.ProductDescription = description;
					subViewModel.ProductId = subItem.ProductId;
					subViewModel.AreaUrl = sub.ProductAreaUrl;
					subViewModel.IconUrl = string.IsNullOrWhiteSpace(sub.ProductIconUrl) ? null : "~/" + sub.ProductIconUrl;
					orgViewModel.Subscriptions.Add(subViewModel);
				}

				model.Organizations.Add(orgViewModel);
			}

			return model;
		}
	}
}
