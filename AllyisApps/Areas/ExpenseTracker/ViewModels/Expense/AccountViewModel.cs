using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AllyisApps.DBModel.Finance;
using AllyisApps.Services;
using AllyisApps.Services.Expense;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// Model for Create Expense Report View.
	/// </summary>
	public class AccountViewModel
	{
		/// <summary>
		/// Gets or sets the organization's ID.
		/// </summary>
		public int AccountId { get; set; }

		/// <summary>
		/// Gets or sets the Account Name.
		/// </summary>
		public string AccountName { get; set; }
	}
}