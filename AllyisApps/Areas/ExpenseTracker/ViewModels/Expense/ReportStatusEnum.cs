using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
    /// <summary>
    /// Represents the status of a expense report
    /// </summary>
    public enum ReportStatusEnum
    {
        /// <summary>
        /// Report is still pending
        /// </summary>
        Pending = 0,
        /// <summary>
        /// Report has been accepted
        /// </summary>
        Accepted = 1,
        /// <summary>
        /// Report has been rejected
        /// </summary>
        Rejected = 2

    }
}