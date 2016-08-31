//------------------------------------------------------------------------------
// <copyright file="OrganizationInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.Services.Org
{
	/// <summary>
	/// An object for keeping track of all the info related to a given organization.
	/// </summary>
	public class OrganizationInfo
	{
		/// <summary>
		/// Gets or sets the Organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Organization name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Organization site URL.
		/// </summary>
		[Display(Name = "Website")]
		public string SiteUrl { get; set; }

		/// <summary>
		/// Gets or sets the Street address.
		/// </summary>
		[Display(Name = "Street Address")]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the City.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the State.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the Country.
		/// </summary>
		[Display(Name = "Country/Region")]
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the Postal code.
		/// </summary>
		[Display(Name = "Postal Code")]
		public string PostalCode { get; set; }

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
		public DateTime DateCreated { get; set; }

		/// <summary>
		/// Gets or sets the Organization subdomain perfix.
		/// </summary>
		public string Subdomain { get; set; }
	}
}