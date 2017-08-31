//------------------------------------------------------------------------------
// <copyright file="SubscriptionDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.Billing
{
	/// <summary>
	/// Represents an organization's product subscription.
	/// </summary>
	public class SubscriptionDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets SubscriptionId.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets OrganizationId.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets SkuId.
		/// </summary>
		public int SkuId { get; set; }

		/// <summary>
		/// Gets or sets number of users.
		/// </summary>
		public int NumberOfUsers { get; set; }

		/// <summary>
		/// Gets or sets Licenses.
		/// </summary>
		public int Licenses { get; set; }

		/// <summary>
		/// Gets or sets CreatedDate.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// gets or sets modified date.
		/// </summary>
		public DateTime ModifiedUtc { get; set; }

		/// <summary>
		/// gets or set promotion expiration date.
		/// </summary>
		public DateTime? PromoExpirationDateUtc { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this subscription is active.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		///  Gets or sets the name of the Sku.
		/// </summary>
		public string Name { get; set; }
	}
}
