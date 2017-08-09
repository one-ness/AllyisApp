//------------------------------------------------------------------------------
// <copyright file="SkuInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// An object for keeping track of all the info related to a given Sku.
	/// </summary>
	public class SkuInfo
	{
		/// <summary>
		/// Gets or sets the subscription id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the Id of the entry in the SKU table.
		/// </summary>
		public int SkuId { get; set; }

		/// <summary>
		/// Gets or sets the SkuId of the upgrade or downgrade version of the Product.
		/// </summary>
		public int SkuIdNext { get; set; }

		/// <summary>
		/// Gets or sets the products id number.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets name of the sku.
		/// </summary>
		public string SkuName { get; set; }

		/// <summary>
		/// Gets or sets the SkuId of the upgrade or downgrade version of the Product.
		/// </summary>
		public string NextName { get; set; }

		/// <summary>
		/// Gets or sets the price.
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// Gets or sets the User limit for the Sku.
		/// </summary>
		public int UserLimit { get; set; }

		/// <summary>
		/// Gets or sets how often the user is billed for the subscription.
		/// </summary>
		public BillingFrequencyEnum BillingFrequency { get; set; }

		/// <summary>
		/// Gets or sets the description of the sku.
		/// </summary>
		public string Description { get; set; }
	}
}
