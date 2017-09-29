//------------------------------------------------------------------------------
// <copyright file="DeletePayClassAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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
		/// Deletes a position level from an org.
		/// </summary>
		/// <param name="positionLevelId"> the id of the level to delete </param>
		/// <param name="userId">The id of the level to delete.</param> // TODO: update this after changing the route
		/// <param name="subscriptionId">The subscription's id.</param>
		/// <returns>Redirects to the settings view.</returns>
		public ActionResult DeletePositionLevel(int positionLevelId, int userId, int subscriptionId)
		{
			try
			{
				AppService.DeletePositionLevel(positionLevelId);
				Notifications.Add(new BootstrapAlert("Successfully deleted position level", Variety.Success));
			}
			catch
			{
				// Should only be here because of permission failures
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId });
		}
	}
}