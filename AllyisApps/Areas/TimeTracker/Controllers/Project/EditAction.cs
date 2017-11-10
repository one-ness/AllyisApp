//------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
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
		/// GET: Project/Edit.
		/// Gets the form for editing an existing Project.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <param name="userId">The project's Id.</param>
		/// <returns>The ActionResult for the Edit view.</returns>
		public ActionResult Edit(int subscriptionId, int userId)
		{
			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			return View(ConstructEditProjectViewModel(userId, subscriptionId));
		}

		/// <summary>
		/// POST: Project/Edit.
		/// Method for editing a project in the database.
		/// </summary>
		/// <param name="model">The model of changes to the project.</param>
		/// <returns>Redirection to customer index page on success, or project index on failure.</returns>
		[HttpPost]
		public async Task<ActionResult> Edit(EditProjectViewModel model)
		{
			var listGet = await AppService.GetNextProjectIdAndSubUsers(model.ParentCustomerId, model.SubscriptionId);
			model.SubscriptionUsers = listGet.Item2.Select(user => new BasicUserInfoViewModel(user.FirstName, user.LastName, user.UserId)).ToList();
			if (!ModelState.IsValid) return View(model);

			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, model.SubscriptionId);

			int orgId = AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId;
			var projIdMatchGet = await AppService.GetAllProjectsForOrganizationAsync(orgId);

			// TODO: Don't check for duplicate projects in controller
			Services.Project.Project projIdMatch = projIdMatchGet.SingleOrDefault(project => project.ProjectOrgId == model.ProjectOrgId && project.owningCustomer?.CustomerId == model.ParentCustomerId);
			if (projIdMatch != null && projIdMatch.ProjectId != model.ProjectId)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.ProjectOrgIdNotUnique, Variety.Danger));
				return View(model);
			}

			try
			{
				UpdateProject(model);
			}
			catch (Exception ex)
			{
				// Update failure
				Notifications.Add(new BootstrapAlert($"{Resources.Strings.FailureProjectEdited} {ex.Message}", Variety.Danger));
				return View(model);
			}

			Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessProjectEdited, Variety.Success));

			return Redirect(string.Format(
				"{0}#customerNumber{1}",
				Url.Action(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId = model.SubscriptionId }),
				model.ParentCustomerId));
		}

		/// <summary>
		/// Uses services to populate a new <see cref="EditProjectViewModel"/> from a project Id and returns it.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <returns>The EditProjectViewModel.</returns>
		public EditProjectViewModel ConstructEditProjectViewModel(int projectId, int subscriptionId)
		{
			var infos = AppService.GetProjectEditInfo(projectId, subscriptionId);
			var projectUsers = infos.Item2.Select(projectUser => new BasicUserInfoViewModel(projectUser.FirstName, projectUser.LastName, projectUser.UserId)).ToList();
			var subscriptionUsers = infos.Item3.Select(su => new BasicUserInfoViewModel(su.FirstName, su.LastName, su.UserId)).ToList();
			string subscriptionNameToDisplay = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;

			return new EditProjectViewModel
			{
				CustomerName = infos.Item1.owningCustomer.CustomerName,
				OrganizationName = infos.Item1.OrganizationName,
				ParentCustomerId = infos.Item1.owningCustomer.CustomerId,
				OrganizationId = infos.Item1.OrganizationId,
				ProjectId = infos.Item1.ProjectId,
				ProjectOrgId = infos.Item1.ProjectOrgId,
				ProjectName = infos.Item1.ProjectName,
				ProjectUsers = projectUsers,
				SubscriptionUsers = subscriptionUsers.Where(user => projectUsers.All(pu => pu.UserId != user.UserId)), // Grab users that are not part of the project
				StartDate = infos.Item1.StartDate,
				EndDate = infos.Item1.EndDate,
				SubscriptionId = subscriptionId,
				SubscriptionName = subscriptionNameToDisplay,
				UserId = AppService.UserContext.UserId
			};
		}
	}
}