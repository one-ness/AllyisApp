//------------------------------------------------------------------------------
// <copyright file="SkuDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

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
		public short SkuId { get; set; }

		/// <summary>
		/// Gets or sets ProductId.
		/// </summary>
		public short ProductId { get; set; }

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
		public byte BillingFrequency { get; set; }

		/// <summary>
		/// Gets or sets the block size
		/// </summary>
		public int BlockSize { get; set; }

		/// <summary>
		/// Gets or sets is active
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets promotional cost per block
		/// </summary>
		public decimal PromoCostPerBlock { get; set; }

		/// <summary>
		/// Gets or sets the date on which the promotional cost ends
		/// </summary>
		public DateTime? PromotDeadline { get; set; }

		/// <summary>
		/// Gets or sets the description
		/// </summary>
		public string Description { get; set; }
	}
}
