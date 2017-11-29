//------------------------------------------------------------------------------
// <copyright file="ProjectController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Project;
using System;

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
		/// <param name="isActive"></param>
		/// <returns></returns>
		public async Task<ActionResult> Index(int subscriptionId, int isActive = 0)
		{
			DateTime currentTime = DateTime.UtcNow;
			ViewData["IsManager"] = AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			ViewData["SubscriptionId"] = subscriptionId;
			ViewData["SubscriptionName"] = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
			ViewData["UserId"] = AppService.UserContext.UserId;
			var orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var projects = (await AppService.GetProjectsByOrganization(orgId, false)).Select(x => new CompleteProjectViewModel(x)).ToList();

			if (isActive == 2)
			{
				projects = projects.Where(p => (p.StartDate == null || (DateTime)p.StartDate >= currentTime) && (p.EndDate == null || (DateTime)p.EndDate <= currentTime)).ToList();
			}
			else if (isActive == 1)
			{
				projects = projects.Where(p => (p.StartDate != null && p.StartDate > currentTime) || (p.EndDate != null && p.EndDate < currentTime)).ToList();
			}

			ProjectsViewModel model = new ProjectsViewModel()
			{
				Projects = projects,
				ForCustomer = isActive == 0
			};

			return View("Projects", model);
		}
	}
}