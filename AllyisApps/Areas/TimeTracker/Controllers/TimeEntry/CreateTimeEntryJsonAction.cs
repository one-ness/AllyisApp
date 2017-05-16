//------------------------------------------------------------------------------
// <copyright file="CreateTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
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
			ProductRoleIdEnum role = UserContext.UserOrganizationInfoList.Where(o => o.OrganizationId == UserContext.ChosenOrganizationId).SingleOrDefault()
				.UserSubscriptionInfoList.Where(s => s.SubscriptionId == UserContext.ChosenSubscriptionId).FirstOrDefault().ProductRole;

			// Check for permission failures
			if (model.UserId == Convert.ToInt32(UserContext.UserId) && !Service.Can(Actions.CoreAction.TimeTrackerEditSelf))
			{
				return this.Json(new
				{
					status = "error",
					message = Resources.Strings.NotAuthZTimeEntry,
					e = new UnauthorizedAccessException(Resources.Strings.NotAuthZTimeEntry)
				});
			}
			else if (model.UserId != Convert.ToInt32(UserContext.UserId) && !Service.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				return this.Json(new
				{
					status = "error",
					message = Resources.Strings.NotAuthZTimeEntryOtherUser
				});
			}

			// Authorized to edit this entry
			try
			{
				float? durationResult;
				if (!(durationResult = this.ParseDuration(model.Duration)).HasValue)
				{
					throw new ArgumentException(Resources.Strings.DurationFormat);
				}
				if (this.ParseDuration(model.Duration) == 0)
				{
					throw new ArgumentException(Resources.Strings.EnterATimeLongerThanZero);
				}

				IEnumerable<TimeEntryInfo> otherEntriesToday = Service.GetTimeEntriesByUserOverDateRange(
					new List<int> { model.UserId },
					Service.GetDateTimeFromDays(model.Date).Value,
					Service.GetDateTimeFromDays(model.Date).Value);
				float durationOther = 0.0f;
				foreach (TimeEntryInfo otherEntry in otherEntriesToday)
				{
					durationOther += otherEntry.Duration;
				}

				DateTime? lockDate = Service.GetLockDate();
				if (durationResult + durationOther > 24.00)
				{
					throw new ArgumentException(Resources.Strings.CannotExceed24);
				}
				else if (model.ProjectId <= 0)
				{
					throw new ArgumentException(Resources.Strings.MustSelectProject);
				}
				else if (model.PayClassId < 1)
				{
					throw new ArgumentException(Resources.Strings.MustSelectPayClass);
				}
				else if (role != ProductRoleIdEnum.TimeTrackerManager && model.Date <= (lockDate == null ? -1 : Service.GetDayFromDateTime(lockDate.Value)))
				{
					throw new ArgumentException(Resources.Strings.CanOnlyEdit + " " + lockDate.Value.ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture));
				}

				int id = Service.CreateTimeEntry(new TimeEntryInfo()
				{
					UserId = model.UserId,
					ProjectId = model.ProjectId,
					PayClassId = model.PayClassId,
					Date = Service.GetDateTimeFromDays(model.Date).Value,
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
