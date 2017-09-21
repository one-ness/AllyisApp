using System.Collections.Generic;
using System.Web.Mvc;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// Account page view model.
	/// </summary>
	public class AccountPageViewModel
	{
		/// <summary>
		/// Gets or sets the accounts on the page.
		/// </summary>
		public List<AccountManagementViewModel> Accounts { get; set; }
	}

	/// <summary>
	/// Represents an account in the view model.
	/// </summary>
	public class AccountManagementViewModel
	{
		/// <summary>
		/// Gets or sets the organization's ID.
		/// </summary>
		public int AccountId { get; set; }

		/// <summary>
		/// Gets or sets the Account Name.
		/// </summary>
		public string AccountName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the account is active.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets the AccountTypeId (TODO: what are the account types).
		/// </summary>
		public int AccountTypeId { get; set; }

		/// <summary>
		/// Gets or sets accounts parent account.
		/// </summary>
		public int? ParentAccountId { get; set; }

		/// <summary>
		/// Gets or sets the parent account name.
		/// </summary>
		public string ParentAccountName { get; set; }

		/// <summary>
		/// Gets or sets the account type name, linked to AccountTypeId .
		/// </summary>
		public string AccountTypeName { get; set; }
	}

	/// <summary>
	/// View model for the create account page.
	/// </summary>
	public class CreateAccountViewModel : AccountManagementViewModel
	{
		/// <summary>
		/// Gets or sets the selected account.
		/// </summary>
		public string SelectedAccount { get; set; }

		/// <summary>
		/// Gets or sets existing accounts that can be a parent account.
		/// </summary>
		public SelectList ParentAccounts { get; set; }
	}
}