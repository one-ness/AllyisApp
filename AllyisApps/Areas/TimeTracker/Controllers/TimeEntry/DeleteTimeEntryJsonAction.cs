﻿//------------------------------------------------------------------------------
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
			ProductRoleIdEnum role = UserContext.ChosenSubscription.ProductRole;

			// Check for permissions
			TimeEntryInfo entry = AppService.GetTimeEntry(model.TimeEntryId);
            int organizationId = AppService.GetSubscription(model.SubscriptionId).OrganizationId;
			if (entry.UserId == Convert.ToInt32(UserContext.UserId))
			{
				if (!AppService.Can(Actions.CoreAction.TimeTrackerEditSelf, false, organizationId, model.SubscriptionId))
				{
					return this.Json(new { status = "error", message = Resources.Strings.NotAuthZTimeEntryDelete, e = new UnauthorizedAccessException(Resources.Strings.NotAuthZTimeEntryDelete) });
				}
			}
			else
			{
				if (!AppService.Can(Actions.CoreAction.TimeTrackerEditOthers, false, organizationId, model.SubscriptionId))
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
			if (role != ProductRoleIdEnum.TimeTrackerManager && entry.Date <= (lockDate == null ? DateTime.MinValue : lockDate.Value))
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
