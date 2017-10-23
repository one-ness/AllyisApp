//------------------------------------------------------------------------------
// <copyright file="EditProfileViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents an editable view of user information.
	/// </summary>
	public class EditProfileViewModel : BaseViewModel
	{
		/*
		 * possible phone validation regex
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "PhoneFormatValidation")] // [Phone] does not work //I am not conviced that this is a good idea either.
		[RegularExpression(@"((\+?\d\s?|)(\(\d{3}\)|\d{3}) ?-? ?|)\d{3} ?-? ?\d{4}", ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "PhoneFormatValidation")]
		possible postal code regex (We shouldn't be validating all postal codes this way as every country has a different format.)
		[RegularExpression(@"(\d{5}(?:[-\s]\d{4})?)", ErrorMessage = "Invalid postal code.")] // Require 5 digits followed by an optional hyphen/whitespace and four more digits
		[RegularExpression(@"([\-\s\w]{3,10})", ErrorMessage = "Invalid postal code.")]

		possible date validation
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		[SQLDateProtector]
		 * */

		/// <summary>
		/// Initializes a new instance of the <see cref="EditProfileViewModel"/> class.
		/// </summary>
		public EditProfileViewModel()
		{
			this.LocalizedCountries = new Dictionary<string, string>();
			this.LocalizedStates = new Dictionary<string, string>();
		}

		/// <summary>
		/// Gets or sets the account e-mail.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "EmailValidation")]
		[EmailAddress(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "EmailFormatValidation")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the user's first name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "FirstNameValidation")]
		[DataType(DataType.Text)]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the user's last name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "LastNameValidation")]
		[DataType(DataType.Text)]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the user's phone number.
		/// </summary>
		[RegularExpression(@"^\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*$", ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "PhoneFormatValidation")] // [Phone] does not work
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the user's address Id.
		/// </summary>
		public int? AddressId { get; set; }

		/// <summary>
		/// Gets or sets the user's street address.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "Street Address")]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the user's city.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "City")]
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the user's postal code.
		/// </summary>
		[DataType(DataType.PostalCode)]
		[Display(Name = "Postal Code")]
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets Date of birth.
		/// </summary>
		[MinDateValidation]
		public int DateOfBirth { get; set; } // has to be int for localization to work correctly. Gets changed to DateTime? when saving data from view.

		/// <summary>
		/// Gets or sets the selected state id.
		/// </summary>
		public int? SelectedStateId { get; set; }

		/// <summary>
		/// Gets or sets the state id and localized names.
		/// </summary>
		[Display(Name = "State")]
		public Dictionary<string, string> LocalizedStates { get; set; }

		/// <summary>
		/// Gets or sets the selected country code.
		/// </summary>
		public string SelectedCountryCode { get; set; }

		/// <summary>
		/// Gets or sets the country code and localized names.
		/// </summary>
		[Display(Name = "Country/Region")]
		public Dictionary<string, string> LocalizedCountries { get; set; }
	}

	/// <summary>
	/// Validates that the integer representing the birthdate meets a minimum requirement.
	/// </summary>
	public class MinDateValidation : ValidationAttribute
	{
		/// <summary>
		/// Validates if the value meets the minimum requirement.
		/// </summary>
		/// <param name="value">The integer value of the birthdate to be validated.</param>
		/// <param name="validationContext">Validation context.</param>
		/// <returns>A validation result with the status of the value being validated.</returns>
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			int minAgeYears = 15;

			// -1 represents a null date
			if ((int)value > -1)
			{
				DateTime dob = new DateTime(1 / 1 / 1).AddDays((int)value);
				dob = new DateTime(dob.Subtract(new DateTime(1 / 1 / 1)).Ticks);
				DateTime minAgeDate = new DateTime(DateTime.Today.Ticks).AddYears(-minAgeYears);
				if (dob > minAgeDate)
				{
					return new ValidationResult("Must be at least " + minAgeYears + " years of age to register");
				}
				else if ((int)value < 639905)
				{
					return new ValidationResult("Please enter a date within the last 150 years");
				}
			}

			return ValidationResult.Success;
		}
	}
}