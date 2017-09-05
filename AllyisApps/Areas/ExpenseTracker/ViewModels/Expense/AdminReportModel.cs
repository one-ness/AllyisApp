using System;
using System.Collections.Generic;
using AllyisApps.Services;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
    /// <summary>
    /// 
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
        /// Gets or sets the check to show an export button.
        /// </summary>
        public bool ShowExport { get; set; }

        /// <summary>
        /// Gets or sets the manager check.
        /// </summary>
        public bool CanManage { get; set; }

        /// <summary>
        /// Gets or sets the list of user in the subscription
        /// </summary>
        public IEnumerable<SelectListItem> Users { get; internal set; }

        /// <summary>
        /// Gets or sets the list or reports to show.
        /// </summary>
        public IEnumerable<ExpenseReport> Reports { get; internal set; }

        /// <summary>
        /// Gets or sets the Preview reports.
        /// </summary>
        public IEnumerable<ExpenseReport> PreviewReports { get; internal set; }

        /// <summary>
        /// Gets or sets the list of statuses.
        /// </summary>
        public IEnumerable<SelectListItem> Statuses { get; internal set; }

        /// <summary>
        /// Gets or sets the page selections.
        /// </summary>
        public AdminReportSelectionModel Selection { get; set; }
    }

    /// <summary>
    /// 
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
        /// Gets or sets the start date of reports to show.
        /// </summary>
        public DateTime? StartDate { get; internal set; }

        /// <summary>
        /// Gets or sets the end date of reports to show.
        /// </summary>
        public DateTime? EndDate { get; internal set; }
    }
}