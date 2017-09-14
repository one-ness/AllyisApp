using System;
using System.Collections.Generic;
using System.Web;
using AllyisApps.DBModel.Finance;
using AllyisApps.Services;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// Model for Create Expense Report View.
	/// </summary>
	public class ExpenseCreateModel
	{
		/// <summary>
		/// Gets or sets whether the submission is an update or create.
		/// </summary>
		public string SubmitType { get; set; }

		/// <summary>
		/// Gets or sets Subscription Id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets Current User Id.
		/// </summary>
		public int CurrentUser { get; set; }

		/// <summary>
		/// Gets or sets the Expense Report.
		/// </summary>
		public ExpenseReportViewModel Report { get; set; }

		/// <summary>
		/// Gets or sets the Start Date.
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the Expense Items.
		/// </summary>
		public IList<ExpenseItemCreateViewModel> Items { get; set; }

		/// <summary>
		/// Gets or sets the Expense Files.
		/// </summary>
		public IList<HttpPostedFileBase> Files { get; set; }

		/// <summary>
		/// Gets or sets the names of files that were previously uploaded.
		/// </summary>
		public IEnumerable<string> PreviousFiles { get; set; }

		/// <summary>
		/// Gets or sets the list of accounts.
		/// </summary>
		public IList<AccountViewModel> AccountList { get; set; }
	}
}