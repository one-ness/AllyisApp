//------------------------------------------------------------------------------
// <copyright file="CreateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.ViewModels;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseProductController
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
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditProject))
			{
				var list = OrgService.GetUsers();
				var subList = new List<BasicUserInfoViewModel>();

				foreach (var user in list)
				{
					subList.Add(new BasicUserInfoViewModel(user.FirstName, user.LastName, user.UserId));        // Change to select list for model binding
				}

				return this.View(
					new EditProjectViewModel()
					{
						ParentCustomerId = id,
						ProjectUsers = new List<BasicUserInfoViewModel>(),
						SubscriptionUsers = subList,
						StartDate = TimeTrackerService.GetDayFromDateTime(DateTime.Today),
						EndDate = TimeTrackerService.GetDayFromDateTime(DateTime.Today.AddMonths(6))
					});
			}
			else
			{
				// Permissions failure
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Project.Strings.ActionUnauthorizedMessage, Variety.Warning));

				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Home);
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
				if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditProject))
				{
					if (model == null)
					{
						throw new ArgumentNullException("model");
					}

					model.ProjectId = CreateProject(model);
					this.UpdateProject(model);
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
			if (model.OrganizationId == null)
			{
				return ProjectService.CreateProjectFromCustomerIdOnly(
					model.ParentCustomerId,
					model.ProjectName,
					model.PriceType,
					TimeTrackerService.GetDateTimeFromDays(model.StartDate),
					TimeTrackerService.GetDateTimeFromDays(model.EndDate));
			}
			else
			{
				return ProjectService.CreateProject(
					(int)model.OrganizationId,
					model.ParentCustomerId,
					model.ProjectName,
					model.PriceType,
					TimeTrackerService.GetDateTimeFromDays(model.StartDate),
					TimeTrackerService.GetDateTimeFromDays(model.EndDate));
			}
		}
	}
}