using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Expense controller.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Exports the expense report.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="organizationId">The organization id.</param>
		/// <param name="model">The model.</param>
		/// <param name="startDate">The start date.</param>
		/// <param name="endDate">The end date.</param>
		/// <returns>A FileStreamResult.</returns>
		public ActionResult ExportExpenseReport(int subscriptionId, int organizationId, AdminReportModel model, DateTime? startDate = null, DateTime? endDate = null)
		{
			List<ExpenseReport> expenses = new List<ExpenseReport>();

			DateTime start = startDate != null ? startDate.Value : DateTime.UtcNow;
			DateTime end = endDate != null ? endDate.Value : DateTime.UtcNow;

			foreach (var user in model.Selection.SelectedUsers)
			{
				var reports = AppService.GetExpenseReportBySubmittedId(user).Select(x => x).Where(x => DateTime.Compare(x.CreatedUtc, start) >= 0 && DateTime.Compare(x.CreatedUtc, end) <= 0);
				reports = reports.Select(x => x).Where(y => model.Selection.Status.IndexOf(y.ReportStatus) != -1);
				expenses.AddRange(reports);
			}

			return File(AppService.PrepareExpenseCSVExport(organizationId, expenses, start, end).BaseStream, "text/csv", "export.csv");
		}
	}
}