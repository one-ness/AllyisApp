using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
    public class ExpenseItem
    {
        /// <summary>
		/// The expense item id
		/// </summary>
		public int ExpenseItemId { get; set; }

        /// <summary>
        /// The expense item name
        /// </summary>
        public string ExpenseItemName { get; set; }

        /// <summary>
        /// The expense discription
        /// </summary>
        public string ItemDiscription { get; set; }

        /// <summary>
        /// The transaction date
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// The amount of the expense
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The expenses report id.
        /// </summary>
        public int ExpenseReportId { get; set; }

        /// <summary>
        /// The expense report status.
        /// </summary>
        public ExpenseStatusEnum ExpenseReportStatus { get; set; }

        /// <summary>
        /// the user id associated with the Expense
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Flag for i the epense can be billed to a customer
        /// </summary>
        public bool IsBillableToCustomer { get; set; }

        /// <summary>
        /// When the item was created
        /// </summary>
        public DateTime ExpenseItemCreatedUtc { get; set; }

        /// <summary>
        /// When it was last modified.
        /// </summary>
        public DateTime ExpenseItemModifiedUtc { get; set; }
    }
}
