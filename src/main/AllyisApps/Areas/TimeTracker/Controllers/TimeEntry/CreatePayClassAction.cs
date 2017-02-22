//------------------------------------------------------------------------------
// <copyright file="CreatePayClassAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using System;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Creates a payclass.
		/// </summary>
		/// <param name="newPayClass">The pay class to create.</param>
		/// <returns>Redirects to the settings view.</returns>
		public ActionResult CreatePayClass(string newPayClass)
		{
			if (string.IsNullOrWhiteSpace(newPayClass))
			{
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.CannotCreateBlankPayClass, Variety.Warning));
			}

			try
			{
				if (TimeTrackerService.CreatePayClass(newPayClass))
				{
					Notifications.Add(new BootstrapAlert("Pay class created successfully.", Variety.Success));
				}
				else
				{ 
					// Should only get here on permissions failure
					Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
				}
			} catch (ArgumentException)
			{
				// Pay class already exists
				Notifications.Add(new BootstrapAlert("Could not create pay class: that pay class name already exists.", Variety.Danger));
			}

			return this.RedirectToAction(ActionConstants.Settings);
		}
	}
}