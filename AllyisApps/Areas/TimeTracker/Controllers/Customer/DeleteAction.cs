//------------------------------------------------------------------------------
// <copyright file="DeleteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services.Crm;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// Deletes a customer.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="userIds">Comma seperated value of user ids.</param>
		/// <returns>Action result to the Index page.</returns>
		[HttpGet]
		public async Task<ActionResult> Delete(int subscriptionId, string userIds)
		{
			var customerIds = userIds.Split(',').Select(id => Convert.ToInt32(id));

			foreach (int customerId in customerIds)
			{
				try
				{
					await AppService.DeleteCustomer(subscriptionId, customerId);
					Notifications.Add(new BootstrapAlert("Customer(s) successfully deleted.", Variety.Success));
				}
				catch
				{
					Customer customer = AppService.GetCustomer(customerId);
					Notifications.Add(new BootstrapAlert(
						$"Cannot delete customer \"{customer.CustomerName}\", there are dependent projects or time entries.", Variety.Warning));
				}
			}

			return RedirectToAction(ActionConstants.Index, new { subscriptionId });
		}
	}
}