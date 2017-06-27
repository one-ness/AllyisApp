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
        /// GET: Reactivate Customer
        /// </summary>
        /// <param name="subscriptionId">The subscription Id</param>
        /// <param name = "id" > The Customer id.</param>
        /// <returns>The Customer index.</returns>
        public ActionResult Reactivate(int subscriptionId, string id)
        {
            int custId;
            bool parsed = Int32.TryParse(id, out custId);

            if (!parsed)
            {
                return this.RedirectToAction(ActionConstants.Index, new { subscriptionId = subscriptionId });
            }

            int orgId = AppService.GetSubscription(subscriptionId).OrganizationId;
            var result = AppService.ReactivateCustomer(custId, subscriptionId, orgId);

            if (result != null && result != "")
            {
                Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", Resources.Strings.CustomerReactivateNotification, AppService.GetCustomer(custId).Name), Variety.Success));
            }
            // Permission failure
            else if (result == null)
            {
                Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
            }
            return this.RedirectToAction(ActionConstants.Index, new { subscriptionId = subscriptionId });
        }
    }
}