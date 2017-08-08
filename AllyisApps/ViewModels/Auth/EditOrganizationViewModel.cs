//------------------------------------------------------------------------------
// <copyright file="EditOrganizationViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataAnnotationsExtensions;

namespace AllyisApps.ViewModels.Auth
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
			this.ValidCountries = new List<string>();
			this.LocalizedCountries = new Dictionary<string, string>();
			this.IsCreating = false;
		}

		/// <summary>
		/// Gets or sets the organization's name.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Organization Name")]
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the organization's website URL.
		/// </summary>
		[DataAnnotationsExtensions.Url(UrlOptions.OptionalProtocol)]
		[DataType(DataType.Url)]
		[Display(Name = "Website")]
		public string SiteUrl { get; set; }

		/// <summary>
		/// Gets or sets the Address Id.
		/// </summary>
		public int AddressId { get; set; }

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
		[Display(Name = "Postal Code")]
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the organization's phone number.
		/// </summary>
		[DataType(DataType.Text)]
		[RegularExpression(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$", ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "PhoneFormatValidation")]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the organization's fax number.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "Fax Number")]
		[RegularExpression(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$", ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "PhoneFormatValidation")]
		public string FaxNumber { get; set; }

		/// <summary>
		/// Gets or sets the owner's Employee Id. Only use on creating orgs.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Owner Employee Id")]
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets the organization's Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a user has permition to delete the org.
		/// </summary>
		public bool CanDelete { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the model is being used for creating an org (true), or editing an existing org (false).
		/// </summary>
		public bool IsCreating { get; set; }

		/// <summary>
		/// Gets a list of valid countries.
		/// </summary>
		public IEnumerable<string> ValidCountries { get; internal set; }

		/// <summary>
		/// selected state id
		/// </summary>
		public short SelectedStateId { get; set; }

		/// <summary>
		/// state id and localized names
		/// </summary>
		public Dictionary<string, string> LocalizedStates { get; set; }

		/// <summary>
		/// selected country code
		/// </summary>
		public int SelectedCountryCode { get; set; }

		/// <summary>
		/// country code and localized names
		/// </summary>
		public Dictionary<string, string> LocalizedCountries { get; set; }

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
