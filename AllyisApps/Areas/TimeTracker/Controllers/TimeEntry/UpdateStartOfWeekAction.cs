﻿//------------------------------------------------------------------------------
// <copyright file="UpdateStartOfWeekAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Updates the start of week for an Organization.
		/// </summary>
		/// <param name="startOfWeek">Start of week selected by Organization admin.</param>
		/// <returns>Action result.</returns>
		public ActionResult UpdateStartOfWeek(int startOfWeek)
		{
			if (startOfWeek < 0 || startOfWeek > 6)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.InvalidSOW, Variety.Warning));
			}
			else if (!TimeTrackerService.UpdateStartOfWeek(startOfWeek))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulSOW, Variety.Success));
			}

			return this.RedirectToAction(ActionConstants.Settings);
		}
	}
}
