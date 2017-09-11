using AllyisApps.DBModel.Finance;
using AllyisApps.Services;
using System.Collections.Generic;

namespace AllyisApps.Areas.ExpenseTracker.ViewModels.Expense
{
	/// <summary>
	/// View model for Expense Items.
	/// </summary>
	public class ExpenseItemModel
	{
		/// <summary>
		/// Gets or sets the list of accounts.
		/// </summary>
		public IList<AccountDBEntity> AccountList { get; set; }

		/// <summary>
		/// Gets or sets the Expense Items.
		/// </summary>
		public ExpenseItem Item { get; set; }

		/// <summary>
		/// Gets or sets the index.
		/// </summary>
		public int Index { get; set; }
	}
}