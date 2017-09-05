using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AllyisApps.Areas.ExpenseTracker.ViewModels.Expense
{
	/// <summary>
	/// View model for Expense Files.
	/// </summary>
	public class ExpenseFileModel
	{
		/// <summary>
		/// Gets or sets the index of the file.
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// Gets or sets the file stream.
		/// </summary>
		public Stream Stream { get; set; }

		/// <summary>
		/// Gets or sets the file content type.
		/// </summary>
		public string ContentType { get; set; }

		/// <summary>
		/// Gets or sets the file name.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Gets or sets the reportId.
		/// </summary>
		public int ReportId { get; set; }
	}
}