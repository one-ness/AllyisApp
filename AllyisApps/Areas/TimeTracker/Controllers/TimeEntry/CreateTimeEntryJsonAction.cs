//------------------------------------------------------------------------------
// <copyright file="CreateTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

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
			if (model.UserId != this.AppService.UserContext.UserId)
			{
				this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, model.SubscriptionId);
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

				IEnumerable<TimeEntry> otherEntriesToday = AppService.GetTimeEntriesByUserOverDateRange(
					new List<int> { model.UserId },
					Utility.GetDateTimeFromDays(model.Date),
					Utility.GetDateTimeFromDays(model.Date),
					AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId);
				float durationOther = 0.0f;
				foreach (TimeEntry otherEntry in otherEntriesToday)
				{
					durationOther += otherEntry.Duration;
				}

				UserContext.SubscriptionAndRole subInfo = null;
				this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(model.SubscriptionId, out subInfo);

				DateTime? lockDate = AppService.GetLockDate(AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId);
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
				else if (subInfo.ProductRoleId != (int)TimeTrackerRole.Manager && model.Date <= (lockDate == null ? -1 : Utility.GetDaysFromDateTime(lockDate.Value)))
				{
					throw new ArgumentException(Resources.Strings.CanOnlyEdit + " " + lockDate.Value.ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture));
				}

				int id = AppService.CreateTimeEntry(new TimeEntry()
				{
					UserId = model.UserId,
					ProjectId = model.ProjectId,
					PayClassId = model.PayClassId,
					Date = Utility.GetDateTimeFromDays(model.Date) ?? DateTime.Now,
					Duration = durationResult.Value,
					Description = model.Description
				});

				return this.Json(new { status = "success", values = new { duration = this.GetDurationDisplay(model.Duration), description = model.Description, projectId = model.ProjectId, id = id, projectName = AppService.GetProject(model.ProjectId).ProjectName } });
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