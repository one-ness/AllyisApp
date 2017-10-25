﻿//------------------------------------------------------------------------------
// <copyright file="Organization.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Lookup;

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// an organization
	/// </summary>
	public class Organization
	{
		/// <summary>
		/// Gets or sets the Organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Organization name.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the Organization site URL.
		/// </summary>
		[Display(Name = "Website")]
		public string SiteUrl { get; set; }

		/// <summary>
		/// Gets or sets the organization's Address.
		/// </summary>
		public Address Address { get; set; }

		/// <summary>
		/// Gets or sets the Phone number.
		/// </summary>
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the Fax number.
		/// </summary>
		[Display(Name = "Fax Number")]
		public string FaxNumber { get; set; }

		/// <summary>
		/// Gets or sets the Date this organization was created.
		/// </summary>
		public DateTime? CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the Organization subdomain perfix.
		/// </summary>
		public string Subdomain { get; set; }

		/// <summary>
		/// Gets or sets if this organization is active
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// list of users in this organization
		/// </summary>
		public List<OrganizationUser> Users { get; set; }

		/// <summary>
		/// list of subscriptions for this organization
		/// </summary>
		public List<Subscription> Subscriptions { get; set; }

		/// <summary>
		/// List of invites sent out by owners of the Organizaion
		/// </summary>
		public List<Invitation> Invitations { get; set; }

		public String StripeToken { get; set; }

		/// <summary>
		/// indicates if the users list has been loaded
		/// </summary>
		public bool IsUsersLoaded { get; set; }

		/// <summary>
		/// indicates if subscriptions list has been loaded
		/// </summary>
		public bool IsSubscriptionsLoaded { get; set; }

		/// <summary>
		/// indicates if inviations list has been loaded
		/// </summary>
		public bool IsInvitationsLoaded { get; set; }

		public Organization()
		{
			Address = new Address();
			this.Users = new List<OrganizationUser>();
			this.Subscriptions = new List<Subscription>();
			this.Invitations = new List<Invitation>();
		}
	}
}