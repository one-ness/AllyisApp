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
		///// <summary>
		///// GET: Customer/subscriptionId/Index.
		///// </summary>
		///// <param name="subscriptionId">The Subscription Id.</param>
		///// <returns>Customer Index.</returns>
		//[HttpGet]
		//public async Task<ActionResult> Index(int subscriptionId)
		//{
		//	if (AppService.UserContext.SubscriptionsAndRoles[subscriptionId].ProductId != ProductIdEnum.StaffingManager)
		//	{
		//		AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.ViewCustomer, subscriptionId);
		//	}
		//	UserContext.SubscriptionAndRole subInfo = null;
		//	AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
		//	return View(await ConstructManageCustomerViewModel(subscriptionId));
		//}

		/// <summary>
		/// Quick fix for new routing issue.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <returns>The index page.</returns>
		public async Task<ActionResult> IndexNoUserId(int subscriptionId)
		{
			if (AppService.UserContext.SubscriptionsAndRoles[subscriptionId].ProductId != ProductIdEnum.StaffingManager)
			{
				AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.ViewCustomer, subscriptionId);
			}
			UserContext.SubscriptionAndRole subInfo = null;
			AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);

			var infos = await AppService.GetTimeEntryIndexInfo(subInfo.OrganizationId, null, null);
			ViewBag.WeekStart = Utility.GetDaysFromDateTime(SetStartingDate(null, infos.Item1.StartOfWeek));
			ViewBag.WeekEnd = Utility.GetDaysFromDateTime(SetEndingDate(null, infos.Item1.StartOfWeek));

			return View("Index", await ConstructManageCustomerViewModel(subscriptionId));
		}

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
			var customers = (await AppService.GetCustomerList(orgId)).Select(x => new CustomerInfoViewModel()
			{
				Address = new CustomerInfoViewModel.AddessViewModel()
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
				OrganizationId = x.OrganizationId,
				Website = x.Website
			}).ToList();

			return View("Customer",
				new AllyisApps.ViewModels.TimeTracker.Customer.MultiCustomerInfoViewModel()
				{
					CustomerList = customers
				});
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
			var projGet = await AppService.GetProjectsByCustomerAsync(customerId);
			model.Projects = projGet.AsParallel()
			.Select(proj => new
			CustomerProjectViewModel.ProjectViewModel
			{
				CustomerId = proj.OwningCustomer.CustomerId,
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
			CustomerProjectViewModel.ProjectViewModel
			{
				CustomerId = proj.OwningCustomer.CustomerId,
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
			AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			var infosTask = AppService.GetProjectsAndCustomersForOrgAndUser(subInfo.OrganizationId);
			var inactiveInfoTask = Task.Run(() => AppService.GetInactiveProjectsAndCustomersForOrgAndUser(subInfo.OrganizationId));

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
				CustomerProjectViewModel customerResult = new CustomerProjectViewModel
				{
					CustomerInfo = new CustomerProjectViewModel.CustomerViewModel
					{
						CustomerName = currentCustomer.CustomerName,
						CustomerId = currentCustomer.CustomerId,
						IsActive = currentCustomer.IsActive
					},
					Projects = from p in projects
							   where p.OwningCustomer.CustomerId == currentCustomer.CustomerId
							   select new CustomerProjectViewModel.ProjectViewModel
							   {
								   CustomerId = p.OwningCustomer.CustomerId,
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
				CustomerProjectViewModel customerResult = new CustomerProjectViewModel
				{
					CustomerInfo = new CustomerProjectViewModel.CustomerViewModel
					{
						CustomerId = currentCustomer.CustomerId,
						CustomerName = currentCustomer.CustomerName,
						IsActive = currentCustomer.IsActive
					},
					Projects = from p in inactiveProjects
							   where p.OwningCustomer.CustomerId == currentCustomer.CustomerId
							   select new CustomerProjectViewModel.ProjectViewModel
							   {
								   CustomerId = p.OwningCustomer.CustomerId,
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
				UserId = AppService.UserContext.UserId
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
					: 6 - (int)today.DayOfWeek + startOfWeek;

				date = today.AddDays(daysLeftInWeek);
			}

			return date.Value.Date;
		}
	}
}