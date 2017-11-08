//------------------------------------------------------------------------------
// <copyright file="DataExportViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using AllyisApps.ViewModels.TimeTracker.Project;

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
}