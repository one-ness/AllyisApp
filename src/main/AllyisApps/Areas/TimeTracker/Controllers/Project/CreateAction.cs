//------------------------------------------------------------------------------
// <copyright file="CreateAction.cs" company="Allyis, Inc.">
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
		/// GET: Project/Create.
		/// Gets the page for creating new projects.
		/// </summary>
		/// <param name="id">Customer Id for the project.</param>
		/// <returns>The ActionResult for the Create view.</returns>
		[HttpGet]
		public ActionResult Create(int id)
		{
			if (Service.Can(Actions.CoreAction.EditProject))
			{
				var list = Service.GetUsers();
				var subList = new List<BasicUserInfoViewModel>();

				foreach (var user in list)
				{
					subList.Add(new BasicUserInfoViewModel(user.FirstName, user.LastName, user.UserId));        // Change to select list for model binding
				}

				return this.View(
					new EditProjectViewModel()
					{
						IsCreating = true,
						ParentCustomerId = id,
						ProjectUsers = new List<BasicUserInfoViewModel>(),
						SubscriptionUsers = subList,
						StartDate = TimeTrackerService.GetDayFromDateTime(DateTime.Today),
						EndDate = TimeTrackerService.GetDayFromDateTime(DateTime.Today.AddMonths(6)),
						ProjectOrgId = Service.GetRecommendedProjectId()
					});
			}
			else
			{
				// Permissions failure
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Project.Strings.ActionUnauthorizedMessage, Variety.Warning));

				return this.RouteHome();
			}
		}

		/// <summary>
		/// POST: Project/Create
		/// Method for creating a new project entry in the database.
		/// </summary>
		/// <param name="model">The model of user input.</param>
		/// <returns>If successful, notifies and redirects to Project/Index. Else, returns to the create project form.</returns>
		[HttpPost]
		public ActionResult Create(EditProjectViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (Service.Can(Actions.CoreAction.EditProject))
				{
					if (model == null)
					{
						throw new ArgumentNullException("model");
					}

					if (Service.GetAllProjectsForOrganization(UserContext.ChosenOrganizationId).Any(project => project.ProjectOrgId == model.ProjectOrgId))
					{
						Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Project.Strings.ProjectOrgIdNotUnique, Variety.Danger));
						return this.View(model);
					}
					try
					{
						model.ProjectId = CreateProject(model);
					}
					catch (Exception ex)
					{
						string message = "Could not create project.";
						if (ex.Message != null)
						{
							message = string.Format("{0} {1}", message, ex.Message);
						}

						//Create failure
						Notifications.Add(new BootstrapAlert(message, Variety.Danger));
						return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
					}
					//this.UpdateProject(model);
					Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Project.Strings.SuccessProjectCreated, Variety.Success));

					return this.Redirect(string.Format("{0}#customerNumber{1}", Url.Action(ActionConstants.Index, ControllerConstants.Customer), model.ParentCustomerId));
				}
				else
				{
					// Permissions failure
					this.Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Project.Strings.ActionUnauthorizedMessage, Variety.Warning));

					return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
				}
			}
			else
			{
				// Invalid Model
				return this.View(model);
			}
		}

		/// <summary>
		/// Creates a new project using a <see cref="EditProjectViewModel"/>.
		/// </summary>
		/// <param name="model"><see cref="EditProjectViewModel"/> representing new project.</param>
		/// <returns>The Project ID.</returns>
		public int CreateProject(EditProjectViewModel model)
		{
			return Service.CreateProject(new ProjectInfo()
			{
				CustomerId = model.ParentCustomerId,
				Name = model.ProjectName,
				Type = model.PriceType,
				ProjectOrgId = model.ProjectOrgId,
				StartingDate = TimeTrackerService.GetDateTimeFromDays(model.StartDate),
				EndingDate = TimeTrackerService.GetDateTimeFromDays(model.EndDate)
			});
		}
	}
}
