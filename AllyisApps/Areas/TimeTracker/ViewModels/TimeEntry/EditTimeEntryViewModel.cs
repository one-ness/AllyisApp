//------------------------------------------------------------------------------
// <copyright file="EditTimeEntryViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Mvc;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Representation of a Time Entry to edit.
	/// </summary>
	public class EditTimeEntryViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EditTimeEntryViewModel" /> class.
		/// </summary>
		public EditTimeEntryViewModel()
		{
			Sample = false;
		}

		/// <summary>
		/// Gets or sets the payClasses for an org.
		/// </summary>
		public IEnumerable<SelectListItem> PayClasses { get; set; }

		/// <summary>
		/// Gets or sets the starting date of the date range.
		/// </summary>
		public int StartingDate { get; set; } // Note: this must be an int and not a DateTime for correct, culture-independant serialization/deserialization

		/// <summary>
		/// Gets or sets the ending date of the date range.
		/// </summary>
		public int EndingDate { get; set; } // Note: this must be an int and not a DateTime for correct, culture-independant serialization/deserialization

		/// <summary>
		/// Gets or sets the edited reference user's id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the subscription's Id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the id of the time entry.
		/// </summary>
		public int? TimeEntryId { get; set; }

		/// <summary>
		/// Gets or sets the id of the project associated with this entry.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets the name of the pay class for this entry.
		/// </summary>
		public int PayClassId { get; set; }

		/// <summary>
		/// Gets or sets the project's name.
		/// </summary>
		public string ProjectName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entry is an off-day (weekend, holiday, etc.) or not.
		/// </summary>
		public bool IsOffDay { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entry is a holiday.
		/// </summary>
		public bool IsHoliday { get; set; }

		/// <summary>
		/// Gets or sets the date associated with this entry.
		/// </summary>
		public int Date { get; set; }  // Note: this must be an int and not a DateTime for correct, culture-independant serialization/deserialization

		/// <summary>
		/// Gets or sets the duration defined in this entry.
		/// </summary>
		public string Duration { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this entry has changed data since being approved/disapproved.
		/// </summary>
		public bool ModSinceApproval { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not this is a Sample entry (blank).
		/// </summary>
		public bool Sample { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not this entry should be hidden from view, but still entered into the DOM.
		/// </summary>
		public bool Hidden { get; set; }

		/// <summary>
		/// Gets or sets the description defined in this entry.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the list of projects available to select from, including inactive projects.
		/// </summary>
		public IEnumerable<SelectListItem> ProjectsWithInactive { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the current user a manager and can approve time entries  [This needs logics].
		/// </summary>
		public bool IsManager { get; set; }

		/// <summary>
		/// Has the user clicked the deleted Button and thus we should delete the record.
		/// </summary>
		public bool IsDeleted { get; set; }

		/// <summary>
		/// Bool if the element is updated
		/// </summary>
		public bool IsEdited { get; set; }

		/// <summary>
		/// On Create of new record.
		/// </summary>
		public bool IsCreated { get; set; }

		/// <summary>
		/// Bool for whether or not this time entry can be edited.
		/// </summary>
		public bool IsEditable { get; set; }

		/// <summary>
		/// Bool for whether or not this time entry can be created (for use in new forms).
		/// </summary>
		public bool IsCreatable { get; set; }

		/// <summary>
		/// Bool for whether or not this time entry can be deleted.
		/// </summary>
		public bool IsDeletable { get; set; }
	}
}