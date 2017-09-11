using AllyisApps.Controllers;
using AllyisApps.Services;
using System.Web.Mvc;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Adds a new item to the model and displays the items in the create report view.
		/// </summary>
		/// <param name="index">The item index.</param>
		/// <returns>A partial view.</returns>
		public ActionResult AddItem(int index)
		{
			return PartialView("_AjaxExpenseReportItems", new ExpenseItem() { Index = index });
		}
	}
}