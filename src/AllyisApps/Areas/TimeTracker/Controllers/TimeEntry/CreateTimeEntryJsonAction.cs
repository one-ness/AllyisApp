//------------------------------------------------------------------------------
// <copyright file="CreateTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
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
		/// Creates a new Time Entry based on the model.
		/// </summary>
		/// <param name="model">The model representing a time entry.</param>
		/// <returns>A JSON object with the results of the action.</returns>
		public ActionResult CreateTimeEntryJson(CreateTimeEntryViewModel model)
		{
			ProductRole role = UserContext.UserOrganizationInfoList.Where(o => o.OrganizationId == UserContext.ChosenOrganizationId).SingleOrDefault()
				.UserSubscriptionInfoList.Where(s => s.SubscriptionId == UserContext.ChosenSubscriptionId).FirstOrDefault().ProductRole;

			// Check for permission failures
			if (model.UserId == Convert.ToInt32(UserContext.UserId) && !Service.Can(Actions.CoreAction.TimeTrackerEditSelf))
			{
				return this.Json(new
				{
					status = "error",
					message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntry,
					e = new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntry)
				});
			}
			else if (model.UserId != Convert.ToInt32(UserContext.UserId) && !Service.Can(Actions.CoreAction.TimeTrackerEditOthers))
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

				IEnumerable<TimeEntryInfo> otherEntriesToday = TimeTrackerService.GetTimeEntriesByUserOverDateRange(
					new List<int> { model.UserId },
					TimeTrackerService.GetDateTimeFromDays(model.Date).Value,
					TimeTrackerService.GetDateTimeFromDays(model.Date).Value);
				float durationOther = 0.0f;
				foreach (TimeEntryInfo otherEntry in otherEntriesToday)
				{
					durationOther += otherEntry.Duration;
				}

				DateTime? lockDate = TimeTrackerService.GetLockDate();
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
				else if (role != ProductRole.TimeTrackerManager && model.Date <= (lockDate == null ? -1 : TimeTrackerService.GetDayFromDateTime(lockDate.Value)))
				{
					throw new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.CanOnlyEdit + " " + lockDate.Value.ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture));
				}

				int id = TimeTrackerService.CreateTimeEntry(new TimeEntryInfo()
				{
					UserId = model.UserId,
					ProjectId = model.ProjectId,
					PayClassId = model.PayClassId,
					Date = TimeTrackerService.GetDateTimeFromDays(model.Date).Value,
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
