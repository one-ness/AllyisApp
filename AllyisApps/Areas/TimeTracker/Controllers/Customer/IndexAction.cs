//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Customer;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// GET: /TimeTracker/Customer/subscriptionId
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// Action for the customers page.
		/// </summary>
		/// <param name="subscriptionId">The current subscription id.</param>
		/// <returns>The Customer page.</returns>
		public async Task<ActionResult> Index(int subscriptionId)
		{
			ViewData["IsManager"] = AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			ViewData["SubscriptionId"] = subscriptionId;
			ViewData["SubscriptionName"] = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
			ViewData["UserId"] = AppService.UserContext.UserId;

			var orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var customers = await AppService.GetCustomersByOrganizationId(orgId);

			var model = new MultiCustomerInfoViewModel
			{
				CustomerList = customers.Select(x => new CustomerInfoViewModel
				{
					Address = new CustomerInfoViewModel.AddessViewModel
					{
						Address1 = x.Address?.Address1,
						Address2 = x.Address?.Address2,
						City = x.Address?.City,
						CountryName = x.Address?.CountryName,
						PostalCode = x.Address?.PostalCode,
						StateName = x.Address?.StateName
					},
					CustomerId = x.CustomerId,
					ContactEmail = x.ContactEmail,
					ContactPhoneNumber = x.ContactPhoneNumber,
					CreatedUtc = x.CreatedUtc,
					CustomerName = x.CustomerName,
					CustomerCode = x.CustomerCode,
					EIN = x.EIN,
					FaxNumber = x.FaxNumber,
					IsActive = x.IsActive,
					ActiveProjects = x.ActiveProjects,
					InactiveProjects = x.InactiveProjects,
					OrganizationId = x.OrganizationId,
					Website = x.Website
				}).ToList()
			};

			return View(ActionConstants.Customer, model);
		}
	}
}