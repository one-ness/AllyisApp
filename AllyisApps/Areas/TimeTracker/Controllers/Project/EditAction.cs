//------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
		/// <param name="id">The project's Id.</param>
		/// <returns>The ActionResult for the Edit view.</returns>
		public ActionResult Edit(int id)
		{
			if (Service.Can(Actions.CoreAction.EditProject))
			{
				return this.View(this.ConstructEditProjectViewModel(id));
			}
			else
			{
				// Permissions Failure
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
			}
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
			var list = Service.GetNextProjectIdAndSubUsers(model.ParentCustomerId).Item2;
			var subList = new List<BasicUserInfoViewModel>();
			foreach (var user in list)
			{
				subList.Add(new BasicUserInfoViewModel(user.FirstName, user.LastName, user.UserId));        // Change to select list for model binding
			}
			model.SubscriptionUsers = subList;
			if (ModelState.IsValid)
			{
				if (Service.Can(Actions.CoreAction.EditProject))
				{
					Project projIdMatch = Service.GetAllProjectsForOrganization(UserContext.ChosenOrganizationId).Where(project => project.ProjectOrgId == model.ProjectOrgId && project.CustomerId == model.ParentCustomerId).SingleOrDefault();
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

						//Update failure
						Notifications.Add(new BootstrapAlert(message, Variety.Danger));
						return this.View(model);
					}
					Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessProjectEdited, Variety.Success));

					return this.Redirect(string.Format("{0}#customerNumber{1}", Url.Action(ActionConstants.Index, ControllerConstants.Customer), model.ParentCustomerId));
				}
				else
				{
					// Permissions failure
					Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
					return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
				}
			}
			else
			{
				return this.View(model);
			}
		}

		/// <summary>
		/// Uses services to populate a new <see cref="EditProjectViewModel"/> from a project Id and returns it.
		/// </summary>
		/// <param name="projectId">Project ID.</param>
		/// <returns>The EditProjectViewModel.</returns>
		public EditProjectViewModel ConstructEditProjectViewModel(int projectId)
		{
			var infos = Service.GetProjectEditInfo(projectId);

			IEnumerable<User> projectUserInfos = infos.Item2;
			var projectUsers = new List<BasicUserInfoViewModel>();
			foreach (var projectUser in projectUserInfos)
			{
				projectUsers.Add(new BasicUserInfoViewModel(projectUser.FirstName, projectUser.LastName, projectUser.UserId));
			}

			IEnumerable<SubscriptionUserInfo> subscriptionUserInfos = infos.Item3;
			var subscriptionUsers = new List<BasicUserInfoViewModel>();

			foreach (var su in subscriptionUserInfos)
			{
				subscriptionUsers.Add(new BasicUserInfoViewModel(su.FirstName, su.LastName, su.UserId));
			}

			return new EditProjectViewModel
			{
				CustomerName = infos.Item1.CustomerName,
				OrganizationName = infos.Item1.OrganizationName,
				ParentCustomerId = infos.Item1.CustomerId,
				OrganizationId = infos.Item1.OrganizationId,
				ProjectId = infos.Item1.ProjectId,
				ProjectOrgId = infos.Item1.ProjectOrgId,
				ProjectName = infos.Item1.ProjectName,
				ProjectUsers = projectUsers,
				SubscriptionUsers = subscriptionUsers.Where(user => !projectUsers.Any(pu => (pu.UserId == user.UserId))), // Grab users that are not part of the project
				PriceType = infos.Item1.PriceType,
				StartDate = TimeTrackerService.GetDayFromDateTime(infos.Item1.StartDate),
				EndDate = TimeTrackerService.GetDayFromDateTime(infos.Item1.EndDate)
			};
		}
	}
}
