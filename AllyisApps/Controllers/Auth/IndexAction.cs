//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Lookup;
using AllyisApps.ViewModels.Auth;

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
			User accountInfo = await AppService.GetCurrentUserAsync();

			AccountIndexViewModel.UserViewModel userViewModel = new AccountIndexViewModel.UserViewModel
			{
				FirstName = accountInfo.FirstName,
				LastName = accountInfo.LastName,
				Email = accountInfo.Email,
				PhoneNumber = accountInfo.PhoneNumber,
				PhoneExtension = accountInfo.PhoneExtension,
				Address1 = accountInfo.Address?.Address1,
				Address2 = accountInfo.Address?.Address2,
				City = accountInfo.Address?.City,
				State = accountInfo.Address?.StateName,
				PostalCode = accountInfo.Address?.PostalCode,
				Country = accountInfo.Address?.CountryName,
			};

			AccountIndexViewModel indexViewModel = new AccountIndexViewModel
			{
				UserInfo = userViewModel
			};

			// Add invitations to view model
			var invitationsList = accountInfo.Invitations;
			foreach (var item in invitationsList)
			{
				indexViewModel.Invitations.Add(new AccountIndexViewModel.InvitationViewModel
				{
					InvitationId = item.InvitationId,
					OrganizationName = item.OrganizationName
				});
			}

			// Add organizations to model
			var orgsList = accountInfo.Organizations;
			foreach (var item in orgsList)
			{
				State state = AppService.GetStates(item.Address.CountryCode).Where(s => s.StateId == item.Address.StateId).FirstOrDefault();
				Country country = AppService.GetCountries().Where(c => c.Key == item.Address.CountryCode).FirstOrDefault().Value;

				AccountIndexViewModel.OrganizationViewModel orgViewModel =
				new AccountIndexViewModel.OrganizationViewModel
				{
					OrganizationId = item.OrganizationId,
					OrganizationName = item.OrganizationName,
					PhoneNumber = item.PhoneNumber,
					Address1 = item.Address?.Address1,
					City = item.Address?.City,
					State = state?.StateName,
					PostalCode = item.Address?.PostalCode,
					Country = country?.CountryName,
					SiteUrl = item.SiteUrl,
					FaxNumber = item.FaxNumber,
					IsCreateSubscriptionAllowed = AppService.CheckOrgAction(AppService.OrgAction.CreateSubscription, item.OrganizationId, false),
					IsReadBillingDetailsAllowed = AppService.CheckOrgAction(AppService.OrgAction.ReadBilling, item.OrganizationId, false),
					IsReadMembersListAllowed = AppService.CheckOrgAction(AppService.OrgAction.ReadUsersList, item.OrganizationId, false),
					IsReadOrgDetailsAllowed = AppService.CheckOrgAction(AppService.OrgAction.ReadOrganization, item.OrganizationId, false),
					IsReadPermissionsListAllowed = AppService.CheckOrgAction(AppService.OrgAction.ReadPermissions, item.OrganizationId, false),
					IsReadSubscriptionsListAllowed = AppService.CheckOrgAction(AppService.OrgAction.ReadSubscriptions, item.OrganizationId, false)
				};

				// Add subscription info
				foreach (var subItem in accountInfo.Subscriptions
					.Where(sub => sub.OrganizationId == item.OrganizationId)
					.OrderBy(sub => sub.ProductId))
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

					subViewModel.ProductName = subItem.ProductName;
					subViewModel.SubscriptionId = subItem.SubscriptionId;
					subViewModel.SubscriptionName = subItem.SubscriptionName;
					subViewModel.ProductDescription = description;
					subViewModel.ProductId = subItem.ProductId;
					subViewModel.AreaUrl = subItem.ProductAreaUrl;
					subViewModel.IconUrl = subItem.SkuIconUrl == null ? null : "~/" + subItem.SkuIconUrl;
					orgViewModel.Subscriptions.Add(subViewModel);
				}

				indexViewModel.Organizations.Add(orgViewModel);
			}

			return indexViewModel;
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