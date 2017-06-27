//------------------------------------------------------------------------------
// <copyright file="DeleteTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Areas.TimeTracker.Core;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System;
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
		/// Deletes a Time Entry.
		/// </summary>
		/// <param name="model">The model representing the time entry to be deleted.</param>
		/// <returns>JSON Status. {status: 'success|error', message: 'a string'}.</returns>
		[HttpPost]
		public ActionResult DeleteTimeEntryJson(DeleteTimeEntryViewModel model)
		{
			// Check for permissions
			TimeEntryInfo entry = AppService.GetTimeEntry(model.TimeEntryId);
			if (entry.UserId != this.AppService.UserContext.UserId)
			{
				if (!this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, model.SubscriptionId))
				{
					return this.Json(new
					{
						status = "error",
						message = Resources.Strings.NotAuthZTimeEntryOtherUserDelete,
						e = new UnauthorizedAccessException(Resources.Strings.NotAuthZTimeEntryOtherUserDelete)
					});
				}
			}

			// Authorized to delete the time entry
			if (entry.ApprovalState == (int)ApprovalState.Approved)
			{
				return this.Json(new
				{
					status = "error",
					message = Resources.Strings.AlreadyApprovedCannotEdit,
					e = new ArgumentException(Resources.Strings.AlreadyApprovedCannotEdit),
					reason = "ALREADY_APPROVED"
				});
			}

			// Time entry is locked
			DateTime? lockDate = AppService.GetLockDate();
			if ((!this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, model.SubscriptionId)) && entry.Date <= (lockDate == null ? DateTime.MinValue : lockDate.Value))
			{
				string errorMessage = Resources.Strings.CanOnlyEdit + " " + lockDate.Value.ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture);
				return this.Json(new
				{
					status = "error",
					message = errorMessage,
					e = new ArgumentException(errorMessage),
					reason = "DATE_LOCKED"
				});
			}

			AppService.DeleteTimeEntry(model.TimeEntryId);
			return this.Json(new { status = "success" });
		}
	}
}
