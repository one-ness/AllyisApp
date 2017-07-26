//------------------------------------------------------------------------------
// <copyright file="OrganizationDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.ComponentModel;

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// Represents the Organizations table in the database.
	/// </summary>
	public class OrganizationDBEntity
	{
		/// <summary>
		/// Gets or sets the organization's Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the organization's e-mail address.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the organization's website URL.
		/// </summary>
		[DisplayName("Website")]
		public string SiteUrl { get; set; }

		/// <summary>
		/// Gets or sets the organization's physical address.
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the organization's city.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the organization's state.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the organization's country.
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the organization's postal code.
		/// </summary>
		[DisplayName("Postal Code")]
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the organization's phone number.
		/// </summary>
		[DisplayName("Phone Number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the organization's fax number.
		/// </summary>
		[DisplayName("Fax Number")]
		public string FaxNumber { get; set; }

		/// <summary>
		/// Gets or sets subdomain.
		/// </summary>
		public string Subdomain { get; set; }

		/// <summary>
		/// Gets or sets DateCreated.
		/// </summary>
		[DisplayName("Date Created")]
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user has permissions to edit organization.
		/// </summary>
		public bool CanEditOrganization { get; set; }
	}
}
