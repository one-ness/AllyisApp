//------------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.BusinessObjects
{
	/// <summary>
	/// An object for keeping track of all the info related to a given customer.
	/// </summary>
	public class CustomerInfo
	{
		/// <summary>
		/// Gets or sets the customer id number.
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets Address.
		/// </summary>
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
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets Email.
		/// </summary>
		public string ContactEmail { get; set; }

		/// <summary>
		/// Gets or sets PhoneNumber.
		/// </summary>
		public string ContactPhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets Fax number.
		/// </summary>
		public string FaxNumber { get; set; }

		/// <summary>
		/// Gets or sets Fax number.
		/// </summary>
		public string Website { get; set; }

		/// <summary>
		/// Gets or sets Employer Identification Number.
		/// </summary>
		public string EIN { get; set; }

		/// <summary>
		/// Gets or sets the date the customer was created.
		/// </summary>
		public string CreatedUTC { get; set; }

		/// <summary>
		/// Gets or sets the id of the organization associated with the customer.
		/// </summary>
		public int OrganizationId { get; set; }
	}
}
