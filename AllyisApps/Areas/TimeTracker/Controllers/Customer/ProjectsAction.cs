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
using System.Collections.Generic;
using System;

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
		/// <param name="isActive">Whether or not active projects are returned.</param>
		/// <returns>The edit customer view - info on all the customer details.</returns>
		[HttpGet]
		public async Task<ActionResult> Projects(int subscriptionId, int customerId, bool isActive = true)
		{
			var currentTime = DateTime.UtcNow;
			var projects = await AppService.GetProjectsByCustomerAsync(customerId);
			var selectedProjects = new List<CustomerProjectViewModel.ProjectViewModel>();
			foreach (var project in projects)
			{
				if ((isActive
					&& (project.StartDate == null || project.StartDate >= currentTime)
					&& (project.EndDate == null || project.EndDate <= currentTime))
					|| (!isActive
					&& project.StartDate > currentTime
					&& project.EndDate < currentTime))
				{
					selectedProjects.Add(new CustomerProjectViewModel.ProjectViewModel()
					{
						CustomerId = customerId,
						OrganizationId = project.OrganizationId,
						ProjectId = project.ProjectId,
						ProjectName = project.ProjectName
					});
				}
			}
			return View(new CustomerProjectViewModel()
			{
				CustomerInfo = new CustomerProjectViewModel.CustomerViewModel()
				{
					CustomerId = customerId
				},
				Projects = selectedProjects
			});
		}
	}
}