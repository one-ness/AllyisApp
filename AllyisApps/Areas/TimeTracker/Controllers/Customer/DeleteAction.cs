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
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// GET: Customer/SubscriptionId/Delete/CustomerId.
		/// </summary>
		/// <param name="subscriptionId">The Subscription Id.</param>
		/// <param name="userIds">The Customer ids.</param>
		/// <returns>The Customer index.</returns>
		[HttpGet]
		public async Task<ActionResult> ToggleStatus(int subscriptionId, string userIds)
		{
			string[] ids = userIds.Split(',');
			var orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var customers = await AppService.GetCustomerList(orgId);

			foreach (var userId in ids)
			{
				int numValue;
				bool parsed = int.TryParse(userId, out numValue);

				if (!parsed)
				{
					return RedirectToAction(ActionConstants.Index, new { subscriptionId = subscriptionId });
				}
				else
				{
					var customer = customers.Where(x => string.Equals(x.CustomerId, numValue)).FirstOrDefault();
					string result = "";

					if (customer.IsActive.Value)
					{
						var canDisable = await CanDisable(customer, orgId);

						if (canDisable)
						{
							result = await AppService.DeleteCustomer(subscriptionId, numValue);
						}
						else
						{
							Notifications.Add(new BootstrapAlert(string.Format("Cannot toggle {0}, dependent projects are still active.", customer.CustomerName), Variety.Warning));
						}
					}
					else
					{
						result = AppService.ReactivateCustomer(numValue, subscriptionId, orgId);
					}

					if (!string.IsNullOrEmpty(result))
					{
						// if deleted successfully

						Notifications.Add(new BootstrapAlert(string.Format("{0} Status was toggled sucessfully.", result), Variety.Success));
					}
					else if (result == null)
					{
						// Permission failure

						Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
					}
				}
			}

			return RedirectToAction(ActionConstants.Index, new { subscriptionId = subscriptionId });
		}

		/// <summary>
		/// Checks if a customer can be disabled or deleted.
		/// </summary>
		/// <param name="customer"></param>
		/// <param name="orgId"></param>
		/// <returns></returns>
		async public Task<bool> CanDisable(Customer customer, int orgId)
		{
			var projects = (await AppService.GetProjectsByOrganization(orgId, false)).Where(x => x.OwningCustomer.CustomerId == customer.CustomerId);

			foreach (var project in projects)
			{
				if ((project.EndDate == null || DateTime.UtcNow.Date < project.EndDate.Value.Date) && (project.StartDate == null || DateTime.UtcNow.Date > project.StartDate.Value.Date))
				{
					return false;
				}
			}

			return true;
		}

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
					var result = await AppService.FullDeleteCustomer(subscriptionId, Convert.ToInt32(id));
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