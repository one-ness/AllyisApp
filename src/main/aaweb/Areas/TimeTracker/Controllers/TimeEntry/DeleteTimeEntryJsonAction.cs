//------------------------------------------------------------------------------
// <copyright file="DeleteTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;

using AllyisApps.Areas.TimeTracker.Core;
using AllyisApps.Areas.TimeTracker.Models;
using AllyisApps.Core;
using AllyisApps.Services.BusinessObjects;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// Deletes a Time Entry.
		/// </summary>
		/// <param name="model">The model representing the time entry to be deleted.</param>
		/// <returns>JSON Status. {status: 'success|error', message: 'a string'}.</returns>
		[HttpPost]
		public ActionResult DeleteTimeEntryJson(DeleteTimeEntryViewModel model)
		{
			// Check for permissions
			TimeEntryInfo entry = TimeTrackerService.GetTimeEntry(model.TimeEntryId);
			if (entry.UserId == Convert.ToInt32(UserContext.UserId))
			{
				if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditSelf))
				{
					return this.Json(new { status = "error", message = "You are not authorized to delete a time entry.", e = new UnauthorizedAccessException("You are not authorized to delete a time entry.") });
				}
			}
			else
			{
				if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers))
				{
					return this.Json(new
					{
						status = "error",
						message = "You are not authorized to delete a time entry for another user!",
						e = new UnauthorizedAccessException("You are not authorized to delete a time entry for another user!")
					});
				}
			}

			// Authorized to delete the time entry
			if (entry.ApprovalState == (int)ApprovalState.Approved)
			{
				return this.Json(new
				{
					status = "error",
					message = "The time entry you are attempting to edit has been approved and can no longer be edited.",
					e = new ArgumentException("The time entry you are attempting to edit has been approved and can no longer be edited."),
					reason = "ALREADY_APPROVED"
				});
			}

			TimeTrackerService.DeleteTimeEntry(model.TimeEntryId);
			return this.Json(new { status = "success" });
		}
	}
}