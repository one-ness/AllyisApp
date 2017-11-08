﻿//------------------------------------------------------------------------------
// <copyright file="TimeEntryOverDateRangeViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.Project;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Representation of the Time entries defined over the specified date range.
	/// </summary>
	public class TimeEntryOverDateRangeViewModel
	{
		private const int PageUserLimit = 35;

		/// <summary>
		/// Gets or sets the lock date for the entry. Note: must be int and not DateTime for Json serialization to work correctly in different cultures.
		/// </summary>
		public int LockDateOld { get; set; }

		/// <summary>
		/// Gets the list of entries, start and end dates, and user/org id's.
		/// </summary>
		public TimeEntryRangeForUserViewModel EntryRange { get; internal set; }

		/// <summary>
		/// Gets a value indicating whether or not user can manage other users.
		/// </summary>
		public bool CanManage { get; internal set; }

		/// <summary>
		/// Gets or sets the subscription Id for the Organization.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the subscription Name for the Organizations subscription.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets the user's Id.
		/// </summary>
		public UserViewModel CurrentUser { get; internal set; }

		/// <summary>
		/// Gets the list of active projects available.
		/// </summary>
		public IEnumerable<CompleteProjectViewModel> Projects { get; internal set; }

		/// <summary>
		/// Gets the list of projects available, including inactive projects.
		/// </summary>
		public IEnumerable<CompleteProjectViewModel> ProjectsWithInactive { get; internal set; }

		/// <summary>
		/// Gets the list of projects available, associated with their total hours.
		/// </summary>
		public IEnumerable<ProjectHours> ProjectHours { get; internal set; }

		/// <summary>
		/// Gets the total hours spent in the current view.
		/// </summary>
		public ProjectHours GrandTotal { get; internal set; }

		/// <summary>
		/// Gets the list of users for the defined organization.
		/// </summary>
		public IEnumerable<UserViewModel> Users { get; internal set; }

		/// <summary>
		/// Gets the total number of users.
		/// </summary>
		public int TotalUsers { get; internal set; }

		/// <summary>
		/// Gets a value indicating the number of users per page to be displayed.
		/// </summary>
		public int PageUserSize => PageUserLimit;

		/// <summary>
		/// Gets a value indicating the number of pages of users to be displayed.
		/// </summary>
		public int UserPageCount => (int)Math.Ceiling(TotalUsers / (double)PageUserLimit);

		/// <summary>
		/// Gets start of week for an Organization.
		/// </summary>
		public StartOfWeekEnum StartOfWeek { get; internal set; }

		/// <summary>
		/// Gets pay classes for an org.
		/// </summary>
		public IEnumerable<PayClassInfoViewModel> PayClasses { get; internal set; }

		/// <summary>
		/// Gets or sets the type of product.
		/// </summary>
		public int ProductRole { get; set; }

		/// <summary>
		/// Gets or sets the lock date for the time entries -- null lock date means no entries are locked
		/// </summary>
		public DateTime? LockDate { get; set; }

		/// <summary>
		/// Gets or sets the payroll process date for time entries -- null payroll process date means no entries are payroll processed
		/// </summary>
		public DateTime? PayrollProcessedDate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int StartDateint { get; internal set; }
		/// <summary>
		/// 
		/// </summary>
		public int EndDateint { get; internal set; }


		/// <summary>
		/// Users for Time Entries.
		/// </summary>
		public class UserViewModel
		{
			/// <summary>
			/// Gets or sets Id of the user.
			/// </summary>
			public int UserId { get; set; }

			/// <summary>
			/// Gets or sets FirstName of the User.
			/// </summary>
			public string FirstName { get; set; }

			/// <summary>
			/// Gets or sets LastName of the user.
			/// </summary>
			public string LastName { get; set; }

			/// <summary>
			/// Gets or sets contact emial of user.
			/// </summary>
			public string Email { get; set; }
		}
	}

	/// <summary>
	/// Unites the project with the total hours duration.
	/// </summary>
	public class ProjectHours
	{
		/// <summary>
		/// Gets or sets the project in use.
		/// </summary>
		public CompleteProjectViewModel Project { get; set; }

		/// <summary>
		/// Gets or sets the total amount of hours attributed to this project.
		/// </summary>
		public float Hours { get; set; }

		/// <summary>
		/// Converts the number of hours to HH:MM.
		/// </summary>
		/// <returns>A string representation in the form of HH:MM.</returns>
		public string GetHoursInHoursMinutes()
		{
			try
			{
				string time = string.Format("{0}:{1}", (int)Hours, ((int)Math.Round((Hours - (int)Hours) * 60.0f)).ToString("00"));
				return time;
			}
			catch (Exception e)
			{
				return e.ToString();
			}
		}
	}
}