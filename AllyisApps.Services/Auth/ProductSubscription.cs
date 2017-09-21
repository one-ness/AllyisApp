//------------------------------------------------------------------------------
// <copyright file="InvitationInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AllyisApps.Services.Billing;

namespace AllyisApps.Services
{
	/// <summary>
	/// Represents a subscription with its product info.
	/// </summary>
	public class ProductSubscription
	{
		public Product Product { get; set; }
		public SubscriptionInfo SubscriptionInfo { get; set; }
		public List<SkuInfo> List { get; set; }
		public string StripeTokenCustId { get; set; }
		public int UserCount { get; set; }

		public ProductSubscription(Product product, SubscriptionInfo subscriptionInfo, List<SkuInfo> list, string stripeTokenCustId, int userCount)
		{
			this.Product = product;
			this.SubscriptionInfo = subscriptionInfo;
			this.List = list;
			this.StripeTokenCustId = stripeTokenCustId;
			this.UserCount = userCount;
		}
	}
}
