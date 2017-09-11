//------------------------------------------------------------------------------
// <copyright file="Organization.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using AllyisApps.Services.Lookup;

namespace AllyisApps.Services
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

		public Organization()
		{
			Address = new Address();
		}
	}
}