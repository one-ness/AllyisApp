//------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
		public async Task<ActionResult> Edit(int subscriptionId, int userId)
		{
			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			var model = await ConstructEditProjectViewModel(userId, subscriptionId);
			return View(model);
		}

		/// <summary>
		/// POST: Project/Edit.
		/// Method for editing a project in the database.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="model">The model of changes to the project.</param>
		/// <returns>Redirection to customer index page on success, or project index on failure.</returns>
		[HttpPost]
		public async Task<ActionResult> Edit(int subscriptionId, EditProjectViewModel model)
		{
			//for repopulating form choices upon edit error
			var totalUsers = model.ProjectUsers.Concat(model.SubscriptionUsers).ToList();
			model.ProjectUsers = totalUsers.Where(subUsers => model.SelectedProjectUserIds.Contains(subUsers.UserId)); //right multi-select box
			model.SubscriptionUsers = totalUsers.Except(model.ProjectUsers); //left multi-select box

			if (!ModelState.IsValid) return View(model);

			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, model.SubscriptionId);

			int orgId = AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId;
			var projects = (await AppService.GetAllProjectsForOrganizationAsync(orgId, false)).ToList();

			// TODO: Don't check for duplicate projects in controller
			
			var projCodeMatch = projects.FindAll(project => project.ProjectCode == model.ProjectCode && project.owningCustomer?.CustomerId == model.ParentCustomerId &&
				model.ProjectId != project.ProjectId);
			if (projCodeMatch.Count() > 0)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.ProjectCodeNotUnique, Variety.Danger));
				return View(model);
			}

			bool projectIsActive = (model.StartDate == null || model.StartDate <= DateTime.Now)
								&& (model.EndDate == null || model.EndDate >= DateTime.Now);
			var projectBeforeEdit = projects.Single(p => p.ProjectId == model.ProjectId);
			model.IsActive = projectIsActive;

			try
			{
				var customer = await AppService.GetCustomerInfo(model.ParentCustomerId);
				if (model.StartDate != projectBeforeEdit.StartingDate || model.EndDate != projectBeforeEdit.EndingDate) // check new date range to see if entries outside of the new range
				{
					await AppService.CheckUpdateProjectStartEndDate(projectBeforeEdit.ProjectId, model.StartDate, model.EndDate);
				}
				if (!customer.IsActive.Value && projectIsActive)
				{
					customer.IsActive = true;
					await AppService.UpdateCustomerAsync(customer, subscriptionId);
					UpdateProject(model);
				}
				else
				{
					UpdateProject(model);
				}
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
				Url.Action(ActionConstants.Index, ControllerConstants.Project, new { subscriptionId = model.SubscriptionId }),
				model.ParentCustomerId));
		}

		/// <summary>
		/// Uses services to populate a new <see cref="EditProjectViewModel"/> from a project Id and returns it.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <returns>The EditProjectViewModel.</returns>
		public async Task<EditProjectViewModel> ConstructEditProjectViewModel(int projectId, int subscriptionId)
		{
			var infos = AppService.GetProjectEditInfo(projectId, subscriptionId);
			var projectUsers = infos.Item2.Select(projectUser => new BasicUserInfoViewModel(projectUser.FirstName, projectUser.LastName, projectUser.UserId)).ToList();
			var subscriptionUsers = infos.Item3.Select(su => new BasicUserInfoViewModel(su.FirstName, su.LastName, su.UserId)).ToList();
			string subscriptionNameToDisplay = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
			var orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var customers = (await AppService.GetCustomerList(orgId)).Select(x => new SelectListItem
			{
				Text = x.CustomerName,
				Value = x.CustomerId.ToString()
			}).ToList();

			var statusOptions = new List<SelectListItem>
			{
				new SelectListItem { Text = "Active", Value = true.ToString() },
				new SelectListItem { Text = "Disabled", Value = false.ToString() }
			};

			return new EditProjectViewModel
			{
				Customers = customers,
				IsActiveOptions = statusOptions,
				OrganizationName = infos.Item1.OrganizationName,
				ParentCustomerId = infos.Item1.owningCustomer.CustomerId,
				OrganizationId = infos.Item1.OrganizationId,
				ProjectId = infos.Item1.ProjectId,
				ProjectCode = infos.Item1.ProjectCode,
				ProjectName = infos.Item1.ProjectName,
				ProjectUsers = projectUsers,
				SubscriptionUsers = subscriptionUsers.Where(user => !projectUsers.Select(pu => pu.UserId).Contains(user.UserId)), // Grab users that are not part of the project
				StartDate = infos.Item1.StartDate,
				EndDate = infos.Item1.EndDate,
				SubscriptionId = subscriptionId,
				SubscriptionName = subscriptionNameToDisplay,
				UserId = AppService.UserContext.UserId
			};
		}
	}
}