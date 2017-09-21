//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers
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
		public ActionResult Index()
		{
			AccountIndexViewModel viewModel = ConstuctIndexViewModel();

			return this.View(viewModel);
		}

		/// <summary>
		/// Constuct index view Model for Accounts.
		/// </summary>
		/// <returns>The Accound Index view model.</returns>
		public AccountIndexViewModel ConstuctIndexViewModel()
		{
			User accountInfo = AppService.GetCurrentUser();

			AccountIndexViewModel.UserViewModel userViewModel = new AccountIndexViewModel.UserViewModel()
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

			AccountIndexViewModel indexViewModel = new AccountIndexViewModel()
			{
				UserInfo = userViewModel
			};

			// Add invitations to view model
			var invitationsList = accountInfo.Invitations;
			foreach (var item in invitationsList)
			{
				indexViewModel.Invitations.Add(new AccountIndexViewModel.InvitationViewModel()
				{
					InvitationId = item.InvitationId,
					OrganizationName = item.OrganizationName,
				});
			}

			// Add organizations to model
			var orgsList = accountInfo.Organizations;
			foreach (var item in orgsList)
			{
				AccountIndexViewModel.OrganizationViewModel orgViewModel =
				new AccountIndexViewModel.OrganizationViewModel()
				{
					OrganizationId = item.Organization.OrganizationId,
					OrganizationName = item.Organization.OrganizationName,
					PhoneNumber = item.Organization.PhoneNumber,
					Address1 = item.Organization.Address?.Address1,
					City = item.Organization.Address?.City,
					State = item.Organization.Address?.StateName,
					PostalCode = item.Organization.Address?.PostalCode,
					Country = item.Organization.Address?.CountryName,
					SiteUrl = item.Organization.SiteUrl,
					FaxNumber = item.Organization.FaxNumber,
					//// TODO: Infomation is dependent on curent user
					IsManageAllowed = AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, item.Organization.OrganizationId, item.OrganizationRole, false)
				};

				// Add subscription info
				foreach (var subItem in accountInfo.Subscriptions.Where(sub => sub.OrganizationId == item.Organization.OrganizationId))
				{
					string description =
						subItem.ProductId == ProductIdEnum.TimeTracker ? Resources.Strings.TimeTrackerDescription :
						subItem.ProductId == ProductIdEnum.ExpenseTracker ? Resources.Strings.ExpenseTrackerDescription :
						string.Empty;
					var subViewModel = new AccountIndexViewModel.OrganizationViewModel.SubscriptionViewModel()
					{
						ProductName = subItem.ProductName,
						SubscriptionId = subItem.SubscriptionId,
						SubscriptionName = subItem.SubscriptionName,
						ProductDescription = description,
						ProductId = subItem.ProductId,
						AreaUrl = subItem.AreaUrl
					};
					switch (subItem.ProductId)
					{
						case ProductIdEnum.TimeTracker:
							{
								int? sDate = null;
								int? eDate = null;
								int startOfWeek = AppService.GetAllSettings(subItem.SubscriptionId).Item1.StartOfWeek;
								sDate = Utility.GetDaysFromDateTime(SetStartingDate(startOfWeek));
								eDate = Utility.GetDaysFromDateTime(SetStartingDate(startOfWeek).AddDays(6));
								subViewModel.ProductGoToUrl = Url.RouteUrl(
									"TimeTracker_NoUserId",
									new
									{
										subscriptionId =
										subItem.SubscriptionId,
										controller = ControllerConstants.TimeEntry,
										startDate = sDate,
										endDate = eDate
									});
								break;
							}
						case ProductIdEnum.ExpenseTracker:
							{
								subViewModel.ProductGoToUrl = Url.RouteUrl(
									"ExpenseTracker_Default",
									new
									{
										subscriptionId = subItem.SubscriptionId,
										controller = ControllerConstants.Expense
									});
								break;
							}
						case ProductIdEnum.StaffingManager:

							subViewModel.ProductGoToUrl = Url.RouteUrl("StaffingManager_default",
								new
								{
									subscriptionId = subItem.SubscriptionId,
									controller = ControllerConstants.Staffing
								});
							break;
					}

					subViewModel.IconUrl = string.Format("Content/icons/{0}.png", subViewModel.ProductName.Replace(" ", string.Empty));
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