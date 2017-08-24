using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
    public class ExpenseReport
    {
        /// <summary>
        /// The expense report id.
        /// </summary>
        public int ExpenseReportId { get; set; }

        /// <summary>
        /// The expense report title
        /// </summary>
		[Required]
        public string ReportTitle { get; set; }

        /// <summary>
        /// the organization id of the expense
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Te submiting user id.
        /// </summary>
        public int SubmittedById { get; set; }

        /// <summary>
        /// The report status
        /// </summary>
        public int ReportStatus { get; set; }

        /// <summary>
        /// The business justification
        /// </summary>
        public string BusinessJustification { get; set; }

        /// <summary>
        /// The date created
        /// </summary>
        public DateTime CreatedUtc { get; set; }

        /// <summary>
        /// The last date modified
        /// </summary>
        public DateTime ModifiedUtc { get; set; }

		/// <summary>
		/// The date submitted
		/// </summary>
		public DateTime SubmittedUtc { get; set; }
	}
}
