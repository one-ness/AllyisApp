//------------------------------------------------------------------------------
// <copyright file="SubscriptionsViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using AllyisApps.Services.Billing;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.Shared
{
	/// <summary>
	/// View model containing lists of subscriptions and available products.
	/// </summary>
	public class SubscriptionsViewModel
	{
		/// <summary>
		/// Gets or sets the Subscription info.
		/// </summary>
		public IEnumerable<SubscriptionDisplayInfo> Subscriptions { get; set; }

		/// <summary>
		/// Gets or sets a List of available products.
		/// </summary>
		public IEnumerable<Product> ProductList { get; set; }

		/// <summary>
		/// Gets an Organization instance for the organization these subscriptions are attached to.
		/// </summary>
		public Organization OrgInfo { get; internal set; }

		/// <summary>
		/// Gets or sets a value indicating whether user has permissions for editing the organization.
		/// </summary>
		public bool CanEditOrganization { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether user has permissions for viewing TimeTracker.
		/// </summary>
		public bool TimeTrackerViewSelf { get; set; }
	}
}