//------------------------------------------------------------------------------
// <copyright file="ProjectController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.ViewModels.TimeTracker.Project;
using AllyisApps.Services;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	[Authorize]
	public partial class ProjectController : BaseController
	{
		/// <summary>
		/// Projects action.
		/// </summary>
		/// <param name="subscriptionId">The current subscription id.</param>
		/// <returns></returns>
		async public Task<ActionResult> Index(int subscriptionId)
		{
			ViewData["IsManager"] = AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			ViewData["SubscriptionId"] = subscriptionId;
			ViewData["SubscriptionName"] = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
			ViewData["UserId"] = AppService.UserContext.UserId;
			var orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var projects = (await AppService.GetProjectsByOrganization(orgId, false)).Select(x => new CompleteProjectViewModel(x)).ToList();

			ProjectsViewModel model = new ProjectsViewModel()
			{
				Projects = projects
			};

			return View("Projects", model);
		}
	}
}