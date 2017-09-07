using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Edits a report.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
        /// <summary>
		/// Edits a report.
		/// </summary>
        /// <param name="subscriptionId">The subscription Id.</param>
        /// <param name="reportId">The report Id.</param>
		/// <returns>A redirect action to the report create page.</returns>
        public ActionResult Edit(int subscriptionId, int reportId)
		{
			return RedirectToAction("Create", new { subscriptionId = subscriptionId, reportId = reportId });
        }
    }
}