using System.Web.Mvc;
using AllyisApps.Areas.ExpenseTracker.ViewModels.Expense;
using AllyisApps.Controllers;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Displays a file associated with report.
		/// </summary>
		/// <param name="model">A ExpenseFile model.</param>
		/// <returns>A ExpenseFile model.</returns>
		public ActionResult DisplayFile(ExpenseFileModel model)
		{
			return PartialView("_AjaxExpenseReportFile", model);
		}
	}
}