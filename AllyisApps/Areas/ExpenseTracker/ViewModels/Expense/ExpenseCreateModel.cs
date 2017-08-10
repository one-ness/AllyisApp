using System;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// Model for Create Expense Report View.
	/// </summary>
	public class ExpenseCreateModel
	{
		/// <summary>
		/// Gets or sets Subscription Id.
		/// </summary>
		public int SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets Current User Id.
        /// </summary>
        public int CurrentUser { get; set; }

        /// <summary>
        /// Gets or sets Start Date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets Expense Items.
        /// </summary>
        public List<ExpenseItemViewModel> Items { get; set; }
	}
}