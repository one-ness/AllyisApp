using System;

namespace AllyisApps.DBModel.Finance
{
	/// <summary>
	/// Represents the Expense Item table in the database.
	/// </summary>
	public class ExpenseReportDBEntity
    {
        /// <summary>
        /// Gets or sets the Expense Report's ID.
        /// </summary>
        public int ExpenseReportId { get; set; }

        /// <summary>
        /// Gets or sets the Expense Report's Title.
        /// </summary>
        public string ReportTitle { get; set; }

        /// <summary>
        /// Gets or sets the Expense Report's date.
        /// </summary>
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// Gets or sets the Expense Reports's parent organization Id.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the Expense Report's parent User ID.
        /// </summary>
        public int SubmittedById { get; set; }

        /// <summary>
        /// Gets or sets the Expense Report's status.
        /// </summary>
        public int ReportStatus { get; set; }
		
        /// <summary>
        /// Gets or sets description of why report is needed or created.
        /// </summary>
        public string BusinessJustification { get; set; }
        
        /// <summary>
        /// Gets or sets the Expense Report's creation date.
        /// </summary>
        public DateTime ExpenseReportCreatedUtc { get; set; }

        /// <summary>
        /// Gets or sets the Expense Report's last-modified-on date.
        /// </summary>
        public DateTime ExpenseReportModifiedUtc { get; set; }
        
    }

}
