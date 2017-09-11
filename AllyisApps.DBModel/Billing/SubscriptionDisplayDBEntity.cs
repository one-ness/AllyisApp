//------------------------------------------------------------------------------
// <copyright file="SubscriptionDisplayDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.Billing
{
	/// <summary>
	/// The table to display the subscription information.
	/// </summary>
	public class SubscriptionDisplayDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets The id of the subscription product.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets The string identifier of the product id.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Prodcut Description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets The subscription id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets The organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets The Sku id.
		/// </summary>
		public int SkuId { get; set; }

		/// <summary>
		/// Gets or sets The number of users.
		/// </summary>
		public int NumberOfUsers { get; set; }

		/// <summary>
		/// Gets or sets Subscription Name.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets The name of the organization.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets The name of the Sku.
		/// </summary>
		public string SkuName { get; set; }

		/// <summary>
		/// Gets or sets Date the entry was added.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets The string representing the tier of the subscription.
		/// </summary>
		public string Tier { get; set; }

		/// <summary>
		/// Get Area Url
		/// </summary>
		public string AreaUrl { get; set; }
	}
}