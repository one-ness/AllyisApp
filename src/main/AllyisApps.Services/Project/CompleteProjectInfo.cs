//------------------------------------------------------------------------------
// <copyright file="CompleteProjectInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services
{
	/// <summary>
	/// Represents a project.
	/// </summary>
	public class CompleteProjectInfo
	{
		/// <summary>
		/// Gets or sets the Project Id.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the project is active.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets the Customer Id that the project belongs to.
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the Customer is active.
		/// </summary>
		public bool IsCustomerActive { get; set; }

		/// <summary>
		/// Gets or sets the Oorganization Id that the customer belongs to.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Date/time of creation.
		/// </summary>
		public DateTime CreatedUTC { get; set; }

		/// <summary>
		/// Gets or sets the Name of the project.
		/// </summary>
		public string ProjectName { get; set; }

		/// <summary>
		/// Gets or sets the Name of the customer.
		/// </summary>
		public string CustomerName { get; set; }

		/// <summary>
		/// Gets or sets the Name of the organization.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the Id of the organization role.
		/// </summary>
		public int OrgRoleId { get; set; }

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
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the Project end date.
		/// </summary>
		public DateTime EndDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the current user can edit the project.
		/// </summary>
		public bool CanEditProject { get; set; }
	}
}