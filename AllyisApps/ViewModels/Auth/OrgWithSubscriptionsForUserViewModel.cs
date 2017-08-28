//------------------------------------------------------------------------------
// <copyright file="OrgWithSubscriptionsForUserViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// View Model for organization info to display on account/index, along with subscription info
	/// tailored to a user for product panel boxes.
	/// </summary>
	public class OrgWithSubscriptionsForUserViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets an OrgInfo object for this organization.
		/// </summary>
		public Organization OrgInfo { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this user can edit the organization.
		/// </summary>
		public bool CanEditOrganization { get; set; }

		/// <summary>
		/// Gets or sets a list of subscription information objects.
		/// </summary>
		public List<SubscriptionDisplayViewModel> Subscriptions { get; set; }
	}
}
