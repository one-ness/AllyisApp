using AllyisApps.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpenseItemViewModel : BaseViewModel
    {
        /// <summary>
        /// The reportId
        /// </summary>
        public int ReportId { get; set; }

        /// <summary>
        /// The user id of the user who submitted the report
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The user name of the user who submitted the report
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// Name of the report
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// Date the report was submitted
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:MM-dd-YYYY")]
        public DateTime SubmittedDate { get; set; }

        /// <summary>
        /// Status of the report
        /// </summary>
        public ExpenseStatusEnum Status { get; set; }

        /// <summary>
        /// the dollar amount that is reported
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The reason for the expense being submitted.
        /// </summary>
        public string Reason { get; set; }

		/// <summary>
		/// List of Items contained in report
		/// </summary>
		public List<ExpenseItem> Items { get; set; }
	}
}