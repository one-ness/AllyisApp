//------------------------------------------------------------------------------
// <copyright file="DeleteTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using AllyisApps.Areas.TimeTracker.Core;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System.Threading.Tasks;

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
		async public Task<ActionResult> DeleteTimeEntryJson(DeleteTimeEntryViewModel model)
		{
			// Check for permissions
			Services.TimeTracker.TimeEntry entry = AppService.GetTimeEntry(model.TimeEntryId);
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
			DateTime? lockDate = await AppService.GetLockDate(AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId);
			if ((!this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.TimeEntry, model.SubscriptionId)) && entry.Date <= (lockDate == null ? DateTime.MinValue : lockDate.Value))
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
			return this.Json(new { status = "success", values = new { duration = this.GetDurationDisplay(model.Duration).Insert(0, "-"), projectId = entry.ProjectId } });
		}

		/// <summary>
		/// Delete Time Entry from Save button.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		async protected Task<ActionResult> DeleteTimeEntryJson(EditTimeEntryViewModel model)
		{
			if (!model.IsDeleted)
			{
				throw new Exception("Attempt to delete edited view that was not marked for deletion");
			}

			return await DeleteTimeEntryJson(new DeleteTimeEntryViewModel()
			{
				ApprovalState = model.ApprovalState,
				Duration = model.Duration,
				SubscriptionId = model.SubscriptionId,
				TimeEntryId = model.TimeEntryId.Value
			});
		}
	}
}