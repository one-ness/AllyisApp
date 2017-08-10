//------------------------------------------------------------------------------
// <copyright file="UserContext.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

#pragma warning disable 1591

namespace AllyisApps.Services
{
	/// <summary>
	/// Logged in user context.
	/// An instance must be created for every HTTP request to the application.
	/// </summary>
	public class UserContext2
	{
		/// <summary>
		/// user information
		/// </summary>
		public int UserId { get; set; }
		public string Email { get; set; }
		public string FistName { get; set; }
		public string LastName { get; set; }
		public string PreferredLanguageId { get; set; }

		/// <summary>
		/// list of organizations this user is member of, and the role of user in that organization
		/// indexed by the organization id
		/// </summary>
		public Dictionary<int, int> OrganizationAndRoles { get; set; }

		/// <summary>
		/// list of subscriptions by the above organizations
		/// indexed by subscription id
		/// </summary>
		public Dictionary<int, SubscriptionAndRole> SubscriptionsAndRoles { get; set; }

		/// <summary>
		/// constructor
		/// </summary>
		public UserContext2()
		{
			this.OrganizationAndRoles = new Dictionary<int, int>();
			this.SubscriptionsAndRoles = new Dictionary<int, SubscriptionAndRole>();
		}

		/// <summary>
		/// subscription information for an organization
		/// </summary>
		public class SubscriptionAndRole
		{
			public int SubscriptionId { get; set; }
			public int SkuId { get; set; }
			public int ProductId { get; set; }
			public int ProductRoleId { get; set; }
			public int OrganizationId { get; set; }
		}
	}
}
