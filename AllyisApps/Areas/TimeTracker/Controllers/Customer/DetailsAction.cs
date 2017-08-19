//------------------------------------------------------------------------------
// <copyright file="DetailsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Customer;
using AllyisApps.DBModel;
using AllyisApps.ViewModels;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// Returns a details page for a customer.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <param name="customerId">Customer id.</param>
		/// <returns>The edit customer view - info on all the customer details.</returns>
		[HttpGet]
		public ActionResult Details(int subscriptionId, int customerId)
		{
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.ViewCustomer, subscriptionId);
			var infos = AppService.GetCustomerInfo(customerId);
            var customer = infos.Item1;
            return this.View(new EditCustomerInfoViewModel
            {
                ContactEmail = customer.ContactEmail,
                CustomerName = customer.CustomerName,
                Address = customer.Address?.Address1,
                City = customer.Address?.City,
                State = customer.Address?.StateName,
                SelectedStateId = customer.Address?.StateId,
                SelectedCountryCode = customer.Address?.CountryCode,
                AddressId = customer.Address?.AddressId,
                Country = customer.Address?.CountryName,
                PostalCode = customer.Address?.PostalCode,
                ContactPhoneNumber = customer.ContactPhoneNumber,
                FaxNumber = infos.Item1.FaxNumber,
                Website = infos.Item1.Website,
                EIN = infos.Item1.EIN,
                OrganizationId = customer.OrganizationId,
                CustomerId = customerId,
                LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService),
				CustomerOrgId = infos.Item1.CustomerOrgId,
				CanEditCustomers = this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId, false)
			});
		}
	}
}
