﻿//------------------------------------------------------------------------------
// <copyright file="ManageOrgViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AllyisApps.Services;
using AllyisApps.Services.Common.Types;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// The org management view model.
	/// </summary>
	[CLSCompliant(false)]
	public class ManageOrgViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the organization's Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the organization's details viewmodel.
		/// </summary>
		public Organization Details { get; set; }

		/// <summary>
		/// Gets or sets the organizations members.
		/// </summary>
		public OrganizationMembersViewModel Members { get; set; }

		/// <summary>
		/// Gets or sets the organization's subscriptions.
		/// </summary>
		public IEnumerable<SubscriptionDisplayViewModel> Subscriptions { get; set; }

		/// <summary>
		/// Gets or sets the members to add.
		/// </summary>
		public OrganizationAddMembersViewModel Add { get; set; }

		/// <summary>
		/// Gets or sets the stripe Token id.
		/// </summary>
		public BillingServicesCustomer BillingCustomer { get; set; }

		/// <summary>
		/// Gets or sets the last four digits of credit card.
		/// </summary>
		public string LastFour { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this customer can edit the organization.
		/// </summary>
		public bool CanEditOrganization { get; set; }

		/// <summary>
		/// Gets or sets the Number of subscriptions this organizaiton has.
		/// </summary>
		public int SubscriptionCount { get; set; }
	}
}
