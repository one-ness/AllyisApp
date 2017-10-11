//------------------------------------------------------------------------------
// <copyright file="ReviewTimeEntriesViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

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
		/// Gets or sets the list of TimeEntries, containing user info too.
		/// </summary>
		public List<TimeEntryViewModel> TimeEntries { get; set; }

		/// <summary>
		/// Int version of start date
		/// </summary>
		public int StartDateInt { get; set; }

		/// <summary>
		/// Int version of end date
		/// </summary>
		public int EndDateInt { get; set; }
	}
}