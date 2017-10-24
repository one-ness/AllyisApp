using System;
using System.Collections.Generic;
using AllyisApps.Services.Auth;

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// a subscription (by an organization to a sku of a product)
	/// </summary>
	public class Subscription
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
		public SkuIdEnum SkuId { get; set; }

		/// <summary>
		/// subscription name
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets number of users.
		/// </summary>
		public int NumberOfUsers { get; set; }

		/// <summary>
		/// Gets or sets CreatedDate.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

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
		public string SkuName { get; set; }

		public string SkuDescription { get; set; }

		public ProductIdEnum ProductId { get; set; }

		public string ProductName { get; set; }

		public string ProductDescription { get; set; }

		public string ProductAreaUrl { get; set; }

		public string SkuIconUrl { get; internal set; }
	}
}
