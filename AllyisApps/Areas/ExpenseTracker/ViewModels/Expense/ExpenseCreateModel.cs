using AllyisApps.ViewModels;
using System;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// Model for Create Expense Report View
	/// </summary>
	public class ExpenseCreateModel
	{
		/// <summary>
		/// Subscription Id
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Current User Id
		/// </summary>
		public int CurrentUser { get; set; }

		/// <summary>
		/// Start Date
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Expense Items
		/// </summary>
		public List<ExpenseItemViewModel> Items { get; set; }
	}
}