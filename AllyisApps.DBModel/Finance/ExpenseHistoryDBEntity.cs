using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel.Finance
{
    /// <summary>
    /// Expense History DB Entity.
    /// </summary>
    public class ExpenseHistoryDBEntity
    {
        /// <summary>
        /// Gets or sets the History Id.
        /// </summary>
        public int HistoryId { get; set; }

        /// <summary>
        /// Gets or sets the expense report id.
        /// </summary>
        public int ExpenseReportId { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the created time.
        /// </summary>
        public DateTime CreatedUtc { get; set; }

        /// <summary>
        /// Gets or sets the modified time.
        /// </summary>
        public DateTime ModifiedUtc { get; set; }
    }
}
