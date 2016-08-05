//------------------------------------------------------------------------------
// <copyright file="PrepareCSVExportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AllyisApps.Core;
using AllyisApps.Services.BusinessObjects;

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
		/// <param name="data">The TimeEntryInfo to be exported.</param>
		/// <param name="projects">The projects being pulled from, for getting project name and customers.</param>
		/// <returns>The stream writer.</returns>
		public StreamWriter PrepareCSVExport(IEnumerable<TimeEntryInfo> data, IEnumerable<CompleteProjectInfo> projects)
		{
			StreamWriter output = new StreamWriter(new MemoryStream());
			output.WriteLine(
				string.Format(
					"\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\"",
					Resources.TimeTracker.Controllers.TimeEntry.Strings.LastName,
					Resources.TimeTracker.Controllers.TimeEntry.Strings.FirstName,
					Resources.TimeTracker.Controllers.TimeEntry.Strings.Date,
					Resources.TimeTracker.Controllers.TimeEntry.Strings.Duration,
					Resources.TimeTracker.Controllers.TimeEntry.Strings.Project,
					Resources.TimeTracker.Controllers.TimeEntry.Strings.Customer,
					Resources.TimeTracker.Controllers.TimeEntry.Strings.Description));
			try
			{
				foreach (TimeEntryInfo entry in data)
				{
					output.WriteLine(
						string.Format(
							"\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\"",
							entry.LastName,
							entry.FirstName,
							entry.Date.ToShortDateString(),
							entry.Duration,
							projects.Where(x => x.ProjectId == entry.ProjectId).SingleOrDefault().ProjectName ?? string.Empty,
							projects.Where(x => x.ProjectId == entry.ProjectId).SingleOrDefault().CustomerName ?? string.Empty,
							entry.Description));
				}
			}
			catch (Exception)
			{
			}

			output.Flush();
			output.BaseStream.Seek(0, SeekOrigin.Begin);

			return output;
		}
	}
}