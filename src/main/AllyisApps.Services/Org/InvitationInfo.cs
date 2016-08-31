//------------------------------------------------------------------------------
// <copyright file="InvitationInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Org
{
	/// <summary>
	/// Represents an invitation.
	/// </summary>
	public class InvitationInfo
	{
		/// <summary>
		/// Gets or sets the Invitation Id.
		/// </summary>
		public int InvitationId { get; set; }

		/// <summary>
		/// Gets or sets the Email address of invitee.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the Compressed version of email address, for display.
		/// </summary>
		public string CompressedEmail { get; set; }

		/// <summary>
		/// Gets or sets the First name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the Last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the Date of birth.
		/// </summary>
		public DateTime DateOfBirth { get; set; }

		/// <summary>
		/// Gets or sets the Organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Access code for accepting the invitation.
		/// </summary>
		public string AccessCode { get; set; }

		/// <summary>
		/// Gets or sets the Organization Role Id.
		/// </summary>
		public int OrgRole { get; set; }

		/// <summary>
		/// Gets or sets the Project Id.
		/// </summary>
		public int? ProjectId { get; set; }
	}
}