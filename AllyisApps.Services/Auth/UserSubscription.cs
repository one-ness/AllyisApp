//------------------------------------------------------------------------------
// <copyright file="UserSubscription.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services
{
	/// <summary>
	/// User subscription info.
	/// </summary>
	public class UserSubscription
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UserSubscription"/> class.
		/// </summary>
		public UserSubscription()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UserSubscription"/> class.
		/// </summary>
		/// <param name="subscriptionId">The subscription ID.</param>
		/// <param name="skuId">The SKU ID.</param>
		/// <param name="productName">The product name.</param>
		/// <param name="role">The role.</param>
		public UserSubscription(int subscriptionId, int skuId, string productName, int role)
		{
			if (subscriptionId <= 0) throw new ArgumentOutOfRangeException("subscriptionId");
			if (skuId <= 0) throw new ArgumentOutOfRangeException("skuId");
			if (string.IsNullOrWhiteSpace(productName)) throw new ArgumentNullException("productName");
			if (role <= 0) throw new ArgumentOutOfRangeException("role");

			this.ProductName = productName;
			this.SkuId = skuId;
			this.SubscriptionId = subscriptionId;
			this.ProductRole = role;
		}

		/// <summary>
		/// Gets or sets the Subscription id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the organization this subscription belongs to.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Sku of this subscription.
		/// </summary>
		public int SkuId { get; set; }

		/// <summary>
		/// Gets or sets the Product Id.
		/// </summary>
		public ProductIdEnum ProductId { get; set; }

		/// <summary>
		/// Gets or sets the Product name.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets the Product Role name.
		/// </summary>
		public string RoleName { get; set; }

		/// <summary>
		/// Gets or sets the Role of the user in this subscription.
		/// </summary>
		public int ProductRole { get; set; }
	}
}
