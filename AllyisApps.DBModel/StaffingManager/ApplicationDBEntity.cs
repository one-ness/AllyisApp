//------------------------------------------------------------------------------
// <copyright file="ApplicationDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.StaffingManager
{
	/// <summary>
	/// Represents the Application table in the database.
	/// </summary>
	public class ApplicationDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets the application's ID.
		/// </summary>
		public int ApplicationId { get; set; }

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

		/// <summary>
		/// the applicant info of the application
		/// </summary>
		public ApplicantDBEntity Applicant { get; set; }

		/// <summary>
		/// Documents for the give application
		/// </summary>
		public ApplicationDocumentDBEntity Documents { get; set; }
	}
}