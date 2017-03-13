//------------------------------------------------------------------------------
// <copyright file="UserOrganizationInfo.cs" company="Allyis, Inc.">
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
	public class UserOrganizationInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UserOrganizationInfo"/> class.
		/// </summary>
		public UserOrganizationInfo()
		{
			this.OrganizationRole = OrganizationRole.Member;
			this.UserSubscriptionInfoList = new List<UserSubscriptionInfo>();
			this.UserProjectList = new List<int>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UserOrganizationInfo"/> class.
		/// </summary>
		/// <param name="organizationId">The Organization ID.</param>
		/// <param name="organizationName">The organization Name.</param>
		/// <param name="role">The role.</param>
		/// <param name="infoList">The info list.</param>
		/// <param name="projectList">The project list.</param>
		public UserOrganizationInfo(
			int organizationId,
			string organizationName,
			OrganizationRole role,
			List<UserSubscriptionInfo> infoList,
			List<int> projectList)
		{
			if (organizationId < 1)
			{
				throw new ArgumentOutOfRangeException("organizationId");
			}

			if (string.IsNullOrWhiteSpace(organizationName))
			{
				throw new ArgumentNullException("organizationName");
			}

			this.OrganizationId = organizationId;
			this.OrganizationName = this.OrganizationName;
			this.OrganizationRole = role;
			if (infoList == null)
			{
				this.UserSubscriptionInfoList = new List<UserSubscriptionInfo>();
			}
			else
			{
				this.UserSubscriptionInfoList = infoList;
			}

			if (projectList == null)
			{
				this.UserProjectList = new List<int>();
			}
			else
			{
				this.UserProjectList = projectList;
			}
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
		public List<UserSubscriptionInfo> UserSubscriptionInfoList { get; set; }

		/// <summary>
		/// Gets or sets the List of projects this user has within this organization.
		/// </summary>
		public List<int> UserProjectList { get; set; } // TODO: Flesh this out into a list of UserProjectInfo objects or something
	}
}
