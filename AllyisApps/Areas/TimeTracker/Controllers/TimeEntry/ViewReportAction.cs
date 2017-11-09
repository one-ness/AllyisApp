//------------------------------------------------------------------------------
// <copyright file="ViewReportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

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
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="dateRangeStart">The beginning of the date range(nullable).</param>
		/// <param name="dateRangeEnd">The end of the date range (nullable).</param>
		/// <param name="showExport">Export button visibility.</param>
		/// <param name="customerSelect">The Customer's id (not required).</param>
		/// <param name="pageNum">The page of results to view.</param>
		/// <param name="projectSelect">The project's id (not required).</param>
		/// <returns>The data in a view dependent on the button's value.</returns>
		public async Task<ActionResult> ViewReport(string viewDataButton, List<int> userSelect, int subscriptionId, int organizationId, DateTime? dateRangeStart, DateTime? dateRangeEnd, bool showExport, int customerSelect, int pageNum, int projectSelect = 0)
		{
			if (viewDataButton.Equals(Strings.Preview))
			{
				return await PreviewReport(userSelect, subscriptionId, organizationId, dateRangeStart, dateRangeEnd, showExport, customerSelect, pageNum, projectSelect);
			}

			if (viewDataButton.Equals(Strings.Export))
			{
				return await ExportReport(subscriptionId, organizationId, userSelect, dateRangeStart, dateRangeEnd, customerSelect, projectSelect);
			}

			return RedirectToAction(ActionConstants.Report);
		}

		/// <summary>
		/// Returns a report view with preview data filled out.
		/// </summary>
		/// <param name="userSelect">Array of selected user Ids.</param>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="dateRangeStart">The beginning of the date range(nullable).</param>
		/// <param name="dateRangeEnd">The end of the date range (nullable).</param>
		/// <param name="showExport">Export button visibility.</param>
		/// <param name="customerSelect">The Customer's id (not required).</param>
		/// <param name="pageNum">The page of results to view.</param>
		/// <param name="projectSelect">The project's id (not required).</param>
		/// <returns>Returns a report view with preview data filled out.</returns>
		public async Task<ActionResult> PreviewReport(List<int> userSelect, int subscriptionId, int organizationId, DateTime? dateRangeStart, DateTime? dateRangeEnd, bool showExport, int customerSelect, int pageNum, int projectSelect = 0)
		{
			var reportVMselect = new ReportSelectionModel
			{
				CustomerId = customerSelect,
				EndDate = dateRangeEnd,
				Page = pageNum,
				ProjectId = projectSelect,
				StartDate = dateRangeStart,
				Users = userSelect ?? (List<int>)TempData["USelect"]
			};

			var infosTask = AppService.GetReportInfo(subscriptionId);
			var subNameTask = AppService.GetSubscriptionName(subscriptionId);

			await Task.WhenAll(infosTask, subNameTask);

			ReportInfo infos = infosTask.Result;
			string subName = subNameTask.Result;

			bool canEditOthers = AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId, false);
			ReportViewModel reportVM = ConstructReportViewModel(AppService.UserContext.UserId, organizationId, canEditOthers, infos.Customers, infos.CompleteProject, showExport, reportVMselect);
			reportVM.SubscriptionName = subName;

			DataExportViewModel dataVM;
			try
			{
				dataVM = await ConstructDataExportViewModel(subscriptionId, organizationId, reportVMselect.Users, dateRangeStart, dateRangeEnd, projectSelect, customerSelect);
			}
			catch (Exception ex)
			{
				// Update failure
				Notifications.Add(new BootstrapAlert($"{Strings.CannotCreateReport} {ex.Message}", Variety.Danger));
				return RedirectToAction(ActionConstants.Report, ControllerConstants.TimeEntry);
			}

			dataVM.PageTotal = SetPageTotal(dataVM.Data, reportVM.PreviewPageSize); // must set PageTotal first and seperately like this.
			dataVM.PreviewData = SetPreviewData(dataVM.Data, reportVM.PreviewPageSize, pageNum).ToList();

			reportVM.PreviewTotal = $"{dataVM.Data.Select(d => d.Duration).Sum()} {Strings.HoursTotal}";
			reportVM.PreviewEntries = dataVM.PreviewData.Any()
				? dataVM.PreviewData
					.Select(data => new
					{
						data,
						orgProj = data.ProjectId == 0 ? AppService.GetProject(0) : infos.CompleteProject.SingleOrDefault(o => o.ProjectId == data.ProjectId)
					})
					.Select(t => new TablePreviewEntry
					{
						CustomerName = t.orgProj.owningCustomer?.CustomerName,
						ProjectName = t.orgProj.ProjectName,
						TimeEntry = t.data
					})
					.ToList()
				: null;
			reportVM.PreviewMessage = dataVM.PreviewData.Any() ? string.Empty : Strings.NoDataPreview;
			reportVM.PreviewPageTotal = SetPageTotal(dataVM.PreviewData, reportVM.PreviewPageSize);
			reportVM.PreviewPageNum = SetPageTotal(dataVM.PreviewData, reportVM.PreviewPageSize);


			TempData["RVM"] = reportVM;
			return RedirectToAction(ActionConstants.Report);
		}

		/// <summary>
		/// Sets the PreviewData for DataExportViewModel.
		/// </summary>
		/// <param name="data">The preview data to set.</param>
		/// <param name="pageSize">The page size.</param>
		/// <param name="page">The page.</param>
		/// <returns>A list of time entry info objects.</returns>
		public IEnumerable<TimeEntryViewModel> SetPreviewData(List<TimeEntryViewModel> data, int pageSize = 0, int page = 1)
		{
			int skipNum = pageSize * (page - 1);
			int limit = pageSize == 0 ? data.Count : pageSize;

			// only process data values for current page
			IEnumerable<TimeEntryViewModel> previewData = data.Select(p => p).Skip(skipNum).Take(limit);

			return previewData;
		}

		/// <summary>
		/// Sets the pageTotal for a DataExportViewModel in preperation for SetPreviewdata.
		/// </summary>
		/// <param name="data">TimeEntry  needed to find total page size.</param>
		/// <param name="pageSize">Total size of preivew data.</param>
		/// <returns>The page total.</returns>
		public int SetPageTotal(IList<TimeEntryViewModel> data, int pageSize)
		{
			return Math.Min(1, 1 + (data.Count - 1) / pageSize);
		}
	}
}