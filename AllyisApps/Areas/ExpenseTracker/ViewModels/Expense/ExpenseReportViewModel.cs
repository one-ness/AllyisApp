using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.Areas.ExpenseTracker.ViewModels.Expense
{
	/// <summary>
	/// Information needed to create a new report
	/// </summary>
	public class ExpenseReportViewModel
	{
		/// <summary>
		/// Date of report
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Name of report
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The jusitification for the report
		/// </summary>
		public string BusinessJustification { get; set; }

		/// <summary>
		/// List of items included in expense report
		/// </summary>
		public IEnumerable<ReportItem> Items { get; set; }
	}

	/// <summary>
	/// Information on each item of expense report
	/// </summary>
	public class ReportItem
	{
		/// <summary>
		/// Name of item
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Date that item was added
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Cost of item
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		/// Whether or not the customer will pay for the item
		/// </summary>
		public bool IsCustomerPaying { get; set; }
	}
}