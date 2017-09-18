//------------------------------------------------------------------------------
// <copyright file="ApplicationDocumentDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.StaffingManager
{
	/// <summary>
	/// Represents the ApplicationDocument table in the database.
	/// </summary>
	public class ApplicationDocumentDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets the ApplicationDocumentId.
		/// </summary>
		public int ApplicationDocumentId { get; set; }

		/// <summary>
		/// Gets or sets the application's ID.
		/// </summary>
		public int ApplicationId { get; set; }

		/// <summary>
		/// Gets or sets the DocumentLink.
		/// </summary>
		public string DocumentLink { get; set; }

		/// <summary>
		/// Gets or sets the DocumentName.
		/// </summary>
		public string DocumentName { get; set; }
	}
}