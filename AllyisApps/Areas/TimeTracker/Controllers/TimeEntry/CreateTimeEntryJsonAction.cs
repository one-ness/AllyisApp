//------------------------------------------------------------------------------
// <copyright file="CreateTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Lib;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
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
		public async Task<ActionResult> CreateTimeEntryJson(CreateTimeEntryViewModel model)
		{
			if (model.UserId != AppService.UserContext.UserId)
			{
				AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, model.SubscriptionId);
			}

			// Authorized to edit this entry
			try
			{
				float? durationResult;
				if (!(durationResult = ParseDuration(model.Duration)).HasValue)
				{
					throw new ArgumentException(Strings.DurationFormat);
				}

				if (ParseDuration(model.Duration) == 0)
				{
					throw new ArgumentException(Strings.EnterATimeLongerThanZero);
				}

				IEnumerable<Services.TimeTracker.TimeEntry> otherEntriesToday = await AppService.GetTimeEntriesByUserOverDateRange(
					new List<int> { model.UserId },
					Utility.GetDateTimeFromDays(model.Date),
					Utility.GetDateTimeFromDays(model.Date),
					AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId);
				float durationOther = 0.0f;
				foreach (Services.TimeTracker.TimeEntry otherEntry in otherEntriesToday)
				{
					durationOther += otherEntry.Duration;
				}

				UserContext.SubscriptionAndRole subInfo = null;
				AppService.UserContext.SubscriptionsAndRoles.TryGetValue(model.SubscriptionId, out subInfo);

				DateTime? lockDate = (await AppService.GetSettingsByOrganizationId(AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId)).LockDate;
				if (durationResult + durationOther > 24.00)
				{
					throw new ArgumentException(Strings.CannotExceed24 + " For time entry on date " + Utility.GetDateTimeFromDays(model.Date).ToString());
				}

				if (model.ProjectId <= 0)
				{
					throw new ArgumentException(Strings.MustSelectProject + " For time entry on date " + Utility.GetDateTimeFromDays(model.Date).ToString());
				}

				if (model.PayClassId < 1)
				{
					throw new ArgumentException(Strings.MustSelectPayClass + " For time entry on date " + Utility.GetDateTimeFromDays(model.Date).ToString());
				}

				if (subInfo.ProductRoleId != (int)TimeTrackerRole.Manager && model.Date <= (lockDate == null ? -1 : Utility.GetDaysFromDateTime(lockDate.Value)))
				{
					throw new ArgumentException(Strings.CanOnlyEdit + " " + lockDate.Value.ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture));
				}

				//validate correct project
				int organizationId = AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId;
				var projects = await AppService.GetProjectsByUserAndOrganization(model.UserId, organizationId);
				var project = projects.SingleOrDefault(p => model.ProjectId == p.ProjectId);
				if (project == null || !project.IsUserActive)
				{
					throw new ArgumentException(Strings.MustBeAssignedToProject);
				}

				if (project.StartDate != null && Utility.GetDateTimeFromDays(model.Date) < project.StartDate
					|| project.EndDate != null && Utility.GetDateTimeFromDays(model.Date) > project.EndDate)
				{
					throw new ArgumentException(Strings.ProjectIsNotActive);
				}



				DateTime dateGet = model.Date != 0 ? Utility.GetDateTimeFromDays(model.Date) : DateTime.Now;

				int id = await AppService.CreateTimeEntry(new Services.TimeTracker.TimeEntry
				{
					UserId = model.UserId,
					ProjectId = model.ProjectId,
					PayClassId = model.PayClassId,
					Date = dateGet,
					Duration = durationResult.Value,
					Description = model.Description
				});

				return Json(new
				{
					status = "success",
					values = new
					{
						duration = GetDurationDisplay(model.Duration),
						description = model.Description,
						projectId = model.ProjectId,
						id = id,
						projectName = AppService.GetProject(model.ProjectId).ProjectName
					}
				});
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
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}

		/// <summary>
		/// Create Action for TimeEntry
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private async Task<ActionResult> CreateTimeEntryJson(EditTimeEntryViewModel model)
		{
			if (!model.IsCreated || model.TimeEntryId.HasValue && model.TimeEntryId.Value != 0)
			{
				throw new Exception("Attempt to create entry that should have been edited");
			}

			return await CreateTimeEntryJson(new CreateTimeEntryViewModel
			{
				Date = model.Date,
				Description = model.Description,
				Duration = model.Duration,
				PayClassId = model.PayClassId,
				ProjectId = model.ProjectId,
				SubscriptionId = model.SubscriptionId,
				UserId = model.UserId
			});
		}
	}
}