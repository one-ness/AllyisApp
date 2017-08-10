using System;
using System.Collections.Generic;
using AllyisApps.Services;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
    /// <summary>
    /// The report view model.
    /// </summary>
    public class ReportViewModel
    {
        /// <summary>
        /// Gets or sets the report id.
        /// </summary>
        public int ReportId { get; set; }

        /// <summary>
        /// Gets or sets the current user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the report title.
        /// </summary>
        public string ReprortTitle { get; set; }

        /// <summary>
        /// Gets or sets the report date.
        /// </summary>
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// Gets or sets the submitting user name.
        /// </summary>
        public string SubmittedBy { get; set; }

        /// <summary>
        /// Gets or sets the report status.
        /// </summary>
        public ExpenseStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the expense justification.
        /// </summary>
        public string Justification { get; set; }

        /// <summary>
        /// Gets or sets the report creation date.
        /// </summary>
        public DateTime CreatedUtc { get; set; }

        /// <summary>
        /// Gets or sets the report last modified time.
        /// </summary>
        public DateTime ModifiedUtc { get; set; }

        /// <summary>
        /// Gets or sets the associated expense items.
        /// </summary>
        public IEnumerable<ExpenseItem> Expenses { get; set; }
    }
}