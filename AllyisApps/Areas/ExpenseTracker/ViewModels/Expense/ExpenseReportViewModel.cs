using System;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// The ExpenseReport view model.
	/// </summary>
	public class ExpenseReportViewModel
	{
		/// <summary>
		/// Gets or sets the expense report id.
		/// </summary>
		public int ExpenseReportId { get; set; }

		/// <summary>
		/// Gets or sets the expense report title.
		/// </summary>
		[Required(ErrorMessage = "Report name is required.")]
		public string ReportTitle { get; set; }

		/// <summary>
		/// Gets or sets he organization id of the expense.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the submiting user id.
		/// </summary>
		public int SubmittedById { get; set; }

		/// <summary>
		/// Gets or sets the report status.
		/// </summary>
		public int ReportStatus { get; set; }

		/// <summary>
		/// Gets or sets the business justification.
		/// </summary>
		[Required(ErrorMessage = "Justification is required.")]
		public string BusinessJustification { get; set; }

		/// <summary>
		/// Gets or sets the date created.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the last date modified.
		/// </summary>
		public DateTime ModifiedUtc { get; set; }

		/// <summary>
		/// Gets or sets the date submitted.
		/// </summary>
		public DateTime? SubmittedUtc { get; set; }
	}
}