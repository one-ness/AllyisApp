﻿using AllyisApps.DBModel.Finance;
using AllyisApps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.Areas.ExpenseTracker.ViewModels.Expense
{
	/// <summary>
	/// View model for Expense Items.
	/// </summary>
	public class ExpenseItemModel
	{
		/// <summary>
		/// Gets or sets the list of accounts
		/// </summary>
		public IList<AccountDBEntity> AccountList { get; set; }

		/// <summary>
		/// Gets or sets the Expense Items
		/// </summary>
		public IList<ExpenseItem> Items { get; set; }
	}
}