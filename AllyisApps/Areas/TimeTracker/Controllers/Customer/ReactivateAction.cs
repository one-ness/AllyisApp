using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
    /// <summary>
    /// Represents pages for the management of a Customer.
    /// </summary>
	public partial class CustomerController : BaseController
    {

        /// <summary>
        /// POST: Reactivate Customer
        /// </summary>
        /// <param name = "id" > The Customer id.</param>
        /// <returns>The Customer index.</returns>
        public ActionResult ReactivateCustomer(int id)
        {
            var result = AppService.ReactivateCustomer(id);

            if (result != null && result != "")
            {
                Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", result, Resources.Strings.CustomerDeleteNotification), Variety.Success));
            }
            // Permission failure
            else if (result == null)
            {
                Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
            }
            return this.RedirectToAction(ActionConstants.Index);
        }


        /// <summary>
        /// POST: Customer/Delete.
        /// </summary>
        /// <returns>The Customer index.</returns>
        public ActionResult Reactivate()
        {
			if (AppService.Can(Actions.CoreAction.ViewCustomer))
			{
				return this.View(this.ConstructManageInactiveCustomerViewModel(UserContext.UserId, UserContext.ChosenOrganizationId));
			}

			Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));

			return this.RouteHome();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageCustomerViewModel" /> class.
        /// </summary>
        /// <param name="userId">The user's Id.</param>
        /// <param name="orgId">The id of the current organization.</param>
        /// <returns>The ManageCustomerViewModel.</returns>
        public ManageCustomerViewModel ConstructManageInactiveCustomerViewModel(int userId, int orgId)
        {
            var infos = AppService.GetInactiveProjectsAndCustomersForOrgAndUser();

            bool canEditProjects = AppService.Can(Actions.CoreAction.EditProject);

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