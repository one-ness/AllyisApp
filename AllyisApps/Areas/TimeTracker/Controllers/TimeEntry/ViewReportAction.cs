//------------------------------------------------------------------------------
// <copyright file="ViewReportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Submits form to view data.
		/// </summary>
		/// <param name="viewDataButton">The value of the button used to submit the form.</param>
		/// <param name="userSelect">Array of selected user Ids.</param>
		/// <param name="subscriptionId">The subscription's Id</param>
		/// <param name="organizationId">The organization's Id</param>
		/// <param name="dateRangeStart">The beginning of the date range(nullable).</param>
		/// <param name="dateRangeEnd">The end of the date range (nullable).</param>
		/// <param name="showExport">Export button visibility.</param>
		/// <param name="customerSelect">The Customer's id (not required).</param>
		/// <param name="pageNum">The page of results to view.</param>
		/// <param name="projectSelect">The project's id (not required).</param>
		/// <returns>The data in a view dependent on the button's value.</returns>
		public ActionResult ViewReport(string viewDataButton, List<int> userSelect, int subscriptionId, int organizationId, int? dateRangeStart, int? dateRangeEnd, bool showExport, int customerSelect, int pageNum, int projectSelect = 0)
		{
			if (viewDataButton.Equals(Resources.Strings.Preview))
			{
				ReportSelectionModel reportVMselect = new ReportSelectionModel
				{
					CustomerId = customerSelect,
					EndDate = dateRangeEnd.Value,
					Page = pageNum,
					ProjectId = projectSelect,
					StartDate = dateRangeStart.Value,
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

				var infos = AppService.GetReportInfo(subscriptionId);
				UserSubscription subInfo = null;
				this.AppService.UserContext.UserSubscriptions.TryGetValue(subscriptionId, out subInfo);

				bool canEditOthers = this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId, false);
				ReportViewModel reportVM = this.ConstructReportViewModel(this.AppService.UserContext.UserId, organizationId, canEditOthers, infos.Item1, infos.Item2, showExport, reportVMselect);
				reportVM.SubscriptionName = subInfo.SubscriptionName;

				DataExportViewModel dataVM = null;
				try
				{
					dataVM = this.ConstructDataExportViewModel(subscriptionId, organizationId, reportVMselect.Users, AppService.GetDateTimeFromDays(dateRangeStart.Value), AppService.GetDateTimeFromDays(dateRangeEnd.Value), projectSelect, customerSelect);
				}
				catch (Exception ex)
				{
					string message = Resources.Strings.CannotCreateReport;
					if (ex.Message != null)
					{
						message = string.Format("{0} {1}", message, ex.Message);
					}

					//Update failure
					Notifications.Add(new BootstrapAlert(message, Variety.Danger));
					return this.RedirectToAction(ActionConstants.Report, ControllerConstants.TimeEntry);
				}

				dataVM.PageTotal = SetPageTotal(dataVM.Data, reportVM.PreviewPageSize, pageNum); // must set PageTotal first and seperately like this.
				dataVM.PreviewData = SetPreviewData(dataVM.Data, reportVM.PreviewPageSize, pageNum);

				float total = (from d in dataVM.Data select d.Duration).Sum();
				int dataCount = dataVM.PreviewData.Count();

				reportVM.PreviewTotal = string.Format("{0} {1}", total, Resources.Strings.HoursTotal);

				IEnumerable<CompleteProjectInfo> orgProjects = infos.Item2;
				CompleteProjectInfo defaultProject = AppService.GetProject(0);
				if (dataCount > 0)
				{
					IList<TablePreviewEntry> pEntries = new List<TablePreviewEntry>();
					foreach (TimeEntryInfo data in dataVM.PreviewData)
					{
						CompleteProjectInfo orgProj = data.ProjectId == 0 ? defaultProject : orgProjects.Where(o => o.ProjectId == data.ProjectId).SingleOrDefault();
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
					reportVM.PreviewMessage = Resources.Strings.NoDataPreview;
					reportVM.PreviewPageTotal = 1;
					reportVM.PreviewPageNum = 1;
				}

				this.TempData["RVM"] = reportVM;
				return this.RedirectToAction(ActionConstants.Report);
			}
			else if (viewDataButton.Equals(Resources.Strings.Export))
			{
				return this.ExportReport(subscriptionId, organizationId, userSelect, AppService.GetDateTimeFromDays(dateRangeStart.Value), AppService.GetDateTimeFromDays(dateRangeEnd.Value), customerSelect, projectSelect);
			}

			return this.RedirectToAction(ActionConstants.Report);
		}

		/// <summary>
		/// Sets the PreviewData for DataExportViewModel.
		/// </summary>
		/// <param name="data">The preview data to set.</param>
		/// <param name="pageSize">The page size.</param>
		/// <param name="page">The page.</param>
		/// <returns>A list of time entry info objects.</returns>
		public IEnumerable<TimeEntryInfo> SetPreviewData(IEnumerable<TimeEntryInfo> data, int pageSize = 0, int page = 1)
		{
			int skipNum = pageSize * (page - 1);
			int limit = pageSize == 0 ? data.Count() : pageSize;

			// only process data values for current page
			IEnumerable<TimeEntryInfo> previewData = (from p in data select p).Skip(skipNum).Take(limit);

			return previewData;
		}

		/// <summary>
		/// Sets the pageTotal for a DataExportViewModel in preperation for SetPreviewdata.
		/// </summary>
		/// <param name="data">TimeEntryInfo  needed to find total page size.</param>
		/// <param name="pageSize">Total size of preivew data.</param>
		/// <param name="page">Offset page amount.</param>
		/// <returns>The page total.</returns>
		public int SetPageTotal(IEnumerable<TimeEntryInfo> data, int pageSize = 0, int page = 1)
		{
			int skipNum = pageSize * (page - 1);
			int pageTotal = 0;
			if (data.Count() == 0)
			{
				pageTotal = 1;
			}
			else
			{
				// Count equals pageSize --> fits on 1 page
				pageTotal = 1 + ((data.Count() - 1) / pageSize);
			}

			return pageTotal;
		}
	}
}
