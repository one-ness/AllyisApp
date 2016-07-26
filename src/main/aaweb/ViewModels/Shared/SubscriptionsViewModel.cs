//------------------------------------------------------------------------------
// <copyright file="SubscriptionsViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

using AllyisApps.Services.BusinessObjects;
using AllyisApps.Services.Crm;

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
		public IEnumerable<ProductInfo> ProductList { get; set; }

		/// <summary>
		/// Gets an OrganizationInfo instance for the organization these subscriptions are attached to.
		/// </summary>
		public OrganizationInfo OrgInfo { get; internal set; }

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