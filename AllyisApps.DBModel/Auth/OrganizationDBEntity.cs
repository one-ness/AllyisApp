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
		public string SiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the organization's Address Id.
        /// </summary>
        public int AddressId { get; set; }

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
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the organization's phone number.
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the organization's fax number.
		/// </summary>
		public string FaxNumber { get; set; }

		/// <summary>
		/// Gets or sets subdomain.
		/// </summary>
		public string Subdomain { get; set; }

		/// <summary>
		/// Gets or sets DateCreated.
		/// </summary>
		public DateTime CreatedUtc { get; set; }
	}
}
