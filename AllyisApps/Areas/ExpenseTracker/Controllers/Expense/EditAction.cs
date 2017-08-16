using System.Web.Mvc;
using AllyisApps.Controllers;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Edits a report.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
        /// <summary>
		/// Edits a report
		/// </summary>
		/// <returns></returns>
        public ActionResult Edit(int subscriptionId, int reportId)
        {
			return RedirectToAction("Create", new { subscriptionId = subscriptionId, reportId = reportId });
        }
    }
}