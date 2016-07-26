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
					message = "You are not authorized to create a time entry.",
					e = new UnauthorizedAccessException("You are not authorized to create a time entry.")
				});
			}
			else if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers))
			{
				return this.Json(new
				{
					status = "error",
					message = "You are not authorized to create a time entry for another user!"
				});
			}

			// Authorized to edit this entry
			try
			{
				float? durationResult;
				if (!(durationResult = this.ParseDuration(model.Duration)).HasValue)
				{
					throw new ArgumentException("You must enter the time duration as HH:MM or H.HH format.");
				}

				IEnumerable<TimeEntryInfo> otherEntriesToday = TimeTrackerService.GetTimeEntriesByUserOverDateRange(new List<int> { model.UserId }, model.OrganizationId, model.Date, model.Date);
				float durationOther = 0.0f;
				foreach (TimeEntryInfo otherEntry in otherEntriesToday)
				{
					durationOther += otherEntry.Duration;
				}

				if (durationResult + durationOther > 24.00)
				{
					throw new ArgumentException("Duration cannot exceed 24 hours in a day.");
				}
				else if (model.ProjectId == 0)
				{
					throw new ArgumentException("You must select a project.");
				}
				else if (model.PayClassId < 1)
				{
					throw new ArgumentException("You must select a pay class.");
				}
				else if (model.Date <= TimeTrackerService.GetLockDate(model.OrganizationId, model.UserId))
				{
					throw new ArgumentException("You can only edit last week, this week, and any date up till the end of next month.");
				}

				int id = TimeTrackerService.CreateTimeEntry(new TimeEntryInfo()
				{
					UserId = model.UserId,
					ProjectId = model.ProjectId,
					PayClassId = model.PayClassId,
					Date = model.Date,
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