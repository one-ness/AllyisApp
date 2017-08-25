﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// The expense discription
        /// </summary>
        public string ItemDescription { get; set; }

        /// <summary>
        /// The transaction date
        /// </summary>
		[Required(ErrorMessage = "Please enter the date of transaction.")]
        public string TransactionDate { get; set; }

        /// <summary>
        /// The amount of the expense
        /// </summary>
		[Required(ErrorMessage = "Please enter the amount.")]
        public decimal Amount { get; set; }

        /// <summary>
        /// The expenses report id.
        /// </summary>
        public int ExpenseReportId { get; set; }

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

		/// <summary>
		/// Index of item in report
		/// </summary>
		public int Index { get; set; }
    }
}
