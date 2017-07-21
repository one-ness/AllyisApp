//------------------------------------------------------------------------------
// <copyright file="ProjectDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.Crm
{
	/// <summary>
	/// The project table.
	/// </summary>
	public class ProjectDBEntity
	{
		/// <summary>
		/// Gets or sets the project id.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets the id of the organization associated with the project.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the id of the customer associated with the project.
		/// </summary>
		public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer name associated with the project.
        /// </summary>
        public string CustomerName { get; set; }

		/// <summary>
		/// Gets or sets the project name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the project type.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the project's ID as used by the organization
		/// </summary>
		public string ProjectOrgId { get; set; }

		/// <summary>
		/// Gets or sets the project's starting date.
		/// </summary>
		public DateTime? StartingDate { get; set; }

		/// <summary>
		/// Gets or sets the project's ending date.
		/// </summary>
		public DateTime? EndingDate { get; set; }
	}
}
