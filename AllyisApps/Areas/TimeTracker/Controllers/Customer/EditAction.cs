﻿//------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
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
		public ActionResult Edit(int subscriptionId, int userId)
		{
			if (AppService.UserContext.SubscriptionsAndRoles[subscriptionId].ProductId != ProductIdEnum.StaffingManager)
			{
				AppService.CheckTimeTrackerAction(TimeTrackerAction.EditCustomer, subscriptionId);
			}

			var customer = AppService.GetCustomerInfo(userId);
			string subscriptionNameToDisplay = AppService.getSubscriptionName(subscriptionId);
			return this.View(new EditCustomerInfoViewModel
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

				PostalCode = customer.Address?.PostalCode,
				ContactPhoneNumber = customer.ContactPhoneNumber,

				FaxNumber = customer.FaxNumber,
				Website = customer.Website,
				EIN = customer.EIN,
				OrganizationId = customer.OrganizationId,
				CustomerId = userId,
				LocalizedCountries = ModelHelper.GetLocalizedCountries(AppService),
				LocalizedStates = ModelHelper.GetLocalizedStates(AppService, customer.Address?.CountryCode),

				CustomerOrgId = customer.CustomerOrgId,
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
		public ActionResult Edit(EditCustomerInfoViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = AppService.UpdateCustomer(
					new Customer()
					{
						CustomerId = model.CustomerId,
						ContactEmail = model.ContactEmail,
						CustomerName = model.CustomerName,
						Address = new Address()
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
						CustomerOrgId = model.CustomerOrgId,
						OrganizationId = model.OrganizationId
					},
					model.SubscriptionId);

				if (result == -1)
				{
					// the new CustOrgId is not unique
					Notifications.Add(new BootstrapAlert(Resources.Strings.CustomerOrgIdNotUnique, Variety.Danger));
					return this.View(model);
				}
				else if (result == 1)
				{
					// updated successfully
					Notifications.Add(new BootstrapAlert(Resources.Strings.CustomerDetailsUpdated, Variety.Success));
					return this.Redirect(string.Format("{0}#customerNumber{1}", Url.Action(ActionConstants.Index, new { subscriptionId = model.SubscriptionId }), model.CustomerId));
				}
				else
				{
					// Permissions failure
					Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
					return this.RedirectToAction(ActionConstants.Index, new { subscriptionId = model.SubscriptionId });
				}
			}

			// Invalid model
			return this.View(model);
		}
	}
}