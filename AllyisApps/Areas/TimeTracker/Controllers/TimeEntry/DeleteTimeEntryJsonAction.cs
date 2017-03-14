//------------------------------------------------------------------------------
// <copyright file="DeleteTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Areas.TimeTracker.Core;
using AllyisApps.Core;
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
			ProductRoleIdEnum role = UserContext.UserOrganizationInfoList.Where(o => o.OrganizationId == UserContext.ChosenOrganizationId).SingleOrDefault()
				.UserSubscriptionInfoList.Where(s => s.SubscriptionId == UserContext.ChosenSubscriptionId).FirstOrDefault().ProductRole;

			// Check for permissions
			TimeEntryInfo entry = TimeTrackerService.GetTimeEntry(model.TimeEntryId);
			if (entry.UserId == Convert.ToInt32(UserContext.UserId))
			{
				if (!Service.Can(Actions.CoreAction.TimeTrackerEditSelf))
				{
					return this.Json(new { status = "error", message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntryDelete, e = new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntryDelete) });
				}
			}
			else
			{
				if (!Service.Can(Actions.CoreAction.TimeTrackerEditOthers))
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

			// Time entry is locked
			DateTime? lockDate = TimeTrackerService.GetLockDate();
			if (role != ProductRoleIdEnum.TimeTrackerManager && entry.Date <= (lockDate == null ? DateTime.MinValue : lockDate.Value))
			{
				string errorMessage = Resources.TimeTracker.Controllers.TimeEntry.Strings.CanOnlyEdit + " " + lockDate.Value.ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture);
				return this.Json(new
				{
					status = "error",
					message = errorMessage,
					e = new ArgumentException(errorMessage),
					reason = "DATE_LOCKED"
				});
			}

			TimeTrackerService.DeleteTimeEntry(model.TimeEntryId);
			return this.Json(new { status = "success" });
		}
	}
}
