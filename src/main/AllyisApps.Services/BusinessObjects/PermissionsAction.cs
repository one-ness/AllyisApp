//------------------------------------------------------------------------------
// <copyright file="PermissionsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace AllyisApps.BusinessObjects
{
	/// <summary>
	/// An object representing the actions being performed.
	/// </summary>
	public class PermissionsAction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PermissionsAction" /> class.
		/// </summary>
		public PermissionsAction()
		{
			this.OrgRoleTarget = 0;
			this.TimeTrackerRoleTarget = 0;
		}

		/// <summary>
		/// Gets or sets the organization's Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the subscription Id for Timetracker if this organization has one.
		/// </summary>
		public int TimeTrackerSubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the set of userIds who are members of the organization.
		/// </summary>
		public ISet<int> OrganizationMembers { get; set; }

		/// <summary>
		/// Gets or sets the target role to set the organization users to.
		/// </summary>
		public int? OrgRoleTarget { get; set; }

		/// <summary>
		/// Gets or sets the target role to set the timetracker subscription users to.
		/// </summary>
		public int? TimeTrackerRoleTarget { get; set; }
	}
}