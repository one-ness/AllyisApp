//------------------------------------------------------------------------------
// <copyright file="Project.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using AllyisApps.Services.Crm;

namespace AllyisApps.Services.Project
{
	/// <summary>
	/// An object for keeping track of all the info related to a given project.
	/// </summary>
	public class Project
	{
		/// <summary>
		/// Gets or sets the projects Id number.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets the id of the organization associated with the project.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the project name.
		/// </summary>
		public string ProjectName { get; set; }

		/// <summary>
		/// Gets or sets the project type -- true == hourly, false == fixed.
		/// </summary>
		public bool IsHourly { get; set; }

		/// <summary>
		/// Gets or sets the project's org id.
		/// </summary>
		public string ProjectCode { get; set; }

		/// <summary>
		/// Gets or sets the project's starting date.
		/// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Gets or sets the project's ending date.
		/// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
		/// Returns the bool value indicating if this project is currently active.
		/// </summary>
		public bool IsActive =>
			(StartDate == null || StartDate <= DateTime.Now) && (EndDate == null || EndDate >= DateTime.Now);

		public Customer OwningCustomer { get; set; }
	}
}