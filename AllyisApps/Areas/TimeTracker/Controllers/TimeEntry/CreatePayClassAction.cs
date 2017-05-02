//------------------------------------------------------------------------------
// <copyright file="CreatePayClassAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using System;
using System.Web.Mvc;

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
			else
			{
				try//should put try catch in 'else'. Creating a blank pay class results in Two alerts: "Cannot create blank pay class" and "pay class already exists"
				{
					if (TimeTrackerService.CreatePayClass(newPayClass))
					{
						Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.SuccessfulCreatePayClass, Variety.Success));
					}
					else
					{
						// Should only get here on permissions failure
						Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
					}
				}
				catch (ArgumentException)
				{
					// Pay class already exists
					Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.FailureCreatePayClassAlreadyExists, Variety.Danger));
				}
			}
			return this.RedirectToAction(ActionConstants.Settings);
		}
	}
}
