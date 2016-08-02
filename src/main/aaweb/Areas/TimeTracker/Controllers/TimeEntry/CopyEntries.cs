﻿//------------------------------------------------------------------------------
// <copyright file="CopyEntries.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
		/// Creates a new Time Entry based on the entries for the provided range of dates.
		/// The target start date must be more recent than the copy date range, and the end copy date must be more recent than the start copy date.
		/// </summary>
		/// <param name="startDateTarget">The start (inclusive) of the date range for the entries to be created.</param>
		/// <param name="startDateCopy">The start (inclusive) of the date range for the entries to be copied.</param>
		/// <param name="endDateCopy">The end (inclusive) of the date range for the entries to be copied.</param>
		/// <param name="userId">The id for the user whose entries are being edited.</param>
		/// <returns>The action result.</returns>
		public ActionResult CopyEntries(DateTime startDateTarget, DateTime startDateCopy, DateTime endDateCopy, int userId)
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
			#endregion

			// Check for permission failures TODO flesh these out
			if (userId == UserContext.UserId && !AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditSelf))
			{
				return this.View(ViewConstants.Error);
			}
			else if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers))
			{
				return this.View(ViewConstants.Error);
			}

			// Authorized to edit this entry
			IEnumerable<TimeEntryInfo> entriesCopy = TimeTrackerService.GetTimeEntriesByUserOverDateRange(new List<int> { userId }, UserContext.ChosenOrganizationId, startDateCopy, endDateCopy);
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

			return this.RedirectToAction(ActionConstants.Index); // Model should be repopulated with new info at the index
		}
	}
}