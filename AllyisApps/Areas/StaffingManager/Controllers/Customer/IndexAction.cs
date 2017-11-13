//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
using AllyisApps.ViewModels.TimeTracker.Customer;

namespace AllyisApps.Areas.StaffingManager.Controllers
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
			UserContext.SubscriptionAndRole subInfo = null;
			AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			return View(await ConstructManageCustomerViewModel(subscriptionId));
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
				AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.ViewCustomer, subscriptionId);
			}

			UserContext.SubscriptionAndRole subInfo = null;
			AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);

			var infos = await AppService.GetTimeEntryIndexInfo(subInfo.OrganizationId, null, null);

			return View("Index", await ConstructManageCustomerViewModel(subscriptionId));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AllyisApps.ViewModels.Staffing.Customer.ManageCustomerViewModel" /> class.
		/// </summary>
		/// <param name="subscriptionId">The id of the current subscription.</param>
		/// <returns>The ManageCustomerViewModel.</returns>
		public async Task<AllyisApps.ViewModels.Staffing.Customer.ManageCustomerViewModel> ConstructManageCustomerViewModel(int subscriptionId)
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
							   where p.owningCustomer.CustomerId == currentCustomer.CustomerId
							   select new CustomerProjectViewModel.ProjectViewModel
							   {
								   CustomerId = p.owningCustomer.CustomerId,
								   OrganizationId = p.OrganizationId,
								   ProjectName = p.ProjectName,
								   ProjectId = p.ProjectId
							   }
				};

				customersList.Add(customerResult);
			}

			IList<CustomerProjectViewModel> inactiveCustomersList = new List<CustomerProjectViewModel>();
			return new AllyisApps.ViewModels.Staffing.Customer.ManageCustomerViewModel
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
	}
}