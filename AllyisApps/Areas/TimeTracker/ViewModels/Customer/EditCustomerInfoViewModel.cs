//------------------------------------------------------------------------------
// <copyright file="EditCustomerInfoViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AllyisApps.ViewModels.TimeTracker.Customer
{
	/// <summary>
	/// Represents an editable view of user information.
	/// </summary>
	public class EditCustomerInfoViewModel : BaseViewModel
	{
		private const string CharsToReplace = @"""/\[]:|<>+=; ,?*'`()@";

		/// <summary>
		/// Initializes a new instance of the <see cref="EditCustomerInfoViewModel"/> class.
		/// </summary>
		public EditCustomerInfoViewModel()
		{
			// Note: this is included soley to keep the model constructed during a POST from complaining about a null reference
			//   as it builds the countries list, even though the list isn't used anymore.
			this.ValidCountries = new List<string>();
			this.IsCreating = false;
		}

		/// <summary>
		/// Gets or sets the account e-mail.
		/// </summary>
		[EmailAddress]
		[Display(Name = "Contact Email")]
		public string ContactEmail { get; set; }

		/// <summary>
		/// Gets or sets the user's first name.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Customer id.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Customer ID")]
		public int CustomerID { get; set; }

		/// <summary>
		/// Gets or sets the user's phone number.
		/// </summary>
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")] // [Phone] does not work
		[Display(Name = "Contact Phone Number")]
		public string ContactPhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the Customer's phone extension.
		/// </summary>
		[Display(Name = "EIN")]
		public string EIN { get; set; }

		/// <summary>
		/// Gets or sets the Customer's address.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "Address")]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the Customer's city.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "City")]
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the Customer's state.
		/// </summary>
		[Display(Name = "State")]
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the Customer's country or region.
		/// </summary>
		[Display(Name = "Country")]
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the Customer's postal code.
		/// </summary>
		[DataType(DataType.PostalCode)]
		[Display(Name = "Postal Code")]
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the Customer's Organization ID.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Customer ID")]
		public string CustomerOrgId { get; set; }

		/// <summary>
		/// Gets or sets the Customer's organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the name of the organization that the Customer belongs too.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the Customer's fax number.
		/// </summary>
		public string FaxNumber { get; set; }

		/// <summary>
		/// Gets or sets the Customer's Web url.
		/// </summary>
		public string Website { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the model is being used for creating a customer (true) or editing an existing customer (false).
		/// </summary>
		public bool IsCreating { get; set; }

		/// <summary>
		/// Gets or sets List of valid countries.
		/// </summary>
		public IEnumerable<string> ValidCountries { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether the user can edit customers
        /// </summary>
        public bool canEditCustomers { get; set; }

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
	}
}
