//------------------------------------------------------------------------------
// <copyright file="TimeEntryRangeForUserViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
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
		/// Gets the list of entries.
		/// </summary>
		public IEnumerable<System.Linq.IGrouping<int, EditTimeEntryViewModel>> weekQuery { get; internal set; }

		/// <summary>
		/// Gets the list of entries.
		/// </summary>
		public IEnumerable<System.Linq.IGrouping<int, EditTimeEntryViewModel>> dateQuery { get; internal set; }
		
		/// <summary>
		/// Gets the list of entries.
		/// </summary>
		public List<DateGroup> weekGrouped { get; internal set; }
		
		/// <summary>
		/// Gets the list of entries.
		/// </summary>
		public List<DateGroup> dateGroup { get; internal set; }

		/// <summary>
		/// Gets the starting date of the date range. Note: must be int and not DateTime for Json serialization to work correctly in different cultures.
		/// </summary>
		public DateTime StartDate { get; internal set; }

        /// <summary>
        /// Gets the ending date of the date range. Note: must be int and not DateTime for Json serialization to work correctly in different cultures.
        /// </summary>
        public DateTime EndDate { get; internal set; }

        /// <summary>
        /// Gets or sets the date range of the payperiod
        /// </summary>
        public PayPeriodRanges PayPeriodRanges { get; set; }

        /// <summary>
        /// Gets the user's Id.
        /// </summary>
        public int UserId { get; internal set; }

        /// <summary>
        /// Gets or sets the subscription's Id.
        /// </summary>
        public int SubscriptionId { get; set; }

		/// <summary>
		/// start of week enum
		/// </summary>
		public DayOfWeek startOfWeek { get; set; }

		/// <summary>
		/// localizes the week based on culture and calender
		/// </summary>
		public Func<int, int> weekProjector { get; set; }
	}


	/// <summary>
	/// used by the view to build each 
	/// </summary>
	public class DateGroup
	{
		/// <summary>
		/// The IEnumerable list of entries for the given date
		/// </summary>
		public List<EditTimeEntryViewModel> theEntries { get; set; }

		/// <summary>
		/// the sample (blank) time entry that should be present in every date group
		/// </summary>
		public EditTimeEntryViewModel theSample { get; set; }

		/// <summary>
		/// total count of entries
		/// </summary>
		public int totalCount { get; set; }

		/// <summary>
		/// count of approved and nonapproved
		/// </summary>
		public int allVotedCount { get; set; }

		/// <summary>
		/// all approved entries count
		/// </summary>
		public int allApprovedCount { get; set; }

		/// <summary>
		/// all rejected entries count
		/// </summary>
		public int allRejectedCount { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool noneExist { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool noneWereVoted { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool allWereApproved { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool allWereRejected { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool anyWereChanged { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string containerClass { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string headerClass { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime currentDate { get; set; }

		/// <summary>
		/// day string numbers 
		/// </summary>
		public string dateStringNums { get; set; }

		/// <summary>
		/// day string long
		/// </summary>
		public string dateStringDayLong { get; set; }

		/// <summary>
		/// day string short
		/// </summary>
		public string dateStringDayShort { get; set; }

		/// <summary>
		/// string for type of off day
		/// </summary>
		public string offDayClass { get; set; }

		/// <summary>
		/// string for type of holiday
		/// </summary>
		public string holidayClass { get; set; }
	}
}