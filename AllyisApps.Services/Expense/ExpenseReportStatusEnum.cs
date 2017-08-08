using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
    /// <summary>
    /// Represents the status of a expense report
    /// </summary>
    public enum ExpenseStatusEnum
    {
        /// <summary>
        /// Expense is still in being drafted.
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Expense is new and not being reviewed.
        /// </summary>
        New = 1,

        /// <summary>
        /// Expense is in the process of being reviewed.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// Expense has been approved
        /// </summary>
        Approved = 3,

        /// <summary>
        /// Expense has been rejected
        /// </summary>
        Rejected = 4,

        /// <summary>
        /// Expense has been paid out.
        /// </summary>
        Paid = 5

    }
}
