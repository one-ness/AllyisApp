//------------------------------------------------------------------------------
// <copyright file="Customer.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services.Lookup;

namespace AllyisApps.Services
{
	/// <summary>
	/// An object for keeping track of all the info related to a given customer.
	/// </summary>
	public class Customer
	{
		/// <summary>
		/// Gets or sets the customer id number.
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string CustomerName { get; set; }

		/// <summary>
		/// Gets or sets Address.
		/// </summary>
		public Address Address { get; set; }

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
		public string CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the id of the organization associated with the customer.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the id of the customer to be used by the users within the organization.
		/// </summary>
		public string CustomerOrgId { get; set; }

		/// <summary>
		/// Gets or sets the bool value indicating if this Customer is currently active.
		/// </summary>
		public bool? IsActive { get; set; }

        public Customer()
        {
            Address = new Address();
        }
	}
}
