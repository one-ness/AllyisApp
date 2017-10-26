//------------------------------------------------------------------------------
// <copyright file="DetailsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
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
		/// Returns a details page for a customer.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <param name="customerId">Customer id.</param>
		/// <returns>The edit customer view - info on all the customer details.</returns>
		[HttpGet]
		public async Task<ActionResult> Details(int subscriptionId, int customerId)
		{
			if (AppService.UserContext.SubscriptionsAndRoles[subscriptionId].ProductId != ProductIdEnum.StaffingManager)
			{
				AppService.CheckTimeTrackerAction(TimeTrackerAction.ViewCustomer, subscriptionId);
			}

			var infos = await AppService.GetCustomerInfo(customerId);
			var customer = infos;
			return View(new EditCustomerInfoViewModel
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
				FaxNumber = infos.FaxNumber,
				Website = infos.Website,
				EIN = infos.EIN,
				OrganizationId = customer.OrganizationId,
				CustomerId = customerId,
				LocalizedCountries = ModelHelper.GetLocalizedCountries(AppService),
				CustomerOrgId = infos.CustomerOrgId,
				CanEditCustomers = AppService.CheckTimeTrackerAction(TimeTrackerAction.EditProject, subscriptionId, false)
			});
		}
	}
}