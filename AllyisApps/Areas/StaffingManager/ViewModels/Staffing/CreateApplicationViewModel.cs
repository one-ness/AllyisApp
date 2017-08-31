//------------------------------------------------------------------------------
// <copyright file="CreateApplicationViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.ViewModels.Staffing
{
	/// <summary>
	/// Represents a position for creation into the database.
	/// </summary>
	public class CreateApplicationViewModel
	{
		/// <summary>
		/// Gets or sets the applicant's ID.
		/// </summary>
		public int ApplicantId { get; set; }

		/// <summary>
		/// Gets or sets the position's ID.
		/// </summary>
		public int PositionId { get; set; }

		/// <summary>
		/// Gets or sets the application status ID.
		/// </summary>
		public int ApplicationStatusId { get; set; }

		/// <summary>
		/// Gets or sets DateCreated.
		/// </summary>
		public DateTime ApplicationCreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets date modified.
		/// </summary>
		public DateTime ApplicationModifiedUtc { get; set; }

		/// <summary>
		/// Gets or sets Notes.
		/// </summary>
		public string Notes { get; set; }

	}
}
