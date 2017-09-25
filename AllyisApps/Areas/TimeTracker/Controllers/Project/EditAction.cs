//------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
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
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			return this.View(this.ConstructEditProjectViewModel(userId, subscriptionId));
		}

		/// <summary>
		/// POST: Project/Edit.
		/// Method for editing a project in the database.
		/// </summary>
		/// <param name="model">The model of changes to the project.</param>
		/// <returns>Redirection to customer index page on success, or project index on failure.</returns>
		[HttpPost]
		public ActionResult Edit(EditProjectViewModel model)
		{
			var list = AppService.GetNextProjectIdAndSubUsers(model.ParentCustomerId, model.SubscriptionId).Item2;
			var subList = new List<BasicUserInfoViewModel>();
			foreach (var user in list)
			{
				subList.Add(new BasicUserInfoViewModel(user.FirstName, user.LastName, user.UserId));        // Change to select list for model binding
			}

			model.SubscriptionUsers = subList;
			int orgId = AppService.UserContext.SubscriptionsAndRoles[model.SubscriptionId].OrganizationId;
			if (ModelState.IsValid)
			{
				this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, model.SubscriptionId);

				Services.Project.Project projIdMatch = AppService.GetAllProjectsForOrganization(orgId).Where(project => project.ProjectOrgId == model.ProjectOrgId && project.owningCustomer?.CustomerId == model.ParentCustomerId).SingleOrDefault();
				if (projIdMatch != null && projIdMatch.ProjectId != model.ProjectId)
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.ProjectOrgIdNotUnique, Variety.Danger));
					return this.View(model);
				}

				try
				{
					UpdateProject(model);
				}
				catch (Exception ex)
				{
					string message = Resources.Strings.FailureProjectEdited;
					if (ex.Message != null)
					{
						message = string.Format("{0} {1}", message, ex.Message);
					}

					// Update failure
					Notifications.Add(new BootstrapAlert(message, Variety.Danger));
					return this.View(model);
				}

				Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessProjectEdited, Variety.Success));

				return this.Redirect(string.Format("{0}#customerNumber{1}", Url.Action(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId = model.SubscriptionId }), model.ParentCustomerId));
			}
			else
			{
				return this.View(model);
			}
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

			IEnumerable<User> projectUserInfos = infos.Item2;
			var projectUsers = new List<BasicUserInfoViewModel>();
			foreach (var projectUser in projectUserInfos)
			{
				projectUsers.Add(new BasicUserInfoViewModel(projectUser.FirstName, projectUser.LastName, projectUser.UserId));
			}

			IEnumerable<SubscriptionUser> subscriptionUserInfos = infos.Item3;
			var subscriptionUsers = new List<BasicUserInfoViewModel>();

			foreach (var su in subscriptionUserInfos)
			{
				subscriptionUsers.Add(new BasicUserInfoViewModel(su.FirstName, su.LastName, su.UserId));
			}

			string subscriptionNameToDisplay = AppService.GetSubscription(subscriptionId).SubscriptionName;

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
				SubscriptionUsers = subscriptionUsers.Where(user => !projectUsers.Any(pu => (pu.UserId == user.UserId))), // Grab users that are not part of the project
				StartDate = Utility.GetDaysFromDateTime(infos.Item1.StartDate),
				EndDate = Utility.GetDaysFromDateTime(infos.Item1.EndDate),
				SubscriptionId = subscriptionId,
				SubscriptionName = subscriptionNameToDisplay,
				UserId = AppService.UserContext.UserId
			};
		}
	}
}