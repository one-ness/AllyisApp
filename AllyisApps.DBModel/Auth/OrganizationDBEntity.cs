//------------------------------------------------------------------------------
// <copyright file="OrganizationDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

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
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the organization's website URL.
		/// </summary>
		public string SiteUrl { get; set; }

		/// <summary>
		/// Gets or sets the organization's Address Id.
		/// </summary>
		public int? AddressId { get; set; }

		/// <summary>
		/// gets or sets whether this organization is active
		/// </summary>
		public bool IsActive { get; set; }

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
		public DateTime? CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the count of users in the organization.
		/// </summary>
		public int UserCount { get; set; }
	}
}