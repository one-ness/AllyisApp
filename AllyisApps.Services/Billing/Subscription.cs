using System;
using System.Collections.Generic;
using AllyisApps.Services.Billing;

namespace AllyisApps.Services
{
	/// <summary>
	/// a subscription (by an organization to a sku of a product)
	/// </summary>
	public class Subscription
	{
		public int SubscriptionId { get; set; }

		public string SubscriptionName { get; set; }

		//Product information
		public SkuIdEnum SkuId { get; set; }

		public string SkuName { get; set; }

		public ProductIdEnum ProductId { get; set; }

		/// <summary>
		/// Containes the possible ProductRoles for the Subscription
		/// </summary>
		public List<ProductRole> ProductRoles { get; set; }

		public string ProductName { get; set; }

		public string AreaUrl { get; set; }

		/// <summary>
		/// Gets or sets Licenses.
		/// </summary>
		public int Licenses { get; set; }

		/// <summary>
		/// Gets or sets a String representing the tier of the subscription.
		/// </summary>
		public string Tier { get; set; }

		public string Description { get; internal set; }

		public string IconUrl { get; internal set; }

		//Organizaion informaiton
		public int NumberOfUsers { get; set; }

		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets a Name of the organization.
		/// </summary>
		public string OrganizationName { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedUtc { get; set; }

		public DateTime? PromoExpirationDateUtc { get; set; }
	}
}