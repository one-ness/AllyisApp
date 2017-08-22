using System;
using System.Collections.Generic;
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
		/// Gets or sets the file.
		/// </summary>
		public HttpPostedFileBase File { get; set; }
	}
}