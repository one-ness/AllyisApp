//------------------------------------------------------------------------------
// <copyright file="UserSubscription.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using AllyisApps.Services.Billing;

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// a subscription that a user belongs to
	/// </summary>
	public class UserSubscription
	{
		public int UserId { get; set; }
		public int ProductRoleId { get; set; }

		public Subscription Subscription { get; set; }
	}
}