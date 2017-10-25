//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
using AllyisApps.ViewModels.TimeTracker.Customer;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// GET: /TimeTracker/Customer/subscriptionId
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// GET: Customer/subscriptionId/Index.
		/// </summary>
		/// <param name="subscriptionId">The Subscription Id.</param>
		/// <returns>Customer Index.</returns>
		[HttpGet]
		public async Task<ActionResult> Index(int subscriptionId)
		{
			if (AppService.UserContext.SubscriptionsAndRoles[subscriptionId].ProductId != ProductIdEnum.StaffingManager)
			{
				this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.ViewCustomer, subscriptionId);
			}
			UserContext.SubscriptionAndRole subInfo = null;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			return this.View(await this.ConstructManageCustomerViewModel(subscriptionId));
		}

		/// <summary>
		/// Quick fix for new routing issue.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <returns>The index page.</returns>
		public async Task<ActionResult> IndexNoUserId(int subscriptionId)
		{
			if (AppService.UserContext.SubscriptionsAndRoles[subscriptionId].ProductId != ProductIdEnum.StaffingManager)
			{
				this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.ViewCustomer, subscriptionId);
			}
			UserContext.SubscriptionAndRole subInfo = null;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);

			var infos = await AppService.GetTimeEntryIndexInfo(subInfo.OrganizationId, null, null);
			ViewBag.WeekStart = Utility.GetDaysFromDateTime(SetStartingDate(null, infos.Item1.StartOfWeek));
			ViewBag.WeekEnd = Utility.GetDaysFromDateTime(SetEndingDate(null, infos.Item1.StartOfWeek));

			return this.View("Index", await this.ConstructManageCustomerViewModel(subscriptionId));
		}

		/// <summary>
		/// Populate Projects.
		/// </summary>
		/// <param name="customerId">The customer id.</param>
		/// <returns>ProjectByCustomer partial view.</returns>
		[HttpPost]
		public async Task<ActionResult> PopulateProjects(int customerId)
		{
			var model = new CustomerProjectViewModel();
			model.CustomerInfo = new CustomerProjectViewModel.CustomerViewModel { CustomerId = customerId };
			var projGet = await AppService.GetProjectsByCustomer(customerId);
			model.Projects = projGet.AsParallel()
			.Select(proj => new
			CustomerProjectViewModel.ProjectViewModel()
			{
				CustomerId = proj.owningCustomer.CustomerId,
				OrganizationId = proj.OrganizationId,
				ProjectId = proj.ProjectId,
				ProjectName = proj.ProjectName
			});
			return PartialView("_ProjectsByCustomer", model);
		}

		/// <summary>
		/// Populate Projects.
		/// </summary>
		/// <param name="customerId">The customer id.</param>
		/// <returns>ProjectByCustomer partial view.</returns>
		[HttpPost]
		public async Task<ActionResult> PopulateInactiveProjects(int customerId)
		{
			var model = new CustomerProjectViewModel();
			model.CustomerInfo = new CustomerProjectViewModel.CustomerViewModel { CustomerId = customerId };
			var projects = await AppService.GetInactiveProjectsByCustomer(customerId);
			model.Projects = projects.AsParallel().Select(proj => new
			CustomerProjectViewModel.ProjectViewModel()
			{
				CustomerId = proj.owningCustomer.CustomerId,
				OrganizationId = proj.OrganizationId,
				ProjectId = proj.ProjectId,
				ProjectName = proj.ProjectName
			});
			return PartialView("_ProjectsByCustomer", model);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ManageCustomerViewModel" /> class.
		/// </summary>
		/// <param name="subscriptionId">The id of the current subscription.</param>
		/// <returns>The ManageCustomerViewModel.</returns>
		public async Task<ManageCustomerViewModel> ConstructManageCustomerViewModel(int subscriptionId)
		{
			UserContext.SubscriptionAndRole subInfo = null;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			var infosTask = AppService.GetProjectsAndCustomersForOrgAndUser(subInfo.OrganizationId);
			var inactiveInfoTask = AppService.GetInactiveProjectsAndCustomersForOrgAndUser(subInfo.OrganizationId);

			await Task.WhenAll(new Task[] { infosTask, inactiveInfoTask });

			var infos = infosTask.Result;
			var inactiveInfo = inactiveInfoTask.Result;

			bool canEditProjects = subInfo.ProductRoleId == (int)TimeTrackerRole.Manager;
			string subName = await AppService.GetSubscriptionName(subscriptionId);
			List<CompleteProject> projects = canEditProjects ? infos.Item1 : infos.Item1.Where(p => p.IsProjectUser == true).ToList();
			List<Customer> customers = infos.Item2;
			IList<CustomerProjectViewModel> customersList = new List<CustomerProjectViewModel>();
			foreach (Customer currentCustomer in customers)
			{
				CustomerProjectViewModel customerResult = new CustomerProjectViewModel()
				{
					CustomerInfo = new CustomerProjectViewModel.CustomerViewModel()
					{
						CustomerName = currentCustomer.CustomerName,
						CustomerId = currentCustomer.CustomerId,
						IsActive = currentCustomer.IsActive
					},
					Projects = from p in projects
							   where p.owningCustomer.CustomerId == currentCustomer.CustomerId
							   select new CustomerProjectViewModel.ProjectViewModel
							   {
								   CustomerId = p.owningCustomer.CustomerId,
								   OrganizationId = p.OrganizationId,
								   ProjectName = p.ProjectName,
								   ProjectId = p.ProjectId
							   }
				};

				// Only add the customer to the list if a project will be displayed to the user (i.e. user is a manager or part of one of the customer's projects)
				if (customerResult.Projects.Count() > 0 || canEditProjects)
				{
					customersList.Add(customerResult);
				}
			}

			List<CompleteProject> inactiveProjects = canEditProjects ? inactiveInfo.Item1 : inactiveInfo.Item1.Where(p => p.IsProjectUser == true).ToList();
			List<Customer> inactiveCustomers = inactiveInfo.Item2;

			IList<CustomerProjectViewModel> inactiveCustomersList = new List<CustomerProjectViewModel>();
			foreach (Customer currentCustomer in inactiveCustomers)
			{
				CustomerProjectViewModel customerResult = new CustomerProjectViewModel()
				{
					CustomerInfo = new CustomerProjectViewModel.CustomerViewModel()
					{
						CustomerId = currentCustomer.CustomerId,
						CustomerName = currentCustomer.CustomerName,
						IsActive = currentCustomer.IsActive
					},
					Projects = from p in inactiveProjects
							   where p.owningCustomer.CustomerId == currentCustomer.CustomerId
							   select new CustomerProjectViewModel.ProjectViewModel
							   {
								   CustomerId = p.owningCustomer.CustomerId,
								   OrganizationId = p.OrganizationId,
								   ProjectName = p.ProjectName,
								   ProjectId = p.ProjectId
							   }
				};

				// Only add the customer to the list if a project will be displayed to the user (i.e. user is a manager or part of one of the customer's projects)
				if (customerResult.Projects.Count() > 0 || canEditProjects)
				{
					inactiveCustomersList.Add(customerResult);
				}
			}

			return new ManageCustomerViewModel
			{
				Customers = customersList,
				InactiveCustomerAndProjects = inactiveCustomersList,
				OrganizationId = subInfo.OrganizationId,
				CanEdit = canEditProjects,
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = this.AppService.UserContext.UserId
			};
		}

		private DateTime SetStartingDate(DateTime? date, int startOfWeek)
		{
			if (date == null && !date.HasValue)
			{
				DateTime today = DateTime.Now;
				int daysIntoTheWeek = (int)today.DayOfWeek < startOfWeek
					? (int)today.DayOfWeek + (7 - startOfWeek)
					: (int)today.DayOfWeek - startOfWeek;

				date = today.AddDays(-daysIntoTheWeek);
			}

			return date.Value.Date;
		}

		private DateTime SetEndingDate(DateTime? date, int startOfWeek)
		{
			if (date == null && !date.HasValue)
			{
				DateTime today = DateTime.Now;

				int daysLeftInWeek = (int)today.DayOfWeek < startOfWeek
					? startOfWeek - (int)today.DayOfWeek - 1
					: (6 - (int)today.DayOfWeek) + startOfWeek;

				date = today.AddDays(daysLeftInWeek);
			}

			return date.Value.Date;
		}
	}
}