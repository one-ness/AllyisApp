//------------------------------------------------------------------------------
// <copyright file="EditTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		private const string HourMinutePattern = @"^(\d+):(\d+)$";
		private const string DecimalPattern = @"^\d*\.?\d*$";
		private const float MinutesInHour = 60.0f;

		/// <summary>
		/// Edits a Time Entry based on the model.
		/// </summary>
		/// <param name="subscriptionId">The subscription that the user belongs to.</param>
		/// <param name="entry">The model representing a time entry.</param>
		/// <returns>The edited version of Time Entry.</returns>
		public async Task<CreateUpdateTimeEntryResult> EditTimeEntry(int subscriptionId, TimeEntry entry)
		{
			// Check permissions
			if (entry.UserId != AppService.UserContext.UserId && !AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId))
			{
				return CreateUpdateTimeEntryResult.NotAuthZTimeEntryOtherUserEdit;
			}

			// Authorized for editing
			int organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var validationResult = await AppService.ValidateTimeEntryCreateUpdate(entry, organizationId);
			if (validationResult != CreateUpdateTimeEntryResult.Success)
			{
				return validationResult;
			}
			//TODO: Any project validation must also take into account editing past entries where the user used to be assigned to the project but now is not

			//Any edits made to an overtime period must be reset to pending
			await AppService.UpdateOvertimePeriodToPending(organizationId, entry.UserId, entry.Date);

			AppService.UpdateTimeEntry(entry);

			await AppService.RecalculateOvertime(organizationId, entry.Date, entry.UserId);

			return CreateUpdateTimeEntryResult.Success;
		}
	}
}
