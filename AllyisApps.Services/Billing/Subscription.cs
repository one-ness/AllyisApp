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
		/// Gets or sets number of users.
		/// </summary>
		public int NumberOfUsers { get; set; }

		/// <summary>
		/// Gets or sets CreatedDate.
		/// </summary>
		public DateTime SubscriptionCreatedUtc { get; set; }

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

		public string SubscriptionName { get; set; }

		public ProductIdEnum ProductId { get; set; }

		public string ProductName { get; set; }

		public string ProductDescription { get; set; }

		public string AreaUrl { get; set; }

		public string IconUrl { get; internal set; }
	}
}
