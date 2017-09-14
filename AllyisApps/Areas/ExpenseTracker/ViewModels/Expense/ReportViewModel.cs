using System;
using System.Collections.Generic;
using AllyisApps.Services;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// The report view model.
	/// </summary>
	public class ReportViewModel
	{
		/// <summary>
		/// Gets or sets the report id.
		/// </summary>
		public int ReportId { get; set; }

		/// <summary>
		/// Gets or sets the current user id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the current subsctipition id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the report title.
		/// </summary>
		public string ReprortTitle { get; set; }

		/// <summary>
		/// Gets or sets the submitting user name.
		/// </summary>
		public string SubmittedBy { get; set; }

		/// <summary>
		/// Gets or sets the report status.
		/// </summary>
		public ExpenseStatusEnum Status { get; set; }

		/// <summary>
		/// Gets or sets the expense justification.
		/// </summary>
		public string Justification { get; set; }

		/// <summary>
		/// Gets or sets the report creation date.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the report last modified date.
		/// </summary>
		public DateTime ModifiedUtc { get; set; }

		/// <summary>
		/// Gets or sets the submission date.
		/// </summary>
		public DateTime? SubmittedUtc { get; set; }

		/// <summary>
		/// Gets or sets the associated expense items.
		/// </summary>
		public IEnumerable<ExpenseItemCreateViewModel> Expenses { get; set; }

		/// <summary>
		/// Gets or sets the associeate report history.
		/// </summary>
		public IEnumerable<ExpenseHistoryViewModel> History { get; set; }

		/// <summary>
		/// Gets or sets the list of attachments.
		/// </summary>
		public IEnumerable<string> Attachments { get; set; }
	}
}