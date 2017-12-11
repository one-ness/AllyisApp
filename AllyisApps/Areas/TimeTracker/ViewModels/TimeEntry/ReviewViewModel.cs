//------------------------------------------------------------------------------
// <copyright file="ReviewTimeEntriesViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// 
	/// </summary>
	public class ReviewViewModel
	{
		/// <summary>
		/// Gets or sets the UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the SubscriptionId.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the id of the organization this review belongs to
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the SubscriptionName.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets the list of pay classes that the organization has
		/// </summary>
		public List<PayClassInfoViewModel> PayClasses { get; set; }

		/// <summary>
		/// Gets or sets the start date of the time entries to pull
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the end date of the time entries to pull
		/// </summary>
		public DateTime EndDate { get; set; }

		/// <summary>
		/// Gets or sets a lookup table of time entries where the key is userId and the value is all the time entries that the user id has
		/// </summary>
		public ILookup<int, TimeEntryViewModel> TimeEntriesByUser { get; set; }

		/// <summary>
		/// Gets or sets a dictionary where the key is userId and the value is
		/// a dictionary where the key is PayClassId and the value is the total
		/// number of hours of all the time entries for that pay class for that user
		/// </summary>
		public Dictionary<int, Dictionary<int, float>> TimeEntryTotalsByUserByPayClass { get; set; }

		/// <summary>
		/// Gets or sets the JSON stringified array of all the time entry ids
		/// </summary>
		public string TimeEntryIdsJSON { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string TimeEntryUserIdsJson { get; set; }

		/// <summary>
		/// Gets or sets the dictionary of all time entry statuses, to be used for the view select
		/// </summary>
		public Dictionary<int, string> TimeEntryStatusOptions { get; set; }

		/// <summary>
		/// Gets or sets the nullable lock date for the time entries.  Null means that no time entries are payroll processed.
		/// </summary>
		public DateTime? LockDate { get; set; }

		/// <summary>
		/// Gets or sets the nullable payroll process date for the time entries.  Null means that no time entries are payroll processed.
		/// </summary>
		public DateTime? PayrollProcessedDate { get; set; }

		/// <summary>
		/// Gets or sets the pay period ranges for the time tracker subscription.  Includes current, previous, and next pay periods.
		/// </summary>
		public PayPeriodRanges PayPeriodRanges { get; set; }
	}

	/// <summary>
	/// T
	/// </summary>
	public class TimeEntryUserReviewViewModel
	{
		/// <summary>
		/// Gets or sets UsersTimeEntries
		/// </summary>
		public IEnumerable<TimeEntryViewModel> UserTimeEntries { get; set; }
		

		/// <summary>
		/// Gets for sets userId
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets Payclases for the organization
		/// </summary>
		public IEnumerable<PayClassInfoViewModel> PayClasses { get; set; }

		/// <summary>
		/// Constuct with empty lists.
		/// </summary>
		public TimeEntryUserReviewViewModel()
		{
			PayClasses = new List<PayClassInfoViewModel>();
			UserTimeEntries = new List<TimeEntryViewModel>();
		}

	}
}