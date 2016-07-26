//------------------------------------------------------------------------------
// <copyright file="ReportViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Areas.TimeTracker.Models
{
    /// <summary>
    /// Adds name fields to TimeEntries for preview.
    /// </summary>
    public struct TablePreviewEntry
	{
		/// <summary>
		/// TimeEntry field.
		/// </summary>
		public TimeEntryInfo TimeEntry;

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
		/// Gets the id of the organization related to the report data.
		/// </summary>
		public int OrganizationId { get; internal set; }

		/// <summary>
		/// Gets the Select List of Users for this organization.
		/// </summary>
		public IEnumerable<SelectListItem> UserView { get; internal set; }

		/// <summary>
		/// Gets the list of Customers for this organization.
		/// </summary>
		public IEnumerable<CustomerInfo> Customers { get; internal set; }

		/// <summary>
		/// Gets the Select List of Customers for this organization.
		/// </summary>
		public IEnumerable<SelectListItem> CustomerView { get; internal set; }

		/// <summary>
		/// Gets the list of Projects for this organization.
		/// </summary>
		public IEnumerable<CompleteProjectInfo> Projects { get; internal set; }

		/// <summary>
		/// Gets the Select List of Projects for this organization.
		/// </summary>
		public IEnumerable<SelectListItem> ProjectView { get; internal set; }

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
		public IEnumerable<TablePreviewEntry> PreviewEntries { get; internal set; }

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
		public ReportSelectionModel Selection { get; internal set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user can manage and can see others reports.
		/// </summary>
		public bool CanManage { get; set; }

		/// <summary>
		/// Gets or sets the start of week.
		/// </summary>
		public StartOfWeekEnum StartOfWeek { get; set; }
	}

	/// <summary>
	/// Model for the selections made on the Reports form.
	/// </summary>
	public class ReportSelectionModel
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
		/// Gets the Start Date selection.
		/// </summary>
		public DateTime? StartDate { get; internal set; }

		/// <summary>
		/// Gets the End Date selection.
		/// </summary>
		public DateTime? EndDate { get; internal set; }

		/// <summary>
		/// Gets the Page selection.
		/// </summary>
		public int Page { get; internal set; }
	}
}