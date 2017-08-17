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
	/// Stores a new report in database and redirects to View
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Displays a file associated with report.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult DisplayFile(ExpenseCreateModel model)
		{
			if (model.Files != null)
			{
				return PartialView("_AjaxExpenseReportFile", model);
			}
			return null;
		}
	}
}