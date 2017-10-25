﻿//------------------------------------------------------------------------------
// <copyright file="InvitationInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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
		public Subscription SubscriptionInfo { get; set; }
		public List<SkuInfo> SkuList { get; set; }
		public string StripeTokenCustId { get; set; }
		public int UserCount { get; set; }

		public ProductSubscription(Product product, Subscription subscriptionInfo, List<SkuInfo> list, string stripeTokenCustId, int userCount)
		{
			Product = product;
			SubscriptionInfo = subscriptionInfo;
			SkuList = list;
			StripeTokenCustId = stripeTokenCustId;
			UserCount = userCount;
		}
	}
}