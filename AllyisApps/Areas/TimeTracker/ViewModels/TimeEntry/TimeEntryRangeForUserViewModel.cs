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
    }
}