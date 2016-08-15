//------------------------------------------------------------------------------
// <copyright file="DataExportViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using AllyisApps.Services.BusinessObjects;

namespace AllyisApps.Areas.TimeTracker.Models
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
		public IEnumerable<CompleteProjectInfo> Projects { get; internal set; }

		/// <summary>
		/// Gets the list of Time entry data.
		/// </summary>
		public IEnumerable<TimeEntryInfo> Data { get; internal set; }

		/// <summary>
		/// Gets the list of Time entry data to preview.
		/// </summary>
		public IEnumerable<TimeEntryInfo> PreviewData { get; internal set; }

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

        #region Oldcode
        /* This was replaced by PrepareCSV/PrepareExcelExportAction and SetPreviewDataAction  

		/// <summary>
		/// Prepares the CSV file for output.
		/// </summary>
		public void CreateCSVFileOutput()
		{
			this.Output = new StreamWriter(new MemoryStream());
			this.Output.WriteLine("\"LastName\",\"FirstName\",\"Date\",\"Duration\",\"Project\",\"Customer\",\"Description\"");
			try
			{
				foreach (TimeEntryInfo entry in this.Data)
				{
					this.Output.WriteLine(string.Format(
						"\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\"",
						entry.LastName,
						entry.FirstName,
						entry.Date.ToShortDateString(),
						entry.Duration,
						this.Projects.Where(x => x.ProjectId == entry.ProjectId).SingleOrDefault().ProjectName ?? string.Empty,
						this.Projects.Where(x => x.ProjectId == entry.ProjectId).SingleOrDefault().CustomerName ?? string.Empty,
						entry.Description));
				}
			}
			catch (Exception)
			{
			}

			this.Output.Flush();
			this.Output.BaseStream.Seek(0, SeekOrigin.Begin);
		}
        
		
        /// <summary>
		/// Prepares the Excel file for output.
		/// </summary>
		/// <param name="limit">The max number of values to return.</param>
		/// <param name="page">The page of results to view.</param>
		public void CreateExcelFileOutput(int limit = 0, int page = 1)
		{

            int skipNum = limit * (page - 1);
            limit = limit == 0 ? this.Data.Count() : limit;

            GridView output = new GridView();

            var dataSkip = (from j in Data
                            select j).Skip(skipNum).Take(limit);

            // only process data values for current page

            // get output column data
            DOES NOT WORK WITH "ALL USERS" SELECTION FOR NO APPARENT REASON 
			var dataMod = from i in dataSkip
						  select new
						  {
							  LastName      = i.LastName,
							  FirstName     = i.FirstName,
							  Date          = i.Date.ToShortDateString(),
							  Duration      = i.Duration,
							  ProjectName   = this.Projects.Where(x => x.ProjectId == i.ProjectId).Single().ProjectName ?? string.Empty,
							  CustomerName  = this.Projects.Where(x => x.ProjectId == i.ProjectId).Single().CustomerName ?? string.Empty,
							  Description   = i.Description
						  };

			output.DataSource = dataMod;
			output.DataBind();
			this.ExcelOutput = new StringWriter();
			HtmlTextWriter htw = new HtmlTextWriter(this.ExcelOutput);
			output.RenderControl(htw);
        }   

		/// <summary>
		/// Prepares the page data to view.
		/// </summary>
		/// <param name="pageSize">The max number of values to set.</param>
		/// <param name="page">The page of results to view.</param>
		public void SetPreviewData(int pageSize = 0, int page = 1)
		{
			int skipNum = pageSize * (page - 1);
			int limit = pageSize == 0 ? this.Data.Count() : pageSize;

			if (this.Data.Count() == 0)
			{
				this.PageTotal = 1;
			}
			else
			{
				// Count equals pageSize --> fits on 1 page
				this.PageTotal = 1 + ((this.Data.Count() - 1) / pageSize);
			}

			// only process data values for current page
			this.PreviewData = (from p in this.Data
								select p).Skip(skipNum).Take(limit);
		}*/
        #endregion Oldcode
    }
}