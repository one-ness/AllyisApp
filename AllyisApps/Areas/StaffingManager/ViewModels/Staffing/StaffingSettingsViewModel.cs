using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AllyisApps.Services;
using AllyisApps.Services.Lookup;
using AllyisApps.Services.StaffingManager;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Staffing home page view model.
	/// </summary>
	public class StaffingSettingsViewModel
	{
		/// <summary>
		/// a list of the short-hand position objects used to populate the views main table
		/// </summary>
		public List<PositionThumbnailInfo> positions { get; set; }

		/// <summary>
		/// organizations identifier number
		/// </summary>
		public int organizationId { get; set; }

		/// <summary>
		/// subscriptions identifier number
		/// </summary>
		public int subscriptionId { get; set; }

		/// <summary>
		/// Address identifier number
		/// </summary>
		public int addressId { get; set; }

		/// <summary>
		/// subscription name
		/// </summary>
		public string subscriptionName { get; set; }

		/// <summary>
		/// users identification number
		/// </summary>
		public int userId { get; set; }

		/// <summary>
		/// list fo tag objects used by the organization
		/// </summary>
		public List<TagViewModel> tags { get; set; }

		/// <summary>
		/// customers from the company
		/// </summary>
		public List<Customer> customers { get; set; }

		/// <summary>
		/// a customer for new customer creation
		/// </summary>
		public Customer newCustomer { get; set; }

		/// <summary>
		/// an address for new customer creation
		/// </summary>
		public NewAddressViewModel newCustomerAddress { get; set; }

		/// <summary>
		/// list of employment types used by the organization
		/// </summary>
		public List<EmploymentTypeViewModel> employmentTypes { get; set; }

		/// <summary>
		/// list of position levels used by the organization
		/// </summary>
		public List<PositionLevelViewModel> positionLevels { get; set; }

		/// <summary>
		/// list of position statuses used by the organization
		/// </summary>
		public List<PositionStatusViewModel> positionStatuses { get; set; }

		/// <summary>
		/// Gets or sets the position's address.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "Address")]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the position's city.
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "City")]
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the position's state.
		/// </summary>
		[Display(Name = "State")]
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the position's country or region.
		/// </summary>
		[Display(Name = "Country")]
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the position's postal code.
		/// </summary>
		[DataType(DataType.PostalCode)]
		[Display(Name = "Postal Code")]
		public string PostalCode { get; set; }

		/// <summary>
		/// selected state id
		/// </summary>
		public int? SelectedStateId { get; set; }

		/// <summary>
		/// state id and localized names
		/// </summary>
		public Dictionary<string, string> LocalizedStates { get; set; }

		/// <summary>
		/// selected country code
		/// </summary>
		public string SelectedCountryCode { get; set; }

		/// <summary>
		/// country code and localized names
		/// </summary>
		public Dictionary<string, string> LocalizedCountries { get; set; }

		/// <summary>
		/// Addres for new Custoemr 
		/// </summary>
		public class NewAddressViewModel {
			/// <summary>
			/// New City
			/// </summary>
			public string City { get; set; }
			/// <summary>
			/// CounteryCode
			/// </summary>
			public string CountryCode { get; set; }
			/// <summary>
			/// StateId
			/// </summary>
			public int StateId {get;set;}

			/// <summary>
			/// Postal/Zip Code 
			/// </summary>
			public string PostalCode { get; set; }

			/// <summary>
			/// Street Address
			/// </summary>
			public string Address1 { get; set; }
		}

	}
}