//------------------------------------------------------------------------------
// <copyright file="DeleteTimeEntryViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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
		public DeleteTimeEntryViewModel() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeleteTimeEntryViewModel" /> class.
		/// </summary>
		/// <param name="timeEntryId">The Time entry to be deleted.</param>
		public DeleteTimeEntryViewModel(int timeEntryId)
		{
			TimeEntryId = timeEntryId;
		}

		/// <summary>
		/// Gets or sets the id of the time entry.
		/// </summary>
		public int TimeEntryId { get; set; }

		/// <summary>
		/// Gets or sets the duration of the time entry.
		/// </summary>
		public string Duration { get; set; }

		/// <summary>
		/// Gets or sets the subscription's Id.
		/// </summary>
		public int SubscriptionId { get; set; }
	}
}