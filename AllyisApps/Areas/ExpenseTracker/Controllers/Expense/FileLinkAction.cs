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
		/// Displays a file with a link associated with report.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>A partial view.</returns>
		public ActionResult DisplayFileLink(ExpenseFileModel model)
		{
			return PartialView("_AjaxExpenseReportFileLink", model);
		}
	}
}
