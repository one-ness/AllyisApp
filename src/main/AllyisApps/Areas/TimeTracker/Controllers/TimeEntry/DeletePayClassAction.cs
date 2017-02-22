﻿//------------------------------------------------------------------------------
// <copyright file="DeletePayClassAction.cs" company="Allyis, Inc.">
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
		/// Deletes a payclass from an org.
		/// </summary>
		/// <param name="payClassId">The name of the class to delete.</param>
		/// <returns>Redirects to the settings view.</returns>
		public ActionResult DeletePayClass(int payClassId)
		{
			if (TimeTrackerService.DeletePayClass(payClassId))
			{
				Notifications.Add(new BootstrapAlert("Pay class deleted successfully", Variety.Success));
			}
			else
			{ 
				// Should only be here because of permission failures
				Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Settings);
		}
	}
}