//------------------------------------------------------------------------------
// <copyright file="SkuDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.Billing
{
	/// <summary>
	/// A class representing the Sku table in the database.
	/// </summary>
	public class SkuDBEntity
	{
		/// <summary>
		/// Gets or sets SubscriptionId.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets SkuId.
		/// </summary>
		public int SkuId { get; set; }

		/// <summary>
		/// Gets or sets ProductId.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets Price.
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// Gets or sets UserLimit.
		/// </summary>
		public int UserLimit { get; set; }

		/// <summary>
		/// Gets or sets BillingFrequency.
		/// </summary>
		public string BillingFrequency { get; set; }
	}
}