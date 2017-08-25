using AllyisApps.Areas.ExpenseTracker.ViewModels.Expense;
using AllyisApps.Controllers;
using System.Web.Mvc;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Expense controller.
	/// </summary>
	public class ExpenseController : BaseController
	{
		/// <summary>
		/// Show the list of expense reports submitted by the logged in user.
		/// </summary>
		/// <param name="subscriptionId">The id of the expense tracker subscription.</param>
		/// <returns>The view for the expense tracker index page.</returns>
		public ActionResult Index(int subscriptionId)
		{
			return View(new ExpenseReportsViewModel());
		}
	}
}
