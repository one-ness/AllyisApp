using AllyisApps.Controllers;
using AllyisApps.Services;
using System.Web.Mvc;
using System.Threading.Tasks;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Collections.Generic;
using System;
using System.Linq;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Finance;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Edits a report
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