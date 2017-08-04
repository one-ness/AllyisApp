using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpenseReportModel : BaseViewModel
    {
        /// <summary>
        /// The reportId
        /// </summary>
        public int ReportId { get; set; }

        /// <summary>
        /// The user id of the user who submitted the report
        /// </summary>
        public int SubmittedByUserId { get; set; }

        /// <summary>
        /// The user name of the user who submitted the report
        /// </summary>
        public string SubmittedByUserName { get; set; }
        
        /// <summary>
        /// Name of the report
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// Date the report was submitted
        /// </summary>
        public DateTime SubmittedDate { get; set; }

        /// <summary>
        /// Status of the report
        /// </summary>
        public ReportStatusEnum Status { get; set; }

        /// <summary>
        /// the dollar amount that is reported
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// The reason for the expense being submitted.
        /// </summary>
        public string Reason { get; set; }

    }
}