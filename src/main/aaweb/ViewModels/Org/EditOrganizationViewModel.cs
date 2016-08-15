//------------------------------------------------------------------------------
// <copyright file="EditOrganizationViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataAnnotationsExtensions;

namespace AllyisApps.ViewModels
{
	/// <summary>
	/// Represents the organization profile edit form.
	/// </summary>
	public class EditOrganizationViewModel : BaseViewModel
	{
		private const string CharsToReplace = @"""/\[]:|<>+=; ,?*'`()@";

		/// <summary>
		/// Initializes a new instance of the <see cref="EditOrganizationViewModel"/> class.
		/// </summary>
		public EditOrganizationViewModel()
		{
			// Note: this is included soley to keep the model constructed during a POST from complaining about a null reference
			//   as it builds the countries list, even though the list isn't used anymore.
			this.ValidCountries = new List<string>();
		}

		/// <summary>
		/// Gets or sets the organization's name.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Organization Name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the organization's name.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Subdomain Name")]
		[RegularExpression(@"^[\w\d]([\w\d\-_]+[\w\d])?$", ErrorMessage = "Must contain only letters, numbers, dashes, and understcores. Must begin and end with a letter or number.")]
		public string SubdomainName { get; set; }

		/// <summary>
		/// Gets or sets the organization's website URL.
		/// </summary>
		[DataAnnotationsExtensions.Url(UrlOptions.OptionalProtocol)]
		[DataType(DataType.Url)]
		[Display(Name = "Website")]
		public string SiteUrl { get; set; }

		/// <summary>
		/// Gets or sets the organization's physical address.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "Street Address")]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the organization's country of residence.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "Country/Region")]
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the organization's state of residence.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "State")]
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the organization's city of residence.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "City")]
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the organization's postal code.
		/// </summary>
		[DataType(DataType.PostalCode)]
		////[RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid postal code")]
		[RegularExpression(@"([\-\s\w]{3,10})", ErrorMessage = "Invalid postal code.")]
		[Display(Name = "Postal Code")]
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the organization's phone number.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the organization's fax number.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "Fax Number")]
		public string FaxNumber { get; set; }

		/// <summary>
		/// Gets the organization's ID.
		/// </summary>
		public int OrganizationId { get; internal set; }

		/// <summary>
		/// Gets or sets a value indicating whether a user has permition to delete the org.
		/// </summary>
		public bool CanDelete { get; set; }

		/// <summary>
		/// Gets a list of valid countries.
		/// </summary>
		public IEnumerable<string> ValidCountries { get; internal set; }

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