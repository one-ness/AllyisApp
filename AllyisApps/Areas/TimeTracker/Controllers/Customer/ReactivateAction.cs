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
		/// GET: Reactivate Customer.
		/// </summary>
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <param name = "userId" > The Customer id.</param>
		/// <returns>The Customer index.</returns>
		public ActionResult Reactivate(int subscriptionId, int userId)
		{
			int orgId = AppService.UserContext.OrganizationSubscriptions[subscriptionId].OrganizationId;
			var result = AppService.ReactivateCustomer(userId, subscriptionId, orgId);

			if (!string.IsNullOrEmpty(result))
			{
				Notifications.Add(new BootstrapAlert(string.Format("{0} {1}", Resources.Strings.CustomerReactivateNotification, AppService.GetCustomer(userId).CustomerName), Variety.Success));
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
