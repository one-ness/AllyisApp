//------------------------------------------------------------------------------
// <copyright file="DeleteTimeEntryViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Representation of the entry to delete.
	/// </summary>
	public class DeleteTimeEntryViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DeleteTimeEntryViewModel" /> class.
		/// </summary>
		public DeleteTimeEntryViewModel()
		{
			this.ApprovalState = -1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeleteTimeEntryViewModel" /> class.
		/// </summary>
		/// <param name="timeEntryId">The Time entry to be deleted.</param>
		public DeleteTimeEntryViewModel(int timeEntryId)
		{
			this.TimeEntryId = timeEntryId;
			this.ApprovalState = -1;
		}

		/// <summary>
		/// Gets or sets the starting date of the date range.
		/// </summary>
		public DateTime StartingDate { get; set; }

		/// <summary>
		/// Gets or sets the ending date of the date range.
		/// </summary>
		public DateTime EndingDate { get; set; }

		/// <summary>
		/// Gets or sets the id of the time entry.
		/// </summary>
		public int TimeEntryId { get; set; }

		/// <summary>
		/// Gets or sets the approval state of the time entry.
		/// </summary>
		public int ApprovalState { get; set; }

		/// <summary>
		/// Gets or sets the subscription's Id
		/// </summary>
		public int SubscriptionId { get; set; }
	}
}
