﻿//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Customer;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// GET: Customer/Index.
		/// </summary>
		/// <returns>Customer Index.</returns>
		[HttpGet]
		public ActionResult Index()
		{
			if (Service.Can(Actions.CoreAction.ViewCustomer))
			{
				return this.View(this.ConstructManageCustomerViewModel(UserContext.UserId, UserContext.ChosenOrganizationId));
			}

			Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.ActionUnauthorizedMessage, Variety.Warning));

			return this.RouteHome();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ManageCustomerViewModel" /> class.
		/// </summary>
		/// <param name="userId">The user's Id.</param>
		/// <param name="orgId">The id of the current organization.</param>
		/// <returns>The ManageCustomerViewModel.</returns>
		public ManageCustomerViewModel ConstructManageCustomerViewModel(int userId, int orgId)
		{
			var infos = Service.GetProjectsAndCustomersForOrgAndUser();

			bool canEditProjects = Service.Can(Actions.CoreAction.EditProject);

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
								   Name = p.ProjectName,
								   ProjectId = p.ProjectId
							   }
				};

				// Only add the customer to the list if a project will be displayed to the user (i.e. user is a manager or part of one of the customer's projects)
				if (customerResult.Projects.Count() > 0 || canEditProjects)
				{
					customersList.Add(customerResult);
				}
			}

			return new ManageCustomerViewModel
			{
				Customers = customersList,
				OrganizationId = orgId,
				canEdit = canEditProjects
			};
		}
	}
}
