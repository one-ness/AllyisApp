//------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Lookup;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.TimeTracker.Customer;
using static AllyisApps.Services.AppService;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// GET: Customer/{SubscriptionId}/Edit.
		/// </summary>
		/// <param name="subscriptionId">The Subscription Id.</param>
		/// <param name="userId">The Customer id.</param>
		/// <returns>Presents a page to edit Customer data.</returns>
		[HttpGet]
		public async Task<ActionResult> Edit(int subscriptionId, int userId)
		{
			if (AppService.UserContext.SubscriptionsAndRoles[subscriptionId].ProductId != ProductIdEnum.StaffingManager)
			{
				AppService.CheckTimeTrackerAction(TimeTrackerAction.EditCustomer, subscriptionId);
			}

			var statusOptions = new List<SelectListItem>
			{
				new SelectListItem { Text = "Active", Value = true.ToString() },
				new SelectListItem { Text = "Disabled", Value = false.ToString() }
			};

			var customer = await AppService.GetCustomerInfo(userId);
			string subscriptionNameToDisplay = await AppService.GetSubscriptionName(subscriptionId);
			return View(new EditCustomerInfoViewModel
			{
				ContactEmail = customer.ContactEmail,
				CustomerName = customer.CustomerName,
				AddressId = customer.Address?.AddressId,
				Address = customer.Address?.Address1,
				City = customer.Address?.City,
				State = customer.Address?.StateName,
				Country = customer.Address?.CountryName,
				SelectedCountryCode = customer.Address?.CountryCode,

				SelectedStateId = customer.Address?.StateId,

				IsActiveOptions = statusOptions,
				IsActive = customer.IsActive,

				PostalCode = customer.Address?.PostalCode,
				ContactPhoneNumber = customer.ContactPhoneNumber,

				FaxNumber = customer.FaxNumber,
				Website = customer.Website,
				EIN = customer.EIN,
				OrganizationId = customer.OrganizationId,
				CustomerId = userId,
				LocalizedCountries = ModelHelper.GetLocalizedCountries(AppService),
				LocalizedStates = ModelHelper.GetLocalizedStates(AppService, customer.Address?.CountryCode),

				CustomerCode = customer.CustomerCode,
				SubscriptionId = subscriptionId,
				SubscriptionName = subscriptionNameToDisplay
			});
		}

		/// <summary>
		/// POST: Customer/Edit.
		/// </summary>
		/// <param name="model">The Customer view model.</param>
		/// <returns>The ActionResult.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(EditCustomerInfoViewModel model)
		{
			// Invalid model
			if (!ModelState.IsValid) return View(model);

			var result = await AppService.UpdateCustomerAsync(
				new Customer
				{
					CustomerId = model.CustomerId,
					ContactEmail = model.ContactEmail,
					CustomerName = model.CustomerName,
					Address = new Address
					{
						Address1 = model.Address,
						AddressId = model.AddressId,
						City = model.City,
						StateId = model.SelectedStateId,
						StateName = model.State,
						CountryCode = model.SelectedCountryCode,
						CountryName = model.Country,
						PostalCode = model.PostalCode
					},
					ContactPhoneNumber = model.ContactPhoneNumber,
					FaxNumber = model.FaxNumber,
					Website = model.Website,
					EIN = model.EIN,
					IsActive = model.IsActive,
					CustomerCode = model.CustomerCode,
					OrganizationId = model.OrganizationId
				},
				model.SubscriptionId);

			switch (result)
			{
				case -1:
					// the new CustOrgId is not unique
					Notifications.Add(new BootstrapAlert(Strings.CustomerCodeNotUnique, Variety.Danger));
					return View(model);
				case -2:
					// there are still active projects underneath this customer -- can't deactivate
					Notifications.Add(new BootstrapAlert($"Cannot deactivate \"{model.CustomerName}\", dependent projects are still active.", Variety.Warning));
					return View(model);
				case 1:
					// updated successfully
					Notifications.Add(new BootstrapAlert(Strings.CustomerDetailsUpdated, Variety.Success));
					return Redirect(string.Format("{0}#customerNumber{1}", Url.Action(ActionConstants.Index, new { subscriptionId = model.SubscriptionId }), model.CustomerId));
				default:
					// Permissions failure
					Notifications.Add(new BootstrapAlert(Strings.ActionUnauthorizedMessage, Variety.Warning));
					return RedirectToAction(ActionConstants.Index, new { subscriptionId = model.SubscriptionId });
			}
		}
	}
}