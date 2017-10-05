//------------------------------------------------------------------------------
// <copyright file="SubscriptionUserDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace AllyisApps.DBModel.Billing
{
	/// <summary>
	/// Subscription User object.
	/// </summary>
	public class SubscriptionUserDBEntity : BaseDBEntity
	{
		/// <summary>
		/// gets or set the product role.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// gets or set the product role.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the emial of the User.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// gets or set the product role.
		/// </summary>
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

		/// <summary>
		///
		/// </summary>
		public int ProductId { get; set; }
	}
}