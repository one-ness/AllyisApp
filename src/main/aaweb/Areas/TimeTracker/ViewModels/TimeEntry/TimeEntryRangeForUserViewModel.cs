//------------------------------------------------------------------------------
// <copyright file="TimeEntryRangeForUserViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.Areas.TimeTracker.Models
{
	/// <summary>
	/// Representation of the Time entries defined over the specified date range for a given user.
	/// </summary>
	public class TimeEntryRangeForUserViewModel
	{
		/// <summary>
		/// Gets the list of entries.
		/// </summary>
		public IList<EditTimeEntryViewModel> Entries { get; internal set; }

		/// <summary>
		/// Gets the starting date of the date range.
		/// </summary>
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime StartDate { get; internal set; }

		/// <summary>
		/// Gets the ending date of the date range.
		/// </summary>
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime EndDate { get; internal set; }

		/// <summary>
		/// Gets the user's Id.
		/// </summary>
		public int UserId { get; internal set; }
	}
}