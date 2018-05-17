using System.Web.Mvc;
using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;
using AllyisApps.Controllers;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View.
	/// </summary>
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// Displays a file associated with report.
		/// </summary>
		/// <param name="model">A ExpenseFile model.</param>
		/// <returns>A ExpenseFile model.</returns>
		public ActionResult DisplayFile(ApplicationDocumentViewModel model)
		{
			return PartialView("ApplicationDocument", model);
		}
	}
}