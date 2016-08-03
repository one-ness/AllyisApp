//------------------------------------------------------------------------------
// <copyright file="CreateTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
		/// Creates a new Time Entry based on the model.
		/// </summary>
		/// <param name="model">The model representing a time entry.</param>
		/// <returns>A JSON object with the results of the action.</returns>
		public ActionResult CreateTimeEntryJson(CreateTimeEntryViewModel model)
		{
			// Check for permission failures
			if (model.UserId == Convert.ToInt32(UserContext.UserId) && !AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditSelf))
			{
				return this.Json(new
				{
					status = "error",
					message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntry,
					e = new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntry)
				});
			}
			else if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers))
			{
				return this.Json(new
				{
					status = "error",
					message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntryOtherUser
				});
			}

			// Authorized to edit this entry
			try
			{
				float? durationResult;
				if (!(durationResult = this.ParseDuration(model.Duration)).HasValue)
				{
					throw new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.DurationFormat);
				}

				IEnumerable<TimeEntryInfo> otherEntriesToday = TimeTrackerService.GetTimeEntriesByUserOverDateRange(new List<int> { model.UserId },
						TimeTrackerService.GetDateTimeFromDays(model.Date), TimeTrackerService.GetDateTimeFromDays(model.Date));
				float durationOther = 0.0f;
				foreach (TimeEntryInfo otherEntry in otherEntriesToday)
				{
					durationOther += otherEntry.Duration;
				}

				if (durationResult + durationOther > 24.00)
				{
					throw new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.CannotExceed24);
				}
				else if (model.ProjectId == 0)
				{
					throw new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.MustSelectProject);
				}
				else if (model.PayClassId < 1)
				{
					throw new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.MustSelectPayClass);
				}
				else if (model.Date <= TimeTrackerService.GetDayFromDateTime(TimeTrackerService.GetLockDate(model.UserId)))
				{
					throw new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.CanOnlyEdit);
				}

				int id = TimeTrackerService.CreateTimeEntry(new TimeEntryInfo()
				{
					UserId = model.UserId,
					ProjectId = model.ProjectId,
					PayClassId = model.PayClassId,
					Date = TimeTrackerService.GetDateTimeFromDays(model.Date),
					Duration = durationResult.Value,
					Description = model.Description
				});

				return this.Json(new { status = "success", values = new { duration = this.GetDurationDisplay(model.Duration), description = model.Description, id = id } });
			}
			catch (ArgumentException e)
			{
				var temp = new
				{
					status = "error",
					message = e.Message
				};

				return this.Json(temp);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}
	}
}