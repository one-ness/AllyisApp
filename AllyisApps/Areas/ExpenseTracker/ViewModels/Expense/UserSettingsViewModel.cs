using System.Collections.Generic;
using AllyisApps.Services;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// Information needed for the Expense report view.
	/// </summary>
	public class UserSettingsViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the users for the expense tracker.
		/// </summary>
		public IEnumerable<User> Users { get; set; }

		/// <summary>
		/// Gets or sets the subscription id.
		/// </summary>
		public int SubscriptionId { get; set; }
	}
}