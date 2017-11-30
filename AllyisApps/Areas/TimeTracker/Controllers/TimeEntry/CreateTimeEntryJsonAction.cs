//------------------------------------------------------------------------------
// <copyright file="CreateTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Lib;
using AllyisApps.Resources;
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
				// DateTime dateGet = model.Date != 0 ? modelDate : DateTime.Now; // do we need this??
				DateTime modelDate = Utility.GetDateTimeFromDays(model.Date);
				float modelDuration = ParseDuration(model.Duration) ?? throw new ArgumentException(Strings.DurationFormat);
				int organizationId = AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId;

				var newEntry = new TimeEntry
				{
					UserId = model.UserId,
					ProjectId = model.ProjectId,
					PayClassId = model.PayClassId,
					Date = modelDate,
					Duration = modelDuration,
					Description = model.Description
				};

				CreateUpdateTimeEntryResult validationResult = await AppService.ValidateTimeEntryCreateUpdate(newEntry, organizationId);

				switch (validationResult)
				{
					case CreateUpdateTimeEntryResult.OvertimePayClass:
						return Json(new
						{
							status = "error",
							message = "Cannot create overtime hours. These are automatically calculated."
						});
					case CreateUpdateTimeEntryResult.InvalidPayClass:
						return Json(new
						{
							status = "error",
							message = Strings.MustSelectPayClass + " For time entry on date " + modelDate.ToShortDateString()
						});
					case CreateUpdateTimeEntryResult.InvalidProject:
						return Json(new
						{
							status = "error",
							message = Strings.MustSelectProject + " For time entry on date " + modelDate.ToShortDateString()
						});
					case CreateUpdateTimeEntryResult.ZeroDuration:
						return Json(new
						{
							status = "error",
							message = Strings.EnterATimeLongerThanZero
						});
					case CreateUpdateTimeEntryResult.Over24Hours:
						return Json(new
						{
							status = "error",
							message = Strings.CannotExceed24 + " For time entry on date " + modelDate.ToShortDateString()
						});
					case CreateUpdateTimeEntryResult.EntryIsLocked:
						DateTime? lockDate = (await AppService.GetSettingsByOrganizationId(organizationId)).LockDate;
						return Json(new
						{
							status = "error",
							message = Strings.CanOnlyEdit + " " + lockDate.Value.ToString("d", Thread.CurrentThread.CurrentCulture)
						});
					case CreateUpdateTimeEntryResult.Success:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				//validate correct project
				var projects = await AppService.GetProjectsByUserAndOrganization(newEntry.UserId, organizationId);
				var project = projects.SingleOrDefault(p => newEntry.ProjectId == p.ProjectId);
				if (project == null || !project.IsUserActive)
				{
					throw new ArgumentException(Strings.MustBeAssignedToProject);
				}

				if (project.StartDate != null && modelDate < project.StartDate
					|| project.EndDate != null && modelDate > project.EndDate)
				{
					throw new ArgumentException(Strings.ProjectIsNotActive);
				}

				//Calculate overtime
				if (newEntry.PayClassId == 1) // Pay class id 1 is regular
				{
					float amountAboveOvertimeLimit = await AppService.GetNewHoursAboveOvertimeLimit(organizationId, newEntry.UserId, modelDuration, modelDate);

					//if we need to have more overtime hours, either add to existing, or create a new overtime entry
					if (amountAboveOvertimeLimit > 0)
					{
						TimeEntry overtimeEntry = (await AppService.GetTimeEntriesByUserOverDateRange(new List<int> { newEntry.UserId }, modelDate, modelDate, organizationId))
							.FirstOrDefault(entry => entry.ProjectId == newEntry.ProjectId && entry.PayClassName == "Overtime");

						modelDuration -= amountAboveOvertimeLimit;

						if (overtimeEntry != null)
						{
							overtimeEntry.Duration += amountAboveOvertimeLimit;
							AppService.UpdateTimeEntry(overtimeEntry);
						}
						else
						{
							overtimeEntry = new TimeEntry
							{
								UserId = newEntry.UserId,
								ProjectId = newEntry.ProjectId,
								PayClassId = 7,
								Date = modelDate,
								Duration = amountAboveOvertimeLimit,
								Description = string.Empty
							};
							await AppService.CreateTimeEntry(overtimeEntry);
						}
					}
				}

				//modelDuration is 0 if all the hours went to overtime
				if (modelDuration > 0)
				{

					int newTimeEntryId = await AppService.CreateTimeEntry(newEntry);

					return Json(new
					{
						status = "success",
						values = new
						{
							duration = GetDurationDisplay(model.Duration),
							description = newEntry.Description,
							projectId = newEntry.ProjectId,
							id = newTimeEntryId,
							projectName = AppService.GetProject(newEntry.ProjectId).ProjectName
						}
					});
				}

				return Json(new { status = "success" });
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