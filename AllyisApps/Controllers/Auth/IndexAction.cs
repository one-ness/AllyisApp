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
			AccountIndexViewModel viewModel = await ConstuctIndexViewModel();

			return View(viewModel);
		}

		/// <summary>
		/// Constuct index view Model for Accounts.
		/// </summary>
		/// <returns>The Accound Index view model.</returns>
		public async Task<AccountIndexViewModel> ConstuctIndexViewModel()
		{
			// get current user
			User2 user = await AppService.GetCurrentUser2Async();

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
			};

			model.UserInfo = userViewModel;

			// add current user invitations to view model
			var invites = await this.AppService.GetCurrentUserInvitationsAsync();
			if (invites.Count > 0)
			{
				// get the list of organizations for the invite, if they are active
				var orgs1 = await this.AppService.GetOrganizationsByIdsAsync(invites.Keys.ToList<int>());
				// set the org name for each invite
				foreach (var item in orgs1)
				{
					Invitation temp = null;
					if (invites.TryGetValue(item.Value.OrganizationId, out temp))
					{
						temp.OrganizationName = item.Value.OrganizationName;
					}
					else
					{
						// invitation is pending for user, but somehow the organization doesn't exist
						// (may be the org was deleted in the meantime)
						// remove the invite from the list
						invites.Remove(item.Value.OrganizationId);
					}
				}
			}

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

			var orgs2 = await this.AppService.GetOrganizationsByIdsAsync(ids2);
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
					FaxNumber = item.FaxNumber,
					IsCreateSubscriptionAllowed = this.AppService.CheckOrgAction(AllyisApps.Services.AppService.OrgAction.CreateSubscription, item.OrganizationId, false),
					IsReadBillingDetailsAllowed = this.AppService.CheckOrgAction(AllyisApps.Services.AppService.OrgAction.ReadBilling, item.OrganizationId, false),
					IsReadMembersListAllowed = this.AppService.CheckOrgAction(AllyisApps.Services.AppService.OrgAction.ReadUsersList, item.OrganizationId, false),
					IsReadOrgDetailsAllowed = this.AppService.CheckOrgAction(AllyisApps.Services.AppService.OrgAction.ReadOrganization, item.OrganizationId, false),
					IsReadPermissionsListAllowed = this.AppService.CheckOrgAction(AllyisApps.Services.AppService.OrgAction.ReadPermissions, item.OrganizationId, false),
					IsReadSubscriptionsListAllowed = this.AppService.CheckOrgAction(AllyisApps.Services.AppService.OrgAction.ReadSubscriptions, item.OrganizationId, false)
				};

				// get a list of org's subscriptions that this user is member of
				List<int> ids3 = new List<int>();
				var subAndRoles = this.AppService.UserContext.SubscriptionsAndRoles.Values.Where(x => x.OrganizationId == item.OrganizationId && x.ProductRoleId != 0).ToList();
				foreach (var subItem in subAndRoles)
				{
					ids3.Add(subItem.SubscriptionId);
				}

				var subs = await this.AppService.GetSubscriptionsByIdsAsync(ids3);
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
							subViewModel.ProductGoToUrl = Url.RouteUrl("StaffingManager_default",
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
					subViewModel.IconUrl = string.IsNullOrWhiteSpace(sub.SkuIconUrl) ? null : "~/" + sub.SkuIconUrl;
					orgViewModel.Subscriptions.Add(subViewModel);
				}

				model.Organizations.Add(orgViewModel);
			}

			return model;
		}

		/// <summary>
		/// Gets the Starting date from an int value.
		/// </summary>
		/// <param name="startOfWeek">Integer value representing a date.</param>
		/// <returns>A datetime object.</returns>
		private DateTime SetStartingDate(int startOfWeek)
		{
			DateTime today = DateTime.Now;
			int daysIntoTheWeek = (int)today.DayOfWeek < startOfWeek
				? (int)today.DayOfWeek + (7 - startOfWeek)
				: (int)today.DayOfWeek - startOfWeek;
			return today.AddDays(-daysIntoTheWeek);
		}
	}
}
