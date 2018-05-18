//------------------------------------------------------------------------------
// <copyright file="OrganizationUser.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// Represents an organization user.
	/// </summary>
	public class OrganizationUser : UserOld
	{
		/// <summary>
		/// Gets or sets the Organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Organization role Id.
		/// </summary>
		public int OrganizationRoleId { get; set; }

		/// <summary>
		/// Gets or sets the Date this user was added to the organization.
		/// </summary>
		public DateTime OrganizationUserCreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the Employee Id.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// approval limit for this user in expense tracker
		/// </summary>
		public decimal MaxApprovalAmount { get; set; }

		public int EmployeeTypeId { get; set; }
	}
}
