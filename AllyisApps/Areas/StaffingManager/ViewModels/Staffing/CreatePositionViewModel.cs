//------------------------------------------------------------------------------
// <copyright file="CreatePositionViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.Staffing
{
	/// <summary>
	/// Represents a position for creation into the database.
	/// </summary>
	public class CreatePositionViewModel
	{
		/// <summary>
		/// Gets or sets a value indicating whether editing or creating a new position.
		/// </summary>
		public bool IsCreating { get; set; }

		/// <summary>
		/// Gets or sets the subscription name.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets the position's Id.
		/// </summary>
		public int PositionId { get; set; }

		/// <summary>
		/// Gets or sets positions associated organization.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets positionss hiring customer.
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets the address of the position location.
		/// </summary>
		public int AddressId { get; set; }

		/// <summary>
		/// Gets or sets the Start date of the position.
		/// </summary>
		public DateTime PositionCreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the Start date of the position.
		/// </summary>
		public DateTime PositionModifiedUtc { get; set; }

		/// <summary>
		/// Gets or sets the Start date of the position.
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the status int(enum) of the position.
		/// </summary>
		public int PositionStatusId { get; set; }

		/// <summary>
		/// Gets or sets the position name.
		/// </summary>
		public string PositionTitle { get; set; }

		/// <summary>
		/// Gets or sets billing rate frequency (eg: Months, Weeks).
		/// </summary>
		public int BillingRateFrequency { get; set; }

		/// <summary>
		/// Gets or sets the billing rate amount in dollars.
		/// </summary>
		public int BillingRateAmount { get; set; }

		/// <summary>
		/// Gets or sets the duration of the position if applicable.
		/// </summary>
		public int DurationMonths { get; set; }

		/// <summary>
		/// Gets or sets the employment type (eg: salary, hourly).
		/// </summary>
		public int EmploymentTypeId { get; set; }

		/// <summary>
		/// Gets or sets the number of hires needed for this position.
		/// </summary>
		public int PositionCount { get; set; }

		/// <summary>
		/// Gets or sets the Required Skills description.
		/// </summary>
		public string RequiredSkills { get; set; }

		/// <summary>
		/// Gets or sets the Job responibilites description.
		/// </summary>
		public string JobResponsibilities { get; set; }

		/// <summary>
		/// Gets or sets the desired skills description.
		/// </summary>
		public string DesiredSkills { get; set; }

		/// <summary>
		/// Gets or sets the poition level description (eg: Senior, Junior).
		/// </summary>
		public int PositionLevelId { get; set; }

		/// <summary>
		/// Gets or sets the name of the responsible hiring manager.
		/// </summary>
		public string HiringManager { get; set; }

		/// <summary>
		/// Gets or sets the name of the team this position is for.
		/// </summary>
		public string TeamName { get; set; }

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
		/// Gets or sets the state id.
		/// </summary>
		public int? SelectedStateId { get; set; }

		/// <summary>
		/// Gets or sets the state id and localized names.
		/// </summary>
		public Dictionary<string, string> LocalizedStates { get; set; }

		/// <summary>
		/// Gets or sets the selected country code.
		/// </summary>
		public string SelectedCountryCode { get; set; }

		/// <summary>
		/// Gets or sets the country code and localized names.
		/// </summary>
		public Dictionary<string, string> LocalizedCountries { get; set; }
	}
}