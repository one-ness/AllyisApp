//------------------------------------------------------------------------------
// <copyright file="UserSubscription.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// a subscription that a user belongs to
	/// </summary>
	public class UserSubscription : Subscription
	{
		public int UserId { get; set; }
		public int ProductRoleId { get; set; }
		public string IconUrl { get; internal set; }
	}
}