using System.Web.Mvc;
using AllyisApps.Controllers;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseController
	{
		/// <summary>
		/// Directs user to the projec import range.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <returns>The project import view</returns>
		[HttpGet]
		public ActionResult ProjectImport(int subscriptionId)
		{
			return View();
		}
	}
}