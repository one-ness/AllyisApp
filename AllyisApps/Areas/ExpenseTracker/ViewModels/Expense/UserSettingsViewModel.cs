using AllyisApps.Services;
using System;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// Information needed for the Expense report view.
	/// </summary>
	public class UserSettingsViewModel : BaseViewModel
	{
		IEnumerable<ExpenseUser> Users { get; set; }
    }
}
