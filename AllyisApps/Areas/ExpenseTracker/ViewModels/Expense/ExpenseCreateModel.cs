using System;
using System.Collections.Generic;
using AllyisApps.Services;

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
		/// Gets or sets the Expense Report.
		/// </summary>
		public ExpenseReport Report { get; set; }

		/// <summary>
		/// Gets or sets the Start Date.
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the Expense Items
		/// </summary>
		public IEnumerable<ExpenseItem> Items { get; set; }
	}
}