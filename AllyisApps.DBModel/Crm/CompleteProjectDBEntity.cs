//------------------------------------------------------------------------------
// <copyright file="CompleteProjectDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.Crm
{
	/// <summary>
	/// Project table in the database, with supplementary information.
	/// </summary>
	public class CompleteProjectDBEntity
	{
		/// <summary>
		/// Gets or sets The project's id.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the project is active.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets The id of the customer that the project belongs to.
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the Customer is active.
		/// </summary>
		public bool IsCustomerActive { get; set; }

		/// <summary>
		/// Gets or sets The id of the organization that the customer belongs to.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets The date/time of creation.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets The name of the project.
		/// </summary>
		public string ProjectName { get; set; }

		/// <summary>
		/// Gets or sets The name of the customer.
		/// </summary>
		public string CustomerName { get; set; }

		/// <summary>
		/// Gets or sets the customer org id.
		/// </summary>
		public string CustomerOrgId { get; set; }

		/// <summary>
		/// Gets or sets The name of the organization.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets The id of the organization role.
		/// </summary>
		public int OrgRoleId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a user is active on the project, If that project was fetched via userId.
		/// </summary>
		public bool IsUserActive { get; set; }

		/// <summary>
		/// Gets or sets Project Pricing Type.
		/// </summary>
		public string PriceType { get; set; }

		/// <summary>
		/// Gets or sets Project start date.
		/// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Gets or sets Project end date.
		/// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
		/// Gets or sets the Project's ID as used by the organization
		/// </summary>
		public string ProjectOrgId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user is a user of this project, in some contexts.
		/// </summary>
		public bool? IsProjectUser { get; set; }
	}
}
