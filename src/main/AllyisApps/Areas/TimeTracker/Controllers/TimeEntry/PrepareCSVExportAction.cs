//------------------------------------------------------------------------------
// <copyright file="PrepareCSVExportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

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
		/// <param name="data">The TimeEntryInfo to be exported.</param>
		/// <param name="projects">The projects being pulled from, for getting project name and customers.</param>
		/// <returns>The stream writer.</returns>
		[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:ClosingParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
		public StreamWriter PrepareCSVExport(IEnumerable<TimeEntryInfo> data, IEnumerable<CompleteProjectInfo> projects)
		{
			StreamWriter output = new StreamWriter(new MemoryStream());
			output.WriteLine(
				string.Format(
                    "\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\"",
					Resources.TimeTracker.Controllers.TimeEntry.Strings.LastName,
					Resources.TimeTracker.Controllers.TimeEntry.Strings.FirstName,
                    "Employee ID",
					Resources.TimeTracker.Controllers.TimeEntry.Strings.Date,
					Resources.TimeTracker.Controllers.TimeEntry.Strings.Duration,
					Resources.TimeTracker.Controllers.TimeEntry.Strings.Project,
                    "Project ID",
					Resources.TimeTracker.Controllers.TimeEntry.Strings.Customer,
                    "Customer ID",
					Resources.TimeTracker.Controllers.TimeEntry.Strings.Description));
			try
			{
				foreach (TimeEntryInfo entry in data)
				{
                    var project = projects.Where(x => x.ProjectId == entry.ProjectId).SingleOrDefault();
                    output.WriteLine(
                        string.Format(
                            "\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\"",
                            entry.LastName,
                            entry.FirstName,
                            this.Service.GetOrganizationMemberList(this.UserContext.ChosenOrganizationId).Where(u => u.UserId == entry.UserId).FirstOrDefault().EmployeeId,
                            entry.Date.ToShortDateString(),
                            entry.Duration,
                            project.ProjectName ?? string.Empty,
                            project.ProjectOrgId ?? string.Empty,
                            project.CustomerName ?? string.Empty,
                            this.Service.GetCustomerList(this.UserContext.ChosenOrganizationId).Where(c => c.Name == project.CustomerName).FirstOrDefault().CustomerOrgId ?? string.Empty,
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