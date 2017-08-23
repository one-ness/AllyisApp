//------------------------------------------------------------------------------
// <copyright file="CreateApplicationDocumentViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.ViewModels.Staffing
{
	/// <summary>
	/// Represents a position for creation into the database.
	/// </summary>
	public class CreateApplicationDocumentViewModel
	{
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
