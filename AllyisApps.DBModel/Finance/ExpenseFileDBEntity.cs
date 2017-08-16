using System;

namespace AllyisApps.DBModel.Finance
{
	/// <summary>
	/// Represents the Expense File table in the database.
	/// </summary>
	public class ExpenseFileDBEntity
    {
        /// <summary>
        /// Gets or sets the Expense File's ID.
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Gets or sets the expense file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the Expense File's description.
        /// </summary>
        public string Url { get; set; }

		/// <summary>
		/// Gets or sets the Expense File's report Id.
		/// </summary>
		public int ExpenseReportId { get; set; }
    }

}
