//------------------------------------------------------------------------------
// <copyright file="EditTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

using AllyisApps.Areas.TimeTracker.Core;
using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

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
		/// <param name="model">The model representing a time entry.</param>
		/// <returns>The edited version of Time Entry.</returns>
		public ActionResult EditTimeEntryJson(EditTimeEntryViewModel model)
		{
			if (model.ApprovalState == -1)
			{
				EditTimeEntryViewModel defaults = this.ConstructEditTimeEntryViewModel(model.TimeEntryId);
				return this.Json(new
				{
					status = "error",
					message = Resources.TimeTracker.Controllers.TimeEntry.Strings.InvalidApprovalState,
					reason = "UNDEFINED_APPROVAL",
					action = "REVERT",
					values = new { duration = this.GetDurationDisplay(defaults.Duration), description = defaults.Description, id = model.TimeEntryId }
				});
			}

			// Check permissions
			if (model.UserId == Convert.ToInt32(UserContext.UserId))
			{
				if (!Service.Can(Actions.CoreAction.TimeTrackerEditSelf))
				{
					EditTimeEntryViewModel defaults = this.ConstructEditTimeEntryViewModel(model.TimeEntryId);
					return this.Json(new
					{
						status = "error",
						message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntryEdit,
						action = "REVERT",
						values = new { duration = this.GetDurationDisplay(defaults.Duration), description = defaults.Description, id = model.TimeEntryId }
					});
				}
			}
			else
			{
				if (!Service.Can(Actions.CoreAction.TimeTrackerEditOthers))
				{
					EditTimeEntryViewModel defaults = this.ConstructEditTimeEntryViewModel(model.TimeEntryId);
					return this.Json(new
					{
						status = "error",
						message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZTimeEntryOtherUserEdit,
						action = "REVERT",
						values = new { duration = this.GetDurationDisplay(defaults.Duration), description = defaults.Description, id = model.TimeEntryId }
					});
				}
			}

			// Authorized for editing
			if (TimeTrackerService.GetTimeEntry(model.TimeEntryId).ApprovalState == (int)ApprovalState.Approved)
			{
				EditTimeEntryViewModel defaults = this.ConstructEditTimeEntryViewModel(model.TimeEntryId);
				return this.Json(new
				{
					status = "error",
					message = Resources.TimeTracker.Controllers.TimeEntry.Strings.AlreadyApprovedCannotEdit,
					action = "REVERT",
					values = new { duration = this.GetDurationDisplay(defaults.Duration), description = defaults.Description, id = model.TimeEntryId }
				});
			}

			try
			{
				EditTimeEntry(model, true);

				return this.Json(new { status = "success", values = new { duration = this.GetDurationDisplay(model.Duration), description = model.Description, id = model.TimeEntryId } });
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
		}

		/// <summary>
		/// Edits a time entry based on a passed in view model.
		/// </summary>
		/// <param name="model"><see cref="EditTimeEntryViewModel"/> to use for edit.</param>
		/// <param name="canManage">Whether user has permission to manage time entry.</param>
		public void EditTimeEntry(EditTimeEntryViewModel model, bool canManage)
        {
            ProductRole role = UserContext.UserOrganizationInfoList.Where(o => o.OrganizationId == UserContext.ChosenOrganizationId).SingleOrDefault()
                .UserSubscriptionInfoList.Where(s => s.SubscriptionId == UserContext.ChosenSubscriptionId).FirstOrDefault().ProductRole;

            float? durationResult;
			model.IsManager = canManage;
			if (!(durationResult = this.ParseDuration(model.Duration)).HasValue)
			{
				throw new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.UnableParseDuration);
			}

			IEnumerable<TimeEntryInfo> otherEntriesToday = TimeTrackerService.GetTimeEntriesByUserOverDateRange(
				new List<int> { model.UserId },
				TimeTrackerService.GetDateTimeFromDays(model.Date),
				TimeTrackerService.GetDateTimeFromDays(model.Date));
			float durationOther = 0.0f;
			foreach (TimeEntryInfo otherEntry in otherEntriesToday)
			{
				if (model.TimeEntryId != otherEntry.TimeEntryId)
				{
					durationOther += otherEntry.Duration;
				}
			}

			if (durationResult + durationOther > 24.00)
			{
				throw new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.CannotExceed24);
			}

			if (model.ProjectId == 0)
			{
				throw new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.MustSelectProject);
			}

			if (model.PayClassId < 1)
			{
				throw new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.MustSelectPayClass);
			}

            DateTime? lockDate = TimeTrackerService.GetLockDate();
            if (role != ProductRole.TimeTrackerManager && model.Date <= (lockDate == null ? -1 : TimeTrackerService.GetDayFromDateTime(lockDate.Value)))
            {
                throw new ArgumentException(Resources.TimeTracker.Controllers.TimeEntry.Strings.CanOnlyEdit + " " + lockDate.Value.ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            TimeTrackerService.UpdateTimeEntry(new TimeEntryInfo()
			{
				TimeEntryId = model.TimeEntryId,
				ProjectId = model.ProjectId,
				PayClassId = model.PayClassId,
				Duration = durationResult.Value,
				Description = model.Description,
				ApprovalState = (int)ApprovalState.NoApprovalState,
				LockSaved = (model.ApprovalState == (int)Core.ApprovalState.Approved || model.Date <= model.LockDate || model.IsProjectDeleted || model.LockSaved) && !canManage
			});
		}

		/// <summary>
		/// Uses the time entry id to construct a new EditTimeEntryViewModel.
		/// </summary>
		/// <param name="timeEntryId">The time entry ID.</param>
		/// <returns>The EditTimeEntryViewModel.</returns>
		public EditTimeEntryViewModel ConstructEditTimeEntryViewModel(int timeEntryId)
		{
			TimeEntryInfo entry = TimeTrackerService.GetTimeEntry(timeEntryId);

            DateTime? lockDate = TimeTrackerService.GetLockDate();
			return new EditTimeEntryViewModel
			{
				UserId = entry.UserId,
				TimeEntryId = entry.TimeEntryId,
				ProjectId = entry.ProjectId,
				PayClassId = entry.PayClassId,
				Date = TimeTrackerService.GetDayFromDateTime(entry.Date),
				Duration = string.Format("{0:D2}:{1:D2}", (int)entry.Duration, (int)Math.Round((entry.Duration - (int)entry.Duration) * MinutesInHour, 0)),
				Description = entry.Description,
				LockSaved = entry.LockSaved,
				LockDate = lockDate == null ? -1 : TimeTrackerService.GetDayFromDateTime(lockDate.Value),
				IsManager = Service.GetProductRoleForUser(ProductNameKeyConstants.TimeTracker, entry.UserId) == "Manager"
			};
		}

		/// <summary>
		/// Takes a string duration value written either in decimal hours or hours/minutes and converts it into hours/minutes (HH:MM) format.
		/// </summary>
		/// <param name="duration">Duration in either format.</param>
		/// <returns>Duration formatted as HH:MM.</returns>
		public string GetDurationDisplay(string duration)
		{
			float? parsedDuration = ParseDuration(duration);
			return parsedDuration.HasValue ? string.Format("{0:D2}:{1:D2}", (int)parsedDuration, (int)Math.Round((parsedDuration.Value - (int)parsedDuration.Value) * 60.0f, 0)) : null;
		}

		/// <summary>
		/// Parses the input duration for either HH.HH or HH:MM format.
		/// </summary>
		/// <param name="duration">Duration in either format.</param>
		/// <returns>Parsed duration or null.</returns>
		public float? ParseDuration(string duration)
		{
			float? durationOut = null;
			Match theMatch;
			if (!string.IsNullOrWhiteSpace(duration))
			{
				if ((theMatch = Regex.Match(duration, HourMinutePattern)).Success)
				{
					float minutes = int.Parse(theMatch.Groups[2].Value) / MinutesInHour;
					durationOut = float.Parse(theMatch.Groups[1].Value) + minutes;
				}
				else if ((theMatch = Regex.Match(duration, DecimalPattern)).Success)
				{
					durationOut = float.Parse(duration);
				}
			}

			return durationOut;
		}
	}
}