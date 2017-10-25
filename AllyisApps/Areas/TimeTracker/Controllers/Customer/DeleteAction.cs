//------------------------------------------------------------------------------
// <copyright file="DeleteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;

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
		/// <param name="userId">The Customer id.</param>
		/// <returns>The Customer index.</returns>
		[HttpGet]
		public async Task<ActionResult> Delete(int subscriptionId, string userId)
		{
			int numValue;
			bool parsed = int.TryParse(userId, out numValue);

			if (!parsed)
			{
				return this.RedirectToAction(ActionConstants.Index, new { subscriptionId = subscriptionId });
			}
			else
			{
				var result = await AppService.DeleteCustomer(subscriptionId, numValue);

				if (!string.IsNullOrEmpty(result))
				{
					// if deleted successfully

					Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", result, Resources.Strings.CustomerDeleteNotification), Variety.Success));
				}
				else if (result == null)
				{
					// Permission failure

					Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
				}

				return this.RedirectToAction(ActionConstants.Index, new { subscriptionId = subscriptionId });
			}
		}
	}
}