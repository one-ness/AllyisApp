//------------------------------------------------------------------------------
// <copyright file="SubscriptionDisplayInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// Represents a subscription's information.
	/// </summary>
	public class SubscriptionDisplayInfo
	{
		/// <summary>
		/// Gets or sets a Product Id.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets a Product name.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets a Subscription id.
		/// </summary>
		public int SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets a Subscription name.
        /// </summary>
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Gets or sets a Organization id.
        /// </summary>
        public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets a Sku Id.
		/// </summary>
		public int SkuId { get; set; }

		/// <summary>
		/// Gets or sets a Number of users.
		/// </summary>
		public int NumberOfUsers { get; set; }

		/// <summary>
		/// Gets or sets a Name of the organization.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets a Name of the Sku.
		/// </summary>
		public string SkuName { get; set; }

		/// <summary>
		/// Gets or sets a Date the entry was added.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets a String representing the tier of the subscription.
		/// </summary>
		public string Tier { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a user has permission to view this subscription.
		/// </summary>
		public bool CanViewSubscription { get; set; }
	}
}
