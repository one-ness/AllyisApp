using System.Collections.Generic;
using AllyisApps.DBModel.Finance;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.ViewModels.Expense
{
	/// <summary>
	/// View model for Expense Items.
	/// </summary>
	public class ExpenseItemModel
	{
		/// <summary>
		/// Gets or sets the list of accounts.
		/// </summary>k
		public IList<AccountViewModel> AccountList { get; set; }

		/// <summary>
		/// Gets or sets the Expense Items.
		/// </summary>
		public ExpenseItemCreateViewModel Item { get; set; }

		/// <summary>
		/// Gets or sets the index.
		/// </summary>
		public int Index { get; set; }
	}
}