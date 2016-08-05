//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Areas.TimeTracker.Models;
using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// GET: /TimeTracker/TimeEntry/Ajax?{params}.
		/// </summary>
		/// <param name="userId">Another user's Id.</param>
		/// <param name="startDate">The beginning of the Date Range.</param>
		/// <param name="endDate">The ending of the Date Range.</param>
		/// <returns>Provides the view for the defined user over the date range defined.</returns>
		public ActionResult Index(int userId = -1, DateTime? startDate = null, DateTime? endDate = null)
		{
			bool manager = AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers);
			
			if (userId != -1 && userId != Convert.ToInt32(UserContext.UserId))
			{ // Trying to edit as another user
				if (!manager)
				{
					throw new UnauthorizedAccessException(AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.CantViewOtherTimeCards);
				}
				//// For a manager editing another user, everything's fine; the next section can be skipped.
			}
			else
			{ // Either userId is -1, or it is the current user
				if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditSelf))
				{ // Current user cannot edit self
					Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
					return this.Redirect("/");
				}

				// Can edit self - we must ensure the userId is actually set and isn't still -1
				userId = Convert.ToInt32(UserContext.UserId);
			}

			ViewBag.canManage = manager;
			try
			{
				TimeEntryOverDateRangeViewModel model = this.ConstructTimeEntryOverDataRangeViewModel(
													userId,
													manager,
													startDate,
													endDate,
													TimeTrackerService.GetLockDate(userId));

				return this.View(model);
			}
			catch (InvalidOperationException e)
			{
				return this.View(ViewConstants.Error, new HandleErrorInfo(e, ControllerConstants.Home, ActionConstants.Index));
			}
		}

		/// <summary>
		/// Constructor for the TimeEntryOverDateRangeViewModel.
		/// </summary>
		/// <param name="userId">The User ID.</param>
		/// <param name="manager">The Manager.</param>
		/// <param name="startingDate">The Starting Date.</param>
		/// <param name="endingDate">The Ending date.</param>
		/// <param name="lockDate">The lock date.</param>
		/// <returns>The constructed TimeEntryOverDateRangeViewModel.</returns>
		public TimeEntryOverDateRangeViewModel ConstructTimeEntryOverDataRangeViewModel(int userId, bool manager, DateTime? startingDate, DateTime? endingDate, DateTime? lockDate)
		{
			// Get all of the projects and initialize their total hours to 0.
			IList<CompleteProjectInfo> allProjects = ProjectService.GetProjectsByUserAndOrganization(userId, false).ToList(); // Must also grab inactive projects, or the app will crash if a user has an entry on a project he is no longer a part of
			IDictionary<int, ProjectHours> hours = new Dictionary<int, ProjectHours>();
			IEnumerable<HolidayInfo> holidays = TimeTrackerService.GetHolidays().Where(x => (startingDate < x.Date && x.Date < endingDate)); // We only care about holidays within the date range

			foreach (CompleteProjectInfo proj in allProjects.Where(p => p.ProjectId > 0))
			{
				// if ( hours.Count == 0 )
				hours.Add(proj.ProjectId, new ProjectHours { Project = proj, Hours = 0.0f });
			}

			allProjects.Insert(0, new CompleteProjectInfo { ProjectId = 0, ProjectName = AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.SelectProject, IsActive = true, IsCustomerActive = true, IsUserActive = true });

			StartOfWeekEnum startOfWeek = TimeTrackerService.GetStartOfWeek();

			IEnumerable<UserInfo> users = CrmService.GetUsersWithSubscriptionToProductInOrganization(UserContext.ChosenOrganizationId, Services.Crm.CrmService.GetProductIdByName(ProductNameKeyConstants.TimeTracker));

			TimeEntryOverDateRangeViewModel result = new TimeEntryOverDateRangeViewModel
			{
				EntryRange = new TimeEntryRangeForUserViewModel
				{
					StartDate = SetStartingDate(startingDate, startOfWeek),
					EndDate = SetEndingDate(endingDate, startOfWeek),
					Entries = new List<EditTimeEntryViewModel>(),
					UserId = userId
				},
				CanManage = manager,
				StartOfWeek = startOfWeek,
				PayClasses = TimeTrackerService.GetPayClasses(),
				GrandTotal = new ProjectHours { Project = new CompleteProjectInfo { ProjectName = "Total" }, Hours = 0.0f },
				Projects = allProjects.Where(x => x.IsActive == true && x.IsCustomerActive == true && x.IsUserActive == true),
				ProjectsWithInactive = allProjects.Where(p => p.ProjectId != 0),
				ProjectHours = hours.Values.Where(x => x.Hours > 0),
				Users = users,
				TotalUsers = users.Count(),
				CurrentUser = users.Where(x => x.UserId == userId).Single(),
				LockDate = TimeTrackerService.GetLockDate(userId)
			};

			// Initialize the starting dates and get all of the time entries within that date range.
			IEnumerable<TimeEntryInfo> timeEntries = TimeTrackerService.GetTimeEntriesByUserOverDateRange(new List<int> { userId }, result.EntryRange.StartDate, result.EntryRange.EndDate);
			IEnumerator<TimeEntryInfo> iter = timeEntries.GetEnumerator();
			iter.MoveNext();

			// Note: Setting this directly insures the weekend highlighting will always be Saturday/Sunday. This makes sense if weekend days are not treated
			// differently with respect to business logic; it's just a stylistic concern. If they need to have separate business logic, this should be switched
			// back to the formula so that the weekend highlighting is based off of start of week.
			// int weekend = 7 + ((int)startOfWeek - 2); // weekend = both days before startOfWeek
			int weekend = (int)StartOfWeekEnum.Saturday;
			bool holidayPopulated = false;

			// For each date in the date range
			for (var date = result.EntryRange.StartDate; date <= result.EntryRange.EndDate;)
			{
				// If has data for this date,
				if (iter.Current != null && iter.Current.Date == date.Date)
				{
					ProjectHours temp = null;

					// Update its project's hours
					if (hours.ContainsKey(iter.Current.ProjectId))
					{
						temp = hours[iter.Current.ProjectId];
						temp.Hours += iter.Current.Duration;
						hours[iter.Current.ProjectId] = temp;
					}

					result.GrandTotal.Hours += iter.Current.Duration;

					// And add its entry to Entries.
					bool isProjectDeleted = result.Projects.Where(x => x.ProjectId == iter.Current.ProjectId).Select(x => x.ProjectName).FirstOrDefault() == null;
					result.EntryRange.Entries.Add(new EditTimeEntryViewModel
					{
						TimeEntryId = iter.Current.TimeEntryId,
						ProjectId = iter.Current.ProjectId,
						PayClassId = iter.Current.PayClassId,
						UserId = iter.Current.UserId,
						Date = TimeTrackerService.GetDayFromDateTime(iter.Current.Date),
						Duration = string.Format("{0:D2}:{1:D2}", (int)iter.Current.Duration, (int)Math.Round((iter.Current.Duration - (int)iter.Current.Duration) * 60, 0)),
						Description = iter.Current.Description,
						StartingDate = TimeTrackerService.GetDayFromDateTime(result.EntryRange.StartDate),
						EndingDate = TimeTrackerService.GetDayFromDateTime(result.EntryRange.EndDate),
						IsOffDay = (weekend % 7 == (int)iter.Current.Date.DayOfWeek || (weekend + 1) % 7 == (int)iter.Current.Date.DayOfWeek) ? true : false,
						IsHoliday = TimeTrackerService.GetHolidays().Any(x => x.Date == date),
						Projects = result.Projects,
						ProjectsWithInactive = result.ProjectsWithInactive,
						ProjectName = allProjects.Where(x => x.ProjectId == iter.Current.ProjectId).Select(x => x.ProjectName).FirstOrDefault(),
						IsProjectDeleted = isProjectDeleted,
						ApprovalState = iter.Current.ApprovalState,
						ModSinceApproval = iter.Current.ModSinceApproval,
						PayClasses = result.PayClasses,
						Locked = iter.Current.ApprovalState == (int)Core.ApprovalState.Approved || isProjectDeleted
					});
					if (holidays.Where(x => x.Date == iter.Current.Date).FirstOrDefault() != null)
					{
						holidayPopulated = true;
					}

					iter.MoveNext();
				}
				else if ((holidays.Where(x => x.Date == date).FirstOrDefault() != null) && (iter.Current == null || iter.Current.Date != date) && !holidayPopulated)
				{
					holidayPopulated = true;

					// Prepopulate holidays (TODO: Also prepopulate PTO?)
					// TODO: There's a lot of design discussion that needs to happen before we implement this. For example, should we have an internal customer with
					// its own projects for Holidays, PTO, etc.? Where should we store the paytypes and durations for holidays? Where do we store the IDs for these?

					result.GrandTotal.Hours += 8;

					TimeEntryInfo timeEntryInfo = new TimeEntryInfo()
					{
						ApprovalState = 1,
						Date = date,
						Duration = 8,
						UserId = userId,
						ProjectId = 0,
						PayClassId = TimeTrackerService.GetPayClassByName("Holiday").PayClassID,
						Description = holidays.Where(x => x.Date == date).First().HolidayName
					};

					int timeEntryId = TimeTrackerService.CreateTimeEntry(timeEntryInfo);

					result.EntryRange.Entries.Add(new EditTimeEntryViewModel
					{
						TimeEntryId = timeEntryId,
						Date = TimeTrackerService.GetDayFromDateTime(date),
						UserId = userId,
						StartingDate = TimeTrackerService.GetDayFromDateTime(result.EntryRange.StartDate),
						EndingDate = TimeTrackerService.GetDayFromDateTime(result.EntryRange.EndDate),
						IsOffDay = (weekend % 7 == (int)date.DayOfWeek || (weekend + 1) % 7 == (int)date.DayOfWeek) ? true : false,
						IsHoliday = TimeTrackerService.GetHolidays().Any(x => x.Date == date),
						Projects = result.Projects,
						ProjectsWithInactive = result.ProjectsWithInactive,
						PayClassId = timeEntryInfo.PayClassId,
						PayClasses = result.PayClasses,
						Duration = string.Format("{0:D2}:{1:D2}", (int)timeEntryInfo.Duration, (int)Math.Round((timeEntryInfo.Duration - (int)timeEntryInfo.Duration) * 60, 0)),
						Description = timeEntryInfo.Description,
						ProjectName = allProjects.Where(x => x.ProjectId == 0).Select(x => x.ProjectName).FirstOrDefault(),
					});
				}
				else
				{
					// Otherwise, create an empty entry.
					result.EntryRange.Entries.Add(new EditTimeEntryViewModel
					{
						Sample = true,
						Date = TimeTrackerService.GetDayFromDateTime(date),
						UserId = userId,
						StartingDate = TimeTrackerService.GetDayFromDateTime(result.EntryRange.StartDate),
						EndingDate = TimeTrackerService.GetDayFromDateTime(result.EntryRange.EndDate),
						IsOffDay = (weekend % 7 == (int)date.DayOfWeek || (weekend + 1) % 7 == (int)date.DayOfWeek) ? true : false,
						IsHoliday = TimeTrackerService.GetHolidays().Any(x => x.Date == date),
						ProjectId = -1,
						Projects = result.Projects,
						ProjectsWithInactive = result.ProjectsWithInactive,
						PayClassId = TimeTrackerService.GetPayClassByName("Regular").PayClassID,
						PayClasses = result.PayClasses
					});

					// Go to the next day.
					date = date.AddDays(1.0d);
					holidayPopulated = false;
				}
			}

			return result;
		}

		private DateTime SetStartingDate(DateTime? date, StartOfWeekEnum startOfWeek)
		{
			if (date == null && !date.HasValue)
			{
				DateTime today = DateTime.Now;
				int daysIntoTheWeek = (int)today.DayOfWeek < (int)startOfWeek
					? (int)today.DayOfWeek + (7 - (int)startOfWeek)
					: (int)today.DayOfWeek - (int)startOfWeek;

				date = today.AddDays(-daysIntoTheWeek);
			}

			return date.Value;
		}

		private DateTime SetEndingDate(DateTime? date, StartOfWeekEnum startOfWeek)
		{
			if (date == null && !date.HasValue)
			{
				DateTime today = DateTime.Now;

				int daysLeftInWeek = (int)today.DayOfWeek < (int)startOfWeek
					? (int)startOfWeek - (int)today.DayOfWeek - 1
					: (6 - (int)today.DayOfWeek) + (int)startOfWeek;

				date = today.AddDays(daysLeftInWeek);
			}

			return date.Value;
		}
	}
}