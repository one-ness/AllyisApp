//------------------------------------------------------------------------------
// <copyright file="DeleteEmploymentTypeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// Deletes a position Status from an org.
		/// </summary>
		/// <param name="employmentTypeId"> the id of the Status to delete </param>
		/// <param name="userId">The id of the Status to delete.</param> // TODO: update this after changing the route
		/// <param name="subscriptionId">The subscription's id.</param>
		/// <returns>Redirects to the settings view.</returns>
		public async Task<ActionResult> DeleteEmploymentType(int employmentTypeId, int userId, int subscriptionId)
		{
			int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			try
			{
				AppService.DeleteEmploymentType(employmentTypeId);
				Notifications.Add(new BootstrapAlert("Successfully deleted Employment Type", Variety.Success));
			}
			catch
			{
				// Should only be here because of permission failures
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}
			await Task.Yield();
			return RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId });
		}
	}
}