//------------------------------------------------------------------------------
// <copyright file="UserSubscription.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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
		public UserSubscription() { }

		/// <summary>
		/// Gets or sets the Subscription id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets the name of the subscription.
		/// </summary>
		public string SubscriptionName { get; set; }

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
		/// Gets or sets the Role of the user in this subscription.
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets the Area URL.
		/// </summary>
		public string AreaUrl { get; set; }
	}
}
