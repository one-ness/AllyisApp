//------------------------------------------------------------------------------
// <copyright file="SubscriptionUser.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// An object for keeping track of all the info related to a given subscription user.
	/// </summary>
	public class SubscriptionUser
	{
		/// <summary>
		/// Gets or sets FirstName.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets LastName.
		/// </summary>
		public string LastName { get; set; }

		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the date the subscription user was added.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the id of the subscription.
		/// </summary>
		public int SubscriptionId { get; set; }
	}
}