using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;

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
			ViewData["IsManager"] = AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			ViewData["SubscriptionId"] = subscriptionId;
			ViewData["SubscriptionName"] = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
			ViewData["UserId"] = AppService.UserContext.UserId;
			return View();
		}
	}
}