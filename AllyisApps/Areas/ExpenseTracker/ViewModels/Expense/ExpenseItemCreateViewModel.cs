using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AllyisApps.Services;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// The AdminReport Model.
	/// </summary>
	public class ExpenseItemCreateViewModel
	{
		/// <summary>
		/// Gets or sets the expense item id
		/// </summary>
		public int ExpenseItemId { get; set; }

		/// <summary>
		/// Gets or sets the expense description
		/// </summary>
		[Required(ErrorMessage = "Description is required.")]
		public string ItemDescription { get; set; }

		/// <summary>
		/// Gets or sets the transaction date
		/// </summary>
		[Required(ErrorMessage = "Date is required.")]
		public string TransactionDate { get; set; }

		/// <summary>
		/// Gets or sets the amount of the expense
		/// </summary>
		[Required(ErrorMessage = "Amount is required.")]
		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public decimal Amount { get; set; }

		/// <summary>
		/// Gets or sets the expenses report id.
		/// </summary>
		public int ExpenseReportId { get; set; }

		/// <summary>
		/// Gets or sets the user id associated with the Expense
		/// </summary>
		public int AccountId { get; set; }

		/// <summary>
		/// Gets or sets the flag for if the expense can be billed to a customer
		/// </summary>
		public bool IsBillableToCustomer { get; set; }

		/// <summary>
		/// Gets or sets when the item was created
		/// </summary>
		public DateTime ExpenseItemCreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets when it was last modified.
		/// </summary>
		public DateTime ExpenseItemModifiedUtc { get; set; }

		/// <summary>
		/// Gets or sets the index of item in report
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// Gets or sets whether to delete.
		/// </summary>
		public bool ToDelete { get; set; }
	}
}