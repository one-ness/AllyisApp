//------------------------------------------------------------------------------
// <copyright file="UserOrganization.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace AllyisApps.Services
{
	/// <summary>
	/// User organization info.
	/// </summary>
	public class UserOrganization
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UserOrganization"/> class.
		/// </summary>
		public UserOrganization()
		{
			this.OrganizationRole = OrganizationRole.Member;
			this.UserSubscriptions = new Dictionary<int, UserSubscription>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UserOrganization"/> class.
		/// </summary>
		/// <param name="organizationId">The Organization Id.</param>
		/// <param name="organizationName">The organization Name.</param>
		/// <param name="role">The role.</param>
		public UserOrganization(int organizationId, string organizationName, OrganizationRole role) : this()
		{
			if (organizationId <= 0) throw new ArgumentException("organizationId");
			if (string.IsNullOrWhiteSpace(organizationName)) throw new ArgumentException("organizationName");

			this.OrganizationId = organizationId;
			this.OrganizationName = organizationName;
			this.OrganizationRole = role;
		}

		/// <summary>
		/// Gets or sets the Organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Organization name.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the Role of the user in the organization.
		/// </summary>
		public OrganizationRole OrganizationRole { get; set; }

		/// <summary>
		/// Gets or sets the List of subscriptions this organization has subscribed to and the user's role in it.
		/// </summary>
		public Dictionary<int, UserSubscription> UserSubscriptions { get; set; }
	}
}
