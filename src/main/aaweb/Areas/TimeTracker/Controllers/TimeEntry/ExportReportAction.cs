//------------------------------------------------------------------------------
// <copyright file="ExportReportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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
		/// Exports a CSV report file.
		/// </summary>
		/// <param name="organizationId">The Organization's Id.</param>
		/// <param name="userId">Array of user Ids.</param>
		/// <param name="dateRangeStart">The beginning of the date range(nullable).</param>
		/// <param name="dateRangeEnd">The end of the date range (nullable).</param>
		/// <param name="customerId">The Customer's id (not required).</param>
		/// <param name="projectId">The project's id (not required).</param>
		/// <returns>CSV report File.</returns>
		public ActionResult ExportReport(int organizationId, List<int> userId, DateTime? dateRangeStart = null, DateTime? dateRangeEnd = null, int customerId = 0, int projectId = 0)
		{
			if (userId == null)
			{
				userId = new List<int> { Convert.ToInt32(UserContext.UserId) };
			}

			// Permissions checks
			if (userId.Count == 1 && userId[0] == Convert.ToInt32(UserContext.UserId))
			{
				// if (Permissions.Cannot(ProductAction.TimeTrackerCreateReportSelf, OrganizationId, TimeTrackerID))
				if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditSelf))
				{
					throw new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.UnauthorizedReports);
				}
			}
			else
			{
				// if (Permissions.Cannot(ProductAction.TimeTrackerCreateReportOthers, OrganizationId, TimeTrackerID))
				if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers))
				{
					throw new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.UnauthorizedReportsOtherUser);
				}
			}

			// Authorized for report exporting
			DataExportViewModel model = this.ConstructDataExportViewModel(organizationId, userId, dateRangeStart, dateRangeEnd, projectId, customerId);
			if (model.Data.Count() == 0)
			{
				Notifications.Add(new AllyisApps.Core.Alert.BootstrapAlert(Resources.TimeTracker.Controllers.TimeEntry.Strings.NoDataToExport, Variety.Warning));
				return this.RedirectToAction(ActionConstants.Report, new { organizationId = organizationId });
			}

			model.Output = PrepareCSVExport(model.Data, model.Projects);
			return this.File(model.Output.BaseStream, "text/csv", "export.csv");
		}
	}
}