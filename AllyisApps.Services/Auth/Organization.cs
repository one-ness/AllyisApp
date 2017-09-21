//------------------------------------------------------------------------------
// <copyright file="Organization.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using AllyisApps.Services.Lookup;
using System.Collections.Generic;

namespace AllyisApps.Services
{
	/// <summary>
	/// an organization
	/// </summary>
	public class Organization
	{
		private string nextEmpolyeeID;

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
		public List<InvitationInfo> Invitations { get; set; }

		public String StripeToken { get; set; }
		
		/// <summary>
		/// Gets next Unique Employee Id. This should only be loaded when populated.
		/// </summary>
		public String NextEmpolyeeID
		{
			get
			{
				if (nextEmpolyeeID == null) throw new Exception("Attempt to get NextEmpolyeeID when not poptualted.");
				return nextEmpolyeeID;
			}
			set
			{
				nextEmpolyeeID = value;
			}
		}

		/// <summary>
		/// indicates if the users list has been loaded
		/// </summary>
		public bool IsUsersLoaded { get; set; }

		/// <summary>
		/// indicates if subscriptions list has been loaded
		/// </summary>
		public bool IsSubscriptionsLoaded { get; set; }

		public Organization()
		{
			Address = new Address();
			this.Users = new List<OrganizationUser>();
			this.Subscriptions = new List<Subscription>();
			this.Invitations = new List<InvitationInfo>();
		}
	}
}
