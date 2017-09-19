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
	public class OrganizationUserDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the user's FirstName.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the user's LastName.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the user's EmailName.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets OrganizationId.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets OrganizationRoleId.
		/// </summary>
		public int OrganizationRoleId { get; set; }

		/// <summary>
		/// Gets or sets the date this user was added to the organization.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the employee id for this user.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the maximum amount that a user can approve of in a report.
		/// </summary>
		public decimal MaxAmount { get; set; }
	}
}
