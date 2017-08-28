using System;
using System.Collections.Generic;
using AllyisApps.Services;
using System.Web;
using AllyisApps.Services.Expense;
using System.Web.Mvc;
using AllyisApps.DBModel.Finance;

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
		public IEnumerable<ExpenseReport> PendingReports { get; set; }
	}
}