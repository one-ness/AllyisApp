//------------------------------------------------------------------------------
// <copyright file="ViewReportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Areas.TimeTracker.Models;
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
		/// Submits form to view data.
		/// </summary>
		/// <param name="viewDataButton">The value of the button used to submit the form.</param>
		/// <param name="organizationId">The Organization's Id.</param>
		/// <param name="userSelect">Array of selected user Ids.</param>
		/// <param name="dateRangeStart">The beginning of the date range(nullable).</param>
		/// <param name="dateRangeEnd">The end of the date range (nullable).</param>
		/// <param name="showExport">Export button visibility.</param>
		/// <param name="customerSelect">The Customer's id (not required).</param>
		/// <param name="pageNum">The page of results to view.</param>
		/// <param name="projectSelect">The project's id (not required).</param>
		/// <returns>The data in a view dependent on the button's value.</returns>
		public ActionResult ViewReport(string viewDataButton, int organizationId, List<int> userSelect, DateTime? dateRangeStart, DateTime? dateRangeEnd, bool showExport, int customerSelect, int pageNum, int projectSelect = 0)
		{
			switch (viewDataButton)
			{
				case "Preview":
					{
						ReportSelectionModel reportVMselect = new ReportSelectionModel
						{
							CustomerId = customerSelect,
							EndDate = dateRangeEnd,
							Page = pageNum,
							ProjectId = projectSelect,
							StartDate = dateRangeStart,
							Users = userSelect ?? (List<int>)this.TempData["USelect"]
						};

						// when selecting page
						if (userSelect == null)
						{
							reportVMselect.Users = (List<int>)this.TempData["USelect"];
						}
						else
						{
							reportVMselect.Users = userSelect;
						}

						ReportViewModel reportVM = this.ConstructReportViewModel(UserContext.UserId, organizationId, AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers), showExport, reportVMselect);

						DataExportViewModel dataVM = this.ConstructDataExportViewModel(organizationId, reportVMselect.Users, dateRangeStart, dateRangeEnd, projectSelect, customerSelect);

						dataVM.PageTotal = SetPageTotal(dataVM.Data, reportVM.PreviewPageSize, pageNum); // must set PageTotal first and seperately like this.
						dataVM.PreviewData = SetPreviewData(dataVM.Data, reportVM.PreviewPageSize, pageNum);

						float total = (from d in dataVM.Data select d.Duration).Sum();
						int dataCount = dataVM.PreviewData.Count();

						reportVM.PreviewTotal = string.Format("{0} hours total", total);

						IEnumerable<CompleteProjectInfo> orgProjects = OrgService.GetProjectsByOrganization(organizationId);
						if (dataCount > 0)
						{
							IList<TablePreviewEntry> pEntries = new List<TablePreviewEntry>();
							foreach (TimeEntryInfo data in dataVM.PreviewData)
							{
								CompleteProjectInfo orgProj = orgProjects.Where(o => o.ProjectId == data.ProjectId).SingleOrDefault();
								TablePreviewEntry previewData = new TablePreviewEntry
								{
									CustomerName = orgProj.CustomerName,
									ProjectName = orgProj.ProjectName,
									TimeEntry = data
								};
								pEntries.Add(previewData);
							}

							reportVM.PreviewEntries = pEntries;
							reportVM.PreviewMessage = string.Empty;
							reportVM.PreviewPageTotal = dataVM.PageTotal;
							reportVM.PreviewPageNum = dataCount <= reportVM.PreviewPageSize
												? 1
												: 1 + ((dataCount - 1) / reportVM.PreviewPageSize);
						}
						else
						{
							reportVM.PreviewEntries = null;
							reportVM.PreviewMessage = "There is no data preview to display.";
							reportVM.PreviewPageTotal = 1;
							reportVM.PreviewPageNum = 1;
						}

						this.TempData["RVM"] = reportVM;
						return this.RedirectToAction("Report");
					}

				case "Export":
					{
						return this.ExportReport(organizationId, userSelect, dateRangeStart, dateRangeEnd, customerSelect, projectSelect);
					}

				default:
					{
						return this.RedirectToAction("Report", new { organizationId = organizationId });
					}
			}
		}
	}
}