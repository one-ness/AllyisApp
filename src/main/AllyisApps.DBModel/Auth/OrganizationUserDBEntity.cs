//------------------------------------------------------------------------------
// <copyright file="OrganizationUserDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// Represents the OrganizationUser Table.
	/// </summary>
	public class OrganizationUserDBEntity
	{
		/// <summary>
		/// Gets or sets UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets OrganizationId.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets OrgRoleId.
		/// </summary>
		public int OrgRoleId { get; set; }

		/// <summary>
		/// Gets the OrganizationName associated with this orguser.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the date this user was added to the organization.
		/// </summary>
		public DateTime CreatedUTC { get; set; }

		/// <summary>
		/// Gets or sets the employee id for this user.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the email for this user.
		/// </summary>
		public string Email { get; set; }
	}
}
