//------------------------------------------------------------------------------
// <copyright file="EditCustomerInfoViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.Staffing.Customer
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
			this.LocalizedCountries = new Dictionary<string, string>();
			this.LocalizedStates = new Dictionary<string, string>();
			this.IsCreating = false;
		}

		/// <summary>
		/// Gets or sets the account e-mail.
		/// </summary>
		[EmailAddress]
		[Display(Name = "Contact Email")]
		public string ContactEmail { get; set; }

		/// <summary>
		/// Gets or sets the customers displayed name.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[RegularExpression(@"^.{1,30}$", ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "CustomerNameWarning")]
		[Display(Name = "Name")]
		public string CustomerName { get; set; }

		/// <summary>
		/// Gets or sets the Customer id.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Customer Id")]
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets the user's phone number.
		/// </summary>
		[RegularExpression(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$", ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "PhoneFormatValidation")] // [Phone] does not work
		[Display(Name = "Contact Phone Number")]
		public string ContactPhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the Customer's fax number.
		/// </summary>
		[RegularExpression(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$", ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "PhoneFormatValidation")] // [Phone] does not work
		public string FaxNumber { get; set; }

		/// <summary>
		/// Gets or sets the Customer's phone extension.
		/// </summary>
		[Display(Name = "EIN")]
		public string EIN { get; set; }

		/// <summary>
		/// Gets or sets the customer's Address Id.
		/// </summary>
		public int? AddressId { get; set; }

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
		/// Gets or sets the Customer's Organization Id.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Customer Id")]
		public string CustomerOrgId { get; set; }

		/// <summary>
		/// Gets or sets the Customer's organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the id of subscription Id that the Customer belongs too.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the name of subscription Id that the Customer belongs too.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets the user's id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the name of the organization that the Customer belongs too.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the Customer's Web url.
		/// </summary>
		public string Website { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the model is being used for creating a customer (true) or editing an existing customer (false).
		/// </summary>
		public bool IsCreating { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user can edit customers.
		/// </summary>
		public bool CanEditCustomers { get; set; }

		/// <summary>
		/// Gets or sets Selected state id.
		/// </summary>
		public int? SelectedStateId { get; set; }

		/// <summary>
		/// Gets or sets the State id and localized names.
		/// </summary>
		public Dictionary<string, string> LocalizedStates { get; set; }

		/// <summary>
		/// Gets or sets the Selected country code.
		/// </summary>
		public string SelectedCountryCode { get; set; }

		/// <summary>
		/// Gets or sets Country code and localized names.
		/// </summary>
		public Dictionary<string, string> LocalizedCountries { get; set; }
	}
}