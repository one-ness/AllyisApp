//------------------------------------------------------------------------------
// <copyright file="CreateTimeEntryViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Represents a time entry for creation into the database.
	/// </summary>
	public class CreateTimeEntryViewModel
	{
		/// <summary>
		/// Gets or sets the edited reference user's id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the subscription's id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the duration defined in this entry.
		/// </summary>
		public string Duration { get; set; }

		/// <summary>
		/// Gets or sets the date associated with this entry.
		/// </summary>
		public int Date { get; set; } // Note: this must be an int and not a DateTime for correct, culture-independant serialization/deserialization

		/// <summary>
		/// Gets or sets the id of the project associated with this entry.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets the name of the pay class for this entry.
		/// </summary>
		public int PayClassId { get; set; }

		/// <summary>
		/// Gets or sets the description defined in this entry.
		/// </summary>
		public string Description { get; set; }
	}
}
