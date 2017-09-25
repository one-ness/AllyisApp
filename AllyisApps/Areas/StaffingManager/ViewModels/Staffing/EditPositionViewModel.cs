//------------------------------------------------------------------------------
// <copyright file="CreateTimeEntryViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AllyisApps.Services;
using AllyisApps.Services.Crm;
using AllyisApps.Services.StaffingManager;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Represents a position for creation into the database.
	/// </summary>
	public class EditPositionViewModel
	{
		/// <summary>
		/// used to tell if editing or creating new position
		/// </summary>
		public bool IsCreating { get; set; }

		/// <summary>
		/// used to tell if editing or creating new position
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// used to tell if editing or creating new position
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the position's Id
		/// </summary>
		public int PositionId { get; set; }

		/// <summary>
		/// Gets or sets positions associated organization
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets positionss hiring customer
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets the address of the position location
		/// </summary>
		public int AddressId { get; set; }

		/// <summary>
		/// Get or sets the Start date of the position
		/// </summary>
		public DateTime PositionCreatedUtc { get; set; }

		/// <summary>
		/// Get or sets the Start date of the position
		/// </summary>
		public DateTime PositionModifiedUtc { get; set; }

		/// <summary>
		/// Get or sets the Start date of the position
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the status int(enum) of the position
		/// </summary>
		public int PositionStatusId { get; set; }

		/// <summary>
		/// default status ID
		/// </summary>
		public int defaultStatusId { get; set; }

		/// <summary>
		/// Gets or sets the position name
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "Position Title")]
		public string PositionTitle { get; set; }

		/// <summary>
		/// Gets or sets billing rate frequency (eg: Months, Weeks)
		/// </summary>
		public BillingRateEnum BillingRateFrequency { get; set; }

		/// <summary>
		/// Get or sets the billing rate amount in dollars
		/// </summary>
		public int BillingRateAmount { get; set; }

		/// <summary>
		/// Gets or sets the duration of the position if applicable
		/// </summary>
		public int DurationMonths { get; set; }

		/// <summary>
		/// Gets or sets the employment type (eg: salary, hourly)
		/// </summary>
		public int EmploymentTypeId { get; set; }

		/// <summary>
		/// Gets or sets the number of hires needed for this position
		/// </summary>
		[DataType(DataType.Text)]
		[Display(Name = "Required Skills")]
		public int PositionCount { get; set; }

		/// <summary>
		/// Get or sets the Required Skills description
		/// </summary>
		public string RequiredSkills { get; set; }

		/// <summary>
		/// Gets or sets the Job responibilites description
		/// </summary>
		public string JobResponsibilities { get; set; }

		/// <summary>
		/// Gets or sets the desired skills description
		/// </summary>
		public string DesiredSkills { get; set; }

		/// <summary>
		/// Gets or sets the poition level description (eg: Senior, Junior)
		/// </summary>
		public int PositionLevelId { get; set; }

		/// <summary>
		/// Get or sets the name of the responsible hiring manager
		/// </summary>
		public string HiringManager { get; set; }

		/// <summary>
		/// Gets or sets the name of the team this position is for
		/// </summary>
		public string TeamName { get; set; }

		/// <summary>
		/// list of tag objects
		/// </summary>
		public string[] Tags { get; set; }

		/// <summary>
		/// the list of tags for the new position to set
		/// </summary>
		public string TagsToSubmit { get; set; }

		/// <summary>
		///
		/// </summary>
		public AddressViewModel PositionAddress { get; set; }

		/// <summary>
		/// CustomerInfo for View Model
		/// </summary>
		public CustomerInfoViewModel CustomerInfo { get; set; }

		/// <summary>
		/// Customer infromation
		/// </summary>
		public class CustomerInfoViewModel
		{
			/// <summary>
			/// selected country code
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// selected country code
			/// </summary>
			public string ContactEmail { get; set; }

			/// <summary>
			/// selected country code
			/// </summary>
			public string ContactPhoneNumber { get; set; }

			/// <summary>
			/// selected country code
			/// </summary>
			public string FaxNumber { get; set; }

			/// <summary>
			/// selected country code
			/// </summary>
			public string EIN { get; set; }

			/// <summary>
			/// selected country code
			/// </summary>
			public int OrganizationId { get; set; }

			/// <summary>
			/// Address country code
			/// </summary>
			public AddressViewModel Address { get; set; }
		}

		/// <summary>
		/// position levels available
		/// </summary>
		public List<PositionLevelSelectViewModel> PositionLevels { get; set; }

		/// <summary>
		/// position statuses available
		/// </summary>
		public List<PositionStatusSelectViewModel> PositionStatuses { get; set; }

		/// <summary>
		/// employment types available
		/// </summary>
		public List<EmploymentTypeSelectViewModel> EmploymentTypes { get; set; }

		/// <summary>
		/// list of customers the org has used
		/// </summary>
		public List<CustomerSelectViewModel> Customers { get; set; }

		/// <summary>
		/// Localized Countries
		/// </summary>
		public Dictionary<string, string> LocalizedCountries { get; set; }

		/// <summary>
		/// Localized States
		/// </summary>
		public Dictionary<string, string> LocalizedStates { get; set; }
	}

	/// <summary>
	/// Edit Position Address View Model
	/// </summary>
	public class AddressViewModel
	{
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
		/// selected country code
		/// </summary>
		public string SelectedCountryCode { get; set; }

		/// <summary>
		/// country code and localized names
		/// </summary>
		public Dictionary<string, string> LocalizedCountries { get; set; }

		/// <summary>
		/// state id and localized names
		/// </summary>
		public Dictionary<string, string> LocalizedStates { get; set; }
	}
}