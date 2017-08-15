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
		private const int MaxNameLength = 32;
		private const int MaxEmailLength = 100;
		private const int MaxPhoneNumberLength = 16;
		private const int MaxAddressLength = 64;
		private const int MaxCityLength = 64;
		private const int MaxPostalCodeLength = 64;

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
		/// The constructor used when GETTING or UPDATING info from the backend (includes ApplicantId and AddressId)
		/// </summary>
		/// <param name="applicantId">The applicant id.</param>
		/// <param name="firstName">The applicant's first name.</param>
		/// <param name="lastName">The applicant's last name.</param>
		/// <param name="email">The applicant's email.</param>
		/// <param name="addressId"></param>
		/// <param name="address">The applicant's address.</param>
		/// <param name="city">The applicant's city.</param>
		/// <param name="state">The applicant's state.</param>
		/// <param name="country">The applicant's country.</param>
		/// <param name="postalCode">The applicant's postal code.</param>
		/// <param name="phoneNumber">The applicant's phone number.</param>
		/// <param name="notes">Notes on the applicant.</param>
		public Applicant(
			int applicantId,
			string firstName,
			string lastName,
			string email,
			int addressId,
			string address,
			string city,
			string state,
			string country,
			string postalCode,
			string phoneNumber,
			string notes)
		{
			ApplicantId = applicantId;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			AddressId = addressId;
			Address = address;
			City = city;
			State = state;
			Country = country;
			PostalCode = postalCode;
			PhoneNumber = phoneNumber;
			Notes = notes;
		}

		/// <summary>
		/// The constructor used when UPDATING or INSERTING into the backend (excludes ApplicantId and AddressId)
		/// </summary>
		/// <param name="firstName">The applicant's first name.</param>
		/// <param name="lastName">The applicant's last name.</param>
		/// <param name="email">The applicant's email.</param>
		/// <param name="address">The applicant's address.</param>
		/// <param name="city">The applicant's city.</param>
		/// <param name="state">The applicant's state.</param>
		/// <param name="country">The applicant's country.</param>
		/// <param name="postalCode">The applicant's postal code.</param>
		/// <param name="phoneNumber">The applicant's phone number.</param>
		/// <param name="notes">Notes on the applicant.</param>
		public Applicant(
			string firstName,
			string lastName,
			string email,
			string address,
			string city,
			string state,
			string country,
			string postalCode,
			string phoneNumber,
			string notes)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Address = address;
			City = city;
			State = state;
			Country = country;
			PostalCode = postalCode;
			PhoneNumber = phoneNumber;
			Notes = notes;
		}

		/// <summary>
		/// Gets or sets the applicant's ID.
		/// </summary>
		public int ApplicantId
		{
			get
			{
				return applicantId;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("ApplicantId", value, "ApplicantId must be greater than 0.");
				}
				applicantId = value;
			}
		}

		/// <summary>
		/// Gets or sets FirstName.
		/// </summary>
		public string FirstName
		{
			get
			{
				return firstName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("FirstName", "FirstName must not be null or empty.");
				}
				else if (value.Length > MaxNameLength)
				{
					throw new ArgumentOutOfRangeException("FirstName", value, "FirstName length must be less than " + MaxNameLength + ".");
				}
				firstName = value;
			}
		}

		/// <summary>
		/// Gets or sets LastName.
		/// </summary>
		public string LastName
		{
			get
			{
				return lastName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("LastName", "LastName must not be null or empty.");
				}
				else if (value.Length > MaxNameLength)
				{
					throw new ArgumentOutOfRangeException("LastName", value, "LastName length must be less than " + MaxNameLength + ".");
				}
				lastName = value;
			}
		}

		/// <summary>
		/// Gets or sets Email.
		/// </summary>
		public string Email
		{
			get
			{
				return email;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("Email", "Email must not be null or empty.");
				}
				else if (value.Length > MaxEmailLength)
				{
					throw new ArgumentOutOfRangeException("Email", value, "Email length must be less than " + MaxEmailLength + ".");
				}
				email = value;
			}
		}

		/// <summary>
		/// Gets or sets user's Address Id.
		/// </summary>
		public int AddressId
		{
			get
			{
				return addressId;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("AddressId", value, "AddressId must be greater than 0.");
				}
				addressId = value;
			}
		}

		/// <summary>
		/// Gets or sets Address.
		/// </summary>
		public string Address
		{
			get
			{
				return address;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > MaxAddressLength)
				{
					throw new ArgumentOutOfRangeException("Address", value, "Address length must be less than " + MaxAddressLength + ".");
				}
				address = value;
			}
		}

		/// <summary>
		/// Gets or sets City.
		/// </summary>
		public string City
		{
			get
			{
				return city;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > MaxCityLength)
				{
					throw new ArgumentOutOfRangeException("City", value, "City length must be less than " + MaxCityLength + ".");
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
			get
			{
				return postalCode;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > MaxPostalCodeLength)
				{
					throw new ArgumentOutOfRangeException("PostalCode", value, "PostalCode length must be less than " + MaxPostalCodeLength + ".");
				}
				postalCode = value;
			}
		}

		/// <summary>
		/// Gets or sets PhoneNumber.
		/// </summary>
		public string PhoneNumber
		{
			get
			{
				return phoneNumber;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > MaxPhoneNumberLength)
				{
					throw new ArgumentOutOfRangeException("PhoneNumber", value, "PhoneNumber length must be less than " + MaxPhoneNumberLength + ".");
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
