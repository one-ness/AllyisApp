//------------------------------------------------------------------------------
// <copyright file="UserSubscriptionInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Account
{
	/// <summary>
	/// User subscription info.
	/// </summary>
	public class UserSubscriptionInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UserSubscriptionInfo"/> class.
		/// </summary>
		public UserSubscriptionInfo()
		{
			this.ProductRole = ProductRole.NotInProduct;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UserSubscriptionInfo"/> class.
		/// </summary>
		/// <param name="subscriptionId">The subscription ID.</param>
		/// <param name="skuId">The SKU ID.</param>
		/// <param name="productName">The product name.</param>
		/// <param name="role">The role.</param>
		public UserSubscriptionInfo(int subscriptionId, int skuId, string productName, ProductRole role)
		{
			if (subscriptionId < 1)
			{
				throw new ArgumentOutOfRangeException("subscriptionId");
			}

			if (skuId < 1)
			{
				throw new ArgumentOutOfRangeException("skuId");
			}

			if (string.IsNullOrWhiteSpace(productName))
			{
				throw new ArgumentNullException("productName");
			}

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
		/// Gets or sets the Sku of this subscription.
		/// </summary>
		public int SkuId { get; set; }

		/// <summary>
		/// Gets or sets the Product name.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets the Role of the user in this subscription.
		/// </summary>
		public ProductRole ProductRole { get; set; }
	}
}