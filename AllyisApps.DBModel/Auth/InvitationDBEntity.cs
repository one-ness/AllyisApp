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
    public class InvitationDBEntity : BaseDBEntity
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
        /// Date invitation is accepted or Rejected
        /// </summary>
        public DateTime? DecisionDateUtc { get; set; }

        /// <summary>
        /// invitation created date
        /// </summary>
        public DateTime InvitationCreatedUtc { get; set; }

        /// <summary>
        /// Invitation stauts of accepted pending or rejected
        /// </summary>
        public int InvitationStatus { get; set; }

        /// <summary>
        /// Gets or sets the id of the inviting organization.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the Organization Name.
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Gets or sets the employee id of the user.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Get or set the Role Json string.
        /// </summary>
        public string ProductRolesJson { get; set; }
    }
}