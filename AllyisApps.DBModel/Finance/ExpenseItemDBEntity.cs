using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel.Finance
{
    /// <summary>
    /// Represents the Expense Item table in the database.
    /// </summary>
    public class ExpenseItemDBEntity
    {
        /// <summary>
        /// Gets or sets the Expense Item's ID.
        /// </summary>
        public int ExpenseItemId { get; set; }

        /// <summary>
        /// Gets or sets the Expense Item's description.
        /// </summary>
        public string ItemDescription { get; set; }

        /// <summary>
        /// Gets or sets the Expense Item's date of transaction.
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets the Expense Item's currency ammount.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the Expense Item's Report ID.
        /// </summary>
        public int ExpenseReportId { get; set; }

        /// <summary>
        /// Gets or sets the Expense Item's Account ID.
        /// </summary>
        public int AccountId { get; set; }


        /// <summary>
        /// Gets or sets whether the Expense Item can be billed to the associated customer.
        /// </summary>
        public bool IsBillableToCustomer { get; set; }
        
        /// <summary>
        /// Gets or sets the Expense Item's creation date.
        /// </summary>
        public DateTime CreatedUtc { get; set; }

        /// <summary>
        /// Gets or sets the Expense Item's last-modified-on date.
        /// </summary>
        public DateTime ModifiedUtc { get; set; }
        
    }

}
