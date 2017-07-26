using AllyisApps.Controllers;
using System.Web.Mvc;
using System.Threading.Tasks;
using AllyisApps.Areas.ExpenseTracker.ViewModels.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// expense controller
	/// </summary>
	public class ExpenseController : BaseController
	{
		/// <summary>
		/// show the list of expense reports submitted by the logged in user
		/// </summary>
		public ActionResult Index(int subscriptionId)
		{
			return View(new ExpenseReportsViewModel());
		}
	}
}
