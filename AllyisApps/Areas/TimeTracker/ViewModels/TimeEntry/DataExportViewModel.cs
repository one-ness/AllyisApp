//------------------------------------------------------------------------------
// <copyright file="DataExportViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Model for exporting Time Entries as CSV or Excel files.
	/// </summary>
	public class DataExportViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataExportViewModel" /> class.
		/// </summary>
		public DataExportViewModel()
		{
		}

		/// <summary>
		/// Gets the list of projects associated with this query.
		/// </summary>
		public List<CompleteProjectViewModel> Projects { get; internal set; }

		/// <summary>
		/// Gets the list of Time entry data.
		/// </summary>
		public List<TimeEntryViewModel> Data { get; internal set; }

		/// <summary>
		/// Gets the list of Time entry data to preview.
		/// </summary>
		public List<TimeEntryViewModel> PreviewData { get; internal set; }

		/// <summary>
		/// Gets the total number of pages available to view.
		/// </summary>
		public int PageTotal { get; internal set; }

		/// <summary>
		/// Gets the StreamWriter for outputting to CSV.
		/// </summary>
		public StreamWriter Output { get; internal set; }

		/// <summary>
		/// Gets the StringWriter for outputting to Excel.
		/// </summary>
		public StringWriter ExcelOutput { get; internal set; }
	}

	/// <summary>
	/// View Model for ProjectInfomation.
	/// </summary>
	public class CompleteProjectViewModel
	{
		/// <summary>
		/// Gets or sets the Project Id.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the project is active.
		/// </summary>
		public bool IsCurrentlyActive { get; set; }

		/// <summary>
		/// Gets or sets the Customer Id that the project belongs to.
		/// </summary>
		public int? CustomerId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the Customer is active.
		/// </summary>
		public bool IsCustomerActive { get; set; }

		/// <summary>
		/// Gets or sets the Oorganization Id that the customer belongs to.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Date/time of creation.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the Name of the project.
		/// </summary>
		public string ProjectName { get; set; }

		/// <summary>
		/// Gets or sets the Name of the customer.
		/// </summary>
		public string CustomerName { get; set; }

		/// <summary>
		/// Gets or sets the Name of the organization.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a user is active on the project, if that project was fetched via userId.
		/// </summary>
		public bool IsUserActive { get; set; }

		/// <summary>
		/// Gets or sets the Project Pricing Type.
		/// </summary>
		public string PriceType { get; set; }

		/// <summary>
		/// Gets or sets the Project start date.
		/// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Gets or sets the Project end date.
		/// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the current user can edit the project.
		/// </summary>
		public bool CanEditProject { get; set; }

		/// <summary>
		/// Gets or sets the Id of the project to be used by the organization.
		/// </summary>
		public string ProjectCode { get; set; }

		/// <summary>
		/// Gets or sets the Id of the customer to be used by the organization.
		/// </summary>
		public string CustomerCode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user is a user of this project, in some contexts.
		/// </summary>
		public bool? IsProjectUser { get; set; }

		/// <summary>
		/// Initializes a instance of see <see cref="CompleteProjectViewModel"/>
		/// </summary>
		public CompleteProjectViewModel()
		{
		}

		/// <summary>
		/// Initializes a instance of see <see cref="CompleteProjectViewModel"/>
		/// </summary>
		public CompleteProjectViewModel(Services.Crm.CompleteProject proj)
		{
			CanEditProject = proj.CanEditProject;
			CreatedUtc = proj.CreatedUtc;
			CustomerId = proj.OwningCustomer?.CustomerId;
			CustomerName = proj.OwningCustomer?.CustomerName;
			CustomerCode = proj.OwningCustomer?.CustomerCode;
			EndDate = proj.EndDate;
			IsCurrentlyActive = proj.IsCurrentlyActive;
			IsCustomerActive = proj.IsCustomerActive;
			IsProjectUser = proj.IsProjectUser;
			IsUserActive = proj.IsUserActive;
			OrganizationId = proj.OrganizationId;
			OrganizationName = proj.OrganizationName;
			PriceType = proj.PriceType;
			ProjectId = proj.ProjectId;
			ProjectName = proj.ProjectName;
			ProjectCode = proj.ProjectCode;
			StartDate = proj.StartDate;
		}
	}
}