//------------------------------------------------------------------------------
// <copyright file="EditTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Areas.TimeTracker.Core;
using AllyisApps.Controllers;
using AllyisApps.Lib;
using AllyisApps.Services;
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
		public async Task<ActionResult> EditTimeEntryJson(EditTimeEntryViewModel model)
		{
			if (model.ApprovalState == -1)
			{
				EditTimeEntryViewModel defaults = await ConstructEditTimeEntryViewModel(model.TimeEntryId.Value, model.SubscriptionId);
				return Json(new
				{
					status = "error",
					message = Resources.Strings.InvalidApprovalState,
					reason = "UNDEFINED_APPROVAL",
					action = "REVERT",
					values = new { duration = GetDurationDisplay(defaults.Duration), description = defaults.Description, id = model.TimeEntryId }
				});
			}

			int organizationId = AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId;

			// Check permissions
			if (model.UserId != AppService.UserContext.UserId)
			{
				if (!AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, model.SubscriptionId))
				{
					EditTimeEntryViewModel defaults = await ConstructEditTimeEntryViewModel(model.TimeEntryId.Value, model.SubscriptionId);
					return Json(new
					{
						status = "error",
						message = Resources.Strings.NotAuthZTimeEntryOtherUserEdit,
						action = "REVERT",
						values = new { duration = GetDurationDisplay(defaults.Duration), description = defaults.Description, id = model.TimeEntryId }
					});
				}
			}

			// Authorized for editing
			var timeGet = await AppService.GetTimeEntry(model.TimeEntryId.Value);
			if (timeGet.ApprovalState == (int)ApprovalState.Approved)
			{
				EditTimeEntryViewModel defaults = await ConstructEditTimeEntryViewModel(model.TimeEntryId.Value, model.SubscriptionId);
				return Json(new
				{
					status = "error",
					message = Resources.Strings.AlreadyApprovedCannotEdit,
					action = "REVERT",
					values = new { duration = GetDurationDisplay(defaults.Duration), description = defaults.Description, id = model.TimeEntryId }
				});
			}

			try
			{
				EditTimeEntry(model, true);

				return Json(new { status = "success", values = new { duration = GetDurationDisplay(model.Duration), description = model.Description, id = model.TimeEntryId, projectId = model.ProjectId } });
			}
			catch (ArgumentException e)
			{
				var temp = new
				{
					status = "error",
					message = e.Message
				};

				return Json(temp);
			}
		}

		/// <summary>
		/// Edits a time entry based on a passed in view model.
		/// </summary>
		/// <param name="model"><see cref="EditTimeEntryViewModel"/> to use for edit.</param>
		/// <param name="canManage">Whether user has permission to manage time entry.</param>
		public async void EditTimeEntry(EditTimeEntryViewModel model, bool canManage)
		{
			float? durationResult;
			model.IsManager = canManage;
			if (!(durationResult = ParseDuration(model.Duration)).HasValue)
			{
				throw new ArgumentException(Resources.Strings.DurationFormat);
			}

			if (ParseDuration(model.Duration) == 0)
			{
				throw new ArgumentException(Resources.Strings.EnterATimeLongerThanZero);
			}

			var otherEntriesToday = await AppService.GetTimeEntriesByUserOverDateRange(
				new List<int> { model.UserId },
				Utility.GetDateTimeFromDays(model.Date),
				Utility.GetDateTimeFromDays(model.Date),
				AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId);
			float durationOther = 0.0f;
			foreach (var otherEntry in otherEntriesToday)
			{
				if (model.TimeEntryId != otherEntry.TimeEntryId)
				{
					durationOther += otherEntry.Duration;
				}
			}

			if (durationResult + durationOther > 24.00)
			{
				throw new ArgumentException(Resources.Strings.CannotExceed24);
			}

			if (model.ProjectId <= 0)
			{
				throw new ArgumentException(Resources.Strings.MustSelectProject);
			}

			if (model.PayClassId < 1)
			{
				throw new ArgumentException(Resources.Strings.MustSelectPayClass);
			}

			DateTime? lockDate = await AppService.GetLockDate(AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId);
			if ((!AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, model.SubscriptionId, false)) && model.Date <= (lockDate == null ? -1 : Utility.GetDaysFromDateTime(lockDate.Value)))
			{
				throw new ArgumentException(Resources.Strings.CanOnlyEdit + " " + lockDate.Value.ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture));
			}

			AppService.UpdateTimeEntry(new Services.TimeTracker.TimeEntry()
			{
				TimeEntryId = model.TimeEntryId.Value,
				ProjectId = model.ProjectId,
				PayClassId = model.PayClassId,
				Duration = durationResult.Value,
				Description = model.Description,
				ApprovalState = (int)ApprovalState.NoApprovalState,
				IsLockSaved = (model.ApprovalState == (int)ApprovalState.Approved || model.Date <= model.LockDate || model.IsProjectDeleted || model.IsLockSaved) && !canManage
			});
		}

		/// <summary>
		/// Uses the time entry id to construct a new EditTimeEntryViewModel.
		/// </summary>
		/// <param name="timeEntryId">The time entry Id.</param>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <returns>The EditTimeEntryViewModel.</returns>
		public async Task<EditTimeEntryViewModel> ConstructEditTimeEntryViewModel(int timeEntryId, int subscriptionId)
		{
			var entry = await AppService.GetTimeEntry(timeEntryId);

			DateTime? lockDate = await AppService.GetLockDate(AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId);
			return new EditTimeEntryViewModel
			{
				UserId = entry.UserId,
				TimeEntryId = entry.TimeEntryId,
				ProjectId = entry.ProjectId,
				PayClassId = entry.PayClassId,
				Date = Utility.GetDaysFromDateTime(entry.Date),
				Duration = string.Format("{0:D2}:{1:D2}", (int)entry.Duration, (int)Math.Round((entry.Duration - (int)entry.Duration) * MinutesInHour, 0)),
				Description = entry.Description,
				IsLockSaved = entry.IsLockSaved,
				LockDate = lockDate == null ? -1 : Utility.GetDaysFromDateTime(lockDate.Value),
				IsManager = AppService.GetProductRoleForUser(ProductNameConstants.TimeTracker, entry.UserId, AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId) == "Manager"
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