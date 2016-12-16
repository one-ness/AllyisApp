//------------------------------------------------------------------------------
// <copyright file="DeleteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseProductController
	{
		/// <summary>
		/// POST: Customer/Delete.
		/// </summary>
		/// <param name="id">The Customer id.</param>
		/// <returns>The Customer index.</returns>
		public ActionResult Delete(int id)
		{
            CustomerInfo customer = Service.GetCustomer(id);

            if (customer != null) {

                if (Service.DeleteCustomer(id))
                {
                    Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", customer.Name, Resources.TimeTracker.Controllers.Customer.Strings.CustomerDeleteNotification), Variety.Success));

                    return this.RedirectToAction(ActionConstants.Index);
                }

                // Permission failure
                Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.ActionUnauthorizedMessage, Variety.Warning));
            }

            return this.RedirectToAction(ActionConstants.Index);
		}
	}
}