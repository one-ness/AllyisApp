//------------------------------------------------------------------------------
// <copyright file="SetLockDateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// Sets the lock date for a specific user.
		/// </summary>
		/// <param name="organizationId">The organization.</param>
		/// <param name="userId">The User.</param>
		/// <param name="startDate">The currently-selected start Date.</param>
		/// <param name="endDate">The currently-selected end Date.</param>
		/// <param name="lockDate">The Date.</param>
		/// <returns>Provides the view for the user.</returns>
		[HttpPost]
		public ActionResult SetLockDate(int organizationId, int userId, DateTime startDate, DateTime endDate, DateTime lockDate)
		{
			if (!TimeTrackerService.SetLockDate(organizationId, userId, lockDate))
			{
				// Should only be here because of permission failures
				Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
			}

			return this.RedirectToAction("Index", new { organizationId, userId, startDate, endDate });
		}
	}
}