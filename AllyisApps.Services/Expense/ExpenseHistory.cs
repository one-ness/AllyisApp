using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.Expense
{
	public class ExpenseHistory
	{
		/// <summary>
		/// Gets or sets the history id.
		/// </summary>
		public int HistoryId { get; set; }

		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the report id.
		/// </summary>
		public int ReportId { get; set; }

		/// <summary>
		/// Gets or sets the created date.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the modified date.
		/// </summary>
		public DateTime ModifiedUtc { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		public int Status { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		public string Text { get; set; }
	}
}