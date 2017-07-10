//------------------------------------------------------------------------------
// <copyright file="DetailsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Customer;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// Returns a details page for a customer
		/// </summary>
		[HttpGet]
		public ActionResult Details(int subscriptionId, int id)
		{
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.ViewCustomer, subscriptionId);
			var infos = AppService.GetCustomerAndCountries(id);
			return this.View(new EditCustomerInfoViewModel
			{
				ContactEmail = infos.Item1.ContactEmail,
				Name = infos.Item1.Name,
				Address = infos.Item1.Address,
				City = infos.Item1.City,
				State = infos.Item1.State,
				Country = infos.Item1.Country,
				PostalCode = infos.Item1.PostalCode,
				ContactPhoneNumber = infos.Item1.ContactPhoneNumber,
				FaxNumber = infos.Item1.FaxNumber,
				Website = infos.Item1.Website,
				EIN = infos.Item1.EIN,
				OrganizationId = infos.Item1.OrganizationId,
				CustomerID = id,
				ValidCountries = infos.Item2,
				CustomerOrgId = infos.Item1.CustomerOrgId,
				canEditCustomers = this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId, false)
			});
		}
	}
}
