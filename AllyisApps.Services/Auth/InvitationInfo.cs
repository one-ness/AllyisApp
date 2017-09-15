//------------------------------------------------------------------------------
// <copyright file="InvitationInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services
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
		/// Gets or sets the Organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Organization Role Id.
		/// </summary>
		public OrganizationRole OrganizationRole { get; set; }

		/// <summary>
		/// Gets or sets the Employee Id.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// Date that user had accepted or rejected the invitation.
		/// </summary>
		public DateTime DecisionDateUtc { get; set; }

		public InvitationStatusEnum status { get; set; }
		public object OrganizationName { get; internal set; }
	}
}