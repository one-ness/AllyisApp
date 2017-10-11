//------------------------------------------------------------------------------
// <copyright file="ReviewTimeEntriesViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

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
	}
}