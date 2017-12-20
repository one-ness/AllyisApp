//------------------------------------------------------------------------------
// <copyright file="ReportViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Adds name fields to TimeEntries for preview.
	/// </summary>
	public struct TablePreviewEntryViewModel
	{
		/// <summary>
		/// TimeEntry field.
		/// </summary>
		public TimeEntryViewModel TimeEntry;

		/// <summary>
		/// Customer name field.
		/// </summary>
		public string CustomerName;

		/// <summary>
		/// Project name field.
		/// </summary>
		public string ProjectName;
	}

	/// <summary>
	/// Model for the Report view.
	/// </summary>
	public class ReportViewModel
	{
		/// <summary>
		/// Gets or sets the id of the user requesting the report.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the subscription Id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the Subscription Name.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets the id of the organization related to the report data.
		/// </summary>
		public int OrganizationId { get; internal set; }

		/// <summary>
		/// Gets the Select List of Users for this organization.
		/// </summary>
		public List<SelectListItem> UserView { get; internal set; }

		/// <summary>
		/// Gets the list of Customers for this organization.
		/// </summary>
		public List<CustomerViewModel> Customers { get; internal set; }

		/// <summary>
		/// Gets the Select List of Customers for this organization.
		/// </summary>
		public List<SelectListItem> CustomerView { get; internal set; }

		/// <summary>
		/// Gets the list of Projects for this organization.
		/// </summary>
		public List<TimeEntryCompleteProjectViewModel> Projects { get; internal set; }

		/// <summary>
		/// Gets the Select List of Projects for this organization.
		/// </summary>
		public List<SelectListItem> ProjectView { get; internal set; }

		/// <summary>
		/// Gets or sets a value indicating whether the export button will be shown when employees selected are from different projects.
		/// </summary>
		public bool ShowExport { get; set; }

		/// <summary>
		/// Gets the number of entries to view per page.
		/// </summary>
		public int PreviewPageSize { get; internal set; }

		/// <summary>
		/// Gets the page number of the data to view.
		/// </summary>
		public int PreviewPageNum { get; internal set; }

		/// <summary>
		/// Gets the preview results data.
		/// </summary>
		public List<TablePreviewEntryViewModel> PreviewEntries { get; internal set; }

		/// <summary>
		/// Gets the preview data total.
		/// </summary>
		public string PreviewTotal { get; internal set; }

		/// <summary>
		/// Gets the preview data display message.
		/// </summary>
		public string PreviewMessage { get; internal set; }

		/// <summary>
		/// Gets the number of pages for total data.
		/// </summary>
		public int PreviewPageTotal { get; internal set; }

		/// <summary>
		/// Gets the selections made on the Reports page.
		/// </summary>
		public ReportSelectionViewModel Selection { get; internal set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user can manage and can see others reports.
		/// </summary>
		public bool CanManage { get; set; }


		/// <summary>
		/// Gets or sets the pay period ranges for use with the date range picker.
		/// </summary>
		public PayPeriodRangesViewModel PayPeriodRanges { get; set; }
	}

	/// <summary>
	/// Model for the selections made on the Reports form.
	/// </summary>
	public class ReportSelectionViewModel
	{
		/// <summary>
		/// Gets the list of Users selected.
		/// </summary>
		public List<int> Users { get; internal set; }

		/// <summary>
		/// Gets the Id of the Customer selection.
		/// </summary>
		public int CustomerId { get; internal set; }

		/// <summary>
		/// Gets the Id of the Project selection.
		/// </summary>
		public int ProjectId { get; internal set; }

		/// <summary>
		/// Gets the Start Date selection. Note: must be int and not DateTime for Json serialization to work correctly in different cultures.
		/// </summary>
		public DateTime? StartDate { get; internal set; }

		/// <summary>
		/// Gets the End Date selection. Note: must be int and not DateTime for Json serialization to work correctly in different cultures.
		/// </summary>
		public DateTime? EndDate { get; internal set; }

		/// <summary>
		/// Gets the Page selection.
		/// </summary>
		public int Page { get; internal set; }
	}

	/// <summary>
	/// View Model for Customer
	/// </summary>
	public class CustomerViewModel
	{
		/// <summary>
		/// Gets or sets the customer id number.
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string CustomerName { get; set; }

		/// <summary>
		/// Gets or sets Address.
		/// </summary>
		public AddressViewModel Address { get; set; }

		/// <summary>
		/// Gets or sets Email.
		/// </summary>
		public string ContactEmail { get; set; }

		/// <summary>
		/// Gets or sets PhoneNumber.
		/// </summary>
		public string ContactPhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets Fax number.
		/// </summary>
		public string FaxNumber { get; set; }

		/// <summary>
		/// Gets or sets Fax number.
		/// </summary>
		public string Website { get; set; }

		/// <summary>
		/// Gets or sets Employer Identification Number.
		/// </summary>
		public string EIN { get; set; }

		/// <summary>
		/// Gets or sets the date the customer was created.
		/// </summary>
		public string CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the id of the organization associated with the customer.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the id of the customer to be used by the users within the organization.
		/// </summary>
		public string CustomerCode { get; set; }

		/// <summary>
		/// Gets or sets the bool value indicating if this Customer is currently active.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets the active projects for a customer.
		/// </summary>
		public int ActiveProjects { get; set; }

		/// <summary>
		/// Gets or sets the inactive projects for a customer.
		/// </summary>
		public int InactiveProjects { get; set; }

		/// <summary>
		/// Constructor for customer view model
		/// </summary>
		public CustomerViewModel()
		{
			//AddressViewModel = new AddressViewModel();
		}
	}

	/// <summary>
	/// View Model for Address.
	/// </summary>
	public class AddressViewModel
	{
		/// <summary>
		/// Gets or sets the address' Id.
		/// </summary>
		public int? AddressId { get; set; }

		/// <summary>
		/// Gets or sets address1.
		/// </summary>
		public string Address1 { get; set; }

		/// <summary>
		/// Gets or sets address2.
		/// </summary>
		public string Address2 { get; set; }

		/// <summary>
		/// Gets or sets the City.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Get or sets the State.
		/// </summary>
		public string StateName { get; set; }

		/// <summary>
		/// Gets or sets the State Id
		/// </summary>
		public int? StateId { get; set; }

		/// <summary>
		/// Gets or sets the PostalCode
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the country
		/// </summary>
		public string CountryName { get; set; }

		/// <summary>
		/// Gets or sets the country code
		/// </summary>
		public string CountryCode { get; set; }
	}
}