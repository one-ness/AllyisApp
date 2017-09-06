using System.Collections.Generic;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// Model for Pending Expense Report View.
	/// </summary>
	public class ExpensePendingModel
	{
		/// <summary>
		/// Gets or sets the pending reports.
		/// </summary>
		public IEnumerable<ExpenseItemViewModel> PendingReports { get; set; }
	}
}