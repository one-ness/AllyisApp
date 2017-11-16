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

namespace AllyisApps.Areas.TimeTracker.Controllers
{
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
		async public Task<ActionResult> Delete(int subscriptionId, string userIds)
		{
			string[] ids = userIds.Split(',');
			var orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

			foreach (var id in ids)
			{
				try
				{
					var result = await AppService.DeleteCustomer(subscriptionId, Convert.ToInt32(id));
					Notifications.Add(new BootstrapAlert(string.Format("Customer(s) successfully deleted"), Variety.Success));
				}
				catch
				{
					Notifications.Add(new BootstrapAlert(string.Format("Cannot Delete Customer {0}, there are dependent projects or time entries.", id), Variety.Warning));
				}
			}

			return RedirectToAction(ActionConstants.Index, new { subscriptionId = subscriptionId });
		}
	}
}