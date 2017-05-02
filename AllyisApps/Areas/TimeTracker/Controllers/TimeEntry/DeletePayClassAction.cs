//------------------------------------------------------------------------------
// <copyright file="DeletePayClassAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Deletes a payclass from an org.
		/// </summary>
		/// <param name="payClassId">The name of the class to delete.</param>
		/// <returns>Redirects to the settings view.</returns>
		public ActionResult DeletePayClass(int payClassId)
		{
			var allPayClasses = TimeTrackerService.GetPayClasses();
			string sourcePayClassName = allPayClasses.Where(pc => pc.PayClassID == payClassId).ElementAt(0).Name;

			//Built-in, non-editable pay classes cannot be deleted
			//Used pay classes cannot be deleted, suggest manager to merge it with another payclass instead
			if (sourcePayClassName == "Regular" || sourcePayClassName == "Overtime" || sourcePayClassName == "Holiday" || sourcePayClassName == "Paid Time Off" || sourcePayClassName == "Unpaid Time Off" || TimeTrackerService.GetTimeEntriesThatUseAPayClass(payClassId).Count() > 0)
			{
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.CannotDeletePayClass, Variety.Warning));
			}
			else
			{
				if (TimeTrackerService.DeletePayClass(payClassId))
				{
					Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.SuccessfulDeletePayClass, Variety.Success));
				}
				else
				{
					// Should only be here because of permission failures
					Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
				}
			}

			return this.RedirectToAction(ActionConstants.Settings);
		}
	}
}
