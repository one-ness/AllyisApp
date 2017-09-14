using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AllyisApps.Services;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// The ExpenseItemViewModel.
	/// </summary>
	public class ExpenseItemViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the reportId.
		/// </summary>
		public int ReportId { get; set; }

		/// <summary>
		/// Gets or sets the user id of the user who submitted the report.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the user name of the user who submitted the report.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the name of the report.
		/// </summary>
		public string ReportName { get; set; }

		/// <summary>
		/// Gets or sets the date the report was submitted.
		/// </summary>
		[DisplayFormat(DataFormatString = "{0:MM-dd-YYYY")]
		public DateTime? SubmittedDate { get; set; }

		/// <summary>
		/// Gets or sets the status of the report.
		/// </summary>
		public ExpenseStatusEnum Status { get; set; }

		/// <summary>
		/// Gets or sets the cost amount that is reported.
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		/// Gets or sets the reason for the expense being submitted.
		/// </summary>
		public string Reason { get; set; }

		/// <summary>
		/// Gets or sets the list of Items contained in report.
		/// </summary>
		public List<ExpenseItemCreateViewModel> Items { get; set; }
	}
}