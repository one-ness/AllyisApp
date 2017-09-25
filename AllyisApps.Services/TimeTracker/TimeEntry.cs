//------------------------------------------------------------------------------
// <copyright file="TimeEntry.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.TimeTracker
{
	/// <summary>
	/// An object for keeping track of all the info related to a given Time entry.
	/// </summary>
	public class TimeEntry
	{
		/// <summary>
		/// Gets or sets the TimeEntryId.
		/// </summary>
		public int TimeEntryId { get; set; }

		/// <summary>
		/// Gets or sets the UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the TimeEntryId.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the TimeEntryId.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the EmployeeId.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the Email address.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the ProjectId.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets the PayClassId.
		/// </summary>
		public int PayClassId { get; set; }

		/// <summary>
		/// Gets or sets the pay class name.
		/// </summary>
		public string PayClassName { get; set; }

		/// <summary>
		/// Gets or sets the Date.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the Duration.
		/// </summary>
		public float Duration { get; set; }

		/// <summary>
		/// Gets or sets the Description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the approval state.
		/// </summary>
		public int ApprovalState { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the saved approval lock.
		/// </summary>
		public bool IsLockSaved { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the data has changed since being approved or dissaproved.
		/// </summary>
		public bool ModSinceApproval { get; set; }
	}
}