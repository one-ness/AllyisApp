//------------------------------------------------------------------------------
// <copyright file="TimeEntryRangeForUserViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

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
		/// Gets the starting date of the date range. Note: must be int and not DateTime for Json serialization to work correctly in different cultures.
		/// </summary>
		public int StartDate { get; internal set; }

		/// <summary>
		/// Gets the ending date of the date range. Note: must be int and not DateTime for Json serialization to work correctly in different cultures.
		/// </summary>
		public int EndDate { get; internal set; }

		/// <summary>
		/// Gets the user's Id.
		/// </summary>
		public int UserId { get; internal set; }
	}
}