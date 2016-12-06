//------------------------------------------------------------------------------
// <copyright file="PrepareExcelExportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// Prepares the Excel file for output.
		/// </summary>
		/// <param name="data">TimeEntryInfo to be exported.</param>
		/// <param name="projects">The relevant projects used to pull project name and customers.</param>
		/// <param name="limit">The limited size of the export file as previously determined.</param>
		/// <param name="page">Page number adjusted to skip the proper pages if nessicary.</param>
		public void PrepareExcelExport(IEnumerable<TimeEntryInfo> data, IEnumerable<CompleteProjectInfo> projects, int limit = 0, int page = 1)
		{
			int skipNum = limit * (page - 1);
			limit = limit == 0 ? data.Count() : limit;

			GridView output = new GridView();

			var dataSkip = (from j in data
							select j).Skip(skipNum).Take(limit);

			var dataMod = from i in dataSkip
						  select new
						  {
							  LastName = i.LastName,
							  FirstName = i.FirstName,
							  Date = i.Date.ToShortDateString(),
							  Duration = i.Duration,
							  ProjectName = projects.Where(x => x.ProjectId == i.ProjectId).Single().ProjectName ?? string.Empty,
							  CustomerName = projects.Where(x => x.ProjectId == i.ProjectId).Single().CustomerName ?? string.Empty,
							  Description = i.Description
						  };

			output.DataSource = dataMod;
			output.DataBind();
			StringWriter excelOutput = new StringWriter();
			HtmlTextWriter htw = new HtmlTextWriter(excelOutput);
			output.RenderControl(htw);
		}
	}
}