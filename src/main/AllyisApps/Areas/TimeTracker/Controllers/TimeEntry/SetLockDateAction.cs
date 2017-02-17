//------------------------------------------------------------------------------
// <copyright file="SetLockDateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
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
		/// <param name="LDsetting">Whether or not to use a lock date.</param>
		/// <param name="LDperiod">The currently-selected period (days/weeks/months).</param>
		/// <param name="LDquantity">The quantity of the selected period.</param>
		/// <returns>Provides the view for the user.</returns>
		[HttpPost]
		public ActionResult SetLockDate(bool LDsetting, string LDperiod, int LDquantity)
		{
            if (Service.Can(Services.Actions.CoreAction.TimeTrackerEditOthers))
            {
                try
                {
                    if (TimeTrackerService.UpdateLockDate(LDsetting, LDperiod, LDquantity))
                    {
                        Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.LockDateUpdate, Variety.Success));
                    }
                    else
                    {
                        Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.LockDateUpdateFail, Variety.Warning));
                    }
                } catch (System.ArgumentException ex)
                {
                    Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.LockDateUpdateFail + " " + ex.Message, Variety.Warning));
                }
                return this.RedirectToAction(ActionConstants.Settings);
            }
            else
            {
                Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
                return this.RedirectToAction(ActionConstants.Index);
            }
		}
	}
}