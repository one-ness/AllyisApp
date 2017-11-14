//------------------------------------------------------------------------------
// <copyright file="CustomerInfoViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------\

namespace AllyisApps.ViewModels.TimeTracker.Customer
{
	/// <summary>
	/// Represents basic Customer info.
	/// </summary>
	public class CustomerInfoViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the name of the organization that the Customer belongs to.
		/// </summary>
		public string OrganizationName { get; set; }

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
		public AddessViewModel Address { get; set; }

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
		public string CustomerCode { get; set; }

		/// <summary>
		/// Gets or sets the bool value indicating if this Customer is currently active.
		/// </summary>
		public bool? IsActive { get; set; }

		/// <summary>
		/// Address View Model.
		/// </summary>
		public class AddessViewModel
		{
			/// <summary>
			/// Gets or sets the address' Id.
			/// </summary>
			public int? AddressId { get; set; }

			/// <summary>
			/// Gets or sets address1.
			/// </summary>
			public string Address1 { get; set; }

			/// <summary>
			/// Gets or sets address2.
			/// </summary>
			public string Address2 { get; set; }

			/// <summary>
			/// Gets or sets the City.
			/// </summary>
			public string City { get; set; }

			/// <summary>
			/// Gets or sets the State.
			/// </summary>
			public string StateName { get; set; }

			/// <summary>
			/// Gets or sets the State Id.
			/// </summary>
			public int? StateId { get; set; }

			/// <summary>
			/// Gets or sets the PostalCode.
			/// </summary>
			public string PostalCode { get; set; }

			/// <summary>
			/// Gets or sets the country.
			/// </summary>
			public string CountryName { get; set; }

			/// <summary>
			/// Gets or sets the country code.
			/// </summary>
			public string CountryCode { get; set; }
		}
	}
}