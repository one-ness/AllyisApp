using System;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// Information needed for the Expense report view.
	/// </summary>
	public class ExpenseIndexViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the Subscription Id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the current user's id.
		/// </summary>
		public int CurrentUser { get; set; }

		/// <summary>
		/// Gets or sets the start date for the expense reports.
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the end date for the expense reports.
		/// </summary>
		public DateTime EndDate { get; set; }

		/// <summary>
		/// Gets a value indicating whether a user can manage the items.
		/// </summary>
		public bool CanManage { get; internal set; }

		/// <summary>
		/// Gets or sets a List of expense reports.
		/// </summary>
		public List<ExpenseItemViewModel> Reports { get; set; }

		/// <summary>
		/// Gets or sets the type of product.
		/// </summary>
		public int ProductRole { get; set; }
	}
}