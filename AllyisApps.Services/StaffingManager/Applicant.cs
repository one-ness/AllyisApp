//------------------------------------------------------------------------------
// <copyright file="Applicant.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services
{
	/// <summary>
	/// An Applicant object, holding all info directly related to the Applicant.  Also performs basic field validation.
	/// </summary>
	public class Applicant
	{
		public const int MaxNameLength = 32;
		public const int MaxEmailLength = 100;
		public const int MaxPhoneNumberLength = 16;
		public const int MaxAddressLength = 64;
		public const int MaxCityLength = 64;
		public const int MaxPostalCodeLength = 64;

		private int applicantId;
		private string firstName;
		private string lastName;
		private string email;
		private int addressId;
		private string address;
		private string city;

		//private string state;
		//private string country;
		private string postalCode;

		private string phoneNumber;

		/// <summary>
		/// Gets or sets the applicant's ID.
		/// </summary>
		public int ApplicantId
		{
			get => applicantId;
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(ApplicantId), value, nameof(ApplicantId) + " must be greater than 0.");
				}
				applicantId = value;
			}
		}

		/// <summary>
		/// Gets or sets FirstName.
		/// </summary>
		public string FirstName
		{
			get => firstName;
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException(nameof(FirstName), nameof(FirstName) + " must not be null or empty.");
				}
				if (value.Length > MaxNameLength)
				{
					throw new ArgumentOutOfRangeException(nameof(FirstName), value, nameof(FirstName) + " length must be less than " + MaxNameLength + ".");
				}
				firstName = value;
			}
		}

		/// <summary>
		/// Gets or sets LastName.
		/// </summary>
		public string LastName
		{
			get => lastName;
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException(nameof(LastName), nameof(LastName) + " must not be null or empty.");
				}
				if (value.Length > MaxNameLength)
				{
					throw new ArgumentOutOfRangeException(nameof(LastName), value, nameof(LastName) + " length must be less than " + MaxNameLength + ".");
				}
				lastName = value;
			}
		}

		/// <summary>
		/// Gets or sets Email.
		/// </summary>
		public string Email
		{
			get => email;
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException(nameof(Email), nameof(Email) + " must not be null or empty.");
				}
				if (value.Length > MaxEmailLength)
				{
					throw new ArgumentOutOfRangeException(nameof(Email), value, nameof(Email) + " length must be less than " + MaxEmailLength + ".");
				}
				email = value;
			}
		}

		/// <summary>
		/// Gets or sets user's Address Id.
		/// </summary>
		public int AddressId
		{
			get => addressId;
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(AddressId), value, nameof(AddressId) + " must be greater than 0.");
				}
				addressId = value;
			}
		}

		/// <summary>
		/// Gets or sets Address.
		/// </summary>
		public string Address
		{
			get => address;
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > MaxAddressLength)
				{
					throw new ArgumentOutOfRangeException(nameof(Address), value, nameof(Address) + " length must be less than " + MaxAddressLength + ".");
				}
				address = value;
			}
		}

		/// <summary>
		/// Gets or sets City.
		/// </summary>
		public string City
		{
			get => city;
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > MaxCityLength)
				{
					throw new ArgumentOutOfRangeException(nameof(City), value, nameof(City) + " length must be less than " + MaxCityLength + ".");
				}
				city = value;
			}
		}

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
		public string PostalCode
		{
			get => postalCode;
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > MaxPostalCodeLength)
				{
					throw new ArgumentOutOfRangeException(nameof(PostalCode), value, nameof(PostalCode) + " length must be less than " + MaxPostalCodeLength + ".");
				}
				postalCode = value;
			}
		}

		/// <summary>
		/// Gets or sets PhoneNumber.
		/// </summary>
		public string PhoneNumber
		{
			get => phoneNumber;
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > MaxPhoneNumberLength)
				{
					throw new ArgumentOutOfRangeException(nameof(PhoneNumber), value, nameof(PhoneNumber) + " length must be less than " + MaxPhoneNumberLength + ".");
				}
				phoneNumber = value;
			}
		}

		/// <summary>
		/// Gets or sets Notes.
		/// </summary>
		public string Notes { get; set; }
	}
}