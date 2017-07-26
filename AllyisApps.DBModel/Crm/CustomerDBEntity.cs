//------------------------------------------------------------------------------
// <copyright file="CustomerDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel;

namespace AllyisApps.DBModel.Crm
{
	/// <summary>
	/// Represents the Users table in the database.
	/// </summary>
	public class CustomerDBEntity
	{
		/// <summary>
		/// Gets or sets CustomerId.
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		[DisplayName("Name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets Address.
		/// </summary>
		[DisplayName("Address")]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets City.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets State.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Gets or sets Country.
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets PostalCode.
		/// </summary>
		[DisplayName("Postal Code")]
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets Email.
		/// </summary>
		public string ContactEmail { get; set; }

		/// <summary>
		/// Gets or sets PhoneNumber.
		/// </summary>
		[DisplayName("Phone Number")]
		public string ContactPhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets Fax number.
		/// </summary>
		[DisplayName("Fax Number")]
		public string FaxNumber { get; set; }

		/// <summary>
		/// Gets or sets Fax number.
		/// </summary>
		[DisplayName("Website")]
		public string Website { get; set; }

		/// <summary>
		/// Gets or sets Employer Identification Number.
		/// </summary>
		[DisplayName("EIN")]
		public string EIN { get; set; }

		/// <summary>
		/// Gets or sets the date the customer was created.
		/// </summary>
		[DisplayName("Created Utc")]
		public string CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the id of the organization associated with the customer.
		/// </summary>
		[DisplayName("OrganizationID")]
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the id of the customer to be used by users within the organization
		/// </summary>
		public string CustomerOrgId { get; set; }

		/// <summary>
		/// Gets or sets the IsActive bool value for the customer. True means currently active
		/// </summary>
		public bool IsActive { get; set; }
	}
}
