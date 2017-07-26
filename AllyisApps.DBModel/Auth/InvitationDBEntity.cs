//------------------------------------------------------------------------------
// <copyright file="InvitationDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// The model for the invitations table.
	/// </summary>
	public class InvitationDBEntity
	{
		/// <summary>
		/// Gets or sets the id of the invitation.
		/// </summary>
		public int InvitationId { get; set; }

		/// <summary>
		/// Gets or sets the email address that the invitation is being sent to.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the first name of the recipiant.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name of the recipiant.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the birthday of the recipiant.
		/// </summary>
		public DateTime DateOfBirth { get; set; }

		/// <summary>
		/// Gets or sets the id of the inviting organization.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the name of the inviting organization.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the access code associated with the invitation.
		/// </summary>
		public string AccessCode { get; set; }

		/// <summary>
		/// Gets or sets the id of the org role the user will be assigned.
		/// </summary>
		public int OrgRoleId { get; set; }

		/// <summary>
		/// Gets or sets the employee id of the user.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the id of the employee type the user will be assigned
		/// </summary>
		public int EmployeeTypeId { get; set; }
	}
}
