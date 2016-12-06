//------------------------------------------------------------------------------
// <copyright file="ProjectDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.Crm
{
	/// <summary>
	/// The project table.
	/// </summary>
	public class ProjectDBEntity : BasePoco
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
		/// Gets or sets the project name.
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Gets or sets the project's ID as used by the organization
        /// </summary>
        public string ProjectOrgId { get; set; }
	}
}