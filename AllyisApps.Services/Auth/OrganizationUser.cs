//------------------------------------------------------------------------------
// <copyright file="OrganizationUser.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace AllyisApps.Services
{
	/// <summary>
	/// Represents an organization user.
	/// </summary>
	public class OrganizationUser : User
	{
		/// <summary>
		/// Gets or sets the Organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Organization role Id.
		/// </summary>
		public int OrganizationRoleId { get; set; }

		public string OrganizationRoleName { get; set; }

		/// <summary>
		/// Gets or sets the Date this user was added to the organization.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the Employee Id.
		/// </summary>
		public string EmployeeId { get; set; }

		
	}
}