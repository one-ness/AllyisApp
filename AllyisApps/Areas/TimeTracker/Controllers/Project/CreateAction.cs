//------------------------------------------------------------------------------
// <copyright file="CreateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
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
		/// <param name="subscriptionId"></param>
		/// <param name="id">Customer Id for the project.</param>
		/// <returns>The ActionResult for the Create view.</returns>
		[HttpGet]
		public ActionResult Create(int subscriptionId, int id)
		{
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.CreateProject, subscriptionId);
			DateTime? defaultStart = null;
			DateTime? defaultEnd = null;
			var idAndUsers = AppService.GetNextProjectIdAndSubUsers(id, subscriptionId);

			var list = idAndUsers.Item2; //Service.GetUsers();
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
					StartDate = AppService.GetDayFromDateTime(defaultStart),
					EndDate = AppService.GetDayFromDateTime(defaultEnd),
					ProjectOrgId = idAndUsers.Item1, //Service.GetRecommendedProjectId()
					CustomerName = AppService.GetCustomer(id).Name,
					SubscriptionId = subscriptionId,
					OrganizationId = 0 // TODO: get the actual org id from user subscription
				});
		}

		/// <summary>
		/// POST: Project/Create
		/// Method for creating a new project entry in the database.
		/// </summary>
		/// <param name="model">The model of user input.</param>
		/// <returns>If successful, notifies and redirects to Project/Index. Else, returns to the create project form.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(EditProjectViewModel model)
		{
			var list = AppService.GetNextProjectIdAndSubUsers(model.ParentCustomerId, model.SubscriptionId).Item2;
			var subList = new List<BasicUserInfoViewModel>();
			foreach (var user in list)
			{
				subList.Add(new BasicUserInfoViewModel(user.FirstName, user.LastName, user.UserId));        // Change to select list for model binding
			}
			int orgId = AppService.GetSubscription(model.SubscriptionId).OrganizationId;
			model.SubscriptionUsers = subList;
			if (ModelState.IsValid)
			{
				if (AppService.Can(Actions.CoreAction.EditProject, false, orgId, model.SubscriptionId))
				{
					if (model == null)
					{
						throw new ArgumentNullException("model");
					}

					try
					{
						var result = CreateProjectAndUpdateItsUserList(model);
						if (result == -1)   //duplicate projectOrgId
						{
							Notifications.Add(new BootstrapAlert(Resources.Strings.ProjectOrgIdNotUnique, Variety.Danger));
						}
						else
						{
							model.ProjectId = result;
							Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessProjectCreated, Variety.Success));
						}
					}
					catch (Exception ex)
					{
						string message = Resources.Strings.FailureProjectCreated;
						if (ex.Message != null)
						{
							message = string.Format("{0} {1}", message, ex.Message);
						}

						//Create failure
						Notifications.Add(new BootstrapAlert(message, Variety.Danger));
						return this.View(model);
					}
					return this.Redirect(string.Format("{0}#customerNumber{1}", Url.Action(ActionConstants.Index, ControllerConstants.Customer, new { model.SubscriptionId }), model.ParentCustomerId));
				}
				else
				{
					// Permissions failure
					this.Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
					return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
				}
			}
			else
			{
				// Invalid Model
				return this.View(model);
			}
		}

		/*
        /// <summary>
		/// POST: Project/Create
		/// Method for creating a new project entry in the database.
		/// </summary>
		/// <param name="model">The model of user input.</param>
		/// <returns>If successful, notifies and redirects to Project/Index. Else, returns to the create project form.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(EditProjectViewModel model)
		{
			var list = AppService.GetNextProjectIdAndSubUsers(model.ParentCustomerId).Item2;
			var subList = new List<BasicUserInfoViewModel>();
			foreach (var user in list)
			{
				subList.Add(new BasicUserInfoViewModel(user.FirstName, user.LastName, user.UserId));        // Change to select list for model binding
			}
			model.SubscriptionUsers = subList;
			if (ModelState.IsValid)
			{
				if (AppService.Can(Actions.CoreAction.EditProject))
				{
					if (model == null)
					{
						throw new ArgumentNullException("model");
					}

					if (AppService.GetAllProjectsForOrganization(UserContext.ChosenOrganizationId).Any(project => project.ProjectOrgId == model.ProjectOrgId && project.CustomerId == model.ParentCustomerId))
					{
						Notifications.Add(new BootstrapAlert(Resources.Strings.ProjectOrgIdNotUnique, Variety.Danger));
						return this.View(model);
					}
					try
					{
						model.ProjectId = CreateProject(model);
					}
					catch (Exception ex)
					{
						string message = Resources.Strings.FailureProjectCreated;
						if (ex.Message != null)
						{
							message = string.Format("{0} {1}", message, ex.Message);
						}

						//Create failure
						Notifications.Add(new BootstrapAlert(message, Variety.Danger));
						return this.View(model);
					}
					this.UpdateProject(model);
					Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessProjectCreated, Variety.Success));

					return this.Redirect(string.Format("{0}#customerNumber{1}", Url.Action(ActionConstants.Index, ControllerConstants.Customer), model.ParentCustomerId));
				}
				else
				{
					// Permissions failure
					this.Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
					return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
				}
			}
			else
			{
				// Invalid Model
				return this.View(model);
			}
		}   
    */

		/// <summary>
		/// Creates a new project using a <see cref="EditProjectViewModel"/> and updates that project's properties and user list..
		/// </summary>
		/// <param name="model"><see cref="EditProjectViewModel"/> representing new project.</param>
		/// <returns>The Project ID if succeed, -1 if the ProjectOrgId is taken by another project under the same customer.</returns>
		public int CreateProjectAndUpdateItsUserList(EditProjectViewModel model)
		{
			IEnumerable<int> userIDs = model.SelectedProjectUserIds.Select(userIdString => int.Parse(userIdString));

			return AppService.CreateProjectAndUpdateItsUserList(new Project()
			{
				CustomerId = model.ParentCustomerId,
				Name = model.ProjectName,
				Type = model.PriceType,
				ProjectOrgId = model.ProjectOrgId,
				StartingDate = AppService.GetDateTimeFromDays(model.StartDate),
				EndingDate = AppService.GetDateTimeFromDays(model.EndDate)
			}, userIDs);
		}

		/// <summary>
		/// Creates a new project using a <see cref="EditProjectViewModel"/>.
		/// </summary>
		/// <param name="model"><see cref="EditProjectViewModel"/> representing new project.</param>
		/// <returns>The Project ID.</returns>
		public int CreateProject(EditProjectViewModel model)
		{
			return AppService.CreateProject(new Project()
			{
				CustomerId = model.ParentCustomerId,
				Name = model.ProjectName,
				Type = model.PriceType,
				ProjectOrgId = model.ProjectOrgId,
				StartingDate = AppService.GetDateTimeFromDays(model.StartDate),
				EndingDate = AppService.GetDateTimeFromDays(model.EndDate)
			});
		}
	}
}
