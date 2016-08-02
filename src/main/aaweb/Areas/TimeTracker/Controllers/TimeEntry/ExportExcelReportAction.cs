//------------------------------------------------------------------------------
// <copyright file="ExportExcelReportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;

using AllyisApps.Areas.TimeTracker.Models;
using AllyisApps.Core;
using AllyisApps.Core.Alert;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// Exports a Excel report file.
		/// </summary>
		/// <param name="organizationId">The Organization's Id.</param>
		/// <param name="userId">Array of user Ids.</param>
		/// <param name="dateRangeStart">The beginning of the date range(nullable).</param>
		/// <param name="dateRangeEnd">The end of the date range (nullable).</param>
		/// <param name="customerId">The Customer's id (not required).</param>
		/// <param name="projectId">The project's id (not required).</param>
		/// <returns>An excel extract of the requested data.</returns>
		public ActionResult ExportExcelReport(int organizationId, List<int> userId, DateTime? dateRangeStart = null, DateTime? dateRangeEnd = null, int customerId = 0, int projectId = 0)
		{
			/* NOT CURRENTLY IN USE */

			if (userId.Count == 1 && userId[0] == Convert.ToInt32(UserContext.UserId))
			{
				if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditSelf))
				{
					throw new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.UnauthorizedReports);
				}
			}
			else
			{
				if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers))
				{
					throw new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.UnauthorizedReportsOtherUser);
				}
			}

			DataExportViewModel model = this.ConstructDataExportViewModel(organizationId, userId, dateRangeStart, dateRangeEnd, projectId, customerId);
			////model.CreateExcelFileOutput();
			PrepareExcelExport(model.Data, model.Projects, 1, 0);
			Response.ClearContent();
			Response.AddHeader("content-disposition", "attachment;filename=export.xls");
			Response.ContentType = "application/vnd.ms-excel";

			if (model.ExcelOutput.ToString() != "<div>\r\n\r\n</div>")
			{
				Response.Output.Write(model.ExcelOutput.ToString());
				Response.End();
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.NoDataToExport, Variety.Warning));
			}

			return this.RedirectToAction(ActionConstants.Report, new { organizationId = organizationId });
		}
	}
}