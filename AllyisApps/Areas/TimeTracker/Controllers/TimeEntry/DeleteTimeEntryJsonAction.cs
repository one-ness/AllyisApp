//------------------------------------------------------------------------------
// <copyright file="DeleteTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{

		/// <summary>
		/// Deletes a Time Entry.
		/// </summary>
		/// <param name="subscriptionId">The subscription that the user belongs to.</param>
		/// <param name="entry">The model representing the time entry to be deleted.</param>
		/// <returns>JSON Status. {status: 'success|error', message: 'a string'}.</returns>
		[HttpPost]
		public async Task<CreateUpdateTimeEntryResult> DeleteTimeEntry(int subscriptionId, TimeEntry entry)
		{
			// Check for permissions
			if (entry.UserId != AppService.UserContext.UserId && !AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId))
			{
				return CreateUpdateTimeEntryResult.NotAuthZTimeEntryOtherUserEdit;
			}

			// Is time entry locked
			int organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			DateTime? lockDate = (await AppService.GetSettingsByOrganizationId(organizationId)).LockDate;
			if (!AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.TimeEntry, subscriptionId) && entry.Date <= (lockDate ?? DateTime.MinValue))
			{
				return CreateUpdateTimeEntryResult.EntryIsLocked;
			}

			//Any edits made to an overtime period must be reset to pending
			await AppService.UpdateOvertimePeriodToPending(organizationId, entry.UserId, entry.Date);

			await AppService.DeleteTimeEntry(entry.TimeEntryId);

			return CreateUpdateTimeEntryResult.Success;
		}
	}
}