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
using AllyisApps.Services.TimeTracker;

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
					return this.Json(new { status = "error", message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntryDelete, e = new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntryDelete) });
				}
			}
			else
			{
				if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers))
				{
					return this.Json(new
					{
						status = "error",
						message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntryOtherUserDelete,
						e = new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntryOtherUserDelete)
					});
				}
			}

			// Authorized to delete the time entry
			if (entry.ApprovalState == (int)ApprovalState.Approved)
			{
				return this.Json(new
				{
					status = "error",
					message = Resources.TimeTracker.Controllers.TimeEntry.Strings.AlreadyApprovedCannotEdit,
					e = new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.AlreadyApprovedCannotEdit),
					reason = "ALREADY_APPROVED"
				});
			}

			TimeTrackerService.DeleteTimeEntry(model.TimeEntryId);
			return this.Json(new { status = "success" });
		}
	}
}