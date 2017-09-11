using AllyisApps.Services;
using System;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// View model for the expense history.
	/// </summary>
	public class ExpenseHistoryViewModel
	{
		/// <summary>
		/// Gets or sets the reviewer name.
		/// </summary>
		public string Reviewer { get; set; }

		/// <summary>
		/// Gets or sets the Status of the report history item.
		/// </summary>
		public ExpenseStatusEnum Status { get; set; }

		/// <summary>
		/// Gets or sets the submitted date.
		/// </summary>
		public DateTime Submitted { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		public string Text { get; set; }
	}
}