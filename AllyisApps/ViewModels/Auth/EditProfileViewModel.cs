﻿//------------------------------------------------------------------------------
// <copyright file="EditProfileViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents an editable view of user information.
	/// </summary>
	public class EditProfileViewModel : BaseViewModel
	{
		private const string CharsToReplace = @"""/\[]:|<>+=; ,?*'`()@";

		/// <summary>
		/// Initializes a new instance of the <see cref="EditProfileViewModel"/> class.
		/// </summary>
		public EditProfileViewModel(bool isEditing = true)
		{
			// Note: this is included soley to keep the model constructed during a POST from complaining about a null reference
			//   as it builds the countries list, even though the list isn't used anymore.
			this.ValidCountries = new List<string>();
			this.isEditing = isEditing; // True by default
		}

		/// <summary>
		///
		/// </summary>
		public EditProfileViewModel()
		{
			this.ValidCountries = new List<string>();
			this.isEditing = true;
		}

		/// <summary>
		/// Gets or sets the account e-mail.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Auth.Strings)), ErrorMessageResourceName = "EmailValidation")]
		[EmailAddress(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Auth.Strings)), ErrorMessageResourceName = "EmailFormatValidation")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the user's first name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Auth.Strings)), ErrorMessageResourceName = "FirstNameValidation")]
		[DataType(DataType.Text)]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the user's last name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Auth.Strings)), ErrorMessageResourceName = "LastNameValidation")]
		[DataType(DataType.Text)]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the user's phone number.
		/// </summary>
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Auth.Strings)), ErrorMessageResourceName = "PhoneFormatValidation")] // [Phone] does not work //I am not conviced that this is a good idea either.
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

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
		/// Gets or sets the user's state.
		/// </summary>
		[Display(Name = "State")]
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the user's country or region.
		/// </summary>
		[Display(Name = "Country/Region")]
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the user's postal code.
		/// </summary>
		[DataType(DataType.PostalCode)]
		[Display(Name = "Postal Code")]
		public string PostalCode { get; set; }

		//// [RegularExpression(@"(\d{5}(?:[-\s]\d{4})?)", ErrorMessage = "Invalid postal code.")] // Require 5 digits followed by an optional hyphen/whitespace and four more digits
		//// [RegularExpression(@"([\-\s\w]{3,10})", ErrorMessage = "Invalid postal code.")]
		//// We shouldn't be validating all postal codes this way as every country has a different format.

		/// <summary>
		/// Gets or sets Date of birth.
		/// </summary>
		//[DataType(DataType.Date)]
		//[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		//[Display(Name = "Date of Birth")]
		//[SQLDateProtector]
		public int DateOfBirth { get; set; } //has to be int for localization to work correctly. Gets changed to DateTime? when saving data from view.

		/// <summary>
		/// Gets or sets a List of valid countries.
		/// </summary>
		public IEnumerable<string> ValidCountries { get; set; }

		/// <summary>
		/// Localized valid countries.
		/// </summary>
		/// <returns>A Dictionary keyed with the English translation and valued with the localized version.</returns>
		public Dictionary<string, string> GetLocalizedValidCoutriesDictionary()
		{
			Dictionary<string, string> countries = new Dictionary<string, string>();

			foreach (string country in ValidCountries)
			{
				string countryKey = Clean(country);

				string localized = AllyisApps.Resources.ViewModels.Auth.Countries.ResourceManager.GetString(countryKey) ?? country;

				countries.Add(country, localized);
			}

			return countries;
		}

		/// <summary>
		/// Cleans things.
		/// </summary>
		/// <param name="stringToClean">The thing to clean.</param>
		/// <returns>The cleaned thing.</returns>
		public string Clean(string stringToClean)
		{
			if (stringToClean == null)
			{
				return string.Empty;
			}
			else
			{
				return CharsToReplace.Aggregate(stringToClean, (str, l) => str.Replace(string.Empty + l, string.Empty));
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this is for editing (true) or just display
		/// </summary>
		public bool isEditing { get; set; }
	}
}
