//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
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
		public ActionResult Index(int subscriptionId)
		{
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.ViewCustomer, subscriptionId);
			UserSubscription subInfo = null;
			this.AppService.UserContext.UserSubscriptions.TryGetValue(subscriptionId, out subInfo);
			return this.View(this.ConstructManageCustomerViewModel(subscriptionId));
		}

        /// <summary>
        /// Quick fix for new routing issue. 
        /// </summary>
        /// <param name="subscriptionId">Subscription id.</param>
        /// <returns>The index page.</returns>
        public ActionResult IndexNoUserId(int subscriptionId)
        {
            this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.ViewCustomer, subscriptionId);
            UserSubscription subInfo = null;
            this.AppService.UserContext.UserSubscriptions.TryGetValue(subscriptionId, out subInfo);

            var infos = AppService.GetTimeEntryIndexInfo(subInfo.OrganizationId, null, null);
            ViewBag.WeekStart = AppService.GetDayFromDateTime(SetStartingDate(null, infos.Item1.StartOfWeek));
            ViewBag.WeekEnd = AppService.GetDayFromDateTime(SetEndingDate(null, infos.Item1.StartOfWeek));

            return this.View("Index", this.ConstructManageCustomerViewModel(subscriptionId));
        }

		/// <summary>
		/// Populate Projects.
		/// </summary>
		/// <param name="customerId">The customer id.</param>
		/// <returns>ProjectByCustomer partial view.</returns>
		[HttpPost]
		public ActionResult PopulateProjects(int customerId)
		{
			var model = new CustomerProjectViewModel();
			model.CustomerInfo = new Customer { CustomerId = customerId };
			model.Projects = AppService.GetProjectsByCustomer(customerId);
			return PartialView("_ProjectsByCustomer", model);
		}

		/// <summary>
		/// Populate Projects.
		/// </summary>
		/// <param name="customerId">The customer id.</param>
		/// <returns>ProjectByCustomer partial view.</returns>
		[HttpPost]
		public ActionResult PopulateInactiveProjects(int customerId)
		{
			var model = new CustomerProjectViewModel();
			model.CustomerInfo = new Customer { CustomerId = customerId };
			model.Projects = AppService.GetInactiveProjectsByCustomer(customerId);
			return PartialView("_ProjectsByCustomer", model);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ManageCustomerViewModel" /> class.
		/// </summary>
		/// <param name="subscriptionId">The id of the current subscription.</param>
		/// <returns>The ManageCustomerViewModel.</returns>
		public ManageCustomerViewModel ConstructManageCustomerViewModel(int subscriptionId)
		{
			UserSubscription subInfo = null;
			this.AppService.UserContext.UserSubscriptions.TryGetValue(subscriptionId, out subInfo);
			var infos = AppService.GetProjectsAndCustomersForOrgAndUser(subInfo.OrganizationId);
			var inactiveInfo = AppService.GetInactiveProjectsAndCustomersForOrgAndUser(subInfo.OrganizationId);
			bool canEditProjects = subInfo.ProductRoleId == (int)TimeTrackerRole.Manager;
			List<CompleteProjectInfo> projects = canEditProjects ? infos.Item1 : infos.Item1.Where(p => p.IsProjectUser == true).ToList();
			List<Customer> customers = infos.Item2;
			IList<CustomerProjectViewModel> customersList = new List<CustomerProjectViewModel>();
			foreach (Customer currentCustomer in customers)
			{
				CustomerProjectViewModel customerResult = new CustomerProjectViewModel()
				{
					CustomerInfo = currentCustomer,
					Projects = from p in projects
							   where p.CustomerId == currentCustomer.CustomerId
							   select new Project
							   {
								   CustomerId = p.CustomerId,
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

			List<CompleteProjectInfo> inactiveProjects = canEditProjects ? inactiveInfo.Item1 : inactiveInfo.Item1.Where(p => p.IsProjectUser == true).ToList();
			List<Customer> inactiveCustomers = inactiveInfo.Item2;

			IList<CustomerProjectViewModel> inactiveCustomersList = new List<CustomerProjectViewModel>();
			foreach (Customer currentCustomer in inactiveCustomers)
			{
				CustomerProjectViewModel customerResult = new CustomerProjectViewModel()
				{
					CustomerInfo = currentCustomer,
					Projects = from p in inactiveProjects
							   where p.CustomerId == currentCustomer.CustomerId
							   select new Project
							   {
								   CustomerId = p.CustomerId,
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
                SubscriptionName = subInfo.SubscriptionName,
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
