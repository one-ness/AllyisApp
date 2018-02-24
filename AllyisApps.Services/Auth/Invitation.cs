//------------------------------------------------------------------------------
// <copyright file="Invitation.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using AllyisApps.Lib;

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// Represents an invitation.
	/// </summary>
	public class Invitation
	{
		/// <summary>
		/// Gets or sets the Invitation Id.
		/// </summary>
		public int InvitationId { get; set; }

		/// <summary>
		/// Gets or sets the Email address of invitee.
		/// </summary>
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the Compressed version of email address, for display.
		/// </summary>
		public string CompressedEmail
		{
			get
			{
				return Utility.GetCompressedEmail(this.Email);
			}
		}

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
		/// gets or sets the organization name
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the EmployeeTypeId.
		/// </summary>
		public int EmployeeTypeId { get; set; }

		/// <summary>
		/// Gets or sets the Employee Id.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the Role json string.
		/// </summary>
		public string ProductRolesJson { get; set; }

		/// <summary>
		/// Date that user had accepted or rejected the invitation.
		/// </summary>
		public DateTime? DecisionDateUtc { get; set; }

		/// <summary>
		/// invitation created
		/// </summary>
		public DateTime InvitationCreatedUtc { get; set; }

        /// <summary>
        /// invitation status
        /// </summary>
        public InvitationStatusEnum InvitationStatus { get; set; }

		/// <summary>
		/// organization role
		/// </summary>
		public OrganizationRoleEnum OrganizationRole { get;  set; }
	}

	public class InvitationPermissionsJson
	{
		/// <summary>
		/// Gets or sets the subscription Id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the product role Id.
		/// </summary>
		public int ProductRoleId { get; set; }
	}
}