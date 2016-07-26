//------------------------------------------------------------------------------
// <copyright file="SubscriptionDisplayDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel.Billing
{
	/// <summary>
	/// The table to display the subscription information. 
	/// </summary>
	public class SubscriptionDisplayDBEntity
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
		/// Gets or sets Subscriptions used.
		/// </summary>
		public int SubscriptionsUsed { get; set; }

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
		public DateTime CreatedUTC { get; set; }

		/// <summary>
		/// Gets or sets The string representing the tier of the subscription.
		/// </summary>
		public string Tier { get; set; }
	}
}
