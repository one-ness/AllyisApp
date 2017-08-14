//------------------------------------------------------------------------------
// <copyright file="ApplicationDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace AllyisApps.DBModel.StaffingManager
{
	/// <summary>
	/// Represents the Application table in the database.
	/// </summary>
	public class ApplicationDBEntity
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
		/// Gets or sets the ApplicationDocument list.
		/// </summary>
		public IEnumerable<ApplicationDocumentDBEntity> ApplicationDocuments { get; set; }

		/// <summary>
		/// Gets or sets the ApplicantDBEntity.
		/// </summary>
		public ApplicantDBEntity Applicant { get; set; }
	}
}
