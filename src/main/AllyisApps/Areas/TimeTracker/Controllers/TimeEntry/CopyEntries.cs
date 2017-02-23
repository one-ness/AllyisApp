//------------------------------------------------------------------------------
// <copyright file="CopyEntries.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
using System;
using System.Collections.Generic;
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
		/// Creates a new Time Entry based on the entries for the provided range of dates.
		/// The target start date must be more recent than the copy date range, and the end copy date must be more recent than the start copy date.
		/// </summary>
		/// <param name="startDateTarget">The start (inclusive) of the date range for the entries to be created.</param>
		/// <param name="startDateCopy">The start (inclusive) of the date range for the entries to be copied.</param>
		/// <param name="endDateCopy">The end (inclusive) of the date range for the entries to be copied.</param>
		/// <param name="userId">The id for the user whose entries are being edited.</param>
		/// <param name="startDate">The start of the date range of the TimeEntry index page.</param>
		/// <param name="endDate">The end of the date range of the TimeEntry index page.</param>
		/// <returns>The action result.</returns>
		public ActionResult CopyEntries(DateTime startDateTarget, DateTime startDateCopy, DateTime endDateCopy, int userId, int startDate, int endDate)
		{
			#region Validation

			// TODO flesh these out
			if (startDateCopy > endDateCopy)
			{
				return this.View(ViewConstants.Error);
			}
			else if (startDateCopy > startDateTarget)
			{
				return this.View(ViewConstants.Error);
			}

			#endregion Validation

			// Check for permission failures TODO flesh these out
			if (userId == UserContext.UserId && !Service.Can(Actions.CoreAction.TimeTrackerEditSelf))
			{
				return this.View(ViewConstants.Error);
			}
			else if (userId != UserContext.UserId && !Service.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				return this.View(ViewConstants.Error);
			}

			// Authorized to edit this entry
			// Remove existing entries in target range
			DateTime endDateTarget = startDateTarget.AddDays(endDateCopy.Subtract(startDateCopy).Days);
			IEnumerable<TimeEntryInfo> entriesRemove = TimeTrackerService.GetTimeEntriesByUserOverDateRange(new List<int> { userId }, startDateTarget, endDateTarget);
			foreach (TimeEntryInfo entry in entriesRemove)
			{
				TimeTrackerService.DeleteTimeEntry(entry.TimeEntryId);
			}

			// Add copied entries
			IEnumerable<TimeEntryInfo> entriesCopy = TimeTrackerService.GetTimeEntriesByUserOverDateRange(new List<int> { userId }, startDateCopy, endDateCopy);
			for (int i = 0; startDateCopy.Date.AddDays(i) <= endDateCopy.Date; ++i)
			{
				// Cover all entries for that day
				foreach (TimeEntryInfo entry in entriesCopy.Where(x => x.Date == startDateCopy.Date.AddDays(i)))
				{
					TimeTrackerService.CreateTimeEntry(new TimeEntryInfo
					{
						UserId = userId,
						ProjectId = entry.ProjectId,
						PayClassId = entry.PayClassId,
						Date = startDateTarget.Date.AddDays(i),
						Duration = entry.Duration,
						Description = entry.Description
					});
				}
			}

			return this.RedirectToAction(
				ActionConstants.Index,
				new
				{
					userId = userId,
					startDate = startDate,
					endDate = endDate
				}); // Model should be repopulated with new info at the index
		}
	}
}
