//------------------------------------------------------------------------------
// <copyright file="SetLockDateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Sets the lock date for an organization.
		/// </summary>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="LDsetting">Whether or not to use a lock date.</param>
		/// <param name="LDperiod">The currently-selected period (days/weeks/months).</param>
		/// <param name="LDquantity">The quantity of the selected period.</param>
		/// <returns>Provides the view for the user.</returns>
		[HttpPost]
		public ActionResult SetLockDate(int subscriptionId, bool LDsetting, int LDperiod, int LDquantity)
		{
			this.AppService.CheckTimeTrackerAction(Services.AppService.TimeTrackerAction.EditOthers, subscriptionId);
			int orgId = AppService.UserContext.OrganizationSubscriptions[subscriptionId].OrganizationId;
			try
			{
				if (AppService.UpdateLockDate(LDsetting, LDperiod, LDquantity, orgId))
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.LockDateUpdate, Variety.Success));
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.LockDateUpdateFail, Variety.Warning));
				}
			}
			catch (System.ArgumentException ex)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.LockDateUpdateFail + " " + ex.Message, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
		}
	}
}
