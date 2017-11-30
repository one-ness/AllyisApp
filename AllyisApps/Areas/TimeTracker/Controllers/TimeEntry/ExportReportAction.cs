//------------------------------------------------------------------------------
// <copyright file="ExportReportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Exports a CSV report file.
		/// </summary>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="userId">Array of user Ids.</param>
		/// <param name="dateRangeStart">The beginning of the date range(nullable).</param>
		/// <param name="dateRangeEnd">The end of the date range (nullable).</param>
		/// <param name="customerId">The Customer's id (not required).</param>
		/// <param name="projectId">The project's id (not required).</param>
		/// <returns>CSV report File.</returns>
		public async Task<ActionResult> ExportReport(int subscriptionId, int organizationId, List<int> userId, DateTime? dateRangeStart = null, DateTime? dateRangeEnd = null, int customerId = 0, int projectId = 0)
		{
			if (userId == null)
			{
				AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);
				userId = new List<int> { AppService.UserContext.UserId };
			}
			var prep = await AppService.PrepareCSVExport(organizationId, userId, dateRangeStart, dateRangeEnd, projectId, customerId);
			return File(prep.BaseStream, "text/csv", "export.csv");
		}
	}
}