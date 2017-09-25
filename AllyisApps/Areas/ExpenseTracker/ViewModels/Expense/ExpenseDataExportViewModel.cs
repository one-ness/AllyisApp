using System.Collections.Generic;
using System.IO;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.ViewModels.Expense
{
	/// <summary>
	/// Expense Data export model.
	/// </summary>
	public class ExpenseDataExportViewModel
	{
		/// <summary>
		/// Gets or sets the report data.
		/// </summary>
		public IEnumerable<ExpenseReportViewModel> Data { get; set; }

		/// <summary>
		/// Gets or sets the preview data.
		/// </summary>
		public IEnumerable<ExpenseReportViewModel> PreviewData { get; set; }

		/// <summary>
		/// Gets the output stream.
		/// </summary>
		public StreamWriter Output { get; internal set; }

		/// <summary>
		/// Gets the out put stream for excel documents.
		/// </summary>
		public StringWriter ExcelOutput { get; internal set; }
	}
}