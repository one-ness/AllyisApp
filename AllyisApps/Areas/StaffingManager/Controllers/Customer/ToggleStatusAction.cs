using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services.Crm;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
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
			var customerIds = userIds.Split(',').Select(id => Convert.ToInt32(id));
			int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var customers = (await AppService.GetCustomersByOrganizationId(orgId)).ToList();

			foreach (int customerId in customerIds)
			{
				Customer customer = customers.FirstOrDefault(x => x.CustomerId == customerId);

				if (customer == null) continue;

				string result;

				if (customer.IsActive)
				{
					if (await CanDisable(customer, orgId))
					{
						result = await AppService.UpdateCustomerIsActive(subscriptionId, customerId, !customer.IsActive);
					}
					else
					{
						Notifications.Add(new BootstrapAlert($"Cannot toggle {customer.CustomerName}, dependent projects are still active.", Variety.Warning));
						continue;
					}
				}
				else
				{
					result = await AppService.UpdateCustomerIsActive(subscriptionId, customerId, !customer.IsActive);
				}

				Notifications.Add(!string.IsNullOrEmpty(result)
					? new BootstrapAlert($"\"{result}\" status was toggled sucessfully.", Variety.Success)
					: new BootstrapAlert("Customer was not found.", Variety.Warning));
			}

			return RedirectToAction(ActionConstants.Index, new { subscriptionId });
		}

		/// <summary>
		/// Checks if a customer can be disabled or deleted.
		/// </summary>
		/// <param name="customer"></param>
		/// <param name="orgId"></param>
		/// <returns></returns>
		public async Task<bool> CanDisable(Customer customer, int orgId)
		{
			var projects = await AppService.GetProjectsByOrganization(orgId, false);

			return projects
				.Where(x => x.OwningCustomer.CustomerId == customer.CustomerId)
				.All(project => project.EndDate != null && DateTime.UtcNow.Date >= project.EndDate.Value.Date
							|| project.StartDate != null && DateTime.UtcNow.Date <= project.StartDate.Value.Date);
		}
	}
}