//------------------------------------------------------------------------------
// <copyright file="CompleteProject.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Crm
{
	/// <summary>
	/// Represents a project.
	/// </summary>
	public class CompleteProject : Project.Project
	{
		

		/// <summary>
		/// Gets or sets a value indicating whether or not the Customer is active.
		/// </summary>
		public bool IsCustomerActive { get; set; }

		/// <summary>
		/// Gets or sets the Date/time of creation.
		/// </summary>
		public DateTime CreatedUtc { get; set; }


		/// <summary>
		/// Gets or sets the Name of the organization.
		/// </summary>
		public string OrganizationName { get; set; }

		

		/// <summary>
		/// Gets or sets a value indicating whether a user is active on the project, if that project was fetched via userId.
		/// </summary>
		public bool IsUserActive { get; set; }

		/// <summary>
		/// Gets or sets the Project Pricing Type.
		/// </summary>
		public string PriceType { get; set; }

		/// <summary>
		/// Gets or sets the Project start date.
		/// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Gets or sets the Project end date.
		/// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the current user can edit the project.
		/// </summary>
		public bool CanEditProject { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user is a user of this project, in some contexts.
		/// </summary>
		public bool? IsProjectUser { get; set; }
	}
}