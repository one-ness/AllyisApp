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
		public DateTime? StartingDate { get; set; }

		/// <summary>
		/// Gets or sets the project's ending date.
		/// </summary>
		public DateTime? EndingDate { get; set; }

		/// <summary>
		/// Gets or sets the bool value indicating if this Customer is currently active.
		/// </summary>
		public bool IsActive { get; set; }

		public Customer owningCustomer { get; set; }
	}
}