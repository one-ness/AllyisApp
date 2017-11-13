﻿//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
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
using AllyisApps.Services.Crm;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.Project;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using static AllyisApps.ViewModels.TimeTracker.TimeEntry.TimeEntryOverDateRangeViewModel;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// GET: /TimeTracker/TimeEntry/Ajax?{params}.
		/// </summary>
		/// <param name="subscriptionId">The SubscriptionId.</param>
		/// <param name="userId">The id of the targeted user. Null means the current user is selected.</param>
		/// <param name="startDate">The beginning of the Date Range.</param>
		/// <param name="endDate">The ending of the Date Range.</param>
		/// <returns>Provides the view for the defined user over the date range defined.</returns>
		public async Task<ActionResult> Index(int subscriptionId, int? startDate = null, int? endDate = null, int? userId = null)
		{
			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.TimeEntry, subscriptionId);
			var sub = AppService.UserContext.SubscriptionsAndRoles[subscriptionId];

			//redirect back to self with non-null default start/end dates so that we can get the desired route url
			if (startDate == null || endDate == null)
			{
				PayPeriodRanges payPeriodRanges = await AppService.GetPayPeriodRanges(sub.OrganizationId);
				startDate = Utility.GetDaysFromDateTime(payPeriodRanges.Current.StartDate);
				endDate = Utility.GetDaysFromDateTime(payPeriodRanges.Current.EndDate);
				return RedirectToAction(ActionConstants.Index, ControllerConstants.TimeEntry, new { subscriptionId, startDate, endDate, userId });
			}

			int actualUserId = userId ?? AppService.UserContext.UserId;
			int productRoleId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].ProductRoleId;
			bool isManager = productRoleId == (int)TimeTrackerRole.Manager;


			ViewBag.GetDateTimeFromDays = new Func<int?, DateTime?>(Utility.GetNullableDateTimeFromDays);
			ViewBag.SignedInUserID = AppService.UserContext.UserId;
			ViewBag.SelectedUserId = actualUserId;
			ViewBag.WeekStart = startDate.Value;
			ViewBag.WeekEnd = endDate.Value;
			ViewBag.CanManage = isManager;

			TimeEntryOverDateRangeViewModel model = await ConstructTimeEntryOverDataRangeViewModel(
				sub.OrganizationId,
				subscriptionId,
				sub.SubscriptionName,
				actualUserId,
				isManager,
				startDate,
				endDate);

			return View("Index2", model);
		}

		/// <summary>
		///  Redirect rout for date picker so route is propperly displayed.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="startDate">The start date.</param>
		/// <param name="endDate">The end date.</param>
		/// <param name="userid"></param>
		/// <returns>A redirect to the index action.</returns>
		public RedirectToRouteResult TimeTrackerDatePickerRedirect(int subscriptionId, DateTime startDate, DateTime endDate, int? userid)
		{
			int start = Utility.GetDaysFromDateTime(startDate);
			int end = Utility.GetDaysFromDateTime(endDate);
			return RedirectToAction(ActionConstants.Index, ControllerConstants.TimeEntry, new { subscriptionId, startDate = start, endDate = end, userid = userid });
		}

		/// <summary>
		/// Constructor for the TimeEntryOverDateRangeViewModel.
		/// </summary>
		/// <param name="orgId">The Organization Id.</param>
		/// <param name="subId">The Subscription's Id.</param>
		/// <param name="subName">The Subscription's Name.</param>
		/// <param name="userId">The User Id.</param>
		/// <param name="isManager">The Manager.</param>
		/// <param name="startingDate">The Starting Date.</param>
		/// <param name="endingDate">The Ending date.</param>
		/// <returns>The constructed TimeEntryOverDateRangeViewModel.</returns>
		public async Task<TimeEntryOverDateRangeViewModel> ConstructTimeEntryOverDataRangeViewModel(int orgId, int subId, string subName, int userId, bool isManager, int? startingDate, int? endingDate)
		{
			DateTime? startingDateTime = null;
			if (startingDate.HasValue)
			{
				startingDateTime = Utility.GetDateTimeFromDays(startingDate.Value);
			}

			DateTime? endingDateTime = null;
			if (endingDate.HasValue)
			{
				endingDateTime = Utility.GetDateTimeFromDays(endingDate.Value);
			}

			var infos = await AppService.GetTimeEntryIndexInfo(orgId, startingDateTime, endingDateTime, userId);
			PayPeriodRanges payRanges = await AppService.GetPayPeriodRanges(orgId);
			int startOfWeek = infos.Item1.StartOfWeek;
			DateTime startDate = startingDateTime != null ? AppService.SetStartingDate(startingDateTime, startOfWeek) : payRanges.Current.StartDate;
			DateTime endDate = endingDateTime ?? payRanges.Current.EndDate;

			// Get all of the projects and initialize their total hours to 0.
			IList<CompleteProject> allProjects = infos.Item4; // Must also grab inactive projects, or the app will crash if a user has an entry on a project he is no longer a part of
			var holidays = infos.Item3.Where(x => startDate <= x.Date.Date && x.Date.Date <= endDate).ToList(); // We only care about holidays within the date range
			var hours = allProjects
				.Where(p => p.ProjectId > 0)
				.ToDictionary(proj =>
					proj.ProjectId,
					proj =>
						new ProjectHours
						{
							Project = new CompleteProjectViewModel(proj),
							Hours = 0.0f
						});

			allProjects.Insert(
				0,
				new CompleteProject
				{
					ProjectId = -1,
					ProjectName = Strings.SelectProject,
					IsActive = true,
					IsCustomerActive = true,
					IsUserActive = true
				});

			IEnumerable<User> users = infos.Item5;

			var result = new TimeEntryOverDateRangeViewModel
			{
				StartDateint = Utility.GetDaysFromDateTime(startDate),
				EndDateint = Utility.GetDaysFromDateTime(endDate),
				EntryRange = new TimeEntryRangeForUserViewModel
				{
					StartDate = startDate,
					EndDate = endDate,
					PayPeriodRanges = payRanges,
					Entries = new List<EditTimeEntryViewModel>(),
					UserId = userId,
					SubscriptionId = subId
				},
				CanManage = isManager,
				StartOfWeek = (StartOfWeekEnum)startOfWeek,
				PayClasses = infos.Item2.Select(payclass => new PayClassInfoViewModel(payclass)),
				GrandTotal = new ProjectHours { Project = new CompleteProjectViewModel { ProjectName = Strings.Total }, Hours = 0.0f },
				Projects = allProjects
					.Where(x => x.IsActive && x.IsCustomerActive && x.IsUserActive)
					.AsParallel()
					.Select(proj => new CompleteProjectViewModel(proj)),
				ProjectsWithInactive = allProjects
					.Where(p => p.ProjectId != 0)
					.AsParallel()
					.Select(proj => new CompleteProjectViewModel(proj)),
				ProjectHours = hours.Values.Where(x => x.Hours > 0),
				Users = users.AsParallel().Select(ConstuctUserViewModel),
				TotalUsers = users.Count(),
				CurrentUser = ConstuctUserViewModel(users.Single(x => x.UserId == userId)),
				LockDate = infos.Item1.LockDate,
				PayrollProcessedDate = infos.Item1.PayrollProcessedDate,
				SubscriptionId = subId,
				SubscriptionName = subName,
				ProductRole = 1
			};

			// Initialize the starting dates and get all of the time entries within that date range.
			IEnumerable<Services.TimeTracker.TimeEntry> timeEntries = infos.Item6; // Service.GetTimeEntriesByUserOverDateRange(new List<int> { userId }, startDate, endDate);
			using (var iter = timeEntries.GetEnumerator())
			{
				iter.MoveNext();

				// Note: Setting this directly insures the weekend highlighting will always be Saturday/Sunday. This makes sense if weekend days are not treated
				// differently with respect to business logic; it's just a stylistic concern. If they need to have separate business logic, this should be switched
				// back to the formula so that the weekend highlighting is based off of start of week.
				// int weekend = 7 + ((int)startOfWeek - 2); // weekend = both days before startOfWeek
				const int weekend = (int)StartOfWeekEnum.Saturday;

				// bool holidayPopulated = false;

				// For each date in the date range
				for (DateTime date = startDate; date <= endDate;)
				{
					// If has time entry data for this date,
					if (iter.Current != null && iter.Current.Date == date.Date)
					{
						// Update its project's hours
						if (hours.ContainsKey(iter.Current.ProjectId))
						{
							ProjectHours temp = hours[iter.Current.ProjectId];
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
							SubscriptionId = subId,
							Date = Utility.GetDaysFromDateTime(iter.Current.Date),
							Duration = string.Format("{0:D2}:{1:D2}", (int)iter.Current.Duration, (int)Math.Round((iter.Current.Duration - (int)iter.Current.Duration) * 60, 0)),
							Description = iter.Current.Description,
							StartingDate = Utility.GetDaysFromDateTime(startDate),
							EndingDate = Utility.GetDaysFromDateTime(endDate),
							IsOffDay = weekend % 7 == (int)iter.Current.Date.DayOfWeek || (weekend + 1) % 7 == (int)iter.Current.Date.DayOfWeek,
							IsHoliday = holidays.Any(x => x.Date.Date == date.Date),
							Projects = result.Projects,
							ProjectsWithInactive = result.ProjectsWithInactive,
							ProjectName = allProjects.FirstOrDefault(x => x.ProjectId == iter.Current.ProjectId)?.ProjectName ?? "",
							IsProjectDeleted = isProjectDeleted,
							ApprovalState = iter.Current.ApprovalState,
							ModSinceApproval = iter.Current.ModSinceApproval,
							PayClasses = result.PayClasses,
							IsLocked = iter.Current.TimeEntryStatusId != (int)TimeEntryStatus.Pending //can't edit
						});

						if (holidays.FirstOrDefault(x => x.Date == iter.Current.Date) != null)
						{
							// holidayPopulated = true;
						}

						iter.MoveNext();
					}
					else
					{
						// Otherwise if date is not locked, create an empty entry.
						result.EntryRange.Entries.Add(new EditTimeEntryViewModel
						{
							Sample = true,
							Date = Utility.GetDaysFromDateTime(date),
							UserId = userId,
							SubscriptionId = subId,
							StartingDate = Utility.GetDaysFromDateTime(startDate),
							EndingDate = Utility.GetDaysFromDateTime(endDate),
							IsOffDay = weekend % 7 == (int)date.DayOfWeek || (weekend + 1) % 7 == (int)date.DayOfWeek,
							IsHoliday = holidays.Any(x => x.Date.Date == date.Date),
							ProjectId = -1,
							Projects = result.Projects,
							ProjectsWithInactive = result.ProjectsWithInactive,
							PayClassId = infos.Item2.FirstOrDefault(p => p.PayClassName.Equals(Strings.Regular, StringComparison.OrdinalIgnoreCase))?.PayClassId ?? 0,
							PayClasses = result.PayClasses,
							IsManager = result.CanManage,
							IsLocked = date <= (result.LockDate ?? result.PayrollProcessedDate ?? DateTime.MinValue) //can't add
						});

						// Go to the next day.
						date = date.AddDays(1);

						// holidayPopulated = false;
					}

					
				}
			}

			return result;
		}

		/// <summary>
		/// Cunstuct User View Model.
		/// </summary>
		/// <param name="user">Sercive Object User.</param>
		/// <returns>User View Model.</returns>
		public UserViewModel ConstuctUserViewModel(User user)
		{
			return new UserViewModel
			{
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserId = user.UserId
			};
		}

		private static DateTime SetEndingDate(int startOfWeek)
		{
			int dayOfWeek = (int)DateTime.Now.DayOfWeek;
			int daysLeftInWeek = startOfWeek - dayOfWeek + (dayOfWeek < startOfWeek ? -1 : 6);
			return DateTime.Now.AddDays(daysLeftInWeek).Date;
		}
	}
}