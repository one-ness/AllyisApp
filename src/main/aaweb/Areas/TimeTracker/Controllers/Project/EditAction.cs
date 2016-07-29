//------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.ViewModels;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseProductController
	{
		/// <summary>
		/// GET: Project/Edit.
		/// Gets the form for editing an existing Project.
		/// </summary>
		/// <param name="id">The project's Id.</param>
		/// <returns>The ActionResult for the Edit view.</returns>
		public ActionResult Edit(int id)
		{
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditProject))
			{
				return this.View(this.ConstructEditProjectViewModel(id));
			}
			else
			{
				// Permissions Failure
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Project.Strings.ActionUnauthorizedMessage, Variety.Warning));
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
			if (ModelState.IsValid)
			{
				if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditProject))
				{
					UpdateProject(model);
					Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Project.Strings.SuccessProjectEdited, Variety.Success));

					return this.Redirect(string.Format("{0}#customerNumber{1}", Url.Action(ActionConstants.Index, ControllerConstants.Customer), model.ParentCustomerId));
				}
				else
				{
					// Permissions failure
					Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Project.Strings.ActionUnauthorizedMessage, Variety.Warning));
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
			CompleteProjectInfo projectInfo = ProjectService.GetProject(projectId);
			IEnumerable<UserInfo> projectUserInfos = ProjectService.GetUsersByProjectId(projectInfo.ProjectId);
			var projectUsers = new List<BasicUserInfoViewModel>();
			foreach (var projectUser in projectUserInfos)
			{
				projectUsers.Add(new BasicUserInfoViewModel(projectUser.FirstName, projectUser.LastName, projectUser.UserId)); // Simplify list for model binding
			}

			IEnumerable<SubscriptionUserInfo> subscriptionUserInfos = OrgService.GetUsers();
			var subscriptionUsers = new List<BasicUserInfoViewModel>();

			foreach (var su in subscriptionUserInfos)
			{
				subscriptionUsers.Add(new BasicUserInfoViewModel(su.FirstName, su.LastName, su.UserId)); // Simplify list for model binding
			}

			return new EditProjectViewModel
			{
				CustomerName = projectInfo.CustomerName,
				OrganizationName = projectInfo.OrganizationName,
				ParentCustomerId = projectInfo.CustomerId,
				OrganizationId = projectInfo.OrganizationId,
				ProjectId = projectInfo.ProjectId,
				ProjectName = projectInfo.ProjectName,
				ProjectUsers = projectUsers,
				SubscriptionUsers = subscriptionUsers.Where(user => !projectUsers.Any(pu => (pu.UserId == user.UserId))), // Grab users that are not part of the project
				PriceType = projectInfo.PriceType,
				StartDate = projectInfo.StartDate,
				EndDate = projectInfo.EndDate
			};
		}
	}
}
