//------------------------------------------------------------------------------
// <copyright file="CreateTimeEntryViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Text.RegularExpressions;

namespace AllyisApps.Areas.TimeTracker.Models
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
		/// Gets or sets the duration defined in this entry.
		/// </summary>
		public string Duration { get; set; }

		/// <summary>
		/// Gets or sets the date associated with this entry.
		/// </summary>
		public DateTime Date { get; set; }

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

		/// <summary>
		/// Gets or sets the Organization's Id.
		/// </summary>
		public int OrganizationId { get; set; }
	}
}