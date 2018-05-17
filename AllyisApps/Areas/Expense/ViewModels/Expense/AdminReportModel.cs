using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// The AdminReport Model.
	/// </summary>
	public class AdminReportModel
	{
		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the subscription id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the subscription name.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets the organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to show the export button.
		/// </summary>
		public bool ShowExport { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user is a manager.
		/// </summary>
		public bool CanManage { get; set; }

		/// <summary>
		/// Gets the list of user in the subscription.
		/// </summary>
		public IEnumerable<SelectListItem> Users { get; internal set; }

		/// <summary>
		/// Gets the list or reports to show.
		/// </summary>
		public IEnumerable<ExpenseReportViewModel> Reports { get; internal set; }

		/// <summary>
		/// Gets the Preview reports.
		/// </summary>
		public IEnumerable<ExpenseReportViewModel> PreviewReports { get; internal set; }

		/// <summary>
		/// Gets the list of statuses.
		/// </summary>
		public IEnumerable<SelectListItem> Statuses { get; internal set; }

		/// <summary>
		/// Gets or sets the page selections.
		/// </summary>
		public AdminReportSelectionModel Selection { get; set; }
	}

	/// <summary>
	/// The Admin report selection model.
	/// </summary>
	public class AdminReportSelectionModel
	{
		/// <summary>
		/// Gets or sets the selected user.
		/// </summary>
		public List<int> SelectedUsers { get; set; }

		/// <summary>
		/// Gets or sets the selected status.
		/// </summary>
		public List<int> Status { get; set; }

		/// <summary>
		/// Gets the start date of reports to show.
		/// </summary>
		public DateTime? StartDate { get; internal set; }

		/// <summary>
		/// Gets the end date of reports to show.
		/// </summary>
		public DateTime? EndDate { get; internal set; }
	}
}