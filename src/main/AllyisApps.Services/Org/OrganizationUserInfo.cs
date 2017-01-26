//------------------------------------------------------------------------------
// <copyright file="OrganizationUserInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services
{
    /// <summary>
    /// Represents an organization user.
    /// </summary>
    public class OrganizationUserInfo
    {
        /// <summary>
        /// Gets or sets the User Id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the Organization Id.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the Organization role Id.
        /// </summary>
        public int OrgRoleId { get; set; }

        /// <summary>
        /// Gets or sets the Date this user was added to the organization.
        /// </summary>
        public DateTime CreatedUTC { get; set; }

        /// <summary>
        /// Gets or sets the Employee Id.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; }
	}
}