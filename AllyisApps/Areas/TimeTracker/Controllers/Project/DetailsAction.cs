//------------------------------------------------------------------------------
// <copyright file="DetailsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Project;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseController
	{
		/// <summary>
		/// GET: Project/Details.
		/// Get's details of the specified project.
		/// </summary>
		/// <param name="subscriptionId">The subscription that the customer belongs to, which owns the project.</param>
		/// <param name="projectId">The project's Id.</param>
		/// <returns>The details view of the project.</returns>
		public async Task<ActionResult> Details(int subscriptionId, int projectId)
		{
			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			var model = await AppService.GetProjectAsUser(projectId);
			model.CanEditProject = true;
			return View(new ProjectCompleteProjectViewModel
			{
				IsCustomerActive = model.IsCustomerActive,
				CreatedUtc = model.CreatedUtc,
				OrganizationName = model.OrganizationName,
				IsUserActive = model.IsUserActive,
				PriceType = model.PriceType,
				StartDate = model.StartDate,
				EndDate = model.EndDate,
				CanEditProject = model.CanEditProject,
				IsProjectUser = model.IsProjectUser,
			});
		}
	}
}