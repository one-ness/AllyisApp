//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services.Project;
using AllyisApps.ViewModels;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseProductController
	{
		/// <summary>
		/// GET: Customer/Index.
		/// </summary>
		/// <returns>Customer Index.</returns>
		[HttpGet]
		public ActionResult Index()
		{
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.ViewCustomer))
			{
				return this.View(this.ConstructManageCustomerViewModel(UserContext.UserId, UserContext.ChosenOrganizationId));
			}

			Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.ActionUnauthorizedMessage, Variety.Warning));

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Home);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ManageCustomerViewModel" /> class.
		/// </summary>
		/// <param name="userId">The user's Id.</param>
		/// <param name="orgId">The id of the current organization.</param>
		/// <returns>The ManageCustomerViewModel.</returns>
		public ManageCustomerViewModel ConstructManageCustomerViewModel(int userId, int orgId)
		{
			bool canEditProjects = AuthorizationService.Can(Services.Account.Actions.CoreAction.EditProject);
			IEnumerable<CompleteProjectInfo> projects = canEditProjects ?   // Show all of a customer's projects for managers, but only projects one belongs to for users
				OrgService.GetProjectsByOrganization(orgId) : ProjectService.GetProjectsByUserId(userId);
			IEnumerable<CustomerInfo> customers = CrmService.GetCustomerList(orgId);
			////IEnumerable<CustomerInfo> customers = new List<CustomerInfo>();                           // TODO: Should we be showing all customers to all users? Probably need to filter this out by projects that the user is part of for non-managers
			//  if(AuthorizationService.Can(Services.Account.Actions.CoreAction.EditProject){           something like this? someone who can actually check might try this out?
			//  customers = CrmService.GetCustomerList(orgId)
			//  } else { customers.Add(CrmService.GetCustomer(userId));}
			IList<CustomerProjectViewModel> customersList = new List<CustomerProjectViewModel>();
			foreach (CustomerInfo currentCustomer in customers)
			{
				CustomerProjectViewModel customerResult = new CustomerProjectViewModel()
				{
					CustomerInfo = currentCustomer,
					Projects = from p in projects
							   where p.CustomerId == currentCustomer.CustomerId
							   select new ProjectInfo
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
				OrganizationId = orgId
			};
		}
	}
}